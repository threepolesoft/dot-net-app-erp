using AppBO.Models;
using AppBO.Utility;
using AppDAL.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;
using AppBO.DbSet.AccessControl;

namespace AppBLL.Services
{
    public class SettingDeviceService
    {
        public PifErpDbContext pifErpDbContext;

        public UserService userService;
        public ShareService shareService;

        public SettingDeviceService(
            PifErpDbContext pifErpDbContext,
            UserService userService,
            ShareService shareService
            )
        {
            this.pifErpDbContext = this.pifErpDbContext;
            this.userService = userService;
            this.shareService = shareService;
        }

        public List<SettingDevice> GetBy(long UserId, string DeviceId)
        {
            List<SettingDevice> data = new List<SettingDevice>();
            data = pifErpDbContext.SettingDevices
                .Where(m => m.UserId == UserId && m.DeviceId == DeviceId)
                .ToList();

            return data;
        }

        public List<SettingDevice> GetByUserId(long UserId)
        {
            List<SettingDevice> data = new List<SettingDevice>();
            data = pifErpDbContext.SettingDevices
                .Where(m => m.UserId == UserId)
                .ToList();

            return data;
        }

        public ProccessResult ResetSave(List<SettingUserModel> model, long LogedUserId)
        {
            ProccessResult result = new();
            Role role = new();
            long UserId = model.FirstOrDefault().UserId;

            bool isDif = 1 == model.DistinctBy(m => m.UserId).ToList().Count();

            if (isDif == false)
            {
                result.Status = false;
                result.Message = $"Different UserId not allow";
                return result;
            }

            //model = model.Where(m => m.Value.ToLower() == "true").ToList();

            model.ForEach(m => m.EntryBy = LogedUserId);

            bool isDuplicate = model.Count() > model.DistinctBy(m => m.SettingId).ToList().Count();

            if (isDuplicate == true)
            {
                result.Status = false;
                result.Message = $"Duplicate SettingId not allow";
                return result;
            }


            foreach (var item in model)
            {
                if (shareService.GetBySettingId(item.SettingId) == null)
                {
                    result.Status = false;
                    result.Message = $"SettingId {item.SettingId} not available";
                    return result;
                }
            }

            var oldSetting = GetByUserId(UserId);
            var oldSettingTemp = GetByUserId(UserId);
            pifErpDbContext.SettingDevices.RemoveRange(oldSetting);

            oldSetting = oldSetting.GroupBy(m => m.DeviceId).Select(m => m.FirstOrDefault()).ToList();

            foreach (var item in oldSetting)
            {
                List<SettingDevice> roleUsers = new List<SettingDevice>();

                foreach (var item1 in model)
                {
                    roleUsers.Add(new SettingDevice
                    {
                        DeviceId = item.DeviceId,
                        UserId = item1.UserId,
                        SettingId = item1.SettingId,
                        Value = item1.Value,
                        EntryBy = LogedUserId,
                        EntryAt = DateTime.UtcNow,
                    });

                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationInsert,
                        Comments = $"This setting belonged to Device({item.DeviceId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }

                pifErpDbContext.SettingDevices.AddRange(roleUsers);

            }

            if (pifErpDbContext.SaveChanges() > -1 && role != null)
            {
                foreach (var item in oldSettingTemp)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationDelete,
                        Comments = $"This setting belonged to Device({item.DeviceId})",
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

        public ProccessResult Save(List<SettingDeviceModel> model, long LogedUserId)
        {
            ProccessResult result = new();
            Role role = new();

            bool isDif = 1 == model.DistinctBy(m => m.DeviceId).Count();

            if (isDif == false)
            {
                result.Status = false;
                result.Message = $"Different DeviceId not allow";
                return result;
            }

            long UserId = model.FirstOrDefault().UserId;
            string DeviceId = model.FirstOrDefault().DeviceId;

            model.ForEach(m => m.EntryBy = LogedUserId);

            bool isDuplicate = model.Count() > model.DistinctBy(m => m.SettingId).Count();

            if (isDuplicate == true)
            {
                result.Status = false;
                result.Message = $"Duplicate SettingId not allow";
                return result;
            }


            foreach (var item in model)
            {
                if (shareService.GetBySettingId(item.SettingId) == null)
                {
                    result.Status = false;
                    result.Message = $"SettingId {item.SettingId} not available";
                    return result;
                }
            }

            var oldSetting = GetBy(UserId, DeviceId);
            pifErpDbContext.SettingDevices.RemoveRange(oldSetting);

            List<SettingDevice> roleUsers = Extention.MapObject<List<SettingDevice>>(model);

            pifErpDbContext.SettingDevices.AddRange(roleUsers);

            if (pifErpDbContext.SaveChanges() > -1 && role != null)
            {
                foreach (var item in oldSetting)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationDelete,
                        Comments = $"This setting belonged to Device({item.DeviceId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }

                foreach (var item in roleUsers)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationInsert,
                        Comments = $"This setting belonged to Device({item.DeviceId})",
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

        public ProccessResult Save(SettingDeviceModel model)
        {
            ProccessResult result = new();
            Role role = new();

            long UserId = model.UserId;
            string DeviceId = model.DeviceId;

            var oldSetting = pifErpDbContext.Settings
                .Where(m => m.SettingName == model.SettingName && m.Scope == model.Scope)
                .Join(pifErpDbContext.SettingDevices,
                a => a.SettingId,
                b => b.SettingId,
                (a, b) => new
                {
                    a.SettingId,
                    a.SettingName,
                    b.SettingDeviceId,
                }).FirstOrDefault();

            if (oldSetting != null)
            {
                SettingDevice oldSettingDevice = pifErpDbContext.SettingDevices
                    .FirstOrDefault(m => m.SettingId == oldSetting.SettingId && m.DeviceId == model.DeviceId && m.UserId == model.UserId);

                if (oldSettingDevice != null)
                {
                    oldSettingDevice.Value = model.Value;
                    pifErpDbContext.SettingDevices.Update(oldSettingDevice);
                }
                else
                {
                    result.Message = $"SettingName {model.SettingName} not available";
                }
            }
            else
            {
                result.Message = $"SettingName {model.SettingName} not available";
            }


            if (pifErpDbContext.SaveChanges() > -1 && oldSetting!=null)
            {

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.SettingDevices),
                    RowId = oldSetting.SettingDeviceId,
                    Operation = keyValue.OperationInsert,
                    Comments = $"Value update to ({oldSetting.SettingDeviceId}) by user device",
                    User = model.UserId,
                    DateTime = DateTime.UtcNow,
                });


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

        public async Task<ProccessResult> SaveAsync(List<SettingDeviceModel> model, long LogedUserId)
        {
            ProccessResult result = new();
            Role role = new();

            // Ensure all items have the same DeviceId
            if (model.Select(m => m.DeviceId).Distinct().Count() > 1)
            {
                result.Status = false;
                result.Message = "Different DeviceId not allowed";
                return result;
            }

            long UserId = model.First().UserId;
            string DeviceId = model.First().DeviceId;

            // Assign EntryBy for all items
            model.ForEach(m => m.EntryBy = LogedUserId);

            // Ensure no duplicate SettingId
            if (model.GroupBy(m => m.SettingId).Any(g => g.Count() > 1))
            {
                result.Status = false;
                result.Message = "Duplicate SettingId not allowed";
                return result;
            }

            // Batch check if all SettingIds exist
            var settingIds = model.Select(m => m.SettingId).ToHashSet();
            var existingSettingIds = (await shareService.GetAllSettingIdsAsync()).ToHashSet(); // Assume `GetAllSettingIdsAsync()` fetches all valid SettingIds

            var invalidIds = settingIds.Except(existingSettingIds).ToList();
            if (invalidIds.Any())
            {
                result.Status = false;
                result.Message = $"SettingIds not available: {string.Join(", ", invalidIds)}";
                return result;
            }

            // Remove old settings for the user/device
            var oldSettings = await pifErpDbContext.SettingDevices.Where(m => m.UserId == UserId && m.DeviceId == DeviceId).ToListAsync();
            pifErpDbContext.SettingDevices.RemoveRange(oldSettings);

            // Map new settings and add them
            var newSettings = model.Select(m => new SettingDevice
            {
                DeviceId = m.DeviceId,
                UserId = m.UserId,
                SettingId = m.SettingId,
                Value = m.Value,
                EntryBy = LogedUserId,
                EntryAt = DateTime.UtcNow,
                UpdatedBy = 0
            }).ToList();

            await pifErpDbContext.SettingDevices.AddRangeAsync(newSettings);

            // Save new settings to get their generated IDs
            // Save changes in one batch
            var saved = await pifErpDbContext.SaveChangesAsync() > 0; // This ensures that SettingDeviceId is populated

            if (UserId != LogedUserId)
            {
                // Create activity logs
                var activityLogs = new List<Activity>();
                foreach (var item in oldSettings)
                {
                    activityLogs.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationDelete,
                        Comments = $"This setting belonged to Device({item.DeviceId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }

                foreach (var item in newSettings)
                {
                    activityLogs.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationInsert,
                        Comments = $"This setting belonged to Device({item.DeviceId})",
                        User = LogedUserId,
                        DateTime = DateTime.UtcNow,
                    });
                }

                await pifErpDbContext.Activities.AddRangeAsync(activityLogs);

                await pifErpDbContext.SaveChangesAsync();
            }

            result.Status = true;
            result.Message = saved ? ProccessStatus.Success : ProccessStatus.Fail;
            return result;
        }

        public ProccessResult GetJunkSettings(DateTime JunkDate)
        {
            var result = new ProccessResult();

            var count = (from sd1 in pifErpDbContext.SettingDevices
                         join ud1 in pifErpDbContext.UsersDevices
                             .Where(ud => ud.LoginAt > JunkDate)
                             on new { sd1.UserId, sd1.DeviceId }
                             equals new { ud1.UserId, ud1.DeviceId }
                             into udJoin
                         from ud2 in udJoin.DefaultIfEmpty()
                         where ud2 == null || ud2.LoginAt == null
                         select sd1).Count();


            result.Status = true;
            result.Data = count;

            return result;
        }

        public ProccessResult JunkSettingsDelete(long LogedUser, DateTime JunkDate)
        {
            var result = new ProccessResult();

            var data = (from sd1 in pifErpDbContext.SettingDevices
                        join ud1 in pifErpDbContext.UsersDevices
                            .Where(u => u.LoginAt > JunkDate)
                            on new { sd1.UserId, sd1.DeviceId }
                            equals new { ud1.UserId, ud1.DeviceId }
                            into udJoin
                        from ud2 in udJoin.DefaultIfEmpty() // LEFT JOIN
                        where ud2 == null || ud2.LoginAt == null
                        select sd1) // 👈 Select the full entity, not new { ... }
                       .ToList();

            // Now you can delete
            pifErpDbContext.SettingDevices.RemoveRange(data);

            if (pifErpDbContext.SaveChanges() > 0)
            {
                // Create activity logs
                var activityLogs = new List<Activity>();

                foreach (var item in data)
                {
                    activityLogs.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.SettingDevices),
                        RowId = item.SettingDeviceId,
                        Operation = keyValue.OperationDelete,
                        Comments = $"Junk setting reason",
                        User = LogedUser,
                        DateTime = DateTime.UtcNow,
                    });
                }

                pifErpDbContext.Activities.AddRange(activityLogs);
                pifErpDbContext.SaveChanges();
            }

            result.Status = true;
            result.Message = "Success";

            return result;
        }
    }
}
