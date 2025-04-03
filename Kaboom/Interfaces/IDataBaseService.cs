using Kaboom.Models.Order;
using Kaboom.Models.product;
using Kaboom.Models.Users;
namespace Kaboom.Interfaces
{
    public interface IDataBaseService
    {
        void SetDatabaseProvider(string provider);
        string GetCurrentDatabaseProvider();

    }
}
