namespace Chartect.IO.Core
{
    using System;

    public enum DetectorState
    {
        Start = 0,
        GotData = 1,
        Done = 2,
    }

    internal enum InputState
    {
        PureASCII = 0,
        EscASCII = 1,
        Highbyte = 2,
    }

    public sealed class UniversalDetector
    {
        public const int FilterChineseSimplified = 1;
        public const int FilterChineseTraditional = 2;
        public const int FilterJapanese = 4;
        public const int FilterKorean = 8;
        public const int FilterNonCjk = 16;
        public const int FilterAll = 31;
        public const int FilterChinese = FilterChineseSimplified | FilterChineseTraditional;
        public const int FilterCjk = FilterJapanese | FilterKorean | FilterChineseSimplified | FilterChineseTraditional;

        private const float SHORTCUTTHRESHOLD = 0.95f;
        private const float MINIMUMTHRESHOLD = 0.20f;
        private const int ProbersNum = 3;

        private InputState inputState;
        private byte lastChar;
        private int bestGuess;
        private int languageFilter;
        private CharsetProber[] charsetProbers = new CharsetProber[ProbersNum];
        private CharsetProber escCharsetProber;
        private string detectedCharset;

        private string charset;
        private float confidence;

        public UniversalDetector(int languageFilter)
        {
            this.DetectorState = DetectorState.Start;
            this.InputState = InputState.PureASCII;
            this.LastChar = 0x00;
            this.BestGuess = -1;
            this.LanguageFilter = languageFilter;
        }

        public DetectorState DetectorState
        {
            get; private set;
        }

        public string Charset
        {
            get
            {
                return this.charset;
            }

            private set
            {
                this.charset = value;
            }
        }

        public float Confidence
        {
            get
            {
                return this.confidence;
            }

            private set
            {
                this.confidence = value;
            }
        }

        private InputState InputState
        {
            get
            {
                return this.inputState;
            }

            set
            {
                this.inputState = value;
            }
        }

        private byte LastChar
        {
            get
            {
                return this.lastChar;
            }

            set
            {
                this.lastChar = value;
            }
        }

        private int LanguageFilter
        {
            get
            {
                return this.languageFilter;
            }

            set
            {
                this.languageFilter = value;
            }
        }

        private CharsetProber EscCharsetProber
        {
            get
            {
                return this.escCharsetProber;
            }

            set
            {
                this.escCharsetProber = value;
            }
        }

        private string DetectedCharset
        {
            get
            {
                return this.detectedCharset;
            }

            set
            {
                this.detectedCharset = value;
            }
        }

        private CharsetProber[] CharsetProbers
        {
            get
            {
                return this.charsetProbers;
            }

            set
            {
                this.charsetProbers = value;
            }
        }

        private int BestGuess
        {
            get
            {
                return this.bestGuess;
            }

            set
            {
                this.bestGuess = value;
            }
        }

        /// <summary>
        /// Read a block of bytes into the detector.
        /// </summary>
        /// <param name="input">input buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="length">number of available bytes</param>
        public void Read(byte[] input, int offset, int length)
        {
            if (this.DetectorState == DetectorState.Done)
            {
                return;
            }

            // If the data starts with BOM, we know it is UTF
            if (length > 0 && this.DetectorState == DetectorState.Start)
            {
                this.DetectorState = DetectorState.GotData;
                this.DetectedCharset = this.DetectByteOrderMark(input);

                if (this.DetectedCharset != null)
                {
                    this.DetectorState = DetectorState.Done;
                    return;
                }
            }

            for (int i = 0; i < length; i++)
            {
                // other than 0xa0, if every other character is ascii, the page is ascii
                if ((input[i] & 0x80) != 0 && input[i] != 0xA0)
                {
                    // we got a non-ascii byte (high-byte)
                    if (this.InputState != InputState.Highbyte)
                    {
                        this.InputState = InputState.Highbyte;

                        // kill EscCharsetProber if it is active
                        if (this.EscCharsetProber != null)
                        {
                            this.EscCharsetProber = null;
                        }

                        // start multibyte and singlebyte charset prober
                        if (this.CharsetProbers[0] == null)
                        {
                            this.CharsetProbers[0] = new MBCSGroupProber();
                        }

                        if (this.CharsetProbers[1] == null)
                        {
                            this.CharsetProbers[1] = new SBCSGroupProber();
                        }

                        if (this.CharsetProbers[2] == null)
                        {
                            this.CharsetProbers[2] = new Latin1Prober();
                        }
                    }
                }
                else
                {
                    if (this.InputState == InputState.PureASCII &&
                        (input[i] == 0x1B || (input[i] == 0x7B && this.LastChar == 0x7E)))
                        {
                        // found escape character or HZ "~{"
                        this.InputState = InputState.EscASCII;
                    }

                    this.LastChar = input[i];
                }
            }

            ProbingState st = ProbingState.NotMe;

            switch (this.InputState)
            {
                case InputState.EscASCII:
                    if (this.EscCharsetProber == null)
                    {
                        this.EscCharsetProber = new EscCharsetProber();
                    }

                    st = this.EscCharsetProber.HandleData(input, offset, length);
                    if (st == ProbingState.FoundIt)
                    {
                        this.DetectorState = DetectorState.Done;
                        this.DetectedCharset = this.EscCharsetProber.GetCharsetName();
                    }

                    break;
                case InputState.Highbyte:
                    for (int i = 0; i < ProbersNum; i++)
                    {
                        if (this.CharsetProbers[i] != null)
                        {
                            st = this.CharsetProbers[i].HandleData(input, offset, length);
                            #if DEBUG
                            this.CharsetProbers[i].DumpStatus();
                            #endif
                            if (st == ProbingState.FoundIt)
                            {
                                this.DetectorState = DetectorState.Done;
                                this.DetectedCharset = this.CharsetProbers[i].GetCharsetName();
                                return;
                            }
                        }
                    }

                    break;
                default:
                    // pure ascii
                    break;
            }

            return;
        }

