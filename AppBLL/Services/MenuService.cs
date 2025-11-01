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
    public class MenuService
    {

        public PifErpDbContext pifErpDbContext;
        public MenuService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(MenuModel model)
        {
            return pifErpDbContext.Menus
                .Any(m => m.MenuTitle == model.MenuTitle.Trim() && m.Scope == model.Scope && m.MenuId != model.MenuId);
        }

        public ProccessResult Save(MenuModel model)
        {
            ProccessResult result = new();
            Menu add = new();

            if (model.Scope != keyValue.RoleScopeApp && model.Scope != keyValue.RoleScopeAdmin)
            {
                result.Status = false;
                result.Message = $"Scop accept only {keyValue.RoleScopeApp} or {keyValue.RoleScopeAdmin}";
                return result;
            }

            if (model.MenuId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.MenuTitle.Trim()} already exists";
                    return result;
                }

                add = pifErpDbContext.Menus
                   .Where(m => m.MenuId == model.MenuId).FirstOrDefault();

                if (add != null)
                {
                    add.MenuUrl = model.MenuUrl.Trim();
                    add.MenuTitle = model.MenuTitle;
                    add.MenuIcon = model.MenuIcon;
                    add.Scope = model.Scope;
                    add.IsDelete = model.IsDelete;
                    add.IsActive = model.IsActive;
                    add.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Menu Id {model.MenuId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.MenuTitle.Trim()} already exists";
                    return result;
                }

                add = Extention.MapObject<Menu>(model);
                add.MenuUrl = add.MenuUrl.Trim();
                add.MenuTitle = add.MenuTitle.Trim();
                add.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Menus.Add(add);

            }

            if (pifErpDbContext.SaveChanges() > -1 && add != null)
            {
                string op = model.MenuId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<MenuModel>(add);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Menus),
                    RowId = model.MenuId,
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

        public ProccessResult GetBy(string Scope="")
        {
            ProccessResult result = new();

            var menus = pifErpDbContext.Menus.AsQueryable();

            if (!string.IsNullOrEmpty(Scope))
            {
                menus.Where(m => m.Scope == Scope);
            }

            result.Data=menus.Select(m => new MenuModel
            {
                MenuId = m.MenuId,
                MenuUrl = m.MenuUrl,
                MenuTitle = m.MenuTitle,
                MenuIcon = m.MenuIcon,
                Scope = m.Scope,
                IsActive = m.IsActive,
                IsDelete = m.IsDelete,
                EntryAt = m.EntryAt,
                EntryBy = m.EntryBy,
                UpdatedBy = m.UpdatedBy
            }).ToList();

            return result;
        }
    }
}