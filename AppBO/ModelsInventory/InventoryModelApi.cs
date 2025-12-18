using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsInventory
{
    public class BrandSaveModelApiReq
    {
        [Required(ErrorMessage = "BrandId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "BrandId must be greater than or equal to 0")]
        public long BrandId { get; set; }

        [Required(ErrorMessage = "BrandName is required")]
        public string BrandName { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class CategorySaveModelApiReq
    {
        [Required(ErrorMessage = "CategoryId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "CategoryId must be greater than or equal to 0")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "CategoryName is required")]
        public string CategoryName { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class ColorSaveModelApiReq
    {
        [Required(ErrorMessage = "ColorId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "ColorId must be greater than or equal to 0")]
        public long ColorId { get; set; }

        [Required(ErrorMessage = "ColorName is required")]
        public string ColorName { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class SizeSaveModelApiReq
    {
        [Required(ErrorMessage = "SizeId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "SizeId must be greater than or equal to 0")]
        public long ColorId { get; set; }

        [Required(ErrorMessage = "SizeName is required")]
        public string SizeName { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class UnitSaveModelApiReq
    {
        [Required(ErrorMessage = "UnitId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "UnitId must be greater than or equal to 0")]
        public long UnitId { get; set; }

        [Required(ErrorMessage = "UnitName is required")]
        public string UnitName { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}
