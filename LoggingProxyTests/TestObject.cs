using System;

namespace LoggingProxyTests
{
    public class TestObject : ITestObject
    {
        public void TestMethod() { }
        public void ExceptionMethod()
        {
            throw new NullReferenceException();
        }
        public void TestMethod(int testItem) { }
    }
}
