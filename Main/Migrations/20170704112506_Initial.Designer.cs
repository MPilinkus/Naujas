﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Main.Models;

namespace Main.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20170704112506_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Main.Models.Worker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthdayDate");

                    b.Property<string>("FirstName");

                    b.Property<string>("SecondName");

                    b.Property<DateTime>("WorkStartDate");

                    b.HasKey("ID");

                    b.ToTable("Worker");
                });
        }
    }
}
