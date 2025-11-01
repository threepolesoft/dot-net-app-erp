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
    public class RoleMenuService
    {
        public PifErpDbContext pifErpDbContext;

        public RoleMenuService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        public ProccessResult Save(List<RoleMenuModel> model, long logedUser)
        {
            ProccessResult result = new();
            RoleMenu add = new();

            try
            {

                long RoleId = model.FirstOrDefault() != null ? model.FirstOrDefault().RoleId : 0;

                var oldMenu = pifErpDbContext.RoleMenus
                    .Where(m => m.RoleId == RoleId).ToList();

                pifErpDbContext.RoleMenus.RemoveRange(oldMenu);

                foreach (var item in oldMenu)
                {
                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.RoleMenus),
                        RowId = item.RoleMenuId,
                        Operation = keyValue.OperationDelete,
                        User = logedUser,
                        DateTime = DateTime.UtcNow,
                    });
                }


                pifErpDbContext.SaveChanges();

                foreach (var item in model.Where(m => m.Status).ToList())
                {
                    add = new RoleMenu
                    {
                        RoleId = item.RoleId,
                        MenuDetailId = item.MenuDetailId,
                        EntryBy = logedUser,
                        EntryAt = DateTime.UtcNow
                    };
                    pifErpDbContext.RoleMenus.Add(add);
                    pifErpDbContext.SaveChanges();

                    pifErpDbContext.Activities.Add(new Activity
                    {
                        TableName = nameof(pifErpDbContext.RoleMenus),
                        RowId = add.RoleMenuId,
                        Operation = keyValue.OperationInsert,
                        User = logedUser,
                        DateTime = DateTime.UtcNow,
                    });

                    pifErpDbContext.SaveChanges();
                }

                result.Status = true;
                result.Message = ProccessStatus.Success;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;
        }

    }
}
