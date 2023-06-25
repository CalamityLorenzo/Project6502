using Project6502;
using System.Diagnostics;

namespace Branches
{
    [TestClass]
    public class Overflow
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("BVC : flag false")]
        public void BVCFlagFalse()
        {
            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0x50, // BVC 02
                0x02,
                0xa9, // LDA 10
                0x0a,
                0xa2, // LDX 10
                0x0a,
                0x03
            };

            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"A = {registers["A"]}");

            Assert.IsTrue(registers["PC"] == "519");
            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["X"] == "10");
        }

        [TestMethod("BVS : flag Set")]
        public void BVSFlagSet()
        {
            var mem = new byte[ushort.MaxValue];

            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0xa9, // LDA #255
                0xFF,
                0x69, // ADC #05
                0x05,
                0x70, // BVS 02
                0x02,
                0xa9, // LDA 10
                0x0a,
                0xa2, // LDX 10
                0x0a,
                0x03
            };

            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"V = {registers["V"]}");

            Assert.IsTrue(registers["PC"] == "523");
            Assert.IsTrue(registers["X"] == "10");
            Assert.IsTrue(registers["A"] == "4");
            Assert.IsTrue(registers["V"] == "True");

        }

    }
}
