using Microsoft.EntityFrameworkCore;

namespace Thesis_backend.Data_Structures
{
    public interface IDataTable<T>
    {
        public abstract Task<List<T>> Select(Predicate<T> predicate);

        public Task<List<T>> AllAsync { get; }

        public Task<T?> Get(long ID);

        public Task<bool> Create(T instance);

        public Task<bool> Update(T instance);

        public Task<bool> Delete(long ID);
    }

    public class DataTable<T> : IDataTable<T> where T : DbElement
    {
        public DbSet<T> All { get; private set; }

        private DbContext context;
        public Task<List<T>> AllAsync => All.ToListAsync();

        public DataTable(DbContext context, DbSet<T> dbSet)
        {
            this.context = context;
            All = dbSet;
        }

        public async Task<List<T>> Select(Predicate<T> predicate)
        {
            return (await AllAsync).FindAll(predicate);
        }

        public async Task<T?> Get(long ID)
        {
            return await All.FindAsync(ID);
        }

        public async Task<bool> Create(T instance)
        {
            var existing = await All.FindAsync(instance.ID);
            if (existing != null)
            {
                return false;
            }

            All.Add(instance);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(T instance)
        {
            T? current = await All.FindAsync(instance.ID);
            if (current is null)
            {
                return false;
            }
            All.Update(instance);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(long ID)
        {
            T? instance = await Get(ID);
            if (instance is null)
            {
                return false;
            }

            All.Remove(instance);
            await context.SaveChangesAsync();
            return true;
        }
    }
}