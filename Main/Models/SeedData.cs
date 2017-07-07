using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Main.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MainContext(
                serviceProvider.GetRequiredService<DbContextOptions<MainContext>>()))
            {
                if (context.Worker.Any())
                {
                    return;
                }

                context.Worker.AddRange(
                    new Worker
                    {
                        FirstName = "Jonas",
                        SecondName = "PJonas",
                        BirthdayDate = DateTime.Parse("1989-01-11"),
                        WorkStartDate = DateTime.Parse("2009-04-30")
                    },

                    new Worker
                    {
                        FirstName = "Jonas2",
                        SecondName = "PaJonas",
                        BirthdayDate = DateTime.Parse("1990-01-11"),
                        WorkStartDate = DateTime.Parse("2010-04-30")
                    },

                    new Worker
                    {
                        FirstName = "Jonas3",
                        SecondName = "PavJonas",
                        BirthdayDate = DateTime.Parse("1991-01-11"),
                        WorkStartDate = DateTime.Parse("2011-04-30")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
