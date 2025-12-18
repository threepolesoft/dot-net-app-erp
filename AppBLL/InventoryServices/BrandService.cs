using AppBO.DbSet.AccessControl;
using AppBO.DbSet.Inventory;
using AppBO.Models;
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
    public class BrandService
    {
        public PifErpDbContext pifErpDbContext;
        public BrandService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(BrandModel model)
        {
            return pifErpDbContext.Brands
                .Any(m => m.BrandName == model.BrandName.Trim());
        }

        public ProccessResult Save(BrandModel model)
        {
            ProccessResult result = new();
            Brand Entity = new();

            if (model.BrandId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.BrandName.Trim()} already exists";
                    return result;
                }

                Entity = pifErpDbContext.Brands
                   .FirstOrDefault(m => m.BrandId == model.BrandId);

                if (Entity != null)
                {
                    Entity.BrandName = model.BrandName.Trim();
                    Entity.IsDelete = model.IsDelete;
                    Entity.IsActive = model.IsActive;
                    Entity.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Brand Id {model.BrandId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.BrandName.Trim()} already exists";
                    return result;
                }

                Entity = Extention.MapObject<Brand>(model);
                Entity.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Brands.Add(Entity);

            }

            if (pifErpDbContext.SaveChanges() > -1 && Entity != null)
            {
                string op = model.BrandId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<BrandModel>(Entity);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Menus),
                    RowId = model.BrandId,
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

            var Entity = pifErpDbContext.Brands.AsQueryable();

            result.Data = Entity.Select(m => new BrandModel
            {
                BrandId = m.BrandId,
                BrandName = m.BrandName,
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
