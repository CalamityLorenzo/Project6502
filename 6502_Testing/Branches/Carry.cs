using Project6502;
using System.Diagnostics;

namespace Branches
{
    [TestClass]
    public class Carry
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("BCC : flag true")]
        public void BCCFlagTrue()
        {
            var mem = new byte[ushort.MaxValue];
            mem[0x211] = 0xA9;
            mem[0x212] = 0x10;
            mem[0x213] = 0x03;

            mem[0x214] = 0xA2;
            mem[0x215] = 0x10;
            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0x90, // BCC 02
                0x02,
                0xa9, // LDA 10
                0x0a,
                0xa2, // LDX 10
                0x0a,
                0x03
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"A = {registers["A"]}");

            Assert.IsTrue(registers["PC"] == "519");
            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["X"] == "10");
        }

        [TestMethod("BCS : flag false")]
        public void BCSFlagFalse()
        {
            var mem = new byte[ushort.MaxValue];

            Six502Processor processor = createProcessor(mem);
            // Branching is relative.
            // THe programm is placed at 0x200
            // so we need to add 14 to get to 0x215  from 0x201
            var program = new byte[]
            {
                0x38,
                0x90, // BCC 02
                0x02,
                0xa9, // LDA 10
                0x0a,
                0xa2, // LDX 10
                0x0a,
                0x03
            };

            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"PC = {registers["PC"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"A = {registers["A"]}");

            Assert.IsTrue(registers["PC"] == "520");
            Assert.IsTrue(registers["X"] == "10");
            Assert.IsTrue(registers["A"] == "10");
        }

    }
}
