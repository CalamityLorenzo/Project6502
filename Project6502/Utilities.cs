﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6502
{
    internal static class Utilities
    {
        public static ushort Absolute(this byte[] @this, ushort firstPosition)
        {
            return (ushort)(@this[firstPosition++] << 8 | @this[firstPosition ++]);
        }

        public static short NextInstructionLE(this byte[] @this, short firstPosition)
        {
            return (short)(@this[firstPosition+1] << 8 | @this[firstPosition]);
        }

        public static byte ToIndexedIndirect(this byte[] @this, byte firstPosition)
        {
            var msb = @this[firstPosition+1];
            var lsb = @this[firstPosition];
            return (byte)(@this[(msb << 8 | lsb)]);
        }

    }
}
