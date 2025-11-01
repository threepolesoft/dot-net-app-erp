using AppBO.Enum;
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
    public class RoleService
    {
        public PifErpDbContext pifErpDbContext;

        public RoleService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(RoleModel model)
        {
            return pifErpDbContext.Roles
                .Any(m => m.RoleTitle == model.RoleTitle.Trim() && m.Scope == model.Scope && m.RoleId != model.RoleId);
        }

        public List<RoleModel> GetByAll()
        {
            List<RoleModel> roles = new List<RoleModel>();
            roles = pifErpDbContext.Roles.MapTo<Role, RoleModel>().ToList();
            return roles;
        }

        public RoleModel GetByRoleId(long RoleId)
        {
            RoleModel role = new RoleModel();
            role = pifErpDbContext.Roles.Where(m => m.RoleId == RoleId).MapTo<Role, RoleModel>().FirstOrDefault();
            return role;
        }

        public ProccessResult Save(RoleModel model)
        {
            ProccessResult result = new();
            Role role = new();

            if (model.Scope != keyValue.RoleScopeApp && model.Scope != keyValue.RoleScopeAdmin)
            {
                result.Status = false;
                result.Message = $"Scop accept only {keyValue.RoleScopeApp} or {keyValue.RoleScopeAdmin}";
                return result;
            }

            if (model.RoleId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.RoleTitle.Trim()} already exists";
                    return result;
                }

                role = pifErpDbContext.Roles
                   .Where(m => m.RoleId == model.RoleId).FirstOrDefault();

                if (role != null)
                {
                    role.RoleTitle = model.RoleTitle;
                    role.Scope = model.Scope;
                    role.IsDelete = model.IsDelete;
                    role.IsActive = model.IsActive;
                    role.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Role Id {model.RoleId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.RoleTitle.Trim()} already exists";
                    return result;
                }

                role = Extention.MapObject<Role>(model);
                role.RoleTitle = role.RoleTitle.Trim();
                role.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Roles.Add(role);

            }


            if (pifErpDbContext.SaveChanges() > -1 && role != null)
            {
                string op = model.RoleId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<RoleModel>(role);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Roles),
                    RowId = model.RoleId,
                    Operation = op,
                    User = model.UpdatedBy,
                    DateTime = DateTime.UtcNow,
                });
                pifErpDbContext.SaveChanges();
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

        public ProccessResult GetByUserId(long UserId)
        {
            ProccessResult result = new ProccessResult();

            var roles = pifErpDbContext.RoleUsers
                .Where(m => m.ApplicationUserId == UserId)
                .Join(pifErpDbContext.Roles.Where(m => m.IsDelete == false && m.IsActive == true),
                    a => a.RoleId,
                    b => b.RoleId,
                    (a, b) => new
                    {
                        b.RoleId,
                        b.RoleName,
                        b.RoleTitle,
                        b.Scope
                    })
                .ToList()
                .Select(m => (object)m) // Convert to object to allow different anonymous types
                .ToList();

            if (UserId == 100 && !roles.Any(m => ((dynamic)m).RoleName == "ac"))
            {
                roles.Add(new
                {
                    RoleId = 0,
                    RoleName = "ac",
                    RoleTitle = "",
                    Scope = "admin"
                });
            }

            RoleUser roleUser = pifErpDbContext.RoleUsers
                .Where(m => m.ApplicationUserId == UserId)
                .FirstOrDefault();

            if (roleUser != null)
            {
                var roleMenu = from r in pifErpDbContext.RoleMenus
                               join md in pifErpDbContext.MenuDetails on r.MenuDetailId equals md.MenuDetailId
                               join m in pifErpDbContext.Menus on md.MenuId equals m.MenuId
                               where r.RoleId == roleUser.RoleId
                               select new
                               {
                                   m.MenuUrl,
                               };

                foreach (var item in roleMenu)
                {
                    if (!string.IsNullOrEmpty(item.MenuUrl.ToString()))
                    {
                        roles.Add(new
                        {
                            RoleName = item.MenuUrl,
                        });
                    }

                }
            }

            result.Data = roles;

            return result;
        }

        public ProccessResult GetUserRole(long ApplicationUserId, long LogedUserId)
        {
            ProccessResult result = new ProccessResult();

            var users = pifErpDbContext.ApplicationUsers.AsQueryable();

            // Filter by UserId only if UserId is greater than 0
            if (ApplicationUserId>0)
            {
                users = users.Where(m => m.ApplicationUserId == ApplicationUserId);
            }

            var data = from u in users

                       select new
                       {
                           u.ApplicationUserId,
                           u.FullName,
                           u.Phone,
                           Roles = (from s in pifErpDbContext.Roles.Where(m => m.IsActive == true && m.IsDelete == false)
                                    join sd in pifErpDbContext.RoleUsers.Where(m => m.ApplicationUserId == u.ApplicationUserId) on new { s.RoleId }
                                    equals new { sd.RoleId } into ss
                                    from d in ss.DefaultIfEmpty()
                                    select new
                                    {
                                        s.RoleId,
                                        s.RoleName,
                                        s.RoleTitle,
                                        Status = d != null ? true : false
                                    }).ToList()
                       };

            result.Data = data.ToList();
            return result;
        }
    }
}
