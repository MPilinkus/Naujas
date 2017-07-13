using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Main.Models;

namespace Main.Models
{
    public class MainContext : DbContext
    {
        public MainContext (DbContextOptions<MainContext> options)
            : base(options)
        {
        }

        public DbSet<Main.Models.Worker> Worker { get; set; }
        public DbSet<Main.Models.BirthdayNotification> BirthdayNotifications { get; set; }
    }
}
