using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Http
{
    public class HttpRequest
    {
        public HttpRequest(string url)
        {
            Url = url;
        }

        [JsonIgnore]
        public string Url { get; set; }
    }
}
