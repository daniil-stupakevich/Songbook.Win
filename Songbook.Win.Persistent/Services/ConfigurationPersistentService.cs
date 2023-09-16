using Dapper;
using Microsoft.Data.Sqlite;
using Songbook.Win.Core.Models.Domain;
using Songbook.Win.Persistent.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songbook.Win.Persistent.Services
{
    public class ConfigurationPersistentService : BasePersistentService, IConfigurationPersistentService
    {
        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            using (var db = new SqliteConnection(ConnectionString))
            {
                return await db.QueryAsync<Language>("SELECT * FROM Languages", commandType:System.Data.CommandType.Text);
            }
        }

        public async Task SaveLanguageCollectionAsync(List<Language> languages)
        {
            string query = "INSERT INTO  Languages (Name, Code) VALUES (@Name, @Code)";
            using (var db = new SqliteConnection(ConnectionString))
            {
                foreach(var lang in languages) 
                {
                    await db.ExecuteAsync(query, new { Name = lang.Name, Code = lang.Code });
                } 
            }
        }
    }
}
