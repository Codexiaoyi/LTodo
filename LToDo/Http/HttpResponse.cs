using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Http
{
    public class HttpResponse
    {
        [JsonIgnore]
        public bool IsSuccessResponse { get; set; }

        [JsonIgnore]
        public int HttpStatusCode { get; set; }
    }
}
