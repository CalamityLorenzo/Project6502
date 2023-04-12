using System.Xml.Serialization;

namespace Project6502
{
    public partial class Six502Processor
    {
        // BOX
        void BranchIfOverflowClear()
        {
            var offSet = (sbyte)memory[_programCounter++];
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

        #region stack operations
        /// <summary>
        /// TSX
        /// Implied
        /// </summary>
        void TransferStackpointertoX()
        {
            this.XRegister = _stackPointer;
            CheckNegativeZeroFlags(XRegister);

        }

        /// <summary>
        /// TXS
        /// </summary>
        void TransferXtoStackPointer()
        {
            this._stackPointer = XRegister;
        }

        /// <summary>
        /// PHA
        /// </summary>
        void PusHAccumulator()
        {
            memory[(0x01 << 8 | _stackPointer--)] = Accumulator;
        }
        /// <summary>
        /// PLA
        /// Pulls an 8 bit value from the stack and into the accumulator. The zero and negative flags are set as appropriate.
        /// </summary>
        void PulLAccumulator()
        {
            Accumulator = memory[(0x01 << 8 | ++_stackPointer)];
            CheckNegativeZeroFlags(Accumulator);
        }
        /// <summary>
        /// PHP
        /// </summary>
        void PusHProcessorStatus()
        {
            byte val = ConvertFromProcessorStatus();
            memory[(0x01 << 8 | _stackPointer--)] = val;
        }

        void PulLProcessorstatus()
        {
            var val = memory[(0x01 << 8 | _stackPointer++)];
            ConvertToProcessorStatus(val);
        }
        #endregion

        #region Register Transfers
        /// <summary>
        /// TYA
        /// </summary>
        void TransferYToAccumulator()
        {
            this.Accumulator = YRegister;
            CheckNegativeZeroFlags(YRegister);

        }

        /// <summary>
        /// TAY
        /// </summary>
        void TransferAccumulatorToY()
        {
            YRegister = this.Accumulator;
            CheckNegativeZeroFlags(Accumulator);
        }

        /// <summary>
        /// TXA
        /// </summary>
        void TransferXToAccumulator()
        {
            this.Accumulator = XRegister;
            CheckNegativeZeroFlags(Accumulator);

        }

        /// <summary>
        /// TAX
        /// </summary>
        void TransferAccumulatorToX()
        {
            XRegister = this.Accumulator;
            CheckNegativeZeroFlags(Accumulator);
        }
        #endregion

        #region LoadStoreOperations
        /// <summary>
        /// LDA
        /// </summary>
        /// <param name="operation"></param>
        void LoaDtheAccumulator(byte operation)
        {
            var operand = operation switch
            {
                0xA9 => ImmediateConstant(),
                0xA5 => memory[ZeroPage()],
                0xB5 => memory[ZeroPage_X()],
                0xAD => memory[Absolute()],
                0xBD => memory[Absolute_X()],
                0xB9 => memory[Absolute_Y()],
                0xA1 => memory[Indirect_X()],
                0xB1 => memory[Indirect_Y()],
                _ => throw new NotImplementedException($"{operation.ToString()} : {Convert.ToString(operation, toBase: 16)}"),
            };
            Accumulator = operand;
            CheckNegativeZeroFlags(Accumulator);

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
                0xA6 => memory[ZeroPage()],
                0xB6 => memory[ZeroPage_Y()],
                0xAE => memory[Absolute()],
                0xBE => memory[Absolute_Y()]
            });
            XRegister = operand;
            CheckNegativeZeroFlags(XRegister);
        }
        /// <summary>
        /// LDY
        /// </summary>
        /// <param name="operation"></param>
        void LoaDIntoYregister(byte operation)
        {
            var operand = (byte)(operation switch
            {
                0xA0 => ImmediateConstant(),
                0xA4 => memory[ZeroPage()],
                0xB4 => memory[ZeroPage_X()],
                0xAC => memory[Absolute()],
                0xBC => memory[Absolute_X()]
            });
            YRegister = operand;
            CheckNegativeZeroFlags(YRegister);
        }
        /// <summary>
        /// STA
        /// </summary>
        /// <param name="operation"></param>
        void StoreTheAccumulator(byte operation)
        {
            // Gets the memory address that we are poking with our value
            var operand = operation switch
            {
                0x85 => ZeroPage(),
                0x95 => ZeroPage_X(),
                0x8D => Absolute(),
                0x9D => Absolute_X(),
                0x99 => Absolute_Y(),
                0x81 => Indirect_X(),
                0x91 => Indirect_Y(),
            };

            memory[operand] = Accumulator;
        }

