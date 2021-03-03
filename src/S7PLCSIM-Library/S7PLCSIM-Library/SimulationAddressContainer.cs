using System;
using System.Collections;
using System.Collections.Generic;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
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
        
        public void Remove(string name)
        {
            if (!_addresses.ContainsKey(name))
                throw new S7PlcSimLibraryException($"Cannot remove address \"{name}\": Address does not exist.");
            _addresses.Remove(name);
        }

        private void Add(string name, T address)
        {
            if (_addresses.ContainsKey(name))
                throw new S7PlcSimLibraryException($"Cannot add address \"{name}\": Address was previously added.");
            _addresses.Add(name, address);
        }

        public void Add(string name, uint byteOffset, byte bitOffset, EPrimitiveDataType dataType)
        {
            var address = (T?)Activator.CreateInstance(typeof(T), name, byteOffset, bitOffset, dataType, _instance);
            if (address == null)
                throw new S7PlcSimLibraryException($"Unable to initialize address of type: {typeof(T).FullName}");
            Add(name, address);
        }
        
        /* Delegate implementation of IReadOnlyDictionary to _addresses dict */
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _addresses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _addresses).GetEnumerator();
        }

        public int Count => _addresses.Count;

        public bool ContainsKey(string key)
        {
            return _addresses.ContainsKey(key);
        }

        public bool TryGetValue(string key, out T? value)
        {
            return _addresses.TryGetValue(key, out value);
        }

        //public T this[string key] => _addresses[key];
        public T this[string key]
        {
            get
            {
                if (!_addresses.ContainsKey(key))
                    throw new S7PlcSimLibraryException($"Address named \"{key}\" does not exist in address container");
                return _addresses[key];
            }
        } 
        
        public IEnumerable<string> Keys => _addresses.Keys;
        public IEnumerable<T> Values => _addresses.Values;
    }
}