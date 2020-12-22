using System.ComponentModel.DataAnnotations;

namespace LTodo.Common
{
    public class UserRequestDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