        void StoreTheXregister(byte operation)
        {
            // Gets the memory address that we are poking with our value
            var operand = operation switch
            {
                0x86 => ZeroPage(),
                0x96 => ZeroPage_Y(),
                0x8E => Absolute(),
            };

            memory[operand] = XRegister;
        }

        void StoreTheYregister(byte operation)
        {
            // Gets the memory address that we are poking with our value
            var operand = operation switch
            {
                0x84 => ZeroPage(),
                0x94 => ZeroPage_X(),
                0x8C => Absolute(),
            };

            memory[operand] = YRegister;
        }

        #endregion

        #region Logical Operations
        /// <summary>
        /// AND
        /// </summary>
        /// <param name="operation"></param>
        void LogicalAND(byte operation)
        {
            var operand = operation switch
            {
                0x29 => ImmediateConstant(),
                0x25 => memory[ZeroPage()],
                0x35 => memory[ZeroPage_X()],
                0x2D => memory[Absolute()],
                0x3D => memory[Absolute_X()],
                0x39 => memory[Absolute_Y()],
                0x21 => memory[Indirect_X()],
                0x31 => memory[Indirect_Y()],
            };
            var val = Accumulator & operand;
            Accumulator = (byte)val;
            CheckNegativeZeroFlags(Accumulator);

        }
        /// <summary>
        /// EOR
        /// </summary>
        /// <param name="operation"></param>
        void ExclusiveOR(byte operation)
        {
            var operand = operation switch
            {
                0x49 => ImmediateConstant(),
                0x45 => memory[ZeroPage()],
                0x55 => memory[ZeroPage_X()],
                0x4D => memory[Absolute()],
                0x5D => memory[Absolute_X()],
                0x59 => memory[Absolute_Y()],
                0x41 => memory[Indirect_X()],
                0x51 => memory[Indirect_Y()],
            };
            var val = Accumulator ^ operand;
            Accumulator = (byte)val;
            CheckNegativeZeroFlags(Accumulator);
        }
        /// <summary>
        /// ORA
        /// </summary>
        /// <param name="operation"></param>
        void LogicalInclusiveOR(byte operation)
        {
            var operand = operation switch
            {
                0x09 => ImmediateConstant(),
                0x05 => memory[ZeroPage()],
                0x15 => memory[ZeroPage_X()],
                0x0D => memory[Absolute()],
                0x1D => memory[Absolute_X()],
                0x19 => memory[Absolute_Y()],
                0x01 => memory[Indirect_X()],
                0x11 => memory[Indirect_Y()],
            };
            var val = Accumulator | operand;
            Accumulator = (byte)val;
            CheckNegativeZeroFlags(Accumulator);
        }
        /// <summary>
        /// BIT
        /// </summary>
        /// <param name=""></param>
        void BIT(byte operation)
        {
            var operand = operation switch
            {
                0x24 => ZeroPage(),
                0x2C => Absolute(),
            };

            var memVal = memory[operand];
            if ((Accumulator & memVal) == 0)
                _processorStatusFlags[1] = true;
            // Bits 6 and 7 are copied to Verlfow and Negative flag.
            _processorStatusFlags[6] = (memVal & 0x40) == 0;
            _processorStatusFlags[7] = (memVal & 0x80) == 0;
        }

        #endregion

