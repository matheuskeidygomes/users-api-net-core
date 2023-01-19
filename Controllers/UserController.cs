using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Rest_ASP_Core.Models;
using API_Rest_ASP_Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using API_Rest_ASP_Core.Repositories.Interfaces;

namespace API_Rest_ASP_Core.Controllers
{
    [Route("api/[controller]")]                         // Especificando a rota da API
    [ApiController]                                     // Especificando que é um controller
    public class UserController : ControllerBase        // O controller deve herdar de ControllerBase
    {
        private readonly IUserRepository userRepository;    // Injeção de dependência

        public UserController(IUserRepository userRepository) // Injeção de dependência
        {
            this.userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]                                                       // Especificando o método HTTP
        async public Task<ActionResult<List<User>>> GetUsers()          // O método deve retornar um ActionResult contendo uma Lista de Usuários 
        {
            List<User> usuarios = await userRepository.FindAll();                                   // Retornando um Ok (status http 200) com uma string
            return Ok(usuarios);
        }

        [Authorize]
        [HttpGet("{id}")]                              
        async public Task<ActionResult<User>> GetUser(int id)          
        {
            User usuario = await userRepository.FindById(id);                                   // Retornando um Ok (status http 200) com uma string
            return Ok(usuario);                                
        }                                                                                                                                    

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            Console.WriteLine("o nome: " + user.Name);
            User usuario = await userRepository.Create(user);
            return Ok(usuario);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User user)
        {
            User usuario = await userRepository.Login(user);
            return Ok(usuario);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            User usuario = await userRepository.Register(user);
            return Ok(usuario);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser([FromBody] User user, int id)
        {
            User usuario = await userRepository.Update(user, id);
            return Ok(usuario);
        }

        // only authorized BEarer jwt token in authorization can acess next router
        [Authorize]
        [HttpDelete("{id}")]
        public void DeleteUser(int id)      
        {
            userRepository.Delete(id);
        }

    }
}
