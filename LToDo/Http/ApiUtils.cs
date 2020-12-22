using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Http
{
    public class ApiUtils
    {
        public static string BaseUrl = "http://localhost:5006";
        //public static string BaseUrl = "http://47.106.139.187:5006";
        public static string Login = $"{BaseUrl}/api/user/login";
        public static string GetTask = $"{BaseUrl}/api/task/get";
        public static string AddTask = $"{BaseUrl}/api/task/add";
        public static string UpdateTask = $"{BaseUrl}/api/task/update";
        public static string UpdateAllTasks = $"{BaseUrl}/api/task/update/all";
        public static string RemoveTask = $"{BaseUrl}/api/task/remove";
    }
}
