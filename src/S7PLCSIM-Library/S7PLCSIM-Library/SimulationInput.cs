using System;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public class SimulationInput : SimulationAddress
    {
        public SimulationInput(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, IInstance instance) : base(name, byteOffset, bitOffset, dataType, instance)
        {
        }

        public void Write(SDataValue value)
        {
            // Record last written value
            Value = value;
            
            byte[] bytes = ValueToBytes(value);
            // Only write a single bit
            if (DataType == EPrimitiveDataType.Bool)
            {
                Instance.InputArea.WriteBit(ByteOffset, BitOffset, bytes[0] > 0);
            }
            else // Otherwise write the whole byte area
            {
                Instance.InputArea.WriteBytes(ByteOffset, bytes);
            }
        }
        public override string ToString() => $"%I{base.ToString()}";
    }
}