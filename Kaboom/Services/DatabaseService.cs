using Kaboom.Interfaces;

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
