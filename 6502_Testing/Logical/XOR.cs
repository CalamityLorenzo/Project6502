using Project6502;
using System.Diagnostics;

namespace Logical
{
    [TestClass]
    public class XOR
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("EOR : Immediate")]
        public void ExclusiveXOR_Immediate()
        {
            var mem = new byte[ushort.MaxValue];
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC

                0x49,
                0xC8  // Val not memory location
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "13");
            Assert.IsTrue(registers["N"] == "False");


        }

        [TestMethod("EOR : ZeroPage")]
        public void ExclusiveXOR_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];
            mem[200] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC
                
                0x45,
                0xC8  
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");
        }

        [TestMethod("EOR : ZeroPage.X")]
        public void ExclusiveXOR_ZeroPageX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[200 + 15] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC

                0xA2,  // LDX #15
                0x0F,
                
                0x55,
                0xC8 // 200 + 15
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"mem[{0xC8 + 0x0F}] = {mem[0xC8 + 0x0F]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");


        }

        [TestMethod("EOR : Absolute")]
        public void ExclusiveXOR_Absolute()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                
                //0xA2,  // LDX #15
                //0x0E,

                0x4D,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"mem[{512}] = {mem[512]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[512] == 90);


        }

        [TestMethod("EOR : Absolute.x")]
        public void ExclusiveXOR_AbsoluteX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512 + 14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA2,  // LDX #15
                0x0E,

                0x5D,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"mem[{512 + 14}] = {mem[512 + 14]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[526] == 90);


        }

        [TestMethod("EOR : Absolute.Y")]
        public void ExclusiveXOR_AbsoluteY()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512 + 14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA0,  // LDX #15
                0x0E,

                0x59,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");

            Trace.WriteLine($"mem[{512 + 14}] = {mem[512 + 14]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[526] == 90);


        }

        [TestMethod("EOR : Indirect.X")]
        public void ExclusiveXOR_IndirectX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[3839] = 90;


            mem[14] = 14;
            mem[13] = 13;

            mem[14 << 8 | 13] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA2,  // LDX #14
                0x0E,

                0x41,
                0xFF,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");

            Trace.WriteLine($"mem[{3597}] = {mem[3597]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[3597] == 90);
        }

        [TestMethod("EOR : Indirect.Y")]
        public void ExclusiveXOR_IndirectY()
        {
            var mem = new byte[ushort.MaxValue];
            mem[3839] = 90;


            mem[256] = 90;
            mem[255] = 21;

            mem[(90 << 8 | 21) + 14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA0,  // LDY #14
                0x0E,

                0x51,
                0xFF,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"Y = {registers["Y"]}");

            Trace.WriteLine($"mem[{23075}] = {mem[23075]}");

            Assert.IsTrue(registers["A"] == "159");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(registers["Y"] == "14");
            Assert.IsTrue(mem[23075] == 90);

        }


    }
}
