﻿namespace Project6502
{
    internal static class Utilities
    {
        public static ushort Absolute(this byte[] @this, ref ushort firstPosition)
        {

            var lsb = @this[firstPosition++];
            var msb = @this[firstPosition++];
            return (ushort)(msb << 8 | lsb);
        }

        public static short NextInstructionLE(this byte[] @this, short firstPosition)
        {
            return (short)(@this[firstPosition + 1] << 8 | @this[firstPosition]);
        }

        public static byte ToIndexedIndirectX(this byte[] @this, byte firstPosition)
        {
            var msb = @this[firstPosition + 1];
            var lsb = @this[firstPosition];
            return (byte)(@this[(msb << 8 | lsb)]);
        }

        public static byte ToIndirectIndexY(this byte[] @this, byte firstPosition, byte yRegister)
        {
            var address = (int)(@this[firstPosition + 1] << 8 | @this[firstPosition]);
            return @this[address + yRegister];
        }

    }
}
