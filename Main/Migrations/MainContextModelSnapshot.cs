using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Main.Models;

namespace Main.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Main.Models.BirthdayNotification", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("FirstNotification");

                    b.Property<DateTime>("LastNotification");

                    b.Property<int?>("WorkerID");

                    b.HasKey("ID");

                    b.HasIndex("WorkerID");

                    b.ToTable("BirthdayNotifications");
                });

            modelBuilder.Entity("Main.Models.Worker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthdayDate");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("SecondName");

                    b.Property<string>("Sex");

                    b.Property<DateTime>("WorkStartDate");

                    b.Property<string>("SlackUsername");

                    b.HasKey("ID");

                    b.ToTable("Worker");
                });

            modelBuilder.Entity("Main.Models.BirthdayNotification", b =>
                {
                    b.HasOne("Main.Models.Worker", "Worker")
                        .WithMany("BirthdayNotifications")
                        .HasForeignKey("WorkerID");
                });
        }
    }
}
