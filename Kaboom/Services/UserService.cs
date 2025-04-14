using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Models.AuthUserModel;
using Kaboom.Models.Order;
using Kaboom.Models.Users;

using MongoDB.Driver;

namespace Kaboom.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataBaseService _dataBaseService;
        private readonly IMongoDatabase _mongoDB;
        private readonly IAuthService _authService;
        public UserService(ApplicationDbContext context, IDataBaseService dataBaseService, IMongoClient mongoClient, IAuthService authService)
        {
            _context = context;
            _dataBaseService = dataBaseService;
            _mongoDB = mongoClient.GetDatabase("MongoDbConnection"); ;
            _authService = authService;
        }

        public List<Users> GetAllUsers()
        {
            var data = _context.Users.ToList();
            return data;
        }

        public Users GetUserByEmail(string email)
        {
            if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                return _context.Users.FirstOrDefault(x => x.UserEmail == email) ?? throw new Exception("Email not Found");
            }
            else if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.MongoDb)
            {
                var collection = _mongoDB.GetCollection<Users>("Users");
                return collection.Find(x => x.UserEmail == email).FirstOrDefault() ?? throw new Exception("Email not Found");
            }
            throw new Exception("Invalid Database Provider");
        }
        public Users GetUserById(int id)
        {
            if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                return _context.Users.FirstOrDefault(x => x.Id == id) ?? throw new Exception("User not Found");
            }
            else if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.MongoDb)
            {
                var collection = _mongoDB.GetCollection<Users>("Users");
                return collection.Find(x => x.Id == id).FirstOrDefault() ?? throw new Exception("User not Found");
            }
            throw new Exception("Invalid Database Provider");
        }

        public List<Orders> GetUserOrderHistory(int userId)
        {
            if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                return _context.Orders.Where(o => o.UserId == userId).ToList();
            }
            else if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                var collection = _mongoDB.GetCollection<Orders>("Orders");
                return collection.Find(o => o.UserId == userId).ToList();
            }
            throw new Exception("Invalid Database Provider");
        }

        public Users Registeruser(Users user)
        {
            var existinguser = _context.Users.FirstOrDefault(x => x.UserEmail == user.UserEmail);
            if (existinguser != null)
            {
                throw new Exception("Email already exist");
            }


            if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                var auth = new AuthUser
                {
                    Name = user.UserName,
                    Email = user.UserEmail,
                    PasswordHash = user.PasswordHash,
                    Role = "User",
                    ProfileImageUrl = user.ProfileImageUrl
                };
                var authregister = _authService.RegisterUser(auth, user.PasswordHash);
                if (authregister == null)
                {
                    throw new Exception("Invalid credentials");
                }

                return user;
            }
            else if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.MongoDb)
            {
                var collection = _mongoDB.GetCollection<Users>("Users");
                var existing = collection.Find(u => u.UserEmail == user.UserEmail).FirstOrDefault();
                if (existing != null)
                    throw new Exception("User already exists");
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                collection.InsertOne(user);
                return user;
            }
            return user;

        }

        public Users UpdateUser(int id, Users Updateuser)
        {
            bool emailUpdated = false;
            bool passwordUpdated = false;
            string? newPasswordHash = null;
            if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.Sqlserver)
            {
                var users = _context.Users.FirstOrDefault(u => u.Id == id);
                if (users == null)
                {
                    throw new Exception("User not Found");
                }

                if (!string.IsNullOrEmpty(Updateuser.UserName))
                {
                    users.UserName = Updateuser.UserName;
                }
                if (!string.IsNullOrEmpty(Updateuser.UserEmail))
                {
                    var emailexists = _context.Users.Any(x => x.UserEmail == Updateuser.UserEmail && x.Id == id);
                    if (emailexists)
                    {
                        throw new Exception("Email is already associated with another account");

                    }
                    users.UserEmail = Updateuser.UserEmail;
                    emailUpdated = true;
                }
                if (!string.IsNullOrWhiteSpace(Updateuser.PasswordHash))
                {
                    users.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Updateuser.PasswordHash);
                    passwordUpdated = true;
                }
                if (!string.IsNullOrEmpty(Updateuser.Role))
                {
                    users.Role = "User";
                }

                if (!string.IsNullOrEmpty(Updateuser.ProfileImageUrl))
                {
                    users.ProfileImageUrl = Updateuser.ProfileImageUrl;
                }
                _context.Update(users);
                _context.SaveChanges();
                var auth = new AuthUser
                {
                    Name = users.UserName,
                    Email = users.UserEmail,
                    PasswordHash = users.PasswordHash,
                    Role = users.Role,
                    ProfileImageUrl = users.ProfileImageUrl,

                };
                if (emailUpdated || passwordUpdated)
                {
                    _authService.UpdateAuthUser(id, auth);
                }
                return users;
            }
            else if (_dataBaseService.GetCurrentDatabaseProvider() == DatabaseProviders.MongoDb)
            {

            }
            throw new Exception("Invalid database provide configuration");
        }

        public bool ValidateUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
                return false;
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
