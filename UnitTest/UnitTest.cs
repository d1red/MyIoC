using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyIoCContainer;
using MyIoCContainer.Abstract;
using UnitTest.Entities;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private IContainer container;
        private IContainer containerEmit;

        [TestInitialize]
        public void Init()
        {
            container = new Container();
            containerEmit = new Container(true);
        }

        [TestMethod]
        public void CreateInstanceTest()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = container.CreateInstance<FirstCustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(FirstCustomerBLL));
        }

        [TestMethod]
        public void CreateInstance_AddTypeTest()
        {
            container.AddType(typeof(FirstCustomerBLL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = container.CreateInstance<FirstCustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(FirstCustomerBLL));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Exception_NotDependencyInCollectionTest()
        {
            container.AddType(typeof(FirstCustomerBLL));

            var customerBll = container.CreateInstance<FirstCustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(FirstCustomerBLL));
        }

        [TestMethod]
        public void EmitCreateInstanceTest()
        {
            containerEmit.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = containerEmit.CreateInstance<FirstCustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(FirstCustomerBLL));
        }

        [TestMethod]
        public void EmitActivator_CreateInstanceTest()
        {
            containerEmit.AddType(typeof(FirstCustomerBLL));
            containerEmit.AddType(typeof(Logger));
            containerEmit.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = containerEmit.CreateInstance<FirstCustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(FirstCustomerBLL));
        }
    }
}
