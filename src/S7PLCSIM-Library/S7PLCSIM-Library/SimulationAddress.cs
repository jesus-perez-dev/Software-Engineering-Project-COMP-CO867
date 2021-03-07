using System;
using System.Collections;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public abstract class SimulationAddress
    {
        /// S7-PLCSim instance of the address
        public IInstance Instance { get; }

        /// Data type at address
        public EPrimitiveDataType DataType { get; }

        /// Byte value in Q{Byte}.{Bit}
        public uint ByteOffset { get; }

        /// Bit value in Q{Byte}.{Bit}
        public byte BitOffset { get; }

        /// Number of bits taken up by primitive type 'type'
        public byte BitSize { get; }
        
        /// Bit offset where this address object begins in memory
        public uint StartBit => ByteOffset * 8 + BitOffset;

        /// Bit offset where this address object ends in memory
        public uint EndBit => StartBit + BitSize - 1;
        
        /// Minimum number of bytes to read from simulation memory to retrieve full value
        public uint ByteSize => ((StartBit + BitSize - 1) / 8) + 1;

        /// Human friendly name to reference to IO address
        public string Name { get; }

        // If bit size is unspecified, use default bit size associated with dataType. eg UInt32 will have BitSize = 32
        public SimulationAddress(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, IInstance instance) 
            : this(name, byteOffset, bitOffset, dataType.ToBitSize(), dataType, instance) { }

        public SimulationAddress(string name, uint byteOffset, byte bitOffset, byte bitSize, EPrimitiveDataType dataType, IInstance instance)
        {
            Instance = instance;
            Name = name;
            ByteOffset = byteOffset;
            BitOffset = bitOffset;
            DataType = dataType;
            BitSize = bitSize;
            
            if (bitOffset > 7)
                throw new ArgumentException($"Invalid Bit Offset specified - Bit offset must be between 0-7. Given Bit Offset = {bitOffset}");
            
            if (bitSize > 64)
                throw new ArgumentException($"Invalid Bit Size specified - Bit size must not exceed 64 bits. Given Bit Size = {bitSize}");

            if (dataType == EPrimitiveDataType.Unspecific || dataType == EPrimitiveDataType.Struct)
                throw new ArgumentException($"Invalid Data Type - The following data types are unsupported: {EPrimitiveDataType.Unspecific.ToHumanString()}, {EPrimitiveDataType.Struct.ToHumanString()}");
        }
        
        
        /// <summary>
        /// Write the bits of `value` overtop bits of `input` starting at `bitOffset` until `bitSize`
        /// </summary>
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

        /// <summary>
        /// Extract a ulong value from input array, interpreting the value from binary as starting at `bitCount` and ranging until `bitOffset`
        /// </summary>
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

        protected byte[] ValueToBytes(SDataValue value)
        {
            switch (DataType)
            {
                case EPrimitiveDataType.Bool: return BitConverter.GetBytes(value.Bool);
                case EPrimitiveDataType.Char: return BitConverter.GetBytes(value.Char);
                case EPrimitiveDataType.Double: return BitConverter.GetBytes(value.Double);
                case EPrimitiveDataType.Float: return BitConverter.GetBytes(value.Float);
                case EPrimitiveDataType.Int8: return BitConverter.GetBytes(value.Int8);
                case EPrimitiveDataType.Int16: return BitConverter.GetBytes(value.Int16);
                case EPrimitiveDataType.Int32: return BitConverter.GetBytes(value.Int32);
                case EPrimitiveDataType.Int64: return BitConverter.GetBytes(value.Int64);
                case EPrimitiveDataType.UInt8: return BitConverter.GetBytes(value.UInt8);
                case EPrimitiveDataType.UInt16: return BitConverter.GetBytes(value.UInt16);
                case EPrimitiveDataType.UInt32: return BitConverter.GetBytes(value.UInt32);
                case EPrimitiveDataType.UInt64: return BitConverter.GetBytes(value.UInt64);
                case EPrimitiveDataType.WChar: return BitConverter.GetBytes(value.WChar);
                default:
                    throw new ArgumentException(
                        $"Cannot convert data type to byte array: {DataType.ToHumanString()} = {DataType}");
            }
        }
        
        public override string ToString() => $"{ByteOffset}.{BitOffset}, Bits={BitSize}, Type={DataType.ToHumanString()} ({DataType})";
    }
}