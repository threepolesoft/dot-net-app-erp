using AppBO.DbSet.AccessControl;
using AppBO.Models;
using AppBO.ModelsApi;
using AppBO.Utility;
using AppDAL.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;

namespace AppBLL.Services
{
    public class MenuDetailService
    {
        public PifErpDbContext pifErpDbContext;
        public List<MenuLeftModel> menuLeftModels = new List<MenuLeftModel>();
        public MenuDetailService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        public int Serial(MenuDetailModel model)
        {
            return pifErpDbContext.MenuDetails.Count(m => m.Parent == model.Parent) + 1;
        }

        public ProccessResult Save(MenuDetailModel model, long logedUser)
        {
            ProccessResult result = new();
            MenuDetail add = new();

            if (model.MenuDetailId > 0)
            {
                add = pifErpDbContext.MenuDetails
                   .Where(m => m.MenuDetailId == model.MenuDetailId).FirstOrDefault();

                if (add != null)
                {
                    add.IsView = model.IsView;
                    add.UpdatedBy = model.UpdatedBy;

                    if (model.IsDelete == true)
                    {
                        pifErpDbContext.MenuDetails.Remove(add);
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Menu Detail Id {model.MenuDetailId} not available";
                    return result;
                }
            }
            else
            {
                add = Extention.MapObject<MenuDetail>(model);
                add.Serial = Serial(model);
                add.EntryBy = logedUser;
                add.EntryAt = DateTime.UtcNow;

                pifErpDbContext.MenuDetails.Add(add);

            }

            if (pifErpDbContext.SaveChanges() > -1 && add != null)
            {
                string op = model.MenuDetailId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<MenuDetailModel>(add);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.MenuDetails),
                    RowId = model.MenuDetailId,
                    Operation = op,
                    User = logedUser,
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

        public ProccessResult Delete(List<long> model, long logedUser)
        {
            ProccessResult result = new();

            var itemsToDelete = pifErpDbContext.MenuDetails.AsQueryable();
            foreach (var id in model)
            {
                MenuDetail menuDetail = pifErpDbContext.MenuDetails.Where(m => m.MenuDetailId == id).FirstOrDefault();

                if (menuDetail != null)
                {
                    pifErpDbContext.MenuDetails.Remove(menuDetail);
                }

            }

            if (pifErpDbContext.SaveChanges() > 0)
            {
                foreach (var item in menuLeftModels)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.MenuDetails),
                        RowId = item.MenuDetailId,
                        Operation = keyValue.OperationDelete,
                        User = logedUser,
                        DateTime = DateTime.UtcNow,
                    });
                }

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

        public ProccessResult GetAll()
        {
            ProccessResult result = new();

            var data = from md in pifErpDbContext.MenuDetails
                       join m in pifErpDbContext.Menus on md.MenuId equals m.MenuId
                       select new MenuLeftModel
                       {
                           MenuDetailId = md.MenuDetailId,
                           Parent = md.Parent,
                           Serial = md.Serial,
                           MenuId = m.MenuId,
                           MenuUrl = m.MenuUrl,
                           MenuTitle = m.MenuTitle,
                           MenuIcon = m.MenuIcon,
                           IsView = md.IsView
                       };

            result.Status = true;
            result.Data = BuildMenuTree(data.ToList());

            return result;
        }

        public ProccessResult GetAll(long logedUser)
        {
            ProccessResult result = new();

            RoleUser roleUser = pifErpDbContext.RoleUsers.Where(m => m.ApplicationUserId == logedUser).FirstOrDefault();

            if (roleUser != null)
            {
                var data = from md in pifErpDbContext.MenuDetails
                           join m in pifErpDbContext.Menus on md.MenuId equals m.MenuId
                           join rm in pifErpDbContext.RoleMenus.Where(rm => rm.RoleId == roleUser.RoleId)
                               on md.MenuDetailId equals rm.MenuDetailId
                           select new MenuLeftModel
                           {
                               MenuDetailId = md.MenuDetailId,
                               Parent = md.Parent,
                               Serial = md.Serial,
                               RoleId = roleUser.RoleId,
                               MenuId = m.MenuId,
                               MenuUrl = m.MenuUrl,
                               MenuTitle = m.MenuTitle,
                               MenuIcon = m.MenuIcon,
                               IsView = md.IsView,
                               Status = true // rm is guaranteed not null now
                           };

                result.Status = true;
                result.Data = BuildMenuTree(data.ToList());
            }

            return result;
        }

        public ProccessResult GetByRole()
        {
            ProccessResult result = new();

            var roles = pifErpDbContext.Roles.ToList();

            List<RoleAccessModel> model = new List<RoleAccessModel>();

            foreach (var item in roles)
            {

                var data = from md in pifErpDbContext.MenuDetails
                           join m in pifErpDbContext.Menus on md.MenuId equals m.MenuId
                           join rm in pifErpDbContext.RoleMenus.Where(m => m.RoleId == item.RoleId) on md.MenuDetailId equals rm.MenuDetailId into rms
                           from rm in rms.DefaultIfEmpty()
                           select new MenuLeftModel
                           {
                               MenuDetailId = md.MenuDetailId,
                               Parent = md.Parent,
                               Serial = md.Serial,
                               RoleId = item.RoleId,
                               MenuId = m.MenuId,
                               MenuUrl = m.MenuUrl,
                               MenuTitle = m.MenuTitle,
                               MenuIcon = m.MenuIcon,
                               IsView = md.IsView,
                               Status = rm != null ? true : false
                           };

                model.Add(new RoleAccessModel
                {
                    RoleId = item.RoleId,
                    RoleTitle = item.RoleTitle,
                    Menus = BuildMenuTree(data.ToList())
                });
            }

            result.Status = true;
            result.Data = model;

            return result;
        }

        public List<MenuLeftModel> BuildMenuTree(List<MenuLeftModel> menuItems, long parentId = 0)
        {
            return menuItems
                .Where(m => m.Parent == parentId)
                .Select(m => new MenuLeftModel
                {
                    MenuDetailId = m.MenuDetailId,
                    Parent = m.Parent,
                    RoleId = m.RoleId,
                    Serial = m.Serial,
                    MenuId = m.MenuId,
                    MenuUrl = m.MenuUrl,
                    MenuTitle = m.MenuTitle,
                    MenuIcon = m.MenuIcon,
                    IsView = m.IsView,
                    Status = m.Status,
                    SubMenus = BuildMenuTree(menuItems, m.MenuDetailId)
                })
                .ToList();
        }

        public void ExtractMenu(MenuDetailDeleteApiReq menuItems)
        {
            menuLeftModels.Add(new MenuLeftModel
            {
                MenuDetailId = menuItems.Parent.MenuDetailId
            });


            foreach (var item in menuItems.Child)
            {
                //ExtractMenu(item);
            }

        }
    }
}
