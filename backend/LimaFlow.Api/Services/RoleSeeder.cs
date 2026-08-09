using Microsoft.AspNetCore.Identity;
using LimaFlow.Api.Models;

namespace LimaFlow.Api.Services;

/// <summary>Garantiza que los roles Ciudadano y Administrador existan al arrancar.</summary>
public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var rol in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }
    }
}
