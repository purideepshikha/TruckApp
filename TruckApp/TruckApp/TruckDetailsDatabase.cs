using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TruckApp
{
    public class TruckDetailsDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<TruckDetailsDatabase> Instance = new AsyncLazy<TruckDetailsDatabase>(async () =>
        {
            var instance = new TruckDetailsDatabase();
            CreateTableResult result = await Database.CreateTableAsync<TruckDetails>();
            return instance;
        });

        public TruckDetailsDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<TruckDetails>> GetItemsAsync()
        {
            return Database.Table<TruckDetails>().ToListAsync();
        }

        public Task<List<TruckDetails>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<TruckDetails>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<TruckDetails> GetItemAsync(int id)
        {
            return Database.Table<TruckDetails>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(TruckDetails item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TruckDetails item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
