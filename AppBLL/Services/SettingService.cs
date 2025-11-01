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
    public class SettingService
    {
        public PifErpDbContext pifErpDbContext;
        public UserService userService;
        public SettingDeviceService settingDeviceService;
        public SettingService(
            PifErpDbContext pifErpDbContext,
            UserService userService,
            SettingDeviceService settingDeviceService
            )
        {
            this.pifErpDbContext = pifErpDbContext;
            this.userService = userService;
            this.settingDeviceService = settingDeviceService;
        }

        private bool IsDuplicate(SettingModel model)
        {
            return pifErpDbContext.Settings
                .Any(m => m.SettingTitle == model.SettingTitle.Trim() && m.Scope == model.Scope && m.SettingId != model.SettingId);
        }

        public SettingModel GetBySettingId(long SettingId)
        {
            SettingModel data = new SettingModel();
            data = pifErpDbContext.Settings.Where(m => m.SettingId == SettingId)
                .MapTo<Setting, SettingModel>().FirstOrDefault();
            return data;
        }

        public List<SettingModel> GetByAll()
        {
            List<SettingModel> all = new List<SettingModel>();
            all = pifErpDbContext.Settings
                .Where(m=>m.IsDelete==false)
                .MapTo<Setting, SettingModel>()
                .ToList();

            return all;
        }

        public List<SettingModel> GetByScope(string Scope)
        {
            List<SettingModel> all = new List<SettingModel>();
            all = pifErpDbContext.Settings
                .Where(m => m.IsDelete == false && m.Scope == Scope)
                .MapTo<Setting, SettingModel>()
                .ToList();
            return all;
        }

        public ProccessResult Save(SettingModel model)
        {
            ProccessResult result = new();
            Setting setting = new();

            if (model.Scope != keyValue.RoleScopeApp && model.Scope != keyValue.RoleScopeAdmin)
            {
                result.Status = false;
                result.Message = $"Scop accept only {keyValue.RoleScopeApp} or {keyValue.RoleScopeAdmin}";
                return result;
            }

            if (model.SettingId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.SettingTitle.Trim()} already exists";
                    return result;
                }

                setting = pifErpDbContext.Settings
                   .Where(m => m.SettingId == model.SettingId).FirstOrDefault();

                if (setting != null)
                {
                    setting.SettingTitle = model.SettingTitle;
                    setting.Scope = model.Scope;
                    setting.IsActive = model.IsActive;
                    setting.IsDelete = model.IsDelete;
                    setting.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"SettingId {model.SettingId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.SettingTitle.Trim()} already exists";
                    return result;
                }

                setting = Extention.MapObject<Setting>(model);
                setting.SettingTitle = setting.SettingTitle.Trim();
                setting.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Settings.Add(setting);

            }


            if (pifErpDbContext.SaveChanges() > -1 && setting != null)
            {
                string op = model.SettingId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;
                string mgs = model.SettingId == 0 ? "Added success" : "Updated success";

                if (model.IsDelete)
                {
                    op = keyValue.OperationDelete;
                }
             

                model = Extention.MapObject<SettingModel>(setting);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Settings),
                    RowId = model.SettingId,
                    Operation = op,
                    User = model.UpdatedBy,
                    DateTime = DateTime.UtcNow,
                });
                pifErpDbContext.SaveChanges();
                result.Status = true;
                result.Message = mgs;
                result.Data = model;
            }
            else
            {
                result.Status = false;
                result.Message = ProccessStatus.Fail;
            }

            return result;
        }

        public List<SettingModel> GetByUserId(long UserId)
        {
            List<SettingModel> roles = new List<SettingModel>();
            roles = pifErpDbContext.SettingUsers
                .Where(m => m.ApplicationUserId == UserId)
                .Join(pifErpDbContext.Settings.Where(m => m.IsDelete == false && m.IsActive == true),
                a => a.SettingId,
                b => b.SettingId,
                (a, b) => new
                {
                    s = b,
                    su = a
                })
                .Select(m => new SettingModel
                {
                    SettingId = m.s.SettingId,
                    SettingName = m.s.SettingName,
                    SettingTitle = m.s.SettingTitle,
                    Scope = m.s.Scope,
                    Value = m.su.Value,
                    IsActive = m.su.IsActive,
                    EntryBy = m.su.EntryBy,
                    EntryAt = m.su.EntryAt,
                    UpdatedBy = m.su.UpdatedBy,
                })
                .ToList();

            return roles;
        }

        public ProccessResult GetAllUserSetting(long LogedUserId)
        {
            ProccessResult result = new ProccessResult();

            var data = from u in pifErpDbContext.ApplicationUsers
                       select new
                       {
                           u.ApplicationUserId,
                           u.FullName,
                           u.Phone,
                           Settings = (from s in pifErpDbContext.Settings.Where(m => m.IsActive == true && m.IsDelete == false)
                                       join sd in pifErpDbContext.SettingUsers.Where(m=>m.ApplicationUserId==u.ApplicationUserId) on new { s.SettingId}
                                       equals new { sd.SettingId } into ss
                                       from d in ss.DefaultIfEmpty()
                                       select new
                                       {
                                           s.SettingId,
                                           s.SettingName,
                                           s.SettingTitle,
                                           Value = d != null ? d.Value : false.ToString()
                                       }).ToList()
                       };

            result.Data = data.ToList();
            return result;
        }
        public ProccessResult GetBy(int ApplicationUserId, long LogedUserId)
        {
            ProccessResult result = new ProccessResult();

            var data = from device in pifErpDbContext.SettingDevices.Where(m => m.UserId == ApplicationUserId)
                       join mu in pifErpDbContext.UsersDevices.Where(m=>m.UserId==ApplicationUserId) on device.DeviceId equals mu.DeviceId into mus
                       from mu in mus.DefaultIfEmpty()
                       group new { device, mu } by new { device.DeviceId, mu.DeviceName } into devices
                       select new
                       {
                           devices.Key.DeviceId,
                           devices.Key.DeviceName,
                           Settings = (from s in pifErpDbContext.Settings.Where(m => m.IsActive == true && m.IsDelete == false)
                                       join sd in pifErpDbContext.SettingDevices.Where(m=>m.UserId==ApplicationUserId) on new { s.SettingId, devices.Key.DeviceId }
                                       equals new { sd.SettingId, sd.DeviceId } into ss
                                       from d in ss.DefaultIfEmpty()
                                       select new
                                       {
                                           s.SettingId,
                                           s.SettingName,
                                           s.SettingTitle,
                                           Value = d != null ? d.Value : false.ToString()
                                       }).ToList()
                       };

            var datas = new
            {
                UserInfo= pifErpDbContext.ApplicationUsers.Where(m=>m.ApplicationUserId==ApplicationUserId)
                .Select(m=>
                new
                {
                    m.FullName,
                    m.Phone
                }).FirstOrDefault(),
                UserSettings = from s in pifErpDbContext.Settings.Where(m=>m.IsActive==true && m.IsDelete==false)
                               join sd in pifErpDbContext.SettingUsers.Where(m => m.ApplicationUserId == ApplicationUserId) on s.SettingId equals sd.SettingId into ss
                               from d in ss.DefaultIfEmpty()
                               select new
                               {
                                   s.SettingId,
                                   s.SettingName,
                                   s.SettingTitle,
                                   Value = d != null ? d.Value : false.ToString()
                               },
                //DeviceSettings = data.ToList(),
                DeviceSettings = data.ToList().GroupBy(m => m.DeviceId).Select(m => m.First()).ToList()
        };

            result.Data = datas;
            return result;
        }

        public ProccessResult GetBy(long UserId, string DeviceId, long LogedUserId)
        {
            ProccessResult result = new ProccessResult();


            var ss = from sd in pifErpDbContext.SettingDevices.Where(m => m.UserId==UserId && m.DeviceId == DeviceId)
                     join s in pifErpDbContext.Settings.Where(m => m.IsActive == true && m.IsDelete == false) on sd.SettingId equals s.SettingId
                     select new
                     {
                         sd.SettingId,
                         s.SettingName,
                         s.SettingTitle,
                         s.Scope,
                         sd.Value,
                     };

            bool isNew = ss.ToList().Count() == 0;

            if (isNew == true)
            {
                ss = from us in pifErpDbContext.SettingUsers.Where(m => m.ApplicationUserId == UserId)
                     join s in pifErpDbContext.Settings.Where(m => m.IsActive == true && m.IsDelete == false) on us.SettingId equals s.SettingId
                     select new
                     {
                         s.SettingId,
                         s.SettingName,
                         s.SettingTitle,
                         s.Scope,
                         us.Value,
                     };

                var userSettings = ss
                    .Select(m => new SettingDeviceModel
                    {
                        DeviceId = DeviceId,
                        UserId = UserId,
                        SettingId = m.SettingId,
                        Value = m.Value,
                        EntryBy = LogedUserId,
                        UpdatedBy = 0,
                        EntryAt = DateTime.UtcNow
                    }).ToList();

                settingDeviceService.Save(userSettings, LogedUserId);
            }

            result.Data = ss.ToList();

            return result;
        }

        public async Task<ProccessResult> GetByAsync(long UserId, string DeviceId, string Scope, long LogedUserId)
        {
            ProccessResult result = new ProccessResult();

            // Query the SettingDevices and Settings tables to check if the device setting exists
            var ss = from sd in pifErpDbContext.SettingDevices
                     join s in pifErpDbContext.Settings.Where(m => m.IsActive && !m.IsDelete && m.Scope== Scope)
                     on sd.SettingId equals s.SettingId
                     where sd.UserId == UserId && sd.DeviceId == DeviceId
                     select new
                     {
                         s.SettingName,
                         s.SettingTitle,
                         sd.Value,
                     };

            // Check if the device settings are new
            bool isNew = !await ss.AnyAsync();  // Efficiently check existence without loading data

            // If new device settings, load the user settings and save them
            if (isNew)
            {
                var userSettingsQuery = from us in pifErpDbContext.SettingUsers.Where(m=>m.ApplicationUserId == UserId)
                                        join s in pifErpDbContext.Settings.Where(m => m.IsActive && !m.IsDelete && m.Scope == Scope)
                                        on us.SettingId equals s.SettingId
                                        select new
                                        {
                                            s.SettingId,
                                            us.Value,
                                        };

                var userSettings = await userSettingsQuery
                    .Select(m => new SettingDeviceModel
                    {
                        DeviceId = DeviceId,
                        UserId = UserId,
                        SettingId = m.SettingId,
                        Value = m.Value,
                    }).ToListAsync();

                // Save the user settings to SettingDevices
                await settingDeviceService.SaveAsync(userSettings, LogedUserId);  // Ensure SaveAsync is used here if it's available
            }

            // Retrieve the final list of settings for the device
            result.Data = await ss.ToListAsync();  // Use ToListAsync for asynchronous execution

            return result;
        }

    }

}
