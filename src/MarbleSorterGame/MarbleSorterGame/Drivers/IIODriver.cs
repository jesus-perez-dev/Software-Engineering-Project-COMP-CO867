namespace MarbleSorterGame
{
    // Defines all of the Input and Output properties that determine decision making in our game.
    // We implement this interface to create a "driver" which could determine inputs from, eg the PLC Simulation or simply from keyboard input
    public interface IIODriver
    {
        // Input: Sensor to determine if trap door 1 has completely opened
        public bool TrapDoor1Open { get; set;  }

        // Input: Sensor to determine if trap door 1 has completely closed
        public bool TrapDoor1Closed { get; set;  }

        // Output: 1 opens trap door, 0 closes trap door. Automatically closes
        public bool TrapDoor1 { get; set;  }

        // Input: Sensor to determine if trap door 2 has completely opened
        public bool TrapDoor2Open { get; set; }

        // Input: Sensor to determine if trap door 2 has completely closed
        public bool TrapDoor2Closed { get; set;  }
       
        // Output: 1 opens trap door, 0 closes trap door. Automatically closes
        public bool TrapDoor2 { get; set; }

        // Input: Sensor to determine if trap door 3 has completely opened
        public bool TrapDoor3Open { get; set; }

        // Input: Sensor to determine if trap door 3 has completely closed
        public bool TrapDoor3Closed { get; set;  }
        
        // Output: 1 opens trap door, 0 closes trap door. Automatically closes
        public bool TrapDoor3 { get; set; }

        // Input: Motion Sensor Detecting Falling Marble - Marble has fallen in into bucket 1 (toggles on and off)
        public bool BucketMotionSensor { get; set; }

        // Output: Whether gate is moving toward "Open" state
        public bool Gate { get; set; }

        // Input: Sensor to determine if gate has completely opened
        public bool GateOpen { get; set; }

        // Input: Sensor to determine if gate has completely closed
        public bool GateClosed { get; set; }

        // Output: Whether conveyor is moving or not
        public bool Conveyor { get; set; }

        // Input: Sensor to determine if marble is on conveyor (any segment past gate)
        public bool ConveyorMotionSensor { set; get; }

        // Input: Sensor to determine if marble is heavy or not
        public byte PressureSensor { get; set; }

        // Input: Sensor to determine the colour of the marble
        public byte ColorSensor { get; set; }

        // Updates driver to communicate to/from the game
        public void Update();

        // Enable or disable the driver (if supported)
        public void SetActive(bool active);
    }
}