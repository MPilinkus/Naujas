using Microsoft.EntityFrameworkCore;

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
