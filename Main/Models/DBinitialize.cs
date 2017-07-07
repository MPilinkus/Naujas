using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Main.Models
{
    public static class DBinitialize
    {
        public static void EnsureCreated(IServiceProvider serviceProvider)
        {
            var context = new MainContext(
                serviceProvider.GetRequiredService<DbContextOptions<MainContext>>());
            context.Database.EnsureCreated();
        }
    }
}