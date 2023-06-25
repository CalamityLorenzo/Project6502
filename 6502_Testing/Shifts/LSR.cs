
using Project6502;
using System.Diagnostics;

namespace Shifts
{
    [TestClass]
    public class LSR
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("LSR : Accumulator")]
        public void LSR_Accumulator()
        {
            Six502Processor processor = createProcessor();

            var program = new byte[]
            {
                0xA9,  // LDA #32
                0x20,

                0x4A
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");

            Assert.IsTrue(registers["A"] == "16");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
        }

        [TestMethod("LSR : ZeroPage")]
        public void LSR_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFF] = 0x20;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x46,
                0xFF
            };
            processor.AdhocProgram(program);

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
            Assert.IsTrue(mem[255] == 16);
        }

        [TestMethod("LSR : ZeroPage.X")]
        public void LSR_ZeroPage_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0xFE + 0x01] = 0x20;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2, //LDX 01
                0x01,

                0x56,
                0xFE
            };
            processor.AdhocProgram(program);

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
            Assert.IsTrue(mem[255] == 16);
        }

        [TestMethod("LSR : Absolute")]
        public void LSR_Absolute()
        {
            var mem = new byte[ushort.MaxValue];

            mem[0x16 << 8 | 0xFE] = 129;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0x4E,
                0xFE,
                0x16,
            };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Z = {registers["Z"]}");
            Trace.WriteLine($"C = {registers["C"]}");
            Trace.WriteLine($"mem[{0x16 << 8 | 0xFE}] = {mem[0x16 << 8 | 0xFE]}");

            Assert.IsTrue(registers["A"] == "0");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(registers["Z"] == "False");
            Assert.IsTrue(registers["C"] == "False");
            Assert.IsTrue(mem[5886] == 64);
        }

        [TestMethod("LSR : Absolute.X")]
        public void LSR_Absolute_X()
        {
            var mem = new byte[ushort.MaxValue];

            mem[(0x16 << 8 | 0xFE) + 1] = 120;
            Six502Processor processor = createProcessor(mem);

            var program = new byte[]
            {
                0xA2, // LDX  #01
                0x01,

                0x5E,
                0xFE,
                0x16,
            };
            processor.AdhocProgram(program);

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


