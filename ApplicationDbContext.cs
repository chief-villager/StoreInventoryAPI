using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using storeInventoryApi.Enum;
using storeInventoryApi.Models;

namespace storeInventoryApi
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<Products> Products { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = nameof(UserRoleTypeEnum.Admin), NormalizedName = nameof(UserRoleTypeEnum.Admin).ToUpper() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = nameof(UserRoleTypeEnum.Manager), NormalizedName = nameof(UserRoleTypeEnum.Manager).ToUpper() },
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = nameof(UserRoleTypeEnum.Cashiers), NormalizedName = nameof(UserRoleTypeEnum.Cashiers).ToUpper() });

        }
    }
}