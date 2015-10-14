﻿namespace Ude.Tests
{
    using System;
    using NUnit.Framework;
    using Ude.Core;

    public class DummyCharsetProber : CharsetProber
    {
        public byte[] TestFilterWithEnglishLetter(byte[] buf, int offset, int len)
        {
            return FilterWithEnglishLetters(buf, offset, len);
        }

        public byte[] TestFilterWithoutEnglishLetter(byte[] buf, int offset, int len)
        {
            return FilterWithoutEnglishLetters(buf, offset, len);
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
