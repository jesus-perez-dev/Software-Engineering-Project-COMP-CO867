using System.ComponentModel.DataAnnotations;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    /// Represents a region of memory in the "output" or "%Q" address area of the simulation.
    public class SimulationOutput : SimulationAddress
    {
        public SimulationOutput(string name, uint byteOffset, byte bitOffset, byte bitSize, EPrimitiveDataType dataType, IInstance instance) 
            : base(name, byteOffset, bitOffset, bitSize, dataType, instance) { }

        private SDataValue _value;
        
        /// Get last value read from simulation memory
        public SDataValue Value => _value;

        /// Read a value from the simulation memory address.
        ///
        /// See Also:
        ///
        ///     - <SimulationInput.Write> for a discussion of SDataValue type behavior.
        public SDataValue Read()
        {
            byte[] bytes = Instance.OutputArea.ReadBytes(ByteOffset, ByteSize);
            _value.UInt64 = ShiftRead(bytes, BitOffset, BitSize);
            return _value;
        }
        
        public override string ToString() => $"%Q{base.ToString()}";
    }
}