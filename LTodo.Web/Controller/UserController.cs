using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LTodo.Web.IRepository;
using LTodo.Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTodo.Web.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Post([FromBody] UserModel user)
        {
            var result = await userRepository.LoginAsync(user);
            return Ok(result);
        }
    }
}
