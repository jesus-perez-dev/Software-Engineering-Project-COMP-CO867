using System;
using System.Linq;
using System.Threading;
using Siemens.Simatic.Simulation.Runtime;

namespace S7PLCSIM_Library
{
    /// Main interface to the Simulation API provided by the Mohawk S7PLCSIM Client Library. Manages simulation state and
    /// collections of input/output addresses for reading/writing C# primitive types to memory locations.
    public class SimulationClient
    {
        /// Simulation instance object provided by Siemens C# simulation client library.
        ///
        /// Remarks:
        ///     IInstance is provided by the official Siemens C# simulation library.
        ///     It is exposed here as an "escape hatch" to lower level simulation functionality if required.
        ///
        /// See Also:
        ///     PLCSIM Advanced V3.0 C# Client Library Documentation
        public IInstance Instance { get; }

        /// IReadOnlyDictionary compatible collection of input (%I) addresses registered with Mohawk simulation client library.
        public SimulationAddressContainer<SimulationInput> IAddress { get; }
        
        /// IReadOnlyDictionary compatible collection of output (%Q) addresses registered with Mohawk simulation client library.
        public SimulationAddressContainer<SimulationOutput> QAddress { get; }

        /// Construct a new SimulationClient instance. Creates a new simulation instance if 'simulationName'
        /// does not exist, otherwise connect to the existing instance.
        /// 
        /// Parameters:
        ///     simulationName - Simulation instance name as it appears in the "S7-PLCSIM Advanced V3.0" tool
        ///     cpuType - CPU type of the simulation instance. Defaults to "Unspecified CPU 1500"
        public SimulationClient(string simulationName, ECPUType cpuType = ECPUType.CPU1500_Unspecified)
        {
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
                Instance = SimulationRuntimeManager.RegisterInstance(cpuType, simulationName);

                if (Instance == null)
                {
                    throw new S7PlcSimLibraryException($"Could not register new simulation instance \"{simulationName}\", an unknown error occurred.");
                }
            }

            IAddress = new SimulationAddressContainer<SimulationInput>(Instance);
            QAddress = new SimulationAddressContainer<SimulationOutput>(Instance);
        }

        /// Operating state of the simulation, eg "Boot", "Off", "Stop", "Run" etc.
        /// 
        /// See Also:
        ///     - PLCSIM Advanced V3.0 C# Client Library Documentation
        public EOperatingState OperatingState => Instance.OperatingState;
        
        /// Power-on the simulation if currently powered off.
        public void PowerOn()
        {
            if (OperatingState == EOperatingState.Off)
            {
                ERuntimeErrorCode error = Instance.PowerOn();
                if (error != ERuntimeErrorCode.OK)
                {
                    throw new S7PlcSimLibraryException($"Error powering on the simulation, error code: {Enum.GetName(typeof(ERuntimeErrorCode), error)}");
                }
                
                // Do not return until we are through the "booting" phase
                // This prevents Run() from not working when we call PowerOn() followed by Run()
                while (OperatingState == EOperatingState.Booting)
                {
                    Thread.Sleep(100);
                }
            }
        }
        
        /// Power-off the simulation if currently powered on.
        public void PowerOff()
        {
            if (OperatingState != EOperatingState.Off)
                Instance.PowerOff();
        }
        
        /// Run the simulation if currently stopped.
        public void Run()
        {
            if (OperatingState == EOperatingState.Stop)
                Instance.Run();
        }

        /// Stop the simulation if currently running.
        public void Stop()
        {
            if (OperatingState == EOperatingState.Run)
                Instance.Stop();
        }
        
        /// Perform a memory-reset of the simulation.
        public void MemoryReset()
        {
            Instance.MemoryReset();
        }
    }
}