using System;
using System.Collections.Generic;
using MarbleSorterGame.Drivers;
using S7PLCSIM_Library;
using Siemens.Simatic.Simulation.Runtime;

namespace MarbleSorterGame
{
    public class S7IODriver : IIODriver
    {
        private SimulationClient _client;

        private SDataValue _trapDoor1;

        public bool TrapDoor1
        {
            get => _trapDoor1.Bool;
            set => _trapDoor1.Bool = value;
        }

        private SDataValue _trapDoor1Open;

        public bool TrapDoor1Open
        {
            get => _trapDoor1Open.Bool;
            set => _trapDoor1Open.Bool = value;
        }

        private SDataValue _trapDoor1Closed;

        public bool TrapDoor1Closed
        {
            get => _trapDoor1Closed.Bool;
            set => _trapDoor1Closed.Bool = value;
        }
        
        private SDataValue _trapDoor2;

        public bool TrapDoor2
        {
            get => _trapDoor2.Bool;
            set => _trapDoor2.Bool = value;
        }

        private SDataValue _trapDoor2Open;

        public bool TrapDoor2Open
        {
            get => _trapDoor2Open.Bool;
            set => _trapDoor2Open.Bool = value;
        }

        private SDataValue _trapDoor2Closed;

        public bool TrapDoor2Closed
        {
            get => _trapDoor2Closed.Bool;
            set => _trapDoor2Closed.Bool = value;
        }

        private SDataValue _trapDoor3;

        public bool TrapDoor3
        {
            get => _trapDoor3.Bool;
            set => _trapDoor3.Bool = value;
        }

        private SDataValue _trapDoor3Open;

        public bool TrapDoor3Open
        {
            get => _trapDoor3Open.Bool;
            set => _trapDoor3Open.Bool = value;
        }

        private SDataValue _trapDoor3Closed;

        public bool TrapDoor3Closed
        {
            get => _trapDoor3Closed.Bool;
            set => _trapDoor3Closed.Bool = value;
        }

        private SDataValue _bucketMotionSensor;

        public bool BucketMotionSensor
        {
            get => _bucketMotionSensor.Bool;
            set => _bucketMotionSensor.Bool = value;
        }

        private SDataValue _gate;

        public bool Gate
        {
            get => _gate.Bool;
            set => _gate.Bool = value;
        }

        private SDataValue _gateOpen;

        public bool GateOpen
        {
            get => _gateOpen.Bool;
            set => _gateOpen.Bool = value;
        }

        private SDataValue _gateClosed;

        public bool GateClosed
        {
            get => _gateClosed.Bool;
            set => _gateClosed.Bool = value;
        }

        private SDataValue _conveyor;

        public bool Conveyor
        {
            get => _conveyor.Bool;
            set => _conveyor.Bool = value;
        }

        private SDataValue _conveyorMotionSensor;

        public bool ConveyorMotionSensor
        {
            get => _conveyorMotionSensor.Bool;
            set => _conveyorMotionSensor.Bool = value;
        }

        private SDataValue _weightSensor;

        public byte PressureSensor
        {
            get => _weightSensor.UInt8;
            set => _weightSensor.UInt8 = value;
        }

        private SDataValue _colorSensor;

        public byte ColorSensor
        {
            get => _colorSensor.UInt8;
            set => _colorSensor.UInt8 = value;
        }

        private ulong _updateCount;
        private readonly uint _updateInterval;

        // Defines the addresses for the S7IO PLC Driver
        public S7IODriver(SimulationDriverOptions options, IList<IoMapConfiguration> iomaps)
        {
            _client = new SimulationClient(options.SimulationName);
            _updateCount = 0;
            _updateInterval = Math.Max(1, options.UpdateInterval);

            foreach (var map in iomaps)
            {
                if (map.MemoryArea == SimulationMemoryArea.I)
                    _client.IAddress.Add(map.EntityName, map.Byte, map.Bit, Enum.Parse<EPrimitiveDataType>(map.Type), map.BitSize);
                else if (map.MemoryArea == SimulationMemoryArea.Q)
                    _client.QAddress.Add(map.EntityName, map.Byte, map.Bit, Enum.Parse<EPrimitiveDataType>(map.Type), map.BitSize);
            }
            
            //_client.PowerOn();
        }
        
        // Sets run state of the client
        public void SetRunState(bool run)
        {
            if (run)
                _client.Run();
            else
                _client.Stop();
        }
        
        // Updates driver values by writing/reading from the game state
        public void Update()
        {
            if (_updateCount % _updateInterval == 0)
            {
                // Write simulation outputs
                _client.IAddress[IOKeys.I.TrapDoor1Open].Write(_trapDoor1Open);
                _client.IAddress[IOKeys.I.TrapDoor2Open].Write(_trapDoor2Open);
                _client.IAddress[IOKeys.I.TrapDoor3Open].Write(_trapDoor3Open);
                _client.IAddress[IOKeys.I.TrapDoor1Closed].Write(_trapDoor1Closed);
                _client.IAddress[IOKeys.I.TrapDoor2Closed].Write(_trapDoor2Closed);
                _client.IAddress[IOKeys.I.TrapDoor3Closed].Write(_trapDoor3Closed);
                _client.IAddress[IOKeys.I.BucketMotionSensor].Write(_bucketMotionSensor);
                _client.IAddress[IOKeys.I.GateOpen].Write(_gateOpen);
                _client.IAddress[IOKeys.I.GateClosed].Write(_gateClosed);
                _client.IAddress[IOKeys.I.ConveyorMotionSensor].Write(_conveyorMotionSensor);
                _client.IAddress[IOKeys.I.WeightSensor].Write(_weightSensor);
                _client.IAddress[IOKeys.I.ColorSensor].Write(_colorSensor);

                // Read simulation inputs
                _trapDoor1 = _client.QAddress[IOKeys.Q.TrapDoor1].Read();
                _trapDoor2 = _client.QAddress[IOKeys.Q.TrapDoor2].Read();
                _trapDoor3 = _client.QAddress[IOKeys.Q.TrapDoor3].Read();
                _gate = _client.QAddress[IOKeys.Q.Gate].Read();
                _conveyor = _client.QAddress[IOKeys.Q.Conveyor].Read();
            }
            _updateCount++;
        }
    }
}