using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_cw1_maria;

namespace UnitTestSE
{
    [TestClass]
    public class MessageTest
    {
        Message message = new Message();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IDFail()
        {
            message.id = "hello";
        }

        [TestMethod]
        public void IDPass()
        {
            message.id = "s123456789";
            string expected = "S123456789";

            Assert.AreEqual(expected, message.id);
        }
        
        [TestMethod]
        public void BodyPass()
        {
            message.body = "this is the body";
            string expected = "this is the body";

            Assert.AreEqual(expected, message.body);
        }
    }
}
