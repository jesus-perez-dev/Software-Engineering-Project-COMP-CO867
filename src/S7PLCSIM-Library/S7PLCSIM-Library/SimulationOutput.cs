using System.ComponentModel.DataAnnotations;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public class SimulationOutput : SimulationAddress
    {
        public SimulationOutput(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, IInstance instance) 
            : base(name, byteOffset, bitOffset, dataType, instance) { }

        private SDataValue _value;
        public SDataValue Value => _value;

        public SDataValue Read()
        {
            byte[] bytes = Instance.OutputArea.ReadBytes(ByteOffset, ByteSize);
            _value.UInt64 = ShiftRead(bytes, BitOffset, BitSize);
            return _value;
        }
        
        public override string ToString() => $"%Q{base.ToString()}";
    }
}