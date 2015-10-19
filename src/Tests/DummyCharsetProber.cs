﻿namespace Chartect.IO.Tests
{
    using System;
    using Chartect.IO.Core;
    using NUnit.Framework;

    internal class DummyCharsetProber : CharsetProber
    {
        public byte[] TestFilterWithEnglishLetter(byte[] buf, int offset, int len)
        {
            return buf.FilterWithEnglishLetters(offset, len);
        }

        public byte[] TestFilterWithoutEnglishLetter(byte[] buf, int offset, int len)
        {
            return buf.FilterWithoutEnglishLetters(offset, len);
        }

        public override float GetConfidence()
        {
            return 0.0f;
        }

        public override void Reset()
        {
        }

        public override string GetCharsetName()
        {
            return null;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            return ProbingState.Detecting;
        }
    }
}
