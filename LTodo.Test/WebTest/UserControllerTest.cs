using LTodo.Web;
using LTodo.Web.Controller;
using LTodo.Web.IRepository;
using LTodo.Web.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LTodo.Test.WebTest
{
    [TestClass]
    public class UserControllerTest
    {
        public UserControllerTest()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());
            Client = server.CreateClient();
        }

        public HttpClient Client { get; }

        [TestMethod]
        public async Task LoginTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { Email = "874183200@qq.com", Password = "aaaaaa" }), Encoding.UTF8, MediaTypeNames.Application.Json);
            var result = await PostTest(content);
            //测试正确登录
            Assert.AreEqual(result.Code, 200);

            content = new StringContent(JsonConvert.SerializeObject(new { Email = "123456@qq.com", Password = "aaaaaa" }), Encoding.UTF8, MediaTypeNames.Application.Json);
            result = await PostTest(content);
            //测试邮箱不存在登录
            Assert.AreEqual(result.Code, 201);

            content = new StringContent(JsonConvert.SerializeObject(new { Email = "874183200@qq.com", Password = "111666" }), Encoding.UTF8, MediaTypeNames.Application.Json);
            result = await PostTest(content);
            //测试密码错误登录
            Assert.AreEqual(result.Code, 202);
        }

        public async Task<ResponseDto> PostTest(StringContent content)
        {
            var response = await Client.PostAsync("/api/user/login", content);
            //测试接口可用
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            return JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
        }
    }
}
