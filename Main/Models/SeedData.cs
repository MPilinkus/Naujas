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
                        FirstName = "John",
                        SecondName = "Snow",
                        BirthdayDate = DateTime.Parse("1989-01-11"),
                        WorkStartDate = DateTime.Parse("2009-04-30"),
                        Email = "John@Snow.com",
                        congratsFlag = false
                    },

                    new Worker
                    {
                        FirstName = "Tyrion",
                        SecondName = "Lanister",
                        BirthdayDate = DateTime.Parse("1990-01-11"),
                        WorkStartDate = DateTime.Parse("2010-04-30"),
                        Email = "Tyrion@Lanister.com",
                        congratsFlag = false
                    },

                    new Worker
                    {
                        FirstName = "Cersei",
                        SecondName = "Lanister",
                        BirthdayDate = DateTime.Parse("1991-01-11"),
                        WorkStartDate = DateTime.Parse("2011-04-30"),
                        Email = "martynas9x@gmail.com",
                        congratsFlag = false
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
