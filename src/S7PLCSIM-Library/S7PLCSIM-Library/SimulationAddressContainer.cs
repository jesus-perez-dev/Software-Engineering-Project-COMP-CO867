using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    /// Represents a collection of simulation input or output addresses.
    /// Implements IReadOnlyDictionary for performing name-based lookups on address objects.
    ///
    /// Remarks:
    ///     <SimulationAddress> can be retrieved from this container using C# dictionary lookup syntax, ie
    ///     "collection[key]", where "key" is a "name" value uniquely identifying an address.
    ///
    /// See:
    ///     - <Add>
    ///     - <Remove>
    public class SimulationAddressContainer<T> : IReadOnlyDictionary<string, T> 
        where T : SimulationAddress
    {
        private Dictionary<string, T> _addresses;
        private IInstance _instance;
        
        internal SimulationAddressContainer(IInstance instance) : 
            this(instance, new Dictionary<string, T>()) { }
        
        internal SimulationAddressContainer(IInstance instance, IDictionary<string, T> dict)
        {
            _instance = instance;
            _addresses = new Dictionary<string, T>(dict);
        }
        
        /// Create new SimulationAddress object and add it to collection.
        ///
        /// Parameters:
        ///     name - String value uniquely identifying the address within the collection.
        ///            Used as the "key" lookup value for the address object. Standard dictionary C# lookup operator []
        ///            can be used to retrieve the newly created address instance using this value.
        ///     byteOffset - Byte value in simulation memory where the address begins, as in "%Q{Byte}.{Bit}"
        ///     bitOffset - Same as 'byteOffset' but for the {Bit} value
        ///     dataType - Type of data stored at this address. Examples include: UInt8, UInt32, UInt64,
        ///     bitSize - Number of bits occupied by the address in memory. Must be less than 64 or an exception will raise.
        ///               When left unspecified or '0' the size is automatically determined based on dataType.
        ///               EG. dataType 'UInt8' causes the address to occupy 8 bits of memory.
        ///
        /// Exceptions:
        ///
        ///     S7PlcSimLibraryException - When address identified by "name" already exists in the collection
        ///     S7PlcSimLibraryException - When "byteOffset" or "bitOffset" values cause the address to overlap an existing address in the collection
        ///     S7PlcSimLibraryException - When bitOffset value is outside the accepted range (0-7)
        ///     S7PlcSimLibraryException - When an invalid dataType is used, such as 'Struct' or 'Unspecified'
        ///     S7PlcSimLibraryException - When bitSize exceeds 64
        /// 
        /// See Also:
        ///     - <Remove>
        public void Add(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType, byte bitSize = 0)
        {
            bitSize = bitSize == 0 ? dataType.ToBitSize() : bitSize; // Use bitsize from datatype by default
            var address = (T?)Activator.CreateInstance(typeof(T), name, byteOffset, bitOffset, bitSize, dataType, _instance);
            
            if (address == null)
                throw new S7PlcSimLibraryException($"Unable to initialize address of type: {typeof(T).FullName}");
            
            if (Overlaps(address, out T? overlapper))
                throw new S7PlcSimLibraryException($"New address \"{name}\" overlaps previously registered address in simulation memory: \"{overlapper?.Name}\"");
            
            if (_addresses.ContainsKey(name))
                throw new S7PlcSimLibraryException($"Cannot add address \"{name}\": Address was previously added.");
            
            _addresses.Add(name, address);
        }
        
        /// Remove simulation address identified by 'name' from collection.
        ///
        /// See Also:
        ///     - <Add>
        public void Remove(string name)
        {
            if (!_addresses.ContainsKey(name))
                throw new S7PlcSimLibraryException($"Cannot remove address \"{name}\": Address does not exist.");
            _addresses.Remove(name);
        }

        // Determine if address overlaps a previously-existing address in collection
        private bool Overlaps(T address, out T? overlapper)
        {
            overlapper = null;
            
            foreach (var existing in _addresses.Values)
            {
                // Address start bit lies between existing address
                bool startBitBetween =
                    (address.StartBit >= existing.StartBit) &&
                    (address.StartBit <= existing.EndBit);

                // Address end bit lies between existing address
                bool endBitBetween =
                    (address.EndBit >= existing.StartBit) &&
                    (address.EndBit <= existing.EndBit);

                if (startBitBetween || endBitBetween)
                {
                    overlapper = existing;
                    return true;
                }
            }

            return false;
        }
        
        /// Implementation of IReadOnlyDictionary
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _addresses.GetEnumerator();
        }

        /// Implementation of IReadOnlyDictionary
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _addresses).GetEnumerator();
        }

        /// Implementation of IReadOnlyDictionary
        public int Count => _addresses.Count;

        /// Implementation of IReadOnlyDictionary
        public bool ContainsKey(string key)
        {
            return _addresses.ContainsKey(key);
        }

        /// Implementation of IReadOnlyDictionary
        public bool TryGetValue(string key, out T? value)
        {
            return _addresses.TryGetValue(key, out value);
        }

        /// Implementation of IReadOnlyDictionary
        public T this[string key]
        {
            get
            {
                if (!_addresses.ContainsKey(key))
                    throw new S7PlcSimLibraryException($"Address named \"{key}\" does not exist in address container");
                return _addresses[key];
            }
        } 
        
        /// Implementation of IReadOnlyDictionary
        public IEnumerable<string> Keys => _addresses.Keys;
        
        /// Implementation of IReadOnlyDictionary
        public IEnumerable<T> Values => _addresses.Values;
    }
}