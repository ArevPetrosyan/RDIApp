using RDIApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDIApp.Services
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<SettingsModelDB>().Wait();          
        }

        public Task<List<SettingsModelDB>> GetSettings()
        {
            return _database.Table<SettingsModelDB>().ToListAsync();
        }

        public Task DeleteItems()
        {
            _database.DeleteAllAsync<SettingsModelDB>();
            return Task.CompletedTask;
        }

        public Task InsertSettingsAsync(SettingsModelDB settings)
        {
            _database.InsertAsync(settings);
            return Task.CompletedTask;
        }
    }
}
