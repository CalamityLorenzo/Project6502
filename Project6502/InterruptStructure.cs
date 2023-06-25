namespace Project6502
{
    /// <summary>
    /// Info to execute an object in an interrupt.
    /// Not sure who handles popping the stack. As this is 40+ years old I assuem the hardway.
    /// </summary>
    /// <param name="address">Address where this list</param>
    /// <param name="method">the actual code</param>
    public record struct InterruptStructure(ushort Address, byte[] Method, Action<Six502Processor> Interrupt);
}
