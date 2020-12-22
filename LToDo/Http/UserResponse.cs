using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Http
{
    public class LoginResponse : HttpResponse
    {
        public string Token { get; set; }
    }
}
