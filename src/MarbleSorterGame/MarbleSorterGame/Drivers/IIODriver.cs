namespace MarbleSorterGame
{
    /// <summary>
    /// Defines all of the Input and Output properties that determine decision making in our game.
    /// We implement this interface to create a "driver" which could determine inputs from, eg the PLC Simulation or simply from keyboard input
    /// </summary>
    public interface IIODriver
    {
        /// <summary>
        /// Input: Sensor to determine if trap door 1 has completely opened
        /// </summary>
        public bool TrapDoor1Open { set; }
        
        /// <summary>
        /// Output: 1 opens trap door, 0 closes trap door. Automatically closes
        /// </summary>
        public bool TrapDoor1 { get; }

        /// <summary>
        /// Input: Sensor to determine if trap door 2 has completely opened
        /// </summary>
        public bool TrapDoor2Open { set; }
        
        /// <summary>
        /// Output: 1 opens trap door, 0 closes trap door. Automatically closes
        /// </summary>
        public bool TrapDoor2 { get; }

        /// <summary>
        /// Input: Sensor to determine if trap door 3 has completely opened
        /// </summary>
        public bool TrapDoor3Open { set; }
        
        /// <summary>
        /// Output: 1 opens trap door, 0 closes trap door. Automatically closes
        /// </summary>
        public bool TrapDoor3 { get; }

        /// <summary>
        /// Input: Motion Sensor Detecting Falling Marble - Marble has fallen in into bucket 1 (toggles on and off)
        /// </summary>
        public bool BucketMotionSensor { get; set; }

        /// <summary>
        /// Output: Whether gate is moving toward "Open" state
        /// </summary>
        public bool Gate { get; set; }

        /// <summary>
        /// Input: Sensor to determine if gate has completely opened
        /// </summary>
        public bool GateOpen { set; }

        /// <summary>
        /// Input: Sensor to determine if gate has completely closed
        /// </summary>
        public bool GateClosed { set; }

        /// <summary>
        /// Output: Whether conveyor is moving or not
        /// </summary>
        public bool Conveyor { get; set; }

        /// <summary>
        /// Input: Sensor to determine if marble is on conveyor (any segment past gate)
        /// </summary>
        public bool ConveyorMotionSensor { set; get; }

        /// <summary>
        /// Input: Sensor to determine if marble is heavy or not
        /// </summary>
        public byte PressureSensor { get; set; }

        /// <summary>
        /// Input: Sensor to determine the colour of the marble
        /// </summary>
        public byte ColorSensor { get; set; }

        /// <summary>
        /// Updates driver to communicate to/from the game
        /// </summary>
        public void Update();
    }
}