using Microsoft.EntityFrameworkCore;
using API_Rest_ASP_Core.Repositories.Interfaces;
using API_Rest_ASP_Core.Models;
using API_Rest_ASP_Core.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Rest_ASP_Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(DatabaseContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;            // dependeny inject of database context
            _configuration = configuration;  // dependency inject of configuration classe (allow get appsettings.json attributes)
        } 

        public Task<User> Create(User user)
        {
            user.Password = user.Password.GetHashCode().ToString();
            _context.Users.Add(user);
            _context.SaveChanges();
            return Task.FromResult(user);
        }

        // LOGIN INTO AN USER 
        public async Task<User> Login(User user)
        {
            string password = user.Password.GetHashCode().ToString();
            User usuario = await _context.Users.FirstOrDefaultAsync(p => p.Email == user.Email && p.Password.Equals(password));
            if (usuario != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key"));       // Setting the jwt key that'll be used into the new token
                var accessTokenDescriptor = new SecurityTokenDescriptor                                    // Setting the token details 
                {
                    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, usuario.Id.ToString()) }),                        // setting wich attributes will be encrypted into the token
                    Expires = DateTime.UtcNow.AddHours(2),                                                                               // setting the token expire
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)   // setting the jwt key early created
                };
                var refreshTokenDescriptor = new SecurityTokenDescriptor                                    // Setting the token details 
                {
                    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, usuario.Id.ToString()) }),                        // setting wich attributes will be encrypted into the token
                    Expires = DateTime.UtcNow.AddHours(24),                                                                               // setting the token expire
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)   // setting the jwt key early created
                };
                var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);      // Creating a new Token with tokenDescriptor attributes
                var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);    // Creating a new Token with tokenDescriptor attributes
                usuario.AcessToken = tokenHandler.WriteToken(accessToken);             // Subscribing the token into user's password 
                usuario.RefreshToken = tokenHandler.WriteToken(refreshToken);          // Subscribing the token into user's password
                _context.Users.Update(usuario);
                _context.SaveChanges();
                return usuario;

            } else
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }
        }

        // REGISTERING A NEW USER
        public async Task<User> Register(User user)
        {
            User usuario = await _context.Users.FirstOrDefaultAsync(p => p.Email == user.Email);
            if (usuario == null)                                                                     // if dont exist any user with that credential do it
            {
                // creating the new user
                user.Password = user.Password.GetHashCode().ToString();
                _context.Users.Add(user);
                _context.SaveChanges();
                
                // create a token to the new user
                var tokenHandler = new JwtSecurityTokenHandler();                                          // create a token handler
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key"));             // Setting the jwt key that'll be used into the new token
                var accessTokenDescriptor = new SecurityTokenDescriptor                                    // Setting the token details 
                {
                    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),                        // setting wich attributes will be encrypted into the token
                    Expires = DateTime.UtcNow.AddHours(2),                                                                               // setting the token expire
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)   // setting the jwt key early created
                };
                var refreshTokenDescriptor = new SecurityTokenDescriptor                                    // Setting the token details 
                {
                    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),                        // setting wich attributes will be encrypted into the token
                    Expires = DateTime.UtcNow.AddHours(24),                                                                              // setting the token expire
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)   // setting the jwt key early created
                };
                var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);      // Creating a new Access Token with tokenDescriptor attributes
                var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);    // Creating a new Refresh Token with tokenDescriptor attributes
                user.AcessToken = tokenHandler.WriteToken(accessToken);                 // Inserting the access token into user
                user.RefreshToken = tokenHandler.WriteToken(refreshToken);              // Inserting the refresh token into user

                // then updating the token attributes from the user
                _context.Update(user);
                _context.SaveChanges();
                
                // then return the new user
                return user;
            }
            else // if already exist that user
            {
                throw new UnauthorizedAccessException("Usuário já existente");    // throw new already existent user error
            }
        }   

        public async Task<List<User>> FindAll()
        {
            List<User> users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> FindById(int id)
        {
            User usuario = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (usuario != null) return usuario;
            else throw new System.NotImplementedException();
        }

        public async Task<User> Update(User user, int id)
        {
            User usuario = await FindById(id);
            if (usuario != null)
            {
                usuario.Name = user.Name;
                usuario.Email = user.Email;
                usuario.Password = user.Password.GetHashCode().ToString();
                _context.Users.Update(usuario);
                _context.SaveChanges();
                return usuario;
            }
            else throw new System.NotImplementedException();
        }
        
        public async void Delete(int id)
        {
            User usuario = await FindById(id);
            if (usuario != null)
            {
                _context.Users.Remove(usuario);
                _context.SaveChanges();
            }
            else throw new System.NotImplementedException();
        }
    }
}
