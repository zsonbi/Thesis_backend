using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Thesis_backend.Data_Structures
{
    public class ThesisDbContext : DbContext
    {
        public DbSet<User> UsersTable { get; set; }
        public DbSet<Task> TasksTable { get; set; }
        public DbSet<Game> GamesTable { get; set; }
        public DbSet<UserSettings> UserSettingsTable { get; set; }
        public DbSet<Friend> FriendsTable { get; set; }

        public DataTable<User> Users { get; set; }
        public DataTable<Task> Tasks { get; set; }

        public DataTable<Friend> Friends { get; set; }


        public DataTable<UserSettings> UserSettings { get; set; }

        public ThesisDbContext(DbContextOptions<ThesisDbContext> options)
        : base(options)
        {
            Users = new DataTable<User>(this, UsersTable!);
            Tasks = new DataTable<Task>(this, TasksTable!);
            UserSettings = new DataTable<UserSettings>(this, UserSettingsTable!);
            Friends = new DataTable<Friend>(this, FriendsTable!);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasMany<Task>(u => u.UserTasks)
                      .WithOne(t => t.TaskOwner);

                entity.HasOne<UserSettings>(us => us.UserSettings)
                .WithOne(t => t.User)
                .HasForeignKey<UserSettings>(fk => fk.UserId);

                entity.HasIndex(u => u.Username)
                      .IsUnique();

                entity.HasIndex(u => u.Email)
                      .IsUnique();
            });

            modelBuilder.Entity<Task>(entity =>
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

            modelBuilder.Entity<Friend>(entity => { 
                entity.HasKey(e => e.ID);
                entity.HasOne<User>(u=>u.Sender);
                entity.HasOne<User>(u=>u.Reciever);
          
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
            if (typeof(T) == typeof(Task)) return (IDataTable<T>)Tasks;
            if (typeof(T) == typeof(UserSettings)) return (IDataTable<T>)UserSettings;

            throw new KeyNotFoundException("No such table is in the db");
        }
    }
}