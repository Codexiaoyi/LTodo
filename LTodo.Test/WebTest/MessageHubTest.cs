using LTodo.Web;
using LTodo.Web.IRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace LTodo.Test.WebTest
{
    [TestClass]
    public class MessageHubTest
    {
        [TestMethod]
        public async Task Connect_Test()
        {
            var mockUserrepo = new Mock<IUserRepository>();
            var mockTaskrepo = new Mock<ITaskRepository>();
            var messageHub = new MessageHub(mockUserrepo.Object, mockTaskrepo.Object);

        }
    }
}
