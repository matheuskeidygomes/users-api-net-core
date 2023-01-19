using Microsoft.EntityFrameworkCore;
using API_Rest_ASP_Core.Repositories.Interfaces;
using API_Rest_ASP_Core.Models;
using API_Rest_ASP_Core.Data;
using API_Rest_ASP_Core.Models.Enums;

namespace API_Rest_ASP_Core.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;            // dependeny inject of database context
        } 

        public Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Task.FromResult(product);
        }
        
        public async Task<List<Product>> FindAll()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return products;
        }
        
        public async Task<Product> FindById(int id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null) return product;
            else throw new System.NotImplementedException();
        }

        public async Task<Product> Update(Product product, int productId, int userId)
        {
            Product produto = await FindById(productId);
            if (produto != null && produto.UserId == userId)
            {
                produto.Name = product.Name;
                produto.Description = product.Description;
                produto.Available = Enum.Parse<AvailableEnum>(product.Available.ToString());
                _context.Products.Update(product);
                _context.SaveChanges();
                return produto;
            }
            else throw new System.NotImplementedException();
        }
        
        public async void Delete(int userId, int productId)
        {
            Product product = await FindById(productId);
            if (product != null && product.UserId == userId)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            else throw new System.NotImplementedException();
        }
    }
}
