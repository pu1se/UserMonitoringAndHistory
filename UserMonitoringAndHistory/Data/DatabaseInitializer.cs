﻿using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using UserMonitoringAndHistory.Data.Enums;
using static System.Formats.Asn1.AsnWriter;

namespace UserMonitoringAndHistory.Data
{
    public static class TestData
    {
        public static readonly string AdminUserEmail = "admin@admin.com";
        public static readonly string AdminUserName = "admin@admin.com";
        public static readonly string AdminUserPassword = "admin@admin.com";
        public static readonly string AdminUserId = "1118aa58-c48e-4696-98cf-1b6e22c63076";
    }

    public static class DatabaseInitializer
    {
        public static void Seed(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            dbContext.Database.Migrate();

            var dbWasInitialized = dbContext.Roles.Any();
            if (dbWasInitialized)
            {
                return;
            }

            roleManager.CreateAsync(new IdentityRole(UserRoleType.Admin.ToString())).Wait();

            var adminUser = new ApplicationUser
            {
                Id = TestData.AdminUserId,
                UserName = TestData.AdminUserName,
                Email = TestData.AdminUserEmail,
                ConcurrencyStamp = DateTime.UtcNow.ToLongTimeString(),
                EmailConfirmed = true,
            };

            var result = userManager.CreateAsync(adminUser, TestData.AdminUserPassword).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, UserRoleType.Admin.ToString()).Wait();
            }
        }
    }
}
