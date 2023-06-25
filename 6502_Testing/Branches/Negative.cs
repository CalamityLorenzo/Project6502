using Project6502;
using System.Diagnostics;

namespace Branches
{
    [TestClass]
    public class Negative
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("BPL : flag false")]
        public void BPLFlagFalse()
        {
            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0x10, // BPL 02
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

        [TestMethod("BMI : flag Set")]
        public void BMIFlagSet()
        {
            var mem = new byte[ushort.MaxValue];

            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0xa9, // LDA #00
                0x00,
                0xe9, // SBC #05
                0x05,
                0x30, // BMI 02
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
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["PC"] == "523");
            Assert.IsTrue(registers["X"] == "10");
            Assert.IsTrue(registers["A"] == "251");
        }

    }
}
