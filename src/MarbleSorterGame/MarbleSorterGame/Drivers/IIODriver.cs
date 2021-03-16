namespace MarbleSorterGame
{
    /// <summary>
    /// Defines all of the Input and Output properties that determine decision making in our game.
    /// We implement this interface to create a "driver" which could determine inputs from, eg the PLC Simulation or simply from keyboard input
    /// </summary>
    public interface IIODriver
    {
        /// Input: Sensor to determine if trap door 1 has completely opened
        public bool TrapDoor1Open { set; }
        
        /// Output: 1 opens trap door, 0 closes trap door. Automatically closes
        public bool TrapDoor1 { get; }

        /// Input: Sensor to determine if trap door 2 has completely opened
        public bool TrapDoor2Open { set; }
        
        /// Output: 1 opens trap door, 0 closes trap door. Automatically closes
        public bool TrapDoor2 { get; }

        /// Input: Sensor to determine if trap door 3 has completely opened
        public bool TrapDoor3Open { set; }
        
        /// Output: 1 opens trap door, 0 closes trap door. Automatically closes
        public bool TrapDoor3 { get; }

        /// Input: Motion Sensor Detecting Falling Marble - Marble has fallen in into bucket 1 (toggles on and off)
        public bool BucketSensor { set; }

        /// Output: Whether gate is moving toward "Open" state
        public bool Gate { get; set; }

        /// Input: Sensor to determine if gate has completely opened
        public bool GateOpen { set; }

        /// Input: Sensor to determine if gate has completely closed
        public bool GateClosed { set; }

        /// Output: Whether conveyor is moving or not
        public bool Conveyor { set; }

        /// Input: Sensor to determine if marble is on conveyor (any segment past gate)
        public bool MotionSensor { set; }

        /// Input: Sensor to determine if marble is heavy or not
        public bool WeightSensor { set; }

        /// Input: Sensor to determine the colour of the marble
        public byte ColorSensor { set; }

        public void Update();
    }
}