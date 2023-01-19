using API_Rest_ASP_Core.Models;

namespace API_Rest_ASP_Core.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> Create(Product product); 
        Task<Product> FindById(int id);
        Task<List<Product>> FindAll();
        Task<Product> Update(Product product, int productId, int userId);
        void Delete(int productId, int userId);
    }
}
