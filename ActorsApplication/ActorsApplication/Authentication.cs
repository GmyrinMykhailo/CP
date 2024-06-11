using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorsApplication
{
    internal class AuthenticationErrors
    {
        public static Error IncorrectCredentialsError => new Error("неверный логин или пароль", "");
        public static Error UsernameIsNullError => new Error("логин не может быть пустой", "");
        public static Error PasswordIsNullError => new Error("пароль не может быть пустой", "");
    }
    internal class Authentication
    {
        readonly public List<User> users = new List<User>()
        {
            new User("Mikhail", "Gmyrin", true, "admin", "admin"), 
            new User("John", "Hoe", false, "john", "123456"),
            new User("Maria", "Shepovalova", false, "marishep", "55555"),
            new User("Petr", "Petrovich", false, "petrpetr", "petr21")
        };
        public Error Login(string username, string password, out User recorded_user)
        {
            recorded_user = null;

            if (username == "") return AuthenticationErrors.UsernameIsNullError;
            if (password == "") return AuthenticationErrors.PasswordIsNullError;

            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    recorded_user = user;
                    return null;
                }
            }
            return AuthenticationErrors.IncorrectCredentialsError;
        }
    }
}
