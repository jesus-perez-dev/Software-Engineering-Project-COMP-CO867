namespace MarbleSorterGame
{
    public class S7IODriver : IIODriver
    {
        public bool TrapDoor1 { get; }
        public bool TrapDoor1Open { get; set; }
        public bool TrapDoor1Closed { get; set; }
        public bool TrapDoor2 { get; }
        public bool TrapDoor2Open { get; set; }
        public bool TrapDoor2Closed { get; set; }
        public bool TrapDoor3 { get; }
        public bool TrapDoor3Open { get; set; }
        public bool TrapDoor3Closed { get; set; }
        public bool BucketSensor1 { get; set; }
        public bool BucketSensor2 { get; set; }
        public bool BucketSensor3 { get; set; }
        public bool Gate { get; set; }
        public bool GateOpened { get; set; }
        public bool GateClosed { get; set; }
        public bool Conveyor { get; set; }
        public bool MotionSensor { get; set; }
        public bool WeightSensor { get; set; }
        public byte ColorSensor { get; set; }
        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}