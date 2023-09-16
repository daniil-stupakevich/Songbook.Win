using Microsoft.Data.Sqlite;
using System.Threading;

namespace Songbook.Win.Persistent.Services
{
    public class BasePersistentService
    {
        protected SqliteConnection Connection;
        protected string ConnectionString => @"Data Source=c:\Songs;";

        public BasePersistentService() 
        {
            SQLitePCL.Batteries.Init();
        }
    }
}
