using System;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public abstract class SimulationAddress
    {
        /// S7-PLCSim instance of the address
        public IInstance Instance { get; }

        /// Data type at address
        public EPrimitiveDataType DataType { get; }

        /// String version of DataType - Helps with error messages
        protected string DataTypeName { get; }

        /// Byte value in Q{Byte}.{Bit}
        public uint ByteOffset { get; }

        /// Bit value in Q{Byte}.{Bit}
        public byte BitOffset { get; }

        /// Number of bits taken up by primitive type 'type'
        public uint Size { get; }
        
        /// Bit offset where this address object begins in memory
        public uint StartBit { get; }
        
        /// Bit offset where this address object ends in memory
        public uint EndBit { get; }

        /// Human friendly name to reference to IO address
        public string Name { get; }

        /// Current value of IO address
        public SDataValue Value { get; protected set; }
        
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
                        $"Cannot convert data type to byte array: {DataTypeName} = {DataType}");
            }
        }

        protected SDataValue BytesToValue(byte[] bytes)
        {
            SDataValue value = new SDataValue();

            // TODO: Confirm `BitConverter` does the right thing WRT endianness compared to the simulator
            switch (DataType)
            {
                case EPrimitiveDataType.Bool:
                    value.Bool = BitConverter.ToBoolean(bytes);
                    break;
                case EPrimitiveDataType.Char:
                    value.Char = (sbyte) bytes[0];
                    break;
                case EPrimitiveDataType.Double:
                    value.Double = BitConverter.ToDouble(bytes);
                    break;
                case EPrimitiveDataType.Float:
                    value.Float = BitConverter.ToSingle(bytes);
                    break;
                case EPrimitiveDataType.Int8:
                    value.Int8 = (sbyte) bytes[0];
                    break;
                case EPrimitiveDataType.Int16:
                    value.Int16 = BitConverter.ToInt16(bytes);
                    break;
                case EPrimitiveDataType.Int32:
                    value.Int32 = BitConverter.ToInt32(bytes);
                    break;
                case EPrimitiveDataType.Int64:
                    value.Int64 = BitConverter.ToInt64(bytes);
                    break;
                case EPrimitiveDataType.UInt8:
                    value.UInt8 = bytes[0];
                    break;
                case EPrimitiveDataType.UInt16:
                    value.UInt16 = BitConverter.ToUInt16(bytes);
                    break;
                case EPrimitiveDataType.UInt32:
                    value.UInt32 = BitConverter.ToUInt32(bytes);
                    break;
                case EPrimitiveDataType.UInt64:
                    value.UInt64 = BitConverter.ToUInt64(bytes);
                    break;
                case EPrimitiveDataType.WChar:
                    value.WChar = BitConverter.ToChar(bytes);
                    break;
                default:
                    throw new ArgumentException(
                        $"Cannot convert byte array to desired Type: {DataTypeName} = {DataType}, Bytes = {{{string.Join(",", bytes)}}}");
            }

            return value;
        }

        // Note: Types 'EPrimitiveDataType.Struct' and 'EPrimitiveDataType.Unspecific' not supported
        public SimulationAddress(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, IInstance instance)
        {
            Instance = instance;
            Name = name;
            ByteOffset = byteOffset;
            BitOffset = bitOffset;
            DataType = dataType;
            DataTypeName = Enum.GetName(typeof(EPrimitiveDataType), dataType);

            switch (dataType)
            {
                case EPrimitiveDataType.Bool:
                    Size = 1;
                    break;
                case EPrimitiveDataType.Char:
                case EPrimitiveDataType.Int8:
                case EPrimitiveDataType.UInt8:
                    Size = 8;
                    break;
                case EPrimitiveDataType.Int16:
                case EPrimitiveDataType.UInt16:
                case EPrimitiveDataType.WChar:
                    Size = 16;
                    break;
                case EPrimitiveDataType.UInt32:
                case EPrimitiveDataType.Int32:
                case EPrimitiveDataType.Float:
                    Size = 32;
                    break;
                case EPrimitiveDataType.Double:
                case EPrimitiveDataType.Int64:
                case EPrimitiveDataType.UInt64:
                    Size = 64;
                    break;
                default:
                    throw new ArgumentException(
                        $"Invalid primitive data type for simulation address: {DataTypeName} = {DataType}");
            }

            StartBit = byteOffset * 8 + bitOffset;
            EndBit = StartBit + Size;
            
            // TODO: Throw an exception bitOffset > 0 when dataType != EPrimitiveDataType.Bool
            if (dataType != EPrimitiveDataType.Bool && bitOffset > 0)
            {
                throw new ArgumentException(
                    @$"Using non-zero bit offset for data types other than ""Bool"" is unsupported. Data-Type = {DataTypeName} = {DataType}, Bit-Offset = {BitOffset}");
            }
        }
        public override string ToString() => $"{ByteOffset}.{BitOffset}, Bits={Size}, Type={DataTypeName} ({DataType})";
    }
}