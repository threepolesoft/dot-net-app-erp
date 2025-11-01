using AppBO.Models;
using AppBO.Utility;
using AppDAL.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBO.DbSet.AccessControl;

namespace AppBLL.Services
{
    public class ShareService
    {
        public PifErpDbContext pifErpDbContext;

        public ShareService(
            PifErpDbContext pifErpDbContext
            )
        {
            this.pifErpDbContext = pifErpDbContext;

        }

        public SettingModel GetBySettingId(long SettingId)
        {
            SettingModel data = new SettingModel();
            data = pifErpDbContext.Settings.Where(m => m.SettingId == SettingId)
                .MapTo<Setting, SettingModel>().FirstOrDefault();
            return data;
        }

        public async Task<HashSet<long>> GetAllSettingIdsAsync()
        {
            return (await pifErpDbContext.Settings
                .Where(m => m.IsActive && !m.IsDelete) // Filter active settings
                .Select(m => m.SettingId)
                .ToListAsync())
                .ToHashSet();
        }
    }
}
