using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTodo.Test.DesktopTest
{
    [TestClass]
    public class BaseTest<T> where T : class, new()
    {
        protected T _testClass;

        [TestInitialize]
        public void Init()
        {
            _testClass = new T();
        }

        [TestCleanup]
        public void Clean()
        {
            _testClass = null;
        }
    }
}
