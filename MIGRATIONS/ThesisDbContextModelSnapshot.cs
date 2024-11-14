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

                    b.Property<long>("ReceiverId")
                        .HasColumnType("bigint");

                    b.Property<long>("SenderId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId", "ReceiverId")
                        .IsUnique();

                    b.ToTable("FriendsTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.Game", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<long>("CurrentXP")
                        .HasColumnType("bigint");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.Property<int>("NextLVLXP")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("GamesTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.GameScore", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("AchievedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("OwnerId");

                    b.ToTable("GameScoresTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.OwnedCar", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<long>("GameId")
                        .HasColumnType("bigint");

                    b.Property<long>("ShopId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("ShopId");

                    b.HasIndex("GameId", "ShopId")
                        .IsUnique();

                    b.ToTable("OwnedCarsTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.PlayerTask", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<bool>("Completed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Deleted")
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

            modelBuilder.Entity("Thesis_backend.Data_Structures.Shop", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<bool>("Buyable")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("CarType")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("ShopTable");

                    b.HasData(
                        new
                        {
                            ID = 1L,
                            Buyable = false,
                            CarType = 0,
                            Cost = 0,
                            ProductName = "Base car"
                        },
                        new
                        {
                            ID = 2L,
                            Buyable = true,
                            CarType = 1,
                            Cost = 50,
                            ProductName = "Sport white car"
                        },
                        new
                        {
                            ID = 3L,
                            Buyable = true,
                            CarType = 2,
                            Cost = 125,
                            ProductName = "Ferrari"
                        },
                        new
                        {
                            ID = 4L,
                            Buyable = true,
                            CarType = 3,
                            Cost = 400,
                            ProductName = "Lamborghini"
                        },
                        new
                        {
                            ID = 5L,
                            Buyable = true,
                            CarType = 1,
                            Cost = 75,
                            ProductName = "Jeep"
                        },
                        new
                        {
                            ID = 6L,
                            Buyable = true,
                            CarType = 2,
                            Cost = 250,
                            ProductName = "Rover"
                        },
                        new
                        {
                            ID = 7L,
                            Buyable = true,
                            CarType = 3,
                            Cost = 450,
                            ProductName = "Forma1"
                        },
                        new
                        {
                            ID = 8L,
                            Buyable = true,
                            CarType = 1,
                            Cost = 100,
                            ProductName = "Ambulance"
                        },
                        new
                        {
                            ID = 9L,
                            Buyable = true,
                            CarType = 0,
                            Cost = 25,
                            ProductName = "Taxi"
                        },
                        new
                        {
                            ID = 10L,
                            Buyable = true,
                            CarType = 0,
                            Cost = 40,
                            ProductName = "Van"
                        });
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.TaskHistory", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<DateTime>("Completed")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskHistoriesTable");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("ID"));

                    b.Property<int>("CompletedBadTasks")
                        .HasColumnType("int");

                    b.Property<int>("CompletedGoodTasks")
                        .HasColumnType("int");

                    b.Property<long>("CurrentTaskScore")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Thesis_backend.Data_Structures.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.Game", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "User")
                        .WithOne("Game")
                        .HasForeignKey("Thesis_backend.Data_Structures.Game", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.GameScore", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.OwnedCar", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.Game", "Game")
                        .WithMany("OwnedCars")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Thesis_backend.Data_Structures.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.PlayerTask", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "TaskOwner")
                        .WithMany("UserTasks")
                        .HasForeignKey("TaskOwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskOwner");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.TaskHistory", b =>
                {
                    b.HasOne("Thesis_backend.Data_Structures.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Thesis_backend.Data_Structures.PlayerTask", "CompletedTask")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompletedTask");

                    b.Navigation("Owner");
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

            modelBuilder.Entity("Thesis_backend.Data_Structures.Game", b =>
                {
                    b.Navigation("OwnedCars");
                });

            modelBuilder.Entity("Thesis_backend.Data_Structures.User", b =>
                {
                    b.Navigation("Game");

                    b.Navigation("UserSettings");

                    b.Navigation("UserTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
