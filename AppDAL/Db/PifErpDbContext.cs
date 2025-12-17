using System;
using System.Collections.Generic;
using AppBO.DbSet.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Db;

public partial class PifErpDbContext : DbContext
{
    public PifErpDbContext()
    {
    }

    public PifErpDbContext(DbContextOptions<PifErpDbContext> options)
        : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Data Source=209.195.14.7,8433;Initial Catalog=pif_erp;User ID=pifdev;Password=qbhmfairnelugjzpdc9o;Persist Security Info=True;TrustServerCertificate=True;");
    //}


    #region Access Control
    public virtual DbSet<Organization>  Organizations  { get; set; }
    public virtual DbSet<Activity> Activities { get; set; }
    public virtual DbSet<UsersDevice> UsersDevices { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }
    public virtual DbSet<SettingUser> SettingUsers { get; set; }
    public virtual DbSet<SettingDevice> SettingDevices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RoleUser> RoleUsers { get; set; }
    public virtual DbSet<RoleDevice> RoleDevices { get; set; }
    public virtual DbSet<RoleMenu> RoleMenus { get; set; }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public virtual DbSet<Menu> Menus { get; set; }
    public virtual DbSet<MenuDetail> MenuDetails { get; set; }
    public virtual DbSet<MenuUser> MenuUsers { get; set; }
    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Role>(entity =>
        {
            // Add unique constraint for Guid
            entity.Property(e => e.RoleName)
                  .IsRequired()
                  .HasMaxLength(2);
            entity.HasIndex(e => e.RoleName).IsUnique(); // Unique constraint

        });

        modelBuilder.Entity<Menu>(entity =>
        {
            // Add unique constraint for Guid
            entity.Property(e => e.MenuName)
                  .IsRequired()
                  .HasMaxLength(2);
            entity.HasIndex(e => e.MenuName).IsUnique(); // Unique constraint

        });

        modelBuilder.Entity<Setting>(entity =>
        {
            // Add unique constraint for Guid
            entity.Property(e => e.SettingName)
                  .IsRequired()
                  .HasMaxLength(2);
            entity.HasIndex(e => e.SettingName).IsUnique(); // Unique constraint

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        // Generate a random alphabetic string (only a-z)
        foreach (var entry in ChangeTracker.Entries<Menu>())
        {
            if (entry.State == EntityState.Added)
            {
                // Generate a random alphabetic string (only a-z)
                var uniqueGuid = new string(
                    Guid.NewGuid()
                        .ToString("N") // Get a 32-character string
                        .Where(char.IsLetter) // Keep only alphabetic characters
                        .Take(2) // Take the first 2 letters
                        .ToArray() // Convert to array
                ); // Convert to lowercase

                entry.Entity.MenuName = uniqueGuid;
            }
        }

        // Generate a random alphabetic string (only a-z)
        foreach (var entry in ChangeTracker.Entries<Role>())
        {
            if (entry.State == EntityState.Added)
            {
                // Generate a random alphabetic string (only a-z)
                var uniqueGuid = new string(
                    Guid.NewGuid()
                        .ToString("N") // Get a 32-character string
                        .Where(char.IsLetter) // Keep only alphabetic characters
                        .Take(2) // Take the first 2 letters
                        .ToArray() // Convert to array
                ); // Convert to lowercase

                entry.Entity.RoleName = uniqueGuid;
            }
        }

        // Generate a random alphabetic string (only a-z)
        foreach (var entry in ChangeTracker.Entries<Setting>())
        {
            if (entry.State == EntityState.Added)
            {
                // Generate a random alphabetic string (only a-z)
                var uniqueGuid = new string(
                    Guid.NewGuid()
                        .ToString("N") // Get a 32-character string
                        .Where(char.IsLetter) // Keep only alphabetic characters
                        .Take(2) // Take the first 2 letters
                        .ToArray() // Convert to array
                ); // Convert to lowercase

                entry.Entity.SettingName = uniqueGuid;
            }
        }
        return base.SaveChanges();
    }
}
