using S7PLCSIM_Library;

namespace MarbleSorterGame
{
    public class S7IODriver : IIODriver
    {

        private SimulationClient _client;
        
        public bool TrapDoor1 { get; }
        public bool TrapDoor1Open { get; set; }
        public bool TrapDoor2 { get; }
        public bool TrapDoor2Open { get; set; }
        public bool TrapDoor3 { get; }
        public bool TrapDoor3Open { get; set; }
        public bool BucketSensor { get; set; }
        public bool Gate { get; set; }
        public bool GateOpen { get; set; }
        public bool GateClosed { get; set; }
        public bool Conveyor { get; set; }
        public bool MotionSensor { get; set; }
        public bool WeightSensor { get; set; }
        public byte ColorSensor { get; set; }

        public S7IODriver(SimulationDriverOptions options)
        {
            _client = new SimulationClient(options.SimulationName);
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
            throw new System.NotImplementedException();
        }
    }
}