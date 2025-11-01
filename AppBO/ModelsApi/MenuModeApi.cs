using AppBO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsApi
{

    public class MenuSaveModelApiReq
    {
        [Required(ErrorMessage = "MenuId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "MenuId must be greater than or equal to 0")]
        public long? MenuId { get; set; }
        public string? MenuUrl { get; set; } = "";
        public string? MenuIcon { get; set; } = "";

        [Required(ErrorMessage = "MenuTitle is required")]
        public string? MenuTitle { get; set; }

        [Required(ErrorMessage = "Scope is required")]
        public string Scope { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class MenuDetailSaveModelApiReq
    {
        [Required(ErrorMessage = "MenuDetailId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "MenuDetailId must be >= 0")]
        public long MenuDetailId { get; set; }

        [Required(ErrorMessage = "MenuId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "MenuId must be >= 1")]
        public long MenuId { get; set; }

        [Required(ErrorMessage = "Parent is required")]
        [Range(0, long.MaxValue, ErrorMessage = "Parent must be >= 0")]
        public int Parent { get; set; }

        [Required(ErrorMessage = "Serial is required")]
        public int Serial { get; set; }
        public bool IsView { get; set; } = true;
        public bool? IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }

    public class MenuDetailDeleteApiReq
    {
        public MenuLeftModel Parent { get; set; }
        public List<MenuLeftModel> Child { get; set; } = new List<MenuLeftModel>();
    }

    public class RoleMenuApiReq
    {
        [Required(ErrorMessage = "RoleId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "RoleId must be >= 1")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "MenuId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "MenuId must be >= 1")]
        public long MenuDetailId { get; set; }

        public bool Status { get; set; }
    }
}
