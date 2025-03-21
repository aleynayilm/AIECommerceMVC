﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

namespace AIECommerce.Web.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        //public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        //{
        //    RepositoryContext context = app
        //        .ApplicationServices
        //        .CreateScope()
        //        .ServiceProvider
        //        .GetRequiredService<RepositoryContext>();
        //    if (context.Database.GetPendingMigrations().Any())
        //    {
        //        context.Database.Migrate();
        //    }
        //}

        public static void ConfigureLocalization(this WebApplication app)
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("tr-TR")
                    .AddSupportedUICultures("tr-TR")
                    .SetDefaultCulture("tr-TR");
            });
        }
        public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminUser = "Admin";
            const string adminPassword = "Admin+123456";

            //UserManager
            UserManager<IdentityUser> userManager = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            //RoleManager
            RoleManager<IdentityRole> roleManager = app
                .ApplicationServices
                .CreateAsyncScope()
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            IdentityUser user= await userManager.FindByNameAsync(adminUser);
            if (user is null) {
            user= new IdentityUser()
            {
                Email="aleyna@gmail.com",
                PhoneNumber="2016252325",
                UserName=adminUser,
            };
                var result= await userManager.CreateAsync(user, adminPassword);
                if (!result.Succeeded){
                    throw new Exception("Admin user could not created");}
                    else
        {
        }

                var roleResult = await userManager.AddToRolesAsync(user,
                    roleManager
                    .Roles
                    .Select(r => r.Name)
                    .ToList()
                    );
                if (!roleResult.Succeeded){
                    throw new Exception("System have problems with role defination for admin.");}
    }
    //KULLANICININ ROLLERİNİ KONTROL ET
    var roles = await userManager.GetRolesAsync(user);
    Console.WriteLine($"Kullanıcı Rolleri: {string.Join(", ", roles)}");

    // Eğer rol atanmamışsa, ekleyelim
    if (!roles.Contains("Admin"))
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
    }
        }
}
