using AppBO.Models;
using AppBO.Utility;
using AppDAL.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;
using AppBO.DbSet.AccessControl;

namespace AppBLL.Services
{
    public class SettingUserService
    {
        public PifErpDbContext pifErpDbContext;
        public UserService userService;
        public SettingService settingService;
        public SettingDeviceService settingDeviceService;

        public SettingUserService(
            PifErpDbContext pifErpDbContext,
            UserService userService,
            SettingService settingService,
            SettingDeviceService settingDeviceService
            )
        {
            this.pifErpDbContext = pifErpDbContext;
            this.userService = userService;
            this.settingService = settingService;
            this.settingDeviceService = settingDeviceService;
        }

        public List<SettingUser> GetByUserId(long ApplicationUserId)
        {
            List<SettingUser> data = new List<SettingUser>();
            data = pifErpDbContext.SettingUsers
                .Where(m => m.ApplicationUserId == ApplicationUserId)
               .ToList();

            return data;
        }

        public ProccessResult Save(List<SettingUserModel> model, long LogedUserId)
        {
            ProccessResult result = new();
            Role role = new();

            bool isDif = 1 == model.DistinctBy(m => m.UserId).ToList().Count();

            if (isDif == false)
            {
                result.Status = false;
                result.Message = $"Different UserId not allow";
                return result;
            }

            long UserId = model.FirstOrDefault().UserId;

            model.ForEach(m => m.EntryBy = LogedUserId);

            bool isDuplicate = model.Count() > model.DistinctBy(m => m.SettingId).ToList().Count();

            if (isDuplicate == true)
            {
                result.Status = false;
                result.Message = $"Duplicate SettingId not allow";
                return result;
            }

            var user = model.GroupBy(m => m.UserId).FirstOrDefault();

            foreach (var item in user)
            {
                if (userService.GeyByApplicationUserId(UserId) == null)
                {
                    result.Status = false;
                    result.Message = $"UserId {UserId} not available";
                    return result;
                }
            }

            foreach (var item in model)
            {
                if (settingService.GetBySettingId(item.SettingId) == null)
                {
                    result.Status = false;
                    result.Message = $"SettingId {item.SettingId} not available";
                    return result;
                }
            }

            var oldSetting = GetByUserId(UserId);
            pifErpDbContext.SettingUsers.RemoveRange(oldSetting);

            List<SettingUser> roleUsers = Extention.MapObject<List<SettingUser>>(model);

            pifErpDbContext.SettingUsers.AddRange(roleUsers);

            if (pifErpDbContext.SaveChanges() > -1 && role != null)
            {
                foreach (var item in oldSetting)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingUsers),
                        RowId = item.SettingUserId,
                        Operation = keyValue.OperationDelete,
                        Comments = $"This setting belonged to User({item.ApplicationUserId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }

                foreach (var item in roleUsers)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingUsers),
                        RowId = item.SettingUserId,
                        Operation = keyValue.OperationInsert,
                        Comments = $"This setting is for User({item.ApplicationUserId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }

                pifErpDbContext.SaveChanges();

                settingDeviceService.ResetSave(model, LogedUserId);


                result.Status = true;
                result.Message = ProccessStatus.Success;
            }
            else
            {
                result.Status = false;
                result.Message = ProccessStatus.Fail;
            }

            return result;
        }
    }
}
