using AppBO.DbSet.AccessControl;
using AppBO.Models;
using AppBO.Utility;
using AppDAL.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;

namespace AppBLL.Services
{
    public class UsersDevicesService
    {
        public PifErpDbContext pifErpDbContext;

        public UsersDevicesService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(UsersDeviceModel model)
        {
            return pifErpDbContext.UsersDevices
                .Any(m => m.UserId == model.UserId && m.DeviceId == model.DeviceId && m.Scope == model.Scope);
        }

        public ProccessResult Save(UsersDeviceModel model)
        {
            ProccessResult result = new();
            UsersDevice oldModel = new();

            if (model.Scope != keyValue.RoleScopeApp && model.Scope != keyValue.RoleScopeAdmin)
            {
                result.Status = false;
                result.Message = $"Scop accept only {keyValue.RoleScopeApp} or {keyValue.RoleScopeAdmin}";
                return result;
            }

            if (IsDuplicate(model) == true)
            {
                oldModel = pifErpDbContext.UsersDevices
                   .Where(m => m.UserId == model.UserId && m.DeviceId==model.DeviceId).FirstOrDefault();

                if (oldModel != null)
                {
                    oldModel.LoginAt = DateTime.UtcNow;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Users DeviceId {model.UsersDeviceId} not available";
                    return result;
                }
            }
            else
            {

                oldModel = Extention.MapObject<UsersDevice>(model);
                oldModel.EntryAt = DateTime.UtcNow;
                oldModel.LoginAt = DateTime.UtcNow;

                pifErpDbContext.UsersDevices.Add(oldModel);

            }


            if (pifErpDbContext.SaveChanges() > -1 && oldModel != null)
            {
                result.Status = true;
                result.Message = ProccessStatus.Success;
                result.Data = model;
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
