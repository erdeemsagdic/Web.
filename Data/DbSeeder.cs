using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SPORSALONUYONETIM.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SPORSALONUYONETIM.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await context.Database.MigrateAsync();

            // --------------------
            // ROLES
            // --------------------
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // --------------------
            // ADMIN USER 
            // --------------------
            var adminEmail = "b241210378@sakarya.edu.tr";
            var adminPassword = "Sau123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            { 
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, adminPassword);

                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // --------------------
            // TRAINERS
            // --------------------
            if (!context.Trainers.Any())
            {
                context.Trainers.AddRange(
                    new Trainer
                    {
                        FullName = "Emre Baş",
                        SportType = SportType.Fitness,
                        WorkStart = new TimeSpan(9, 0, 0),
                        WorkEnd = new TimeSpan(17, 0, 0)
                    },
                    new Trainer
                    {
                        FullName = "Sena Baş",
                        SportType = SportType.Yoga,
                        WorkStart = new TimeSpan(9, 0, 0),
                        WorkEnd = new TimeSpan(17, 0, 0)
                    },
                    new Trainer
                    {
                        FullName = "Ege Fitness",
                        SportType = SportType.CrossFit,
                        WorkStart = new TimeSpan(16, 0, 0),
                        WorkEnd = new TimeSpan(23, 59, 59)
                    },
                    new Trainer
                    {
                        FullName = "Gökhan Saki",
                        SportType = SportType.KickBoks,
                        WorkStart = new TimeSpan(16, 0, 0),
                        WorkEnd = new TimeSpan(23, 59, 59)
                    }
                );

                await context.SaveChangesAsync();
            }

            // --------------------
            // SERVICES
            // --------------------
            if (!context.Services.Any())
            {
                var emre = context.Trainers.First(t => t.FullName == "Emre Baş");
                var sena = context.Trainers.First(t => t.FullName == "Sena Baş");
                var ege = context.Trainers.First(t => t.FullName == "Ege Fitness");
                var gokhan = context.Trainers.First(t => t.FullName == "Gökhan Saki");

                context.Services.AddRange(
                    new Service
                    {
                        Name = "Fitness",
                        DurationMinutes = 60,
                        Price = 300,
                        TrainerId = emre.Id
                    },
                    new Service
                    {
                        Name = "Yoga",
                        DurationMinutes = 60,
                        Price = 250,
                        TrainerId = sena.Id
                    },
                    new Service
                    {
                        Name = "CrossFit",
                        DurationMinutes = 45,
                        Price = 350,
                        TrainerId = ege.Id
                    },
                    new Service
                    {
                        Name = "Kick Boks",
                        DurationMinutes = 60,
                        Price = 400,
                        TrainerId = gokhan.Id
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
