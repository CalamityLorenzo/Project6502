using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6502
{
    internal static class Utilities
    {
        public static short NextInstruction(this byte[] @this, short firstPosition)
        {
            return (short)(@this[firstPosition] << 8 | @this[firstPosition + 1]);
        }

        public static short NextInstructionLE(this byte[] @this, short firstPosition)
        {
            return (short)(@this[firstPosition+1] << 8 | @this[firstPosition]);
        }
    }
}
