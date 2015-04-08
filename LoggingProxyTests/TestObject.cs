using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggingProxyTests
{
    public class TestObject : ITestObject
    {
        public void TestMethod(){}
        public void ExceptionMethod()
        {
            throw new NullReferenceException();
        }
        public void TestMethod(int testItem){}
    }
}
