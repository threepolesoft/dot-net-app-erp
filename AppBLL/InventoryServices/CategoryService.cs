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
    public class CategoryService
    {
        public PifErpDbContext pifErpDbContext;
        public CategoryService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;
        }

        private bool IsDuplicate(CategoryModel model)
        {
            return pifErpDbContext.Categories
                .Any(m => m.CategoryName == model.CategoryName.Trim());
        }

        public ProccessResult Save(CategoryModel model)
        {
            ProccessResult result = new();
            Category Entity = new();

            if (model.CategoryId > 0)
            {
                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.CategoryName.Trim()} already exists";
                    return result;
                }

                Entity = pifErpDbContext.Categories
                   .FirstOrDefault(m => m.CategoryId == model.CategoryId);

                if (Entity != null)
                {
                    Entity.CategoryName = model.CategoryName.Trim();
                    Entity.IsDelete = model.IsDelete;
                    Entity.IsActive = model.IsActive;
                    Entity.UpdatedBy = model.UpdatedBy;
                }
                else
                {
                    result.Status = false;
                    result.Message = $"Category Id {model.CategoryId} not available";
                    return result;
                }
            }
            else
            {

                if (IsDuplicate(model) == true)
                {
                    result.Status = false;
                    result.Message = $"{model.CategoryName.Trim()} already exists";
                    return result;
                }

                Entity = Extention.MapObject<Category>(model);
                Entity.EntryAt = DateTime.UtcNow;

                pifErpDbContext.Categories.Add(Entity);

            }

            if (pifErpDbContext.SaveChanges() > -1 && Entity != null)
            {
                string op = model.CategoryId == 0 ? keyValue.OperationInsert : keyValue.OperationUpdate;

                if (model.IsDelete == true)
                {
                    op = keyValue.OperationDelete;
                }

                model = Extention.MapObject<CategoryModel>(Entity);

                pifErpDbContext.Activities.Add(new Activity
                {
                    TableName = nameof(pifErpDbContext.Menus),
                    RowId = model.CategoryId,
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

            var Entity = pifErpDbContext.Categories.AsQueryable();

            result.Data = Entity.Select(m => new CategoryModel
            {
                CategoryId = m.CategoryId,
                CategoryName = m.CategoryName,
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
