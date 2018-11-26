using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SE_cw1_maria;
using Data;

namespace UnitTestSE
{
    [TestClass]
    public class UITest
    {
        [TestMethod]
        public void smsprocessTest()
        {
            Abbreviations abb = new Abbreviations();

            string value = abb.main("Hello everyone LOL ASAP ROTFL bye bye crocodile.");
            string expected = "Hello everyone LOL <Laughing out loud> ASAP <As soon as possible> ROTFL <Rolling on the floor laughing> bye bye crocodile.";

            Assert.AreEqual(value, expected);
        }
    }
}
