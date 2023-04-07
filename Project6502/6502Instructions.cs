using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project6502
{
    public partial class Six502Processor
    {
        // BOX
        void BranchIfOverflowClear(sbyte offSet)
        {
            this._programCounter += offSet;
        }

        // CLC
        void CLearCarry()
        {
            this._processorStatusFlags[0] = false;
        }
        /// <summary>
        /// CLD
        /// Implied
        /// </summary>
        void CLearDecimal()
        {
            this._processorStatusFlags[3] = false;
        }
        // CLI
        // Implied
        void CLearInterrupt()
        {
            this._processorStatusFlags[2] = false;
        }
        /// <summary>
        /// CLV
        /// Implied
        /// </summary>
        void CLearoVerflow()
        {
            this._processorStatusFlags[6] = false;
        }

        /// <summary>
        /// TSX
        /// Implied
        /// </summary>
        void TransferStackPointertoX()
        {
            this.IndexX = _stackPointer;
        }
        /// <summary>
        /// TXS
        /// </summary>
        void TransferXtoStackPoint()
        {
            this._stackPointer = IndexX;
        }
        /// <summary>
        /// TYA
        /// </summary>
        void TransferYToAccumulator()
        {
            this.Accumulator = IndexY;
            if (Accumulator > 127)
            {
                this._processorStatusFlags[7] = true;
                return;
            }
            if (Accumulator == 0)
            {
                this._processorStatusFlags[1] = true;
            }
        }

        void TransferAccumulatorY()
        {
            IndexY = this.Accumulator;
            if (Accumulator > 127)
            {
                this._processorStatusFlags[7] = true;
                return;
            }
            if (Accumulator == 0)
            {
                this._processorStatusFlags[1] = true;
            }
        }

    }
}
