using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Siemens.Simatic.Simulation.Runtime;

/* Problem: I want to expose a key-value interface for IAddress/QAddress, but I dont want (1) library users to mutate them and (2) it to be awkward/difficult to update for us internally.
 * - also it would be nice if we weren't stuck with a single type, ReadOnlyDictionary is very specific, it would be best if we had our own interface here in case we want to change the underlying types/implementation
 */

namespace S7PLCSIM_Library
{
    public class SimulationClient
    {
        public IInstance Instance { get; }

        public SimulationAddressContainer<SimulationInput> IAddress { get; }
        public SimulationAddressContainer<SimulationOutput> QAddress { get; }

        // Only expose read-only dictionaries publically. Add and remove operations must go through the class
        public SimulationClient(string simulationName)
        {
            // TODO: Avoid hard-coding CPU Type to 1500_Unspecified
            // TODO: Avoid logging directly to Console, consider using ILogger and/or log to file
            
            if (!SimulationRuntimeManager.IsInitialized)
            {
                throw new S7PlcSimLibraryException("Simulation Runtime Manager is uninitialized. Make sure you have S7-PLCSIM Advanced V3 (Update 2) installed and running");
            }

            bool instanceExists = SimulationRuntimeManager.RegisteredInstanceInfo
                .Select(info => info.Name)
                .Contains(simulationName);

            if (instanceExists)
            {
                Console.WriteLine($"Connecting to existing instance: {simulationName}");
                Instance = SimulationRuntimeManager.CreateInterface(simulationName);
            }
            else
            {
                Console.WriteLine($"Registering new simulation instance: {simulationName}");
                Instance = SimulationRuntimeManager.RegisterInstance(ECPUType.CPU1500_Unspecified, simulationName);

                if (Instance == null)
                {
                    throw new S7PlcSimLibraryException($"Could not register new simulation instance \"{simulationName}\", an unknown error occurred.");
                }
            }

            IAddress = new SimulationAddressContainer<SimulationInput>(Instance);
            QAddress = new SimulationAddressContainer<SimulationOutput>(Instance);
        }
        
        
        private static void RemoveAddress<T>(Dictionary<string, T> dict, string name)
        {
            if (!dict.ContainsKey(name))
                throw new S7PlcSimLibraryException($"Cannot remove address \"{name}\": Address does not exist.");
            dict.Remove(name);
        }

        private static void AddAddress<T>(Dictionary<string, T> dict, string name, T address)
        {
            if (dict.ContainsKey(name))
                throw new S7PlcSimLibraryException($"Cannot add address \"{name}\": Address was previously added.");
            dict.Add(name, address);
        }
        
        public void PowerOn()
        {
            ERuntimeErrorCode error = Instance.PowerOn();
            if (error != ERuntimeErrorCode.OK)
            {
                throw new S7PlcSimLibraryException($"Error powering on the simulation, error code: {Enum.GetName(typeof(ERuntimeErrorCode), error)}");
            }
        }
        
        public void PowerOff()
        {
            Instance.PowerOff();
        }

        public void Run()
        {
            Instance.Run();
        }

        public void Stop()
        {
            Instance.Stop();
        }
        
        public void MemoryReset()
        {
            Instance.MemoryReset();
        }
    }
}