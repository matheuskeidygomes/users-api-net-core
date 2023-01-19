using System.ComponentModel.DataAnnotations;

namespace API_Rest_ASP_Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? AcessToken { get; set; }

        public string? RefreshToken { get; set; }

        public User()
        {
        }

        public User (string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public User (string name, string email, string password, string token) : this(name, email, password)
        {
            Name = name;
            Email = email;
            Password = password;
            AcessToken = token;
        }
        public User(string name, string email, string password, string accessToken, string refreshToken) : this(name, email, password, accessToken)
        {
            Name = name;
            Email = email;
            Password = password;
            AcessToken = accessToken;
            RefreshToken = refreshToken;
        }

    }
}
