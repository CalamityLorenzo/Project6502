using Project6502;
using System.Diagnostics;

namespace StackOperations
{
    [TestClass]
    public class StackOps
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("TSX : ")]
        public void TSX()
        {
            Six502Processor processor = createProcessor();
            var r1 = processor.Registers();
            var program = new byte[]
            {
                0x9A
            };
            processor.AdhocProgram(program);
            var r2 = processor.Registers();
            Trace.WriteLine($"X (orig) {r1["X"]}");
            Trace.WriteLine($"X (current) {r2["X"]}");
            Trace.WriteLine($"SP {r2["SP"]}");

            Assert.IsTrue(r1["X"] == "0");
            Assert.IsTrue(r2["X"] == "255");
            Assert.IsTrue(r2["SP"] == "255");

        }

        [TestMethod("TXS : ")]
        public void TXS()
        {
            Six502Processor processor = createProcessor();
            var program = new byte[]
            {
                0xA2, // LDX #32
                0x1F,
                0xBA // TXS
            };
            processor.AdhocProgram(program);

            var r2 = processor.Registers();
            Trace.WriteLine($"X {r2["X"]}");
            Trace.WriteLine($"SP {r2["SP"]}");

            Assert.IsTrue(r2["X"] == "31");
            Assert.IsTrue(r2["SP"] == "31");

        }

        [TestMethod("PHA")]
        public void PHA()
        {
            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
                0xA9, // LDA #15
                0x0F,
                0x48 // PHA
            };

            processor.AdhocProgram(program);
            var r = processor.Registers();
            Trace.WriteLine($"A = {r["A"]}");
            Trace.WriteLine($"SP = {r["SP"]}");
            Trace.WriteLine($"mem[{0x01FF}] = {mem[0x01FF]}");

            Assert.IsTrue(r["A"] == "15");
            Assert.IsTrue(r["SP"] == "254");
            Assert.IsTrue(mem[511] == 15);
            processor.AdhocProgram(program);
        }

        [TestMethod("PLA")]
        // Pulls an 8 bit value from the stack and into the accumulator. The zero and negative flags are set as appropriate.
        public void PLA()
        {
            var mem = new byte[ushort.MaxValue];
            // Top of stack, 
            // going to acc
            mem[0x01FF] = 0;
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
                // Preamble have to move the stack pointer to stop a wrap around of SP (increments on push)
                0xA2, // LDX 254
                0xFf,
                0xBA, // TXS

                0xA9, //lda 129
                0x81, 

                0x48, // pHA 


                0x68
            };

            processor.AdhocProgram(program);
            var r = processor.Registers();
            Trace.WriteLine($"A {r["A"]}");
            Trace.WriteLine($"SP {r["SP"]}");
            Trace.WriteLine($"N {r["N"]}");
            Trace.WriteLine($"mem[{0x01Ff}] {mem[0x01Ff]}");

            Assert.IsTrue(r["A"] == "129");
            Assert.IsTrue(r["SP"] == "255");
            Assert.IsTrue(r["N"] == "True"); // FUCKING VB!!
            Assert.IsTrue(mem[511] == 129);
            processor.AdhocProgram(program);

        }

        [TestMethod("PHP")]
        public void PHP()
        {
            var mem = new byte[ushort.MaxValue];
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
              // Preamble have to move the stack pointer to stop a wrap around of
              // SP (increments on push)
                0xA2, // LDX 254
                0xFE,
                0xBA, // TXS
              
                0x08
            };
            processor.AdhocProgram(program);
            var r = processor.Registers();
            Trace.WriteLine($"N {r["N"]}");
            Trace.WriteLine($"mem[{0x01FE}] == {mem[0x01FE]}");
            Assert.IsTrue(r["N"] == "True");
            Assert.IsTrue(mem[0x01FE] == 1);
        }

        [TestMethod("PLP")]
        public void PLP()
        {
            var mem = new byte[ushort.MaxValue];
            // This is our new binary thingy for the processor status,.
            mem[0x01FF] = 77;
            Six502Processor processor = createProcessor(mem);
            var program = new byte[]
            {
                0xA9,
                0x00,
                0x08,
                0x28
            };
            processor.AdhocProgram(program);
            var r = processor.Registers();
            Trace.WriteLine($"N {r["N"]}");
            Trace.WriteLine($"Z {r["Z"]}");
            Trace.WriteLine($"I {r["I"]}");
            Trace.WriteLine($"mem[{0x01FF}] == {mem[0x01FF]}");
            Assert.IsTrue(r["C"] == "False");
            Assert.IsTrue(r["Z"] == "True");
            Assert.IsTrue(r["I"] == "False");
            //Assert.IsTrue(mem[0x01FE] == 1);
        }

    }
}
