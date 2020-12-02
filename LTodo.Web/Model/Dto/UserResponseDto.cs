using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Model.Dto
{
    public class UserResponseDto : ResponseDto
    {
        public string Token { get; set; }
    }
}
