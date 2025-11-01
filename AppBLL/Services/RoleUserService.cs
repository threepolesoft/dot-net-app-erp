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
    public class RoleUserService
    {
        public PifErpDbContext pifErpDbContext;
        public UserService userService;
        public RoleService roleService;

        public RoleUserService(
            PifErpDbContext pifErpDbContext, 
            UserService userService,
            RoleService roleService
            )
        {
            this.pifErpDbContext = pifErpDbContext;
            this.userService = userService; 
            this.roleService = roleService;
        }

        private List<RoleUser> GetByUserId(long ApplicationUserId)
        {
           var roles = pifErpDbContext.RoleUsers.Where(m => m.ApplicationUserId == ApplicationUserId).ToList();
            return roles;
        }

        public ProccessResult Save(List<RoleUserModel> model, long LogedUserId)
        {
            ProccessResult result = new();
            Role role = new();

            bool isDif = 1 == model.DistinctBy(m => m.ApplicationUserId).ToList().Count();

            if (isDif == false)
            {
                result.Status = false;
                result.Message = $"Different UserId not allow";
                return result;
            }

            long UserId = model.FirstOrDefault().ApplicationUserId;

            model.ForEach(m => m.EntryBy = LogedUserId);

            bool isDuplicate = model.Count()> model.DistinctBy(m => m.RoleId).ToList().Count();

            if (isDuplicate == true)
            {
                result.Status = false;
                result.Message = $"Duplicate RoleId not allow";
                return result;
            }

            var user=model.GroupBy(m => m.ApplicationUserId).FirstOrDefault();

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
                if (item.RoleId > 0 && roleService.GetByRoleId(item.RoleId) == null)
                {
                    result.Status = false;
                    result.Message = $"RoleId {item.RoleId} not available";
                    return result;
                }
            }

            var oldRole = GetByUserId(UserId);
            pifErpDbContext.RemoveRange(oldRole);

            List<RoleUser> roleUsers = Extention.MapObject<List<RoleUser>>(model.Where(m=>m.RoleId>0));

            pifErpDbContext.RoleUsers.AddRange(roleUsers);

            if (pifErpDbContext.SaveChanges() > -1 && role != null)
            {
                foreach (var item in oldRole)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.RoleUsers),
                        RowId = item.RoleUserId,
                        Operation = keyValue.OperationDelete,
                        Comments = $"This role belonged to User({item.ApplicationUserId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                } 
                
                foreach (var item in roleUsers)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.RoleUsers),
                        RowId = item.RoleUserId,
                        Operation = keyValue.OperationInsert,
                        Comments = $"This role is for User({item.ApplicationUserId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }
                
                pifErpDbContext.SaveChanges();
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
