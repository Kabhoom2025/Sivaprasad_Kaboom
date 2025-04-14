using Kaboom.Models.Order;
using Kaboom.Models.Users;

namespace Kaboom.Interfaces
{
    public interface IUserService
    {
        List<Users> GetAllUsers();
        Users GetUserById(int id);
        Users GetUserByEmail(string email);
        Users Registeruser(Users user);
        Users UpdateUser(int id, Users user);
        bool ValidateUser(string email, string password);
        List<Orders> GetUserOrderHistory(int userId);
    }
}
