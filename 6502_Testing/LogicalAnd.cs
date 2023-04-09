using Project6502;
using System.Diagnostics;

namespace _6502_Testing
{
    [TestClass]
    public class LogicalAnd
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("AND : Immediate")]
        public void LogicalAnd_Immediate()
        {
            var mem = new byte[ushort.MaxValue]; 
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC
                0x29,
                0xC8  // Val not memory location
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "192");
            Assert.IsTrue(registers["N"] == "True");


        }

        [TestMethod("AND : ZeroPage")]
        public void LogicalAnd_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];
            mem[200] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC
                0x25,
                0xC8
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");


        }

        [TestMethod("AND : ZeroPage.X")]
        public void LogicalAnd_ZeroPageX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[200+15] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0xA2,  // LDX #15
                0x0F,
                0x18, // CLC
                0x35,
                0xC8 // 200 + 15
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"mem[{0xC8+0x0F}] = {mem[(0xC8 + 0x0F)]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");


        }

        [TestMethod("AND : Absolute")]
        public void LogicalAnd_Absolute()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                
                //0xA2,  // LDX #15
                //0x0E,

                0x2D,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"mem[{512}] = {mem[(512)]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(mem[512] == 90);


        }


        [TestMethod("AND : Absolute.x")]
        public void LogicalAnd_AbsoluteX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512+14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA2,  // LDX #15
                0x0E,

                0x3D,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"mem[{512 + 14}] = {mem[(512 + 14)]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(mem[526] == 90);


        }

        [TestMethod("AND : Absolute.Y")]
        public void LogicalAnd_AbsoluteY()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512 + 14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA0,  // LDX #15
                0x0E,

                0x39,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");

            Trace.WriteLine($"mem[{512 + 14}] = {mem[(512 + 14)]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(mem[526] == 90);


        }

        [TestMethod("AND : Indirect.X")]
        public void LogicalAnd_IndirecX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[3839] = 90;


            mem[14] = 14;
            mem[13] = 13;

            mem[(14 << 8 | 13)] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA2,  // LDX #14
                0x0E,

                0x21,
                0xFF,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");

            Trace.WriteLine($"mem[{3597}] = {mem[(3597)]}");

            Assert.IsTrue(registers["A"] == "64");
            Assert.IsTrue(registers["N"] == "False");
            Assert.IsTrue(mem[3597] == 90);


        }
    }
}
