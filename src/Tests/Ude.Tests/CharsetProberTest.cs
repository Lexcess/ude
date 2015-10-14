namespace Ude.Tests
{
    using System;
    using NUnit.Framework;
    using Ude.Core;

    [TestFixture]
    public class CharsetProberTest
    {
        [Test]
        public void TestFilterWithEnglishLetter()
        {
            byte[] buf = { 0xBF, 0x68, 0x21, 0x21, 0x65, 0x6C, 0x6F, 0x21, 0x21 };
            DummyCharsetProber p = new DummyCharsetProber();
            p.TestFilterWithEnglishLetter(buf, 0, buf.Length);
        }

        [Test]
        public void TestFilterWithoutEnglishLetter()
        {
            byte[] buf = { 0xEE, 0x21, 0x6C, 0x21, 0xEE, 0x6C, 0x6C };
            DummyCharsetProber p = new DummyCharsetProber();
            p.TestFilterWithoutEnglishLetter(buf, 0, buf.Length);
        }
    }
}