using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SCMM.Shared.Data.Store.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task EnsureDatabaseIsInitialisedAsync<T>(this IApplicationBuilder app) where T : DbContext
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<T>();
            await context.Database.EnsureCreatedAsync();
        }
    }
}
