using AppBO.DbSet.AccessControl;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.Utility;
using AppDAL.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;

namespace AppBLL.Services
{
    public class ApplicationUserService
    {
        public PifErpDbContext pifErpDbContext;
        public ApplicationUserService(PifErpDbContext pifErpDbContext)
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(ApplicationUserModel model)
        {
            return pifErpDbContext.ApplicationUsers
                .Any(m => m.UserName == model.UserName.Trim() && m.ApplicationUserId!=model.ApplicationUserId);
        }

        public ProccessResult Save(ApplicationUserModel model)
        {
            ProccessResult result = new();
            ApplicationUser applicationUser = new();

            if (model.ApplicationUserId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.UserName.Trim()} already exists";
                    return result;
                }

                applicationUser = pifErpDbContext.ApplicationUsers
                   .Where(m => m.ApplicationUserId == model.ApplicationUserId).FirstOrDefault();

                if (applicationUser != null)
                {
                    applicationUser.FullName = model.FullName;
                    applicationUser.RoleId = model.RoleId;
                    applicationUser.Email = model.Email;
                    applicationUser.Phone = model.Phone;
                    applicationUser.IsActive = model.IsActive;
                    applicationUser.IsDelete = model.IsDelete;
                    applicationUser.UpdatedBy = model.UpdatedBy;

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        applicationUser.Password = PasswordHash(model.Password);
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = $"ApplicationUserId {model.ApplicationUserId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.UserName.Trim()} already exists";
                    return result;
                }

                applicationUser = Extention.MapObject<ApplicationUser>(model);
                applicationUser.UserName = applicationUser.UserName.Trim();
                applicationUser.Password = PasswordHash("1234");
                applicationUser.EntryAt = DateTime.UtcNow;
                pifErpDbContext.ApplicationUsers.Add(applicationUser);
            }


            if (pifErpDbContext.SaveChanges() > -1 && applicationUser != null)
            {
                string op = model.ApplicationUserId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;
                string mgs = model.ApplicationUserId == 0 ? "Added success" : "Updated success";

                if (model.IsDelete)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<ApplicationUserModel>(applicationUser);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.ApplicationUsers),
                    RowId = model.ApplicationUserId,
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

        public List<ApplicationUserModel> GetByAll()
        {
            List<ApplicationUserModel> all = new List<ApplicationUserModel>();
            all = pifErpDbContext.ApplicationUsers
                .Where(m => m.IsDelete == false)
                .MapTo<ApplicationUser, ApplicationUserModel>()
                .ToList();

            return all;
        }
        public string PasswordHash(string Password)
        {
            StringBuilder sb = new StringBuilder();

            //Create an MD5 object
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
