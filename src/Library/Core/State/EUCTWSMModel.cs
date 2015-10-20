﻿namespace Chartect.IO.Core
{
    using System;

    internal class EUCTWSMModel : StateMachineModel
    {
        private static readonly int[] ModelClassTable =
        {
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 00 - 07
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 0, 0),  // 08 - 0f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 10 - 17
            BitPackage.Pack4bits(2, 2, 2, 0, 2, 2, 2, 2),  // 18 - 1f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 20 - 27
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 28 - 2f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 30 - 37
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 38 - 3f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 40 - 47
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 48 - 4f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 50 - 57
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 58 - 5f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 60 - 67
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 68 - 6f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 70 - 77
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 78 - 7f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 80 - 87
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 6, 0),  // 88 - 8f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 90 - 97
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 98 - 9f
            BitPackage.Pack4bits(0, 3, 4, 4, 4, 4, 4, 4),  // a0 - a7
            BitPackage.Pack4bits(5, 5, 1, 1, 1, 1, 1, 1),  // a8 - af
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // b0 - b7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // b8 - bf
            BitPackage.Pack4bits(1, 1, 3, 1, 3, 3, 3, 3),  // c0 - c7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // c8 - cf
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // d0 - d7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // d8 - df
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // e0 - e7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // e8 - ef
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // f0 - f7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 0) // f8 - ff
        };

        private static readonly int[] ModelStateTable =
        {
            BitPackage.Pack4bits(Error, Error, Start,    3,    3,    3,    4, Error), // 00-07
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, ItsMe, ItsMe), // 08-0f
            BitPackage.Pack4bits(ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, Error, Start, Error), // 10-17
            BitPackage.Pack4bits(Start, Start, Start, Error, Error, Error, Error, Error), // 18-1f
            BitPackage.Pack4bits(5, Error, Error, Error, Start, Error, Start, Start), // 20-27
            BitPackage.Pack4bits(Start, Error, Start, Start, Start, Start, Start, Start) // 28-2f
        };

        private static readonly int[] CharacterLengthTable = { 0, 0, 1, 2, 2, 2, 3 };

        public EUCTWSMModel()
            : base(
              ModelClassTable.To4BitPackage(),
              7,
              ModelStateTable.To4BitPackage(),
              CharacterLengthTable,
              Charsets.EUCTW)
        {
        }
    }
}