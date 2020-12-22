using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Http
{
    public class LoginRequest : HttpRequest
    {
        public LoginRequest(string email, string password) : base(ApiUtils.Login)
        {
            Email = email;
            Password = password;
        }

        [JsonProperty]
        public string Email { get; set; }
        [JsonProperty]
        public string Password { get; set; }
    }
}
