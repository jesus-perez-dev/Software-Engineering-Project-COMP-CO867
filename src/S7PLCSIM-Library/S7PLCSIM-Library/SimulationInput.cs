using System;
using System.Text;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public class SimulationInput : SimulationAddress
    {
        public SimulationInput(string name, uint byteOffset, byte bitOffset, byte bitSize, EPrimitiveDataType dataType, IInstance instance) 
            : base(name, byteOffset, bitOffset, bitSize, dataType, instance) { }

        private SDataValue _value;
        public SDataValue Value => _value;

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