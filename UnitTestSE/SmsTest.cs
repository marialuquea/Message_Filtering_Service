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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SenderFail()
        {
            sms.Sender = "1";
        }

        [TestMethod]
        public void SenderPass()
        {
            sms.Sender = "07599973293";
            string expected = "07599973293";

            Assert.AreEqual(expected, sms.Sender);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TextFail()
        {
            // 154 characters, only 140 are allowed
            string lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse et ipsum varius, porta purus at, lobortis justo. Ut mollis diam tincidunt sem feugiat";
            sms.Text = lipsum;
        }

        [TestMethod]
        public void TextPass()
        {
            sms.Text = "this is less than 140 characters so it will pass";
            string expected = "this is less than 140 characters so it will pass";

            Assert.AreEqual(expected, sms.Text);
        }
    }
}
