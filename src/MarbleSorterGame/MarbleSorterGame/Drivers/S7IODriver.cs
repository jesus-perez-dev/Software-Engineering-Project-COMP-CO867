using System;
using System.Collections.Generic;
using S7PLCSIM_Library;
using Siemens.Simatic.Simulation.Runtime;

namespace MarbleSorterGame
{
    public class S7IODriver : IIODriver
    {
        // Defines output keys of game entities
        static class QKeys
        {
            public const string TrapDoor1 = "TrapDoor1";
            public const string TrapDoor2 = "TrapDoor2";
            public const string TrapDoor3 = "TrapDoor3";
            public const string Gate = "Gate";
            public const string Conveyor = "Conveyor";
        }

        // Defines input keys of game entities
        static class IKeys
        {
            public const string TrapDoor1Open = "TrapDoor1Open";
            public const string TrapDoor2Open = "TrapDoor2Open";
            public const string TrapDoor3Open = "TrapDoor3Open";
            public const string BucketMotionSensor = "BucketMotionSensor";
            public const string GateOpen = "GateOpen";
            public const string GateClosed = "GateClosed";
            public const string ConveyorMotionSensor = "ConveyorMotionSensor";
            public const string WeightSensor = "WeightSensor";
            public const string ColorSensor = "ColorSensor";
        }

        private SimulationClient _client;

        private SDataValue _trapDoor1Open;

        public bool TrapDoor1Open
        {
            get => _trapDoor1Open.Bool;
            set => _trapDoor1Open.Bool = value;
        }

        private SDataValue _trapDoor1;

        public bool TrapDoor1
        {
            get => _trapDoor1.Bool;
            set => _trapDoor1.Bool = value;
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

        // Defines the addresses for the S7IO PLC Driver
        public S7IODriver(SimulationDriverOptions options, IList<IoMapConfiguration> iomaps)
        {
            _client = new SimulationClient(options.SimulationName);

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
            // Write simulation outputs
            _client.IAddress[IKeys.TrapDoor1Open].Write(_trapDoor1Open);
            _client.IAddress[IKeys.TrapDoor2Open].Write(_trapDoor2);
            _client.IAddress[IKeys.TrapDoor3Open].Write(_trapDoor3);
            _client.IAddress[IKeys.BucketMotionSensor].Write(_bucketMotionSensor);
            _client.IAddress[IKeys.GateOpen].Write(_gateOpen);
            _client.IAddress[IKeys.GateClosed].Write(_gateClosed);
            _client.IAddress[IKeys.ConveyorMotionSensor].Write(_conveyorMotionSensor);
            _client.IAddress[IKeys.WeightSensor].Write(_weightSensor);
            _client.IAddress[IKeys.ColorSensor].Write(_colorSensor);

            // Read simulation inputs
            _trapDoor1 = _client.QAddress[QKeys.TrapDoor1].Read();
            _trapDoor2 = _client.QAddress[QKeys.TrapDoor2].Read();
            _trapDoor3 = _client.QAddress[QKeys.TrapDoor3].Read();
            _gate = _client.QAddress[QKeys.Gate].Read();
            _conveyor = _client.QAddress[QKeys.Conveyor].Read();
        }
    }
}