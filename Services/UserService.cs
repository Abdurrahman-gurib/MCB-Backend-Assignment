using BCrypt.Net;
using MCBBackend.Models;
using System.Collections.Concurrent;

namespace MCBBackend.Services
{
    public interface IUserService
    {
        User Register(User user);
        User Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        private static ConcurrentDictionary<string, User> _users = new();

        public User Register(User user)
        {
            if (_users.ContainsKey(user.Username))
                return null;

            // Hash and salt password before storing
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _users[user.Username] = user;
            return user;
        }

        public User Authenticate(string username, string password)
        {
            if (_users.TryGetValue(username, out var user))
            {
                // Verify hashed password
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    return user;
            }
            return null;
        }
    }
}
