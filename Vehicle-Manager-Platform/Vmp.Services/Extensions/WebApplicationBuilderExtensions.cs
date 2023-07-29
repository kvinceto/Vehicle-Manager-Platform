namespace Vmp.Services.Extensions
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using Vmp.Data.Models;

    using static Vmp.Common.GlobalApplicationConstants;


    public static class WebApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedRoles(this IApplicationBuilder app)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<ApplicationUser> userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole<Guid>> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                {
                    return;
                }

                IdentityRole<Guid> role =
                    new IdentityRole<Guid>(AdminRoleName);

                await roleManager.CreateAsync(role);

                ApplicationUser adminUser =
                    await userManager.FindByEmailAsync("admin@admin.bg");

                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            })
            .GetAwaiter()
            .GetResult();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(UserRoleName))
                {
                    return;
                }

                IdentityRole<Guid> role =
                    new IdentityRole<Guid>(UserRoleName);

                await roleManager.CreateAsync(role);                
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }
    }
}
