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
    public class SizeService
    {
        public PifErpDbContext pifErpDbContext;
        public SizeService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(SizeModel model)
        {
            return pifErpDbContext.Sizes
                .Any(m => m.SizeName == model.SizeName.Trim());
        }

        public ProccessResult Save(SizeModel model)
        {
            ProccessResult result = new();
            Size Entity = new();

            if (model.SizeId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.SizeName.Trim()} already exists";
                    return result;
                }

                Entity = pifErpDbContext.Sizes
                   .FirstOrDefault(m => m.SizeId == model.SizeId);

                if (Entity != null)
                {
                    Entity.SizeName = model.SizeName.Trim();
                    Entity.IsDelete = model.IsDelete;
                    Entity.IsActive = model.IsActive;
                    Entity.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Size Id {model.SizeId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.SizeName.Trim()} already exists";
                    return result;
                }

                Entity = Extention.MapObject<Size>(model);
                Entity.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Sizes.Add(Entity);

            }

            if (pifErpDbContext.SaveChanges() > -1 && Entity != null)
            {
                string op = model.SizeId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<SizeModel>(Entity);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Menus),
                    RowId = model.SizeId,
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
