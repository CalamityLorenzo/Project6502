namespace Project6502
{
    public partial class Six502Processor
    {
        // BOX
        void BranchIfOverflowClear()
        {
            var offSet = (sbyte)_programBuffer[_programCounter ++];
            this._programCounter = ((ushort)(_programCounter + offSet));
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
            this.XRegister = _stackPointer;
        }
        /// <summary>
        /// TXS
        /// </summary>
        void TransferXtoStackPointer()
        {
            this._stackPointer = XRegister;
        }
        /// <summary>
        /// TYA
        /// </summary>
        void TransferYToAccumulator()
        {
            this.Accumulator = YRegister;
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

        /// <summary>
        /// TAY
        /// </summary>
        void TransferAccumulatorToY()
        {
            YRegister = this.Accumulator;
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

        /// <summary>
        /// TXA
        /// </summary>
        void TransferXToAccumulator()
        {
            this.Accumulator = XRegister;
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

        /// <summary>
        /// TAX
        /// </summary>
        void TransferAccumulatorToX()
        {
            XRegister = this.Accumulator;
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



        /// <summary>
        /// ROL
        /// </summary>
        void ROtateLeft(byte operation)
        {
            var operand = (byte)(operation switch
            {
                0x2A => Accumulator,
                0x26 => memory[_programBuffer[_programCounter ++]],
                0x36 => memory[_programBuffer[_programCounter ++] + XRegister],
                0x2E => memory[(_programBuffer[_programCounter ++] + _programBuffer[_programCounter ++])],
                0x3E => memory[(_programBuffer[_programCounter ++] + _programBuffer[_programCounter ++])] + XRegister
            });
            var value = byte.RotateLeft(operand, 1) + (_processorStatusFlags[0] ? 1 : 0);
            _processorStatusFlags[0] = (operand >> 7) == 1 ? true : _processorStatusFlags[0];
        }
        /// <summary>
        /// ROR
        /// </summary>
        void ROtateRight(byte operation)
        {
            var operand = (byte)(operation switch
            {
                0x6A => Accumulator,
                0x66 => memory[_programBuffer[_programCounter ++]],
                0x76 => memory[_programBuffer[_programCounter ++] + XRegister],
                0x6E => memory[(_programBuffer[_programCounter ++] + _programBuffer[_programCounter ++])],
                0x7E => memory[(_programBuffer[_programCounter ++] + _programBuffer[_programCounter ++])] + XRegister
            });
            var value = byte.RotateRight(operand, 1) + (_processorStatusFlags[0] ? 128 : 0);
            _processorStatusFlags[0] = (byte)(operand & 1) == 1 ? true : _processorStatusFlags[0];
        }

        /// <summary>
        /// LDA
        /// </summary>
        /// <param name="operation"></param>
        void LoaDtheAccumulator(byte operation)
        {
            var operand = operation switch
            {
                0xA9 => ImmediateConstant(),
                0xA5 => ZeroPage(),
                0xB5 => ZeroPage_X(),
                0xAD => Absolute(),
                0xBD => Absolute_X(),
                0xB9 => Absolute_Y(),
                0xA1 => Indirect_X(),
                0xB1 => Indirect_Y(),

            } ;
            Accumulator = operand;
            if(Accumulator == 0)
            {
                _processorStatusFlags[1] = true;
            }
            else
            {
                _processorStatusFlags[7] = (operand >> 7) == 1 ? true : _processorStatusFlags[7];
            }
        }
        
        /// <summary>
        /// LDX
        /// </summary>
        /// <param name="operation"></param>
        void LoaDIntoXregister(byte operation)
        {
            var operand = (byte)(operation switch
            {
                0xA2 => ImmediateConstant(),
                0xAb => ZeroPage(),
                0xB6 => ZeroPage_Y(),
                0xAE => Absolute(),
                0xBe => Absolute_Y()
            });
            XRegister = operand;
            if (XRegister == 0)
            {
                _processorStatusFlags[1] = true;
            }
            else
            {
                _processorStatusFlags[7] = (operand >> 7) == 1 ? true : _processorStatusFlags[7];
            }
        }

        void LoaDIntoYregister(byte operation)
        {
            var operand = (byte)(operation switch
            {
                0xA0 => ImmediateConstant(),
                0xA4 => ZeroPage(),
                0xB4 => ZeroPage_X(),
                0xAc => Absolute(),
                0xBC => Absolute_X()
            });
            YRegister = operand;
            if (YRegister == 0)
            {
                _processorStatusFlags[1] = true;
            }
            else
            {
                _processorStatusFlags[7] = (operand >> 7) == 1 ? true : _processorStatusFlags[7];
            }
        }


        private byte ImmediateConstant() => _programBuffer[_programCounter++];

        private byte ZeroPage() => memory[_programBuffer[_programCounter++]];

        private byte ZeroPage_X() => memory[(_programBuffer[_programCounter++] + XRegister) & 0xFF];
        private byte ZeroPage_Y() => memory[(_programBuffer[_programCounter++] + YRegister) & 0xFF];
        private byte Absolute() => memory[_programBuffer.Absolute(_programCounter)];

        private byte Absolute_X() => memory[_programBuffer.Absolute(_programCounter) + XRegister];
        
        private byte Absolute_Y() => memory[_programBuffer.Absolute(_programCounter) + YRegister];

        private byte Indirect_X() => memory.ToIndexedIndirectX((byte)((_programBuffer[_programCounter++] + XRegister) & 0xFF)); // Indexed Indirect x ($,X)

        private byte Indirect_Y() => memory.ToIndirectIndexY(_programBuffer[_programCounter++], YRegister); // INdreict index Y ($),y



        void Jump()
        {
            var bottom = memory[_programBuffer[_programCounter ++]];
            var top = memory[_programBuffer[_programCounter ++]];
            _programCounter = (ushort)(top << 7 | bottom);
        }

    }
}
