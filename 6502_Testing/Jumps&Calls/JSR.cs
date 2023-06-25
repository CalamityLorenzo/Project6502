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

            var p = new byte[32880];
            var program = new byte[]{
                0x20,  // JSR 80 6A (32874)
                0x6A,
                0x82,
            };

            program.CopyTo(p, 0);

            var program2= new byte[]
            {
                0x68,    // PLA
                0xAA,   // TAX
                0x68,   // PLA  
                0xA8,   // TAY
                0x60    //  JSR
            };

            program2.CopyTo(p, 32874);
            processor.Reset();
            processor.AdhocProgram(p);
            var r = processor.Registers();

            Trace.WriteLine($"X = {r["X"]}");
            Trace.WriteLine($"Y = {r["Y"]}");
            Trace.WriteLine($"PC = {r["PC"]}");

            // MSB
            Assert.IsTrue(r["X"] == "106");
            // LSB
            Assert.IsTrue(r["Y"] == "130");
            Assert.IsTrue(r["PC"] == "1");

            processor.AdhocProgram(program);
        }
    }
}
