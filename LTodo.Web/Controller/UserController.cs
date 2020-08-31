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
        public async Task<ActionResult> Login([FromBody] UserModel user)
        {
            var userModel = await userRepository.QueryByEmailAsync(user.Email);
            if (userModel == null)
            {
                return Ok(new ResponseViewModel { Code = 201, Message = "邮箱不存在" });
            }
            if (userModel.Password != user.Password)
            {
                return Ok(new ResponseViewModel { Code = 202, Message = "密码错误" });
            }
            return Ok(new ResponseViewModel { Code = 200, Message = "登录成功" });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserModel user)
        {
            var userModel = await userRepository.QueryByEmailAsync(user.Email);
            if (userModel != null)
            {
                return Ok(new ResponseViewModel { Code = 201, Message = "邮箱已存在" });
            }
            var result = await userRepository.AddAsync(user);
            if (result == 1)
            {
                return Ok(new ResponseViewModel { Code = 200, Message = "注册成功" });
            }

            return Ok(new ResponseViewModel { Code = 202, Message = "注册失败" });
        }
    }
}
