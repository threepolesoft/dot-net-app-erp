using AppBO.DbSet.AccessControl;
using AppBO.DbSet.Inventory;
using AppBO.ModelsInventory;
using AppBO.Utility;
using AppDAL.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppBO.Utility.Utility;

namespace AppBLL.InventoryServices
{
    public class ColorService
    {
        public PifErpDbContext pifErpDbContext;
        public ColorService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(ColorModel model)
        {
            return pifErpDbContext.Colors
                .Any(m => m.ColorName == model.ColorName.Trim());
        }

        public ProccessResult Save(ColorModel model)
        {
            ProccessResult result = new();
            Color Entity = new();

            if (model.ColorId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.ColorName.Trim()} already exists";
                    return result;
                }

                Entity = pifErpDbContext.Colors
                   .FirstOrDefault(m => m.ColorId == model.ColorId);

                if (Entity != null)
                {
                    Entity.ColorName = model.ColorName.Trim();
                    Entity.IsDelete = model.IsDelete;
                    Entity.IsActive = model.IsActive;
                    Entity.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Color Id {model.ColorId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.ColorName.Trim()} already exists";
                    return result;
                }

                Entity = Extention.MapObject<Color>(model);
                Entity.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Colors.Add(Entity);

            }

            if (pifErpDbContext.SaveChanges() > -1 && Entity != null)
            {
                string op = model.ColorId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<ColorModel>(Entity);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Menus),
                    RowId = model.ColorId,
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

        public ProccessResult GetBy()
        {
            ProccessResult result = new();

            var Entity = pifErpDbContext.Colors.AsQueryable();

            result.Data = Entity.Select(m => new ColorModel
            {
                ColorId = m.ColorId,
                ColorName = m.ColorName,
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
