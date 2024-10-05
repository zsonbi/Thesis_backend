using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Thesis_backend.Data_Structures
{
    public class ThesisDbContext : DbContext
    {
        public DbSet<User> UsersTable { get; set; }
        public DbSet<PlayerTask> TasksTable { get; set; }
        public DbSet<Game> GamesTable { get; set; }
        public DbSet<UserSettings> UserSettingsTable { get; set; }
        public DbSet<Friend> FriendsTable { get; set; }
        public DbSet<Shop> ShopTable { get; set; }
        public DbSet<OwnedCar> OwnedCarsTable { get; set; }
        public DbSet<TaskHistory> TaskHistoriesTable { get; set; }
        public DbSet<GameScore> GameScoresTable { get; set; }

        public DataTable<User> Users { get; set; }
        public DataTable<Game> Games { get; set; }
        public DataTable<PlayerTask> Tasks { get; set; }
        public DataTable<Friend> Friends { get; set; }
        public DataTable<UserSettings> UserSettings { get; set; }
        public DataTable<Shop> Shop { get; set; }
        public DataTable<OwnedCar> OwnedCars { get; set; }
        public DataTable<GameScore> GameScores { get; set; }
        public DataTable<TaskHistory> TaskHistories { get; set; }

        public ThesisDbContext(DbContextOptions<ThesisDbContext> options)
        : base(options)
        {
            Users = new DataTable<User>(this, UsersTable!);
            Tasks = new DataTable<PlayerTask>(this, TasksTable!);
            UserSettings = new DataTable<UserSettings>(this, UserSettingsTable!);
            Friends = new DataTable<Friend>(this, FriendsTable!);
            Shop = new DataTable<Shop>(this, ShopTable!);
            OwnedCars = new DataTable<OwnedCar>(this, OwnedCarsTable!);
            Games = new DataTable<Game>(this, GamesTable!);
            GameScores = new DataTable<GameScore>(this, GameScoresTable!);
            TaskHistories = new DataTable<TaskHistory>(this, TaskHistoriesTable!);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasMany<PlayerTask>(u => u.UserTasks)
                      .WithOne(t => t.TaskOwner);

                entity.HasOne<UserSettings>(us => us.UserSettings)
                .WithOne(t => t.User)
                .HasForeignKey<UserSettings>(fk => fk.UserId);

                entity.HasIndex(u => u.Username)
                      .IsUnique();

                entity.HasIndex(u => u.Email)
                      .IsUnique();
                // Combined unique index on both fields
                entity.HasIndex(u => new { u.Username, u.Email })
                      .IsUnique();
                entity.HasOne(u => u.Game)
                      .WithOne(g => g.User)
                      .HasForeignKey<Game>(g => g.UserId);
            });

            modelBuilder.Entity<PlayerTask>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasOne<User>(u => u.TaskOwner)
                .WithMany(t => t.UserTasks);
            });

            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.HasOne<User>(u => u.User)
                .WithOne(t => t.UserSettings)
                .HasForeignKey<UserSettings>(fk => fk.UserId);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasOne(f => f.Sender)
                .WithMany() // Assuming a user can send many friends but no inverse navigation property in User
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Receiver)
                        .WithMany() // Assuming a user can receive many friends but no inverse navigation property in User
                        .HasForeignKey(f => f.ReceiverId)
                        .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(f => new { f.SenderId, f.ReceiverId })
                    .IsUnique();
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasMany(g => g.OwnedCars)
                      .WithOne(o => o.Game)
                      .HasForeignKey(o => o.GameId);
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasMany<OwnedCar>()
                      .WithOne(o => o.Shop)
                      .HasForeignKey(o => o.ShopId);

                entity.HasData(
                    new Shop() { ID = 1, CarType = CarType.Common, Cost = 0, ProductName = "Base car", Buyable = false },
                    new Shop() { ID = 2, CarType = CarType.Rare, Cost = 50, ProductName = "Sport white car", Buyable = true },
                    new Shop() { ID = 3, CarType = CarType.Epic, Cost = 125, ProductName = "Ferrari", Buyable = true },
                    new Shop() { ID = 4, CarType = CarType.Legendary, Cost = 400, ProductName = "Lamborghini", Buyable = true },
                    new Shop() { ID = 5, CarType = CarType.Rare, Cost = 75, ProductName = "Jeep", Buyable = true },
                    new Shop() { ID = 6, CarType = CarType.Legendary, Cost = 250, ProductName = "Rover", Buyable = true }
                    );
            });

            modelBuilder.Entity<OwnedCar>(entity =>
            {
                entity.HasIndex(goc => new { goc.GameId, goc.ShopId })
                      .IsUnique(); // Composite unique constraint
            });

            modelBuilder.Entity<GameScore>(entity =>
            {
                entity.HasOne(u => u.Owner)
                      .WithMany()
                      .HasForeignKey(o => o.OwnerId);
            });

            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasOne(u => u.Owner)
                      .WithMany()
                      .HasForeignKey(o => o.OwnerId);

                entity.HasOne(t => t.CompletedTask)
                      .WithMany()
                      .HasForeignKey(t => t.TaskId);
            });
        }

        public bool Exists()
        {
            return UsersTable.Count() > 0;
        }

        public User? Get()
        {
            if (!Exists())
            {
                return null;
            }

            return UsersTable.First();
        }

        public async Task<bool> Create(User user)
        {
            if (Exists())
            {
                return false;
            }

            user.LastLoggedIn = DateTime.UtcNow;
            user.Registered = DateTime.UtcNow;
            UsersTable.Add(user);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(User admin)
        {
            var existing = await UsersTable.FindAsync(admin.ID);
            if (existing == null)
            {
                return false;
            }

            admin.LastLoggedIn = DateTime.UtcNow;
            UsersTable.Update(admin);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete()
        {
            if (!Exists())
            {
                return false;
            }

            var entities = await UsersTable.ToListAsync();
            UsersTable.RemoveRange(entities);
            await SaveChangesAsync();
            return true;
        }

        public IDataTable<T> GetTableManager<T>() where T : DbElement
        {
            if (typeof(T) == typeof(User)) return (IDataTable<T>)Users;
            if (typeof(T) == typeof(PlayerTask)) return (IDataTable<T>)Tasks;
            if (typeof(T) == typeof(UserSettings)) return (IDataTable<T>)UserSettings;
            if (typeof(T) == typeof(Friend)) return (IDataTable<T>)Friends;
            if (typeof(T) == typeof(Shop)) return (IDataTable<T>)Shop;
            if (typeof(T) == typeof(OwnedCar)) return (IDataTable<T>)OwnedCars;
            if (typeof(T) == typeof(Game)) return (IDataTable<T>)Games;
            if (typeof(T) == typeof(GameScore)) return (IDataTable<T>)GameScores;
            if (typeof(T) == typeof(TaskHistory)) return (IDataTable<T>)TaskHistories;

            throw new KeyNotFoundException("No such table is in the db");
        }
    }
}