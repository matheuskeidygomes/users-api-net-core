using API_Rest_ASP_Core.Models;

namespace API_Rest_ASP_Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> Create(User user); 
        Task<User> Login(User user);
        Task<User> Register(User user); 
        Task<User> FindById(int id);
        Task<List<User>> FindAll();
        Task<User> Update(User user, int id);
        void Delete(int id);
    }
}
