﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Thesis_backend.Data_Structures;

#nullable disable

namespace Thesis_backend.Migrations
{
    [DbContext(typeof(ThesisDbContext))]
    partial class ThesisDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Thesis_backend.Data_Structures.Friend", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<bool>("Pending")
                        .HasColumnType("tinyint(1)");

                    b.Property<long?>("ReceiverId")
                        .HasColumnType("bigint");

                    b.Property<long?>("SenderId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("ReceiverId")
                        .IsUnique();

                    b.HasIndex("SenderId")
                        .IsUnique();

                    b.ToTable("FriendsTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.Game", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<long>("Currency")
                        .HasColumnType("bigint");

                    b.Property<long>("CurrentXP")
                        .HasColumnType("bigint");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.Property<int>("NextLVLXP")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("GamesTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.Task", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<bool>("Completed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastCompleted")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PeriodRate")
                        .HasColumnType("int");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("TaskOwnerID")
                        .HasColumnType("bigint");

                    b.Property<bool>("TaskType")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("TaskOwnerID");

                    b.ToTable("TasksTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<long>("Currency")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastLoggedIn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Registered")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("TotalScore")
                        .HasColumnType("bigint");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.HasIndex("Username", "Email")
                        .IsUnique();

                    b.ToTable("UsersTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.UserSettings", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("ProfilePic")
                        .HasColumnType("longtext");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("privacy")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettingsTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.Friend", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Thesis_backend.Data_Structures.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.Task", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "TaskOwner")
                        .WithMany("UserTasks")
                        .HasForeignKey("TaskOwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskOwner");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.UserSettings", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("Thesis_backend.Data_Structures.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.User", b =>
                {
                    b.Navigation("UserSettings");

                    b.Navigation("UserTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
