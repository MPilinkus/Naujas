using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slack.Webhooks.Core;
using Microsoft.EntityFrameworkCore;

namespace Main.Models 
{
    public class BirthdayMessage 
    {

        public static void Startup() {
            while (true)
            {
                /*using ( var context = new MainContext(
                serviceProvider.GetRequiredService<DbContextOptions<MainContext>>()) )
                {*/
                   /* var workers = from w in context.Worker
                                  select w;*/

                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    });
                    t.Wait();
                    Console.WriteLine("TEST TEST TEST");
                //}
             }
        }
    }
}
