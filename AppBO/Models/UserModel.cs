using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBO.Models
{
    public class UserModel
    {
        public long Id { get; set; }

        public string LId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? Email { get; set; }

        public DateTime EmailVerifiedAt { get; set; }

        public List<SettingModel> Settings { get; set; }

        public string Password { get; set; } = null!;

        public string ApiToken { get; set; } = null!;

        public string RememberToken { get; set; } = null!;

        public string AdUser { get; set; } = null!;

        public DateTime? LastVisitDate { get; set; }

        public int? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
