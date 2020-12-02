using System.Threading.Tasks;
using LTodo.Web.Common;
using LTodo.Web.IRepository;
using LTodo.Web.Model;
using LTodo.Web.Model.Dto;
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
        public async Task<ActionResult> Login([FromBody] UserRequestDto userDto)
        {
            var user = await userRepository.QueryByEmailAsync(userDto.Email);
            if (user == null)
            {
                return Ok(new ResponseDto { Code = 201, Message = "邮箱不存在" });
            }
            if (user.Password != userDto.Password)
            {
                return Ok(new ResponseDto { Code = 202, Message = "密码错误" });
            }
            var token = JWT.GetToken(user);
            return Ok(new UserResponseDto { Code = 200, Message = "登录成功", Token = token });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRequestDto userDto)
        {
            var user = await userRepository.QueryByEmailAsync(userDto.Email);
            if (user != null)
            {
                return Ok(new ResponseDto { Code = 201, Message = "邮箱已存在" });
            }
            var userModel = new UserModel()
            {
                Email = userDto.Email,
                Password = userDto.Password
            };
            var result = await userRepository.AddAsync(userModel);
            if (result == 1)
            {
                return Ok(new ResponseDto { Code = 200, Message = "注册成功" });
            }
            return Ok(new ResponseDto { Code = 202, Message = "注册失败" });
        }
    }
}
