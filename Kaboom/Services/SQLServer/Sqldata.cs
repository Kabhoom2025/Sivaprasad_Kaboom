//using Kaboom.Interfaces;
//using Kaboom.Models;
//using Kaboom.Models.AuthUserModel;
//using Kaboom.Models.Users;
//using Microsoft.EntityFrameworkCore;
//using MongoDB.Driver;

//namespace Kaboom.Services.SQLServer
//{
//    public class Sqldata
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IMongoDatabase _mongodatabase;
//        private IConfiguration _configuration;
//        private readonly IDataBaseService _dataBaseService;
//        public Sqldata(ApplicationDbContext context, IMongoClient mongoClient, IConfiguration configuration, IDataBaseService dataBaseService)
//        {
//            _context = context;
//            _mongodatabase = mongoClient.GetDatabase("MongoDbConnection");
//            _configuration = configuration;
//            _dataBaseService = dataBaseService;
//        }

//        public string RegisterSqlAuthUser(AuthUser user, string password)
//        {
//            var (hashedpassword, salt) = HashPassword(password);
//            user.PasswordHash = hashedpassword;
//            user.Salt = salt;
//            AuthUser existingUser = null;
//            existingUser = _context.AuthUser.FirstOrDefault(x => x.Email == user.Email);
//            if (existingUser != null)
//            {
//                throw new InvalidOperationException("Email is already in use. Please use a different email.");
//            }
//            //user.PasswordHash = HashPassword(password);
//            var data = new AuthUser
//            {
//                Name = user.Name,
//                Email = user.Email,
//                PasswordHash = hashedpassword,
//                Salt = salt,
//                Role = user.Role,
//                RefreshTokens = user.RefreshTokens,
//                ProfileImageUrl = user.ProfileImageUrl

//            };
//            _context.AuthUser.Add(data);
//            _context.SaveChanges();

//            // **Add to Users table after AuthUser is created**
//            var newUser = new Users
//            {
//                UserEmail = user.Email,
//                // UserName = user.Email.Split('@')[0],  // Default username from email
//                UserName = user.Name,
//                PasswordHash = hashedpassword,
//                Role = user.Role,
//                ProfileImageUrl = user.ProfileImageUrl
//            };
//            _context.Users.Add(newUser);
//            _context.SaveChanges();
//        }
//    }
//}
