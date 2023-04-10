﻿using Project6502;
using System.Diagnostics;

namespace _6502_Testing
{
    [TestClass]
    public class ORA
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("ORA : Immediate")]
        public void LogicalORA_Immediate()
        {
            var mem = new byte[ushort.MaxValue]; 
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC
                0x09,
                0xC8  // Val not memory location
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "205");
            Assert.IsTrue(registers["N"] == "True");


        }

        [TestMethod("ORA : ZeroPage")]
        public void LogicalORA_ZeroPage()
        {
            var mem = new byte[ushort.MaxValue];
            mem[200] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x18, // CLC
                
                0x05,
                0xC8
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "223");
            Assert.IsTrue(registers["N"] == "True");


        }

        [TestMethod("ORA : ZeroPage.X")]
        public void LogicalORA_ZeroPageX()
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
                
                0x15,
                0xC8 // 200 + 15
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"mem[{0xC8+0x0F}] = {mem[(0xC8 + 0x0F)]}");

            Assert.IsTrue(registers["A"] == "223");
            Assert.IsTrue(registers["N"] == "True");


        }

        [TestMethod("ORA : Absolute")]
        public void LogicalORA_Absolute()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                
                //0xA2,  // LDX #15
                //0x0E,

                0x0D,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"mem[{512}] = {mem[(512)]}");

            Assert.IsTrue(registers["A"] == "223");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[512] == 90);


        }


        [TestMethod("ORA : Absolute.x")]
        public void LogicalORA_AbsoluteX()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512+14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA2,  // LDX #15
                0x0E,

                0x1D,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");
            Trace.WriteLine($"mem[{512 + 14}] = {mem[(512 + 14)]}");

            Assert.IsTrue(registers["A"] == "223");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[526] == 90);


        }

        [TestMethod("ORA : Absolute.Y")]
        public void LogicalORA_AbsoluteY()
        {
            var mem = new byte[ushort.MaxValue];
            mem[512 + 14] = 90;
            var processor = createProcessor(mem);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,

                0xA0,  // LDX #15
                0x0E,

                0x19,
                0x02, // 200 + 15
                0x00,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");

            Trace.WriteLine($"mem[{512 + 14}] = {mem[(512 + 14)]}");

            Assert.IsTrue(registers["A"] == "223");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[526] == 90);


        }

        [TestMethod("ORA : Indirect.X")]
        public void LogicalORA_IndirectX()
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

                0x01,
                0xFF,
            };
            processor.Process(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            Trace.WriteLine($"N = {registers["N"]}");
            Trace.WriteLine($"X = {registers["X"]}");

            Trace.WriteLine($"mem[{3597}] = {mem[(3597)]}");

            Assert.IsTrue(registers["A"] == "223");
            Assert.IsTrue(registers["N"] == "True");
            Assert.IsTrue(mem[3597] == 90);


        }
    }
}
