using System;
using FakeItEasy;
using log4net;
using LoggingProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoggingProxyTests
{
    [TestClass]
    public class CreatingObjectsWithFactory
    {
        [TestMethod]
        public void ObtainObjectsOfExpectedType()
        {
            Assert.IsInstanceOfType(
                LoggingProxyFactory.Create<ITestObject>(_ =>
                {
                    _.CreateFunction(() => new TestObject());
                })
                , typeof(ITestObject));
        }

        [TestMethod]
        public void LogEveryOperation()
        {
            var logger = A.Fake<ILog>();

            var sut = LoggingProxyFactory.Create<ITestObject>(
                _ =>
                {
                    _.UsingLogger(logger);
                    _.CreateFunction(() => new TestObject());
                });
            sut.TestMethod();

            A.CallTo(() => logger.Info("Executing 'TestMethod' with Args ()")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void LogEveryOperationEvenWithArgs()
        {
            var logger = A.Fake<ILog>();

            var sut = LoggingProxyFactory.Create<ITestObject>(
                _ =>
                {
                    _.UsingLogger(logger);
                    _.CreateFunction(() => new TestObject());
                });
            sut.TestMethod(1);

            A.CallTo(() => logger.Info("Executing 'TestMethod' with Args (1)")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void LogException()
        {
            var logger = A.Fake<ILog>();

            var sut = LoggingProxyFactory.Create<ITestObject>(
                _ =>
                {
                    _.UsingLogger(logger);
                    _.CreateFunction(() => new TestObject());
                });

            sut.ExceptionMethod();
            A.CallTo(() => logger.Error("Exception Object reference not set to an instance of an object. occurred executing 'ExceptionMethod'"
                ,new NullReferenceException()))
                .MustHaveHappened(Repeated.Exactly.Once);
        }


    }
}
