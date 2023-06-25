using Project6502;
using System.Diagnostics;

namespace SystemCalls
{
    [TestClass]
    public class Interrupts
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);
        Six502Processor createProcessor(byte[] mem, InterruptStructure brk) => new Six502Processor(mem, brk);

        [TestMethod("BRK : Delegate")]
        public void Break()
        {
            var mem = Enumerable.Range(0, ushort.MaxValue).Select(a => (byte)0xEA).ToArray();

            var brk = new InterruptStructure(0x1000, Array.Empty<byte>(), (p) =>
            {
                p.AdhocInstruction(new byte[]
                {
                            0xA9,
                            0x01,
                });

                p.InstructionStep();

            });
            var processor = createProcessor(mem, brk);

            var program = new byte[]{
                0xA9,  // LDA #197
                0xC5,
                0x00,
                };
            processor.AdhocProgram(program);

            var registers = processor.Registers();
            Trace.WriteLine($"A = {registers["A"]}");
            //Trace.WriteLine($"N = {registers["N"]}");

            Assert.IsTrue(registers["A"] == "01");
            //Assert.IsTrue(registers["N"] == "True");


        }
    }
}
