using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Rest_ASP_Core.Models;
using API_Rest_ASP_Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using API_Rest_ASP_Core.Repositories.Interfaces;

namespace API_Rest_ASP_Core.Controllers
{
    [Route("api/[controller]")]                         // Especificando a rota da API
    [ApiController]                                     // Especificando que é um controller
    public class ProductController : ControllerBase       // O controller deve herdar de ControllerBase
    {
        private readonly IProductRepository productRepository;    // Injeção de dependência

        public ProductController(IProductRepository productRepository) // Injeção de dependência
        {
            this.productRepository = productRepository;
        }

        [Authorize]
        [HttpGet]                                           // Especificando o método HTTP
        async public Task<ActionResult<List<Product>>> GetProducts()                 // O método deve retornar um ActionResult contendo uma Lista de Usuários 
        {
            List<Product> products = await productRepository.FindAll();              // Retornando um Ok (status http 200) com uma string
            return Ok(products);
        }
        
        [Authorize]
        [HttpGet("{id}")]                              
        async public Task<ActionResult<Product>> GetProduct(int id)          
        {
            Product product = await productRepository.FindById(id);                  // Retornando um Ok (status http 200) com uma string
            return Ok(product);                                
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product )
        {
            var token = new JwtSecurityToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            int userId = int.Parse(token.Claims.First(c => c.Type == "unique_name").Value);
            product.UserId = userId;
            Product produto = await productRepository.Create(product);
            return Ok(produto);
        }

        [Authorize]
        [HttpPut("{productId}")]
        public async Task<ActionResult<Product>> PutProduct([FromBody] Product product, int productId)
        {
            var token = new JwtSecurityToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            int userId = int.Parse(token.Claims.First(c => c.Type == "unique_name").Value);
            Product produto = await productRepository.Update(product, productId, userId);
            return Ok(produto);
        }

        // only authorized Bearer jwt token in authorization can acess next router
        [Authorize]
        [HttpDelete("{id}")]
        public void DeleteProduct(int productId)      
        {
            var token = new JwtSecurityToken(Request.Headers["Authorization"].ToString().Split(" ")[1]);
            int userId = int.Parse(token.Claims.First(c => c.Type == "unique_name").Value);
            productRepository.Delete(productId, userId);
        }

    }
}
