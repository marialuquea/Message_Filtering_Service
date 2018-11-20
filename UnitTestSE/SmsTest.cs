using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_cw1_maria;

namespace UnitTestSE
{
    [TestClass]
    public class SmsTest
    {
        Sms sms = new Sms();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CountryCodeFail()
        {
            sms.CountryCode = "12345";
        }

        [TestMethod]
        public void ContryCodePass()
        {
            sms.CountryCode = "+34";
            string expected = "+34";

            Assert.AreEqual(expected, sms.CountryCode);
        }
    }
}