        private string DetectByteOrderMark(byte[] input)
        {
            string charset = null;
            if (input.Length > 3)
            {
                switch (input[0])
                {
                    case 0xEF:
                        if (0xBB == input[1] && 0xBF == input[2])
                        {
                            charset = Charsets.Utf8;
                        }

                        break;
                    case 0xFE:
                        if (0xFF == input[1] && 0x00 == input[2] && 0x00 == input[3])
                        {
                            // FE FF 00 00  UCS-4, unusual octet order BOM (3412)
                            charset = Charsets.UCS43412;
                        }
                        else if (0xFF == input[1])
                        {
                            charset = Charsets.UTF16BE;
                        }

                        break;
                    case 0x00:
                        if (0x00 == input[1] && 0xFE == input[2] && 0xFF == input[3])
                        {
                            charset = Charsets.UTF32BE;
                        }
                        else if (0x00 == input[1] && 0xFF == input[2] && 0xFE == input[3])
                        {
                            // 00 00 FF FE  UCS-4, unusual octet order BOM (2143)
                            charset = Charsets.UCS42413;
                        }

                        break;
                    case 0xFF:
                        if (0xFE == input[1] && 0x00 == input[2] && 0x00 == input[3])
                        {
                            charset = Charsets.UTF32LE;
                        }
                        else if (0xFE == input[1])
                        {
                            charset = Charsets.Utf16LE;
                        }

                        break;
                } // switch
            }

            return charset;
        }

        /// <summary>
        /// Tell the detector that there is no more data and it must make its
        /// decision.
        /// </summary>
        public void DataEnd()
        {
            if (this.DetectorState == DetectorState.Start)
            {
                // we haven't got any data yet, return immediately
                // caller program sometimes call DataEnd before anything has
                // been sent to detector
                return;
            }

            if (this.DetectedCharset != null)
            {
                this.DetectorState = DetectorState.Done;
                this.Report(this.DetectedCharset, 1.0f);
                return;
            }

            if (this.InputState == InputState.Highbyte)
            {
                float proberConfidence = 0.0f;
                float maxProberConfidence = 0.0f;
                int maxProber = 0;
                for (int i = 0; i < ProbersNum; i++)
                {
                    if (this.CharsetProbers[i] != null)
                    {
                        proberConfidence = this.CharsetProbers[i].GetConfidence();
                        if (proberConfidence > maxProberConfidence)
                        {
                            maxProberConfidence = proberConfidence;
                            maxProber = i;
                        }
                    }
                }

                if (maxProberConfidence > MINIMUMTHRESHOLD)
                {
                    this.Report(this.CharsetProbers[maxProber].GetCharsetName(), maxProberConfidence);
                }
            }
            else if (this.InputState == InputState.PureASCII)
            {
                this.Report(Charsets.Ascii, 1.0f);
            }
        }

        /// <summary>
        /// Clear internal state of charset detector.
        /// In the original interface this method is protected.
        /// </summary>
        public void Reset()
        {
            this.Charset = null;
            this.Confidence = 0.0f;
            this.DetectorState = DetectorState.Start;
            this.DetectedCharset = null;
            this.BestGuess = -1;
            this.InputState = InputState.PureASCII;
            this.LastChar = 0x00;
            if (this.EscCharsetProber != null)
            {
                this.EscCharsetProber.Reset();
            }

            for (int i = 0; i < ProbersNum; i++)
            {
                if (this.CharsetProbers[i] != null)
                {
                    this.CharsetProbers[i].Reset();
                }
            }
        }

        private void Report(string charset, float confidence)
        {
            this.Charset = charset;
            this.Confidence = confidence;

            // if (Finished != null)
            // {
            // Finished(charset, confidence);
            // }
        }
    }
}
