using System;
using System.Text;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    /// Represents a region of memory from the "input" or "%I" address area of the simulation.
    public class SimulationInput : SimulationAddress
    {
        public SimulationInput(string name, uint byteOffset, byte bitOffset, byte bitSize, EPrimitiveDataType dataType, IInstance instance) 
            : base(name, byteOffset, bitOffset, bitSize, dataType, instance) { }

        private SDataValue _value;
        
        /// The last value written to the simulation input area.
        ///
        /// See Also:
        ///     - <Write>
        public SDataValue Value => _value;

        /// Write a value to the simulation input area.
        ///
        /// Remarks:
        ///     SDataValue is a union struct provided by the Siemens C# client library. It is backed by a C++ struct from the
        ///     underlying C++ version of the Siemens library. Setting any of this structure's C# properties manipulates the same 64 bit region in memory
        ///     occupied by the C++ struct. The amount of bits actually written by this method are constrained to
        ///     the region of simulation memory defined by the simulation object bit/byte offset and size properties. As a consequence, if the data type of
        ///     the simulation address is (for example) EPrimitiveDataType.Bool, the bit size is determined to be 1 by default,
        ///     so writing (SDataValue.UInt64 = ulong.MaxValue) only writes the first bit of this 64-bit value to simulation memory. The additional 63 bits
        ///     in SDataValue are ignored and the previous state of simulation memory is preserved.
        ///
        ///     When writing new values, it is strongly suggested library users set the SDataField property corresponding to the EPrimitiveDataType enum
        ///     provided when creating/adding the address. This is not enforced by the type system or library, but it can help document the actual
        ///     size/memory range the address occupies and prevent writing values with C# types that are too large for the memory region (avoiding silent truncation).
        public void Write(SDataValue value)
        {
            byte[] input = Instance.InputArea.ReadBytes(ByteOffset, ByteSize);
            byte[] output = ShiftWrite(input, value.UInt64, BitOffset, BitSize);
            Instance.InputArea.WriteBytes(ByteOffset, output);
            _value = value; // Set last written value
        }
        
        public override string ToString() => $"%I{base.ToString()}";
    }
}