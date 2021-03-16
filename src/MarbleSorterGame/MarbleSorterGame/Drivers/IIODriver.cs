namespace MarbleSorterGame
{
    /// <summary>
    /// Defines all of the Input and Output properties that determine decision making in our game.
    /// We implement this interface to create a "driver" which could determine inputs from, eg the PLC Simulation or simply from keyboard input
    /// </summary>
    public interface IIODriver
    {
        /// Output: Direction of Motor - Whether trap door 1 is moving toward "Open" state
        public bool TrapDoor1 { get; }

        /// Input: Sensor to determine if trap door 1 has completely opened
        public bool TrapDoor1Open { set; }

        /// Input: Sensor to determine if trap door 1 has completely closed
        public bool TrapDoor1Closed { set; }

        /// Output: Direction of Motor - Whether trap door 2 is moving toward "Open" state
        public bool TrapDoor2 { get; }

        /// Input: Sensor to determine if trap door 2 has completely opened
        public bool TrapDoor2Open { set; }

        /// Input: Sensor to determine if trap door 2 has completely closed
        public bool TrapDoor2Closed { set; }

        /// Output: Direction of Motor - Whether trap door 3 is moving toward "Open" state
        public bool TrapDoor3 { get; }

        /// Input: Sensor to determine if trap door 3 has completely opened
        public bool TrapDoor3Open { set; }

        /// Input: Sensor to determine if trap door 3 has completely closed
        public bool TrapDoor3Closed { set; }

        /// Input: Motion Sensor Detecting Falling Marble - Marble has fallen in into bucket 1 (toggles on and off)
        public bool BucketSensor1 { set; }

        /// Input: Motion Sensor Detecting Falling Marble - Marble has fallen in into bucket 2 (toggles on and off)
        public bool BucketSensor2 { set; }

        /// Input: Motion Sensor Detecting Falling Marble - Marble has fallen in into bucket 3 (toggles on and off)
        public bool BucketSensor3 { set; }

        /// Output: Whether gate is moving toward "Open" state
        public bool Gate { get; set; }

        /// Input: Sensor to determine if gate has completely opened
        public bool GateOpened { set; }

        /// Input: Sensor to determine if gate has completely closed
        public bool GateClosed { set; }

        /// Output: Whether conveyor is moving or not
        public bool Conveyor { get; set; }

        /// Input: Sensor to determine if marble is on conveyor (any segment past gate)
        public bool MotionSensor { set; }

        /// Input: Sensor to determine if marble is heavy or not
        public bool WeightSensor { set; }

        /// Input: Sensor to determine the colour of the marble
        public byte ColorSensor { set; }

        public void Update();
    }
}