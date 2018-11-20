using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_cw1_maria;

namespace UnitTestSE
{
    [TestClass]
    public class EmailTest
    {
        Email email = new Email();

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SenderFail()
        {
            email.Sender = "this is not an email";
        }

        [TestMethod]
        public void SenderPass()
        {
            email.Sender = "mluqueanguita@gmail.com";
            string expected = "mluqueanguita@gmail.com";

            Assert.AreEqual(expected, email.Sender);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SubjectFail()
        {
            email.Subject = "subject longer than twenty one characters";
        }

        [TestMethod]
        public void SubjectPass()
        {
            email.Subject = "subject < 21 chars";
            string expected = "subject < 21 chars";

            Assert.AreEqual(expected, email.Subject);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TextFail()
        {
            // 1031 characters
            string lipsum1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget gravida libero. Quisque egestas vestibulum risus quis consequat. Aliquam quis pulvinar dolor. Etiam sed posuere leo. Sed vel nunc quam. Phasellus tellus neque, laoreet id vehicula in, scelerisque ut enim. Maecenas euismod libero ac pellentesque commodo. Proin ut finibus turpis. Nunc pharetra sit amet ipsum vitae aliquet. Curabitur sodales dapibus faucibus. Nunc pretium placerat erat, ut viverra urna vulputate vel. Aliquam dignissim pharetra est, et dictum ex egestas ut. Nullam feugiat hendrerit sapien in luctus. Curabitur eu felis condimentum, feugiat elit et, suscipit mauris. Phasellus sed accumsan erat. Phasellus elementum lacus quis eros tincidunt, vel imperdiet elit finibus. Aenean a purus massa. Proin nisi urna, vehicula quis mi eu, egestas consectetur mi. Maecenas convallis feugiat euismod. Curabitur malesuada, ex et hendrerit eleifend, dui elit vehicula magna, at mattis erat enim non dolor. Sed vestibulum fermentum augue, vel posuere urn";
            email.Text = lipsum1;
        }

        [TestMethod]
        public void TextPass()
        {
            email.Text = "subject less than 1029 chars";
            string expected = "subject less than 1029 chars";

            Assert.AreEqual(expected, email.Text);
        }
    }
}
