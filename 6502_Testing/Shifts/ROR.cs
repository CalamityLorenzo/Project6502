using Project6502;
using System.Diagnostics;

namespace Shifts
{
    [TestClass]
    public class ROR
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("ROR : Accumulator")]
        public void ROR_Accumulator()
        {
            Six502Processor processor = createProcessor();
            var program = new byte[]
            {
                0xA9,  // LDA #32
                0x80,

                0x6A
            };
            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
        }

        [TestMethod("ROR : ZeroPage")]
        public void ROR_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFF] = 0x80;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x66,
                0xFF
            };
            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0xFF}] = {mem[0xFF]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[255] == 64);
        }

        [TestMethod("ROR : ZeroPage.X")]
        public void ROR_ZeroPage_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFE + 0x01] = 0x80;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2, //LDX 01
                0x01,

                0x76,
                0xFE
            };
            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0xFF}] = {mem[0xFF]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[255] == 64);
        }

        [TestMethod("ROR : Absolute")]
        public void ROR_Absolute()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0x16 << 8 | 0xFE] = 129;
            Six502Processor processor = createProcessor(mem);
            var t = Byte.RotateRight(129, 1);
            var program = new byte[]
            {
                0x6E,
                0x16,
                0xFE
            };
            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0x16 << 8 | 0xFE}] = {mem[0x16 << 8 | 0xFE]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "True");
            Assert.IsTrue(mem[5886] == 192);
        }

        [TestMethod("ROR : Absolute.X")]
        public void ROR_Absolute_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[(0x16 << 8 | 0xFE) + 1] = 120;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2, // LDX  #01
                0x01,

                0x7E,
                0x16,
                0xFE
            };
            processor.AdhocProcess(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{(0x16 << 8 | 0xFE) + 1}] = {mem[(0x16 << 8 | 0xFE) + 1]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[5887] == 60);
        }

    }
}