using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_cw1_maria;

namespace UnitTestSE
{
    [TestClass]
    public class TweetTest
    {
        Tweet tweet = new Tweet();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SenderFail()
        {
            tweet.Sender = "does not start with @";
        }

        [TestMethod]
        public void SenderPass()
        {
            tweet.Sender = "@maria";
            string expected = "@maria";

            Assert.AreEqual(expected, tweet.Sender);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TextFail()
        {
            // 142 characters
            string lipsum3 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget gravida libero. Quisque egestas vestibulum risus quis consequat. Alia";
            tweet.Text = lipsum3;
        }

        [TestMethod]
        public void TextPass()
        {
            tweet.Text = "less than 140 characters";
            string expected = "less than 140 characters";

            Assert.AreEqual(expected, tweet.Text);
        }
    }
}