        #region shifts
        /// <summary>
        /// ASL
        /// </summary>
        /// <param name="operation"></param>
        void ASL(byte operation)
        {
            var operand = operation switch
            {
                0x0A => Accumulator,
                0x06 => ZeroPage(),
                0x16 => ZeroPage_X(),
                0x0E => Absolute(),
                0x1E => Absolute_X(),
            };
            var numToProcess = operation != 0x0A ? memory[operand] : (byte)operand;
            var val = (byte)(numToProcess << 1);

            _processorStatusFlags[0] = ((numToProcess >> 7) == 1);
            CheckNegativeZeroFlags(val);

            if (operation != 0x0A)
            {
                memory[operand] = val;
            }
            else
            {
                Accumulator = val;
            }

        }
        /// <summary>
        /// LSR
        /// </summary>
        /// <param name="operation"></param>
        void LSR(byte operation)
        {
            var operand = operation switch
            {
                0x4A => Accumulator,
                0x46 => ZeroPage(),
                0x56 => ZeroPage_X(),
                0x4E => Absolute(),
                0x5E => Absolute_X(),
            };
            var numToProcess = operation != 0x4A ? memory[operand] : (byte)operand;
            var val = (byte)(numToProcess >> 1);
            // Move bit 0 into the carry flag
            _processorStatusFlags[0] = ((numToProcess & 01) == 1);
            CheckNegativeZeroFlags(val);

            if (operation != 0x4A)
            {
                memory[operand] = val;
            }
            else
            {
                Accumulator = val;
            }
        }
        /// <summary>
        /// ROL
        /// </summary>
        void ROtateLeft(byte operation)
        {
            var operand = (operation switch
            {
                0x2A => Accumulator,
                0x26 => ZeroPage(),
                0x36 => ZeroPage_X(),
                0x2E => Absolute(),
                0x3E => Absolute_X()
            });
            var numToProcess = operation != 0x2A ? memory[operand] : (byte)operand;
            byte value = (byte)(byte.RotateLeft(numToProcess, 1) + (_processorStatusFlags[0] ? 1 : 0));
            _processorStatusFlags[0] = ((numToProcess >> 7) == 1);
            CheckNegativeZeroFlags(value);

            if (operation != 0x2A)
            {
                memory[operand] = value;
            }
            else
            {
                Accumulator = value;
            }
        }
        /// <summary>
        /// ROR
        /// </summary>
        void ROtateRight(byte operation)
        {
            var operand = (operation switch
            {
                0x6A => Accumulator,
                0x66 => ZeroPage(),
                0x76 => ZeroPage_X(),
                0x6E => Absolute(),
                0x7E => Absolute_X()
            });
            var numToProcess = operation != 0x6A ? memory[operand] : (byte)operand;
            var value = (byte)(byte.RotateRight(numToProcess, 1) + (_processorStatusFlags[0] ? 128 : 0));
            _processorStatusFlags[0] = ((byte)(numToProcess & 1) == 1);
            if (operation != 0x6A)
            {
                memory[operand] = value;
            }
            else
            {
                Accumulator = value;
            }
        }
        #endregion shifts
        /// <summary>
        /// JMP
        /// </summary>
        /// <param name="operation"></param>
        void Jump(byte operation)
        {
            var operand = operation switch
            {
                0x4C => Absolute(),
                0x6C => Indirect()
            };
            _programCounter = (ushort)operand;
        }

        /// <summary>
        /// JSR
        /// </summary>
        void JumptoSubRoutine()
        {
            var operand = Absolute();
            // Set ther return address1
            // msb
            memory[(0x01 << 8 | _stackPointer--)] = (byte)(operand >> 8);
            // lsb 
            memory[(0x01 << 8 | _stackPointer--)] = (byte)(operand & 0xFF);
            // Skip the operands for the comwmand.
            //_programCounter++;
            //_programCounter++;
            _programCounter = (ushort)operand;
        }

        // RTS
        void ReturnfromSubroutine()
        {
            var lsb = memory[_stackPointer--];
            var msb = memory[_stackPointer--];
            var newProgramCounter = (msb << 8 | lsb);
            _programCounter = (ushort)newProgramCounter;
        }

    }
}
