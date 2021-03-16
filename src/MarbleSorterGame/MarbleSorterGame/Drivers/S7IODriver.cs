using S7PLCSIM_Library;
using Siemens.Simatic.Simulation.Runtime;

namespace MarbleSorterGame
{
    public class S7IODriver : IIODriver
    {
        static class QKeys
        {
            public const string TrapDoor1 = "TrapDoor1";
            public const string TrapDoor2 = "TrapDoor2";
            public const string TrapDoor3 = "TrapDoor3";
            public const string Gate = "Gate";
            public const string Conveyor = "Conveyor";
        }

        static class IKeys
        {
            public const string TrapDoor1Open = "TrapDoor1Open";
            public const string TrapDoor2Open = "TrapDoor2Open";
            public const string TrapDoor3Open = "TrapDoor3Open";
            public const string BucketSensor = "BucketSensor";
            public const string GateOpen = "GateOpen";
            public const string GateClosed = "GateClosed";
            public const string MotionSensor = "MotionSensor";
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

        private SDataValue _bucketSensor;

        public bool BucketSensor
        {
            get => _bucketSensor.Bool;
            set => _bucketSensor.Bool = value;
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

        private SDataValue _motionSensor;

        public bool MotionSensor
        {
            get => _motionSensor.Bool;
            set => _motionSensor.Bool = value;
        }

        private SDataValue _weightSensor;

        public bool WeightSensor
        {
            get => _weightSensor.Bool;
            set => _weightSensor.Bool = value;
        }

        private SDataValue _colorSensor;

        public byte ColorSensor
        {
            get => _colorSensor.UInt8;
            set => _colorSensor.UInt8 = value;
        }

        public S7IODriver(SimulationDriverOptions options)
        {
            _client = new SimulationClient(options.SimulationName);

            // Inputs to the simulation (game *Writes* these values)
            _client.IAddress.Add(IKeys.TrapDoor1Open, 0, 0, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.TrapDoor2Open, 0, 1, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.TrapDoor3Open, 0, 2, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.BucketSensor, 1, 0, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.GateOpen, 2, 0, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.GateClosed, 2, 1, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.MotionSensor, 3, 0, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.WeightSensor, 3, 1, EPrimitiveDataType.Bool);
            _client.IAddress.Add(IKeys.ColorSensor, 3, 2, EPrimitiveDataType.UInt8, 2);

            // Outputs from the simulation (game *Reads* these values)
            _client.QAddress.Add(QKeys.TrapDoor1, 0, 0, EPrimitiveDataType.Bool);
            _client.QAddress.Add(QKeys.TrapDoor2, 0, 1, EPrimitiveDataType.Bool);
            _client.QAddress.Add(QKeys.TrapDoor3, 0, 2, EPrimitiveDataType.Bool);
            _client.QAddress.Add(QKeys.Gate, 0, 3, EPrimitiveDataType.Bool);
            _client.QAddress.Add(QKeys.Conveyor, 0, 4, EPrimitiveDataType.Bool);

            _client.PowerOn();
        }

        public void Start()
        {
            _client.Run();
        }

        public void Stop()
        {
            _client.Stop();
        }

        public void Update()
        {
            // Write simulation outputs
            _client.IAddress[IKeys.TrapDoor1Open].Write(_trapDoor1Open);
            _client.IAddress[IKeys.TrapDoor2Open].Write(_trapDoor2);
            _client.IAddress[IKeys.TrapDoor3Open].Write(_trapDoor3);
            _client.IAddress[IKeys.BucketSensor].Write(_bucketSensor);
            _client.IAddress[IKeys.GateOpen].Write(_gateOpen);
            _client.IAddress[IKeys.GateClosed].Write(_gateClosed);
            _client.IAddress[IKeys.MotionSensor].Write(_motionSensor);
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