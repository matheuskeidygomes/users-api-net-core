using Microsoft.EntityFrameworkCore;
using API_Rest_ASP_Core.Models;
using API_Rest_ASP_Core.Data.Maps;

namespace API_Rest_ASP_Core.Data
{
    public class DatabaseContext : DbContext                 // Herdando de DbContext
    {

        public DbSet<User> Users { get; set; }    // DbSet é uma classe do Entity Framework Core que representa uma tabela do banco de dados, ou seja, uma tabela do banco de dados é representada por uma propriedade do tipo DbSet
        public DbSet<Product> Products { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}   // Construtor da classe responsável por receber as configurações do database
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());    // Aplicando as configurações do mapeamento da classe UserMap
            modelBuilder.ApplyConfiguration(new ProductMap()); // Aplicando as configurações do mapeamento da classe ProductMap
            base.OnModelCreating(modelBuilder);
        }

    }
}
