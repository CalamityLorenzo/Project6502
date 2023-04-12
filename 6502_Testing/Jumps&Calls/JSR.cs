using Project6502;
using System.Diagnostics;

namespace Jumps_Calls
{
    [TestClass]
    public class JSR
    {
        Six502Processor createProcessor() => new Six502Processor(new byte[ushort.MaxValue]);
        Six502Processor createProcessor(byte[] mem) => new Six502Processor(mem);

        [TestMethod("JSR : Absolute")]
        public void JSR_Absolute()
        {
            var mem = new byte[ushort.MaxValue];
            var processor = createProcessor(mem);

            var program = new byte[]{
                0x20,  // JSR 80 6A (32874)
                0x80,
                0x6A
            };

            var p = new byte[32880];
            program.CopyTo(p, 0);

            var by = new byte[]
            {
                0x68,    // PLA
                0xAA,   // TAX
                0x68,   // PLA  
                0xA8,   // TAY

                // 0x60    //  JSR
            };
            by.CopyTo(p, 32874);
            processor.Reset();
            processor.AdhocProcess(p);
            var r = processor.Registers();

            Trace.WriteLine($"X = {r["X"]}");
            Trace.WriteLine($"Y = {r["Y"]}");
            Trace.WriteLine($"PC = {r["PC"]}");

            // MSB
            Assert.IsTrue(r["Y"] == "128");
            // LSB
            Assert.IsTrue(r["X"] == "106");
            Assert.IsTrue(r["PC"] == "32881");

            processor.AdhocProcess(program);
        }
    }
}
