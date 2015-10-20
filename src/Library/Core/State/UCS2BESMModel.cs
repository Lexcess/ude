﻿namespace Chartect.IO.Core
{
    using System;

    internal class UCS2BESMModel : StateMachineModel
    {
        private static readonly int[] ModelClassTable =
        {
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 00 - 07
            BitPackage.Pack4bits(0, 0, 1, 0, 0, 2, 0, 0),  // 08 - 0f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 10 - 17
            BitPackage.Pack4bits(0, 0, 0, 3, 0, 0, 0, 0),  // 18 - 1f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 20 - 27
            BitPackage.Pack4bits(0, 3, 3, 3, 3, 3, 0, 0),  // 28 - 2f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 30 - 37
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 38 - 3f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 40 - 47
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 48 - 4f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 50 - 57
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 58 - 5f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 60 - 67
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 68 - 6f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 70 - 77
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 78 - 7f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 80 - 87
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 88 - 8f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 90 - 97
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 98 - 9f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // a0 - a7
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // a8 - af
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // b0 - b7
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // b8 - bf
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // c0 - c7
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // c8 - cf
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // d0 - d7
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // d8 - df
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // e0 - e7
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // e8 - ef
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // f0 - f7
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 4, 5),  // f8 - ff
        };

        private static readonly int[] ModelStateTable =
        {
            BitPackage.Pack4bits(5,         7,     7, Error,     4,     3, Error, Error), // 00-07
            BitPackage.Pack4bits(Error, Error, Error, Error, ItsMe, ItsMe, ItsMe, ItsMe), // 08-0f
            BitPackage.Pack4bits(ItsMe, ItsMe,     6,     6,     6,     6, Error, Error), // 10-17
            BitPackage.Pack4bits(6,         6,     6,     6,     6, ItsMe,     6,     6), // 18-1f
            BitPackage.Pack4bits(6,         6,     6,     6,     5,     7,     7, Error), // 20-27
            BitPackage.Pack4bits(5,         8,     6,     6, Error,     6,     6,     6), // 28-2f
            BitPackage.Pack4bits(6,         6,     6,     6, Error, Error, Start, Start), // 30-37
        };

        private static readonly int[] CharacterLengthTable = { 2, 2, 2, 0, 2, 2 };

        public UCS2BESMModel()
            : base(
              ModelClassTable.To4BitPackage(),
              6,
              ModelStateTable.To4BitPackage(),
              CharacterLengthTable,
              Charsets.UTF16BE)
        {
        }
    }
}