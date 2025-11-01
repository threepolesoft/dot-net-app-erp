using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.ModelsApi
{

    public class UserRollModelApiReq
    {
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "UserId must be greater than or equal to 1")]
        public long? ApplicationUserId { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        [Range(0, long.MaxValue, ErrorMessage = "RoleId must be greater than or equal to 0")]
        public long? RoleId { get; set; }
    }

}
