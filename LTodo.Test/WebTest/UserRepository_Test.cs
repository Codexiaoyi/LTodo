using LTodo.Web.IRepository;
using LTodo.Web.Model;
using LTodo.Web.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTodo.Test.WebTest
{
    [TestClass]
    public class UserRepository_Test
    {
        private IUserRepository userRepo;
        private const String Forever_USER_EMAIL = "Forever@qq.com";
        private const String TEMP_USER_EMAIL1 = "test1@qq.com";

        [TestInitialize]
        public async Task Init_User_Test()
        {
            var context = new Mock<LDbContext>();
            userRepo = new UserRepository(context.Object);
            var user = new UserModel()
            {
                Email = Forever_USER_EMAIL,
                Password = "xxx",
                Platform = Platform.Android
            };
            await userRepo.AddAsync(user);
        }

        [TestMethod]
        public async Task Add_User_Test()
        {
            var user = new UserModel()
            {
                Email = TEMP_USER_EMAIL1,
                Password = "xxx",
                Platform = Platform.Android
            };
            var result1 = await userRepo.AddAsync(user);
            Assert.AreEqual(1, result1);
        }

        [TestMethod]
        public async Task Get_User_Test()
        {
            var result1 = await userRepo.QueryByEmailAsync(Forever_USER_EMAIL);
            Assert.IsNotNull(result1);
            var result2 = await userRepo.QueryByEmailAsync("Never@qq.com");
            Assert.IsNull(result2);
            var result3 = await userRepo.QueryByIdAsync(result1.Id);
            Assert.IsNotNull(result3);
        }

        [TestMethod]
        public async Task Update_User_Test()
        {
            var user = await userRepo.QueryByEmailAsync(Forever_USER_EMAIL);
            Assert.AreEqual("xxx", user.Password);
            Assert.AreEqual(Platform.Android, user.Platform);
            user.Password = "aaa";
            user.Platform = Platform.Windows;
            var result = await userRepo.UpdateAsync(user);
            Assert.AreEqual(1, result);
            var newUser = await userRepo.QueryByEmailAsync(Forever_USER_EMAIL);
            Assert.AreEqual("aaa", newUser.Password);
            Assert.AreEqual(Platform.Windows, newUser.Platform);
        }

        [TestMethod]
        public async Task Delete_User_Test()
        {
            var user = await userRepo.QueryByEmailAsync(Forever_USER_EMAIL);
            var result = await userRepo.DeleteByIdAsync(user.Id);
            Assert.IsTrue(result);
        }

        [TestCleanup]
        public async Task Destroy_User_Test()
        {
            var foreverUser = await userRepo.QueryByEmailAsync(Forever_USER_EMAIL);
            if (foreverUser != null)
            {
                await userRepo.DeleteByIdAsync(foreverUser.Id);
            }
            var testUser = await userRepo.QueryByEmailAsync(TEMP_USER_EMAIL1);
            if (testUser != null)
            {
                await userRepo.DeleteByIdAsync(testUser.Id);
            }
        }
    }
}
