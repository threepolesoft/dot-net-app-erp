using AppBO.DbSet.AccessControl;
using AppBO.DbSet.Inventory;
using AppBO.ModelsInv;
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
    public class UnitService
    {
        public PifErpDbContext pifErpDbContext;
        public UnitService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(UnitModel model)
        {
            return pifErpDbContext.Units
                .Any(m => m.UnitName == model.UnitName.Trim());
        }

        public ProccessResult Save(UnitModel model)
        {
            ProccessResult result = new();
            Unit Entity = new();

            if (model.UnitId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.UnitName.Trim()} already exists";
                    return result;
                }

                Entity = pifErpDbContext.Units
                   .FirstOrDefault(m => m.UnitId == model.UnitId);

                if (Entity != null)
                {
                    Entity.UnitName = model.UnitName.Trim();
                    Entity.IsDelete = model.IsDelete;
                    Entity.IsActive = model.IsActive;
                    Entity.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Unit Id {model.UnitId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.UnitName.Trim()} already exists";
                    return result;
                }

                Entity = Extention.MapObject<Unit>(model);
                Entity.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Units.Add(Entity);

            }

            if (pifErpDbContext.SaveChanges() > -1 && Entity != null)
            {
                string op = model.UnitId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<UnitModel>(Entity);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Menus),
                    RowId = model.UnitId,
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

            var Entity = pifErpDbContext.Sizes.AsQueryable();

            result.Data = Entity.Select(m => new SizeModel
            {
                SizeId = m.SizeId,
                SizeName = m.SizeName,
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
