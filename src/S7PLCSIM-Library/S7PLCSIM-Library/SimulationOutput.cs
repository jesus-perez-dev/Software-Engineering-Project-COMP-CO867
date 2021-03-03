using System.ComponentModel.DataAnnotations;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    public class SimulationOutput : SimulationAddress
    {
        public SimulationOutput(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, IInstance instance) : base(name, byteOffset, bitOffset, dataType, instance)
        {
        }
    
        public SDataValue Read()
        {
            SDataValue value = new SDataValue();
            if (DataType == EPrimitiveDataType.Bool)
            {
                value.Bool = Instance.OutputArea.ReadBit(ByteOffset, BitOffset);
            }
            else
            {
                byte[] bytes = Instance.OutputArea.ReadBytes(ByteOffset, BitOffset);
                value = BytesToValue(bytes);
            }
            // Record last-read value and return it
            Value = value;
            return value;
        }
        
        public override string ToString() => $"%Q{base.ToString()}";
    }
}