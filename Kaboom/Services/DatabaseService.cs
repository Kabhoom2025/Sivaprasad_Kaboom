using Kaboom.Interfaces;
using Kaboom.Models.Order;
using Kaboom.Models.product;
using Kaboom.Models.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kaboom.Services
{
    public class DatabaseService : IDataBaseService
    {
        private string _currentDatabaseProvider = "SqlServerProvider";
        public string GetCurrentDatabaseProvider()
        {
            return _currentDatabaseProvider;
        }

        public void SetDatabaseProvider(string provider)
        {
            _currentDatabaseProvider = provider;
        }
    }
}
