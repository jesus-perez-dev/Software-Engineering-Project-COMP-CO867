using System;
using System.Collections;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    /// Abstract class representing a region of simulation memory.
    /// Library users should access properties of this class via one of the derived classes listed below.
    ///
    /// The actual PLC "Memory Area" (ie. %I or %Q addresses) are determined by the subclasses SimulationInput and SimulationOutput respectively.
    ///
    /// See Also:
    ///     - <SimulationInput>
    ///     - <SimulationOutput>
    public abstract class SimulationAddress
    {
        // S7-PLCSIM instance of the address.
        internal IInstance Instance { get; }

        /// Human friendly name to reference or identify the simulation memory address.
        public string Name { get; }
        
        /// Type of data stored at this simulation memory address. Examples: UInt64, Bool, Int16, Char, Double ...
        ///
        /// Remarks:
        ///     EPrimitiveDataType is an enum provided by the official Siemens C# simulation library.
        public EPrimitiveDataType DataType { get; }

        /// Byte value location of the address, as in "%Q{Byte}.{Bit}"
        public uint ByteOffset { get; }

        /// Bit value location of the address, as in "%Q{Byte}.{Bit}"
        public byte BitOffset { get; }

        /// Total number of bits occupied by the simulation address.
        public byte BitSize { get; }
        
        /// Bit offset where this address begins in memory.
        public uint StartBit => ByteOffset * 8 + BitOffset;

        /// Bit offset where this address ends in memory.
        public uint EndBit => StartBit + BitSize - 1;
        
        /// Minimum number of bytes occupied in simulation memory by this address object.
        ///
        /// Remarks:
        ///     This is considered a "minimum" value because address offset can begin or end at arbitrary bits within a byte.
        ///     For example, a single byte can occupy range %I0.4 to %I1.3, in which case ByteSize would be 2 even though the
        ///     simulation address object only occupies 8 bits or 1 byte of simulation memory.
        public uint ByteSize => ((StartBit + BitSize - 1) / 8) + 1;

        internal SimulationAddress(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, IInstance instance) 
            : this(name, byteOffset, bitOffset, dataType.ToBitSize(), dataType, instance) { }

        internal SimulationAddress(string name, uint byteOffset, byte bitOffset, byte bitSize, EPrimitiveDataType dataType, IInstance instance)
        {
            Instance = instance;
            Name = name;
            ByteOffset = byteOffset;
            BitOffset = bitOffset;
            DataType = dataType;
            BitSize = bitSize;
            
            if (bitOffset > 7)
                throw new S7PlcSimLibraryException($"Invalid Bit Offset specified - Bit offset must be between 0-7. Given Bit Offset = {bitOffset}");
            
            if (bitSize > 64)
                throw new S7PlcSimLibraryException($"Invalid Bit Size specified - Bit size must not exceed 64 bits. Given Bit Size = {bitSize}");

            if (dataType == EPrimitiveDataType.Unspecific || dataType == EPrimitiveDataType.Struct)
                throw new S7PlcSimLibraryException($"Invalid Data Type - The following data types are unsupported: {EPrimitiveDataType.Unspecific.ToHumanString()}, {EPrimitiveDataType.Struct.ToHumanString()}");
        }
        
        // Write the bits of `value` overtop bits of `input` starting at `bitOffset` until `bitSize`
        protected static byte[] ShiftWrite(byte[] inputBytes, ulong value, byte bitOffset, byte bitSize)
        {
            var inputBitArray = new BitArray(inputBytes);
            var valueBitArray = new BitArray(BitConverter.GetBytes(value));
            for (int i = bitOffset, j = 0; i < bitOffset+bitSize; i++, j++)
                inputBitArray.Set(i, valueBitArray[j]);
            // 8 bytes + 1 extra byte for bit offset. Eg. write Int64 to %I0.2 -> Reads 9 bytes from simulation
            byte[] outputBytes = new byte[inputBytes.Length];
            inputBitArray.CopyTo(outputBytes, 0);
            return outputBytes;
        }

        // Extract a ulong value from input array, interpreting the value from binary as starting at `bitCount` and ranging until `bitOffset`
        protected static ulong ShiftRead(byte[] inputBytes, byte bitOffset, byte bitSize)
        {
            var inputBitArray = new BitArray(inputBytes);
            var outputBitArray = new BitArray(bitSize);
            for (int i=bitOffset, j = 0; i < bitOffset + bitSize; i++, j++)
                outputBitArray.Set(j, inputBitArray[i]);
            byte[] outputBytes = new byte[8];
            outputBitArray.CopyTo(outputBytes, 0);
            return BitConverter.ToUInt64(outputBytes);
        }
        
        public override string ToString() => $"{ByteOffset}.{BitOffset}, Bits={BitSize}, Type={DataType.ToHumanString()} ({DataType})";
    }
}