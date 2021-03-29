using SFML.Window;

namespace MarbleSorterGame
{
    /// <summary>
    /// Driver for communicating with the game through a keyboard.
    /// Full game will use the S7IO Driver class, this driver is for early debug purposes.
    /// </summary>
    public class KeyboardIODriver : IIODriver
    {
        public bool TrapDoor1 { get; set;  }
        public bool TrapDoor1Open { get; set; }
        public bool TrapDoor2 { get; set;  }
        public bool TrapDoor2Open { get; set; }
        public bool TrapDoor3 { get; set;  }
        public bool TrapDoor3Open { get; set; }
        public bool BucketMotionSensor { get; set; }
        public bool Gate { get; set; }
        public bool GateOpen { get; set; }
        public bool GateClosed { get; set; }
        public bool Conveyor { get; set; }
        public bool ConveyorMotionSensor { get; set; }
        public byte PressureSensor { get; set; }
        public byte ColorSensor { get; set; }

        /// This implementation does not update programatically, it uses keyboard input. See: UpdateByKey()
        public void Update()
        {
        }

        /// Associates and Updates keyboard inputs to game inputs
        public void UpdateByKey(KeyEventArgs key)
        {
            // Outputs: TrapDoor1, TrapDoor2, TrapDoor3, Gate, Conveyer
            if (key.Code == Keyboard.Key.Num1)
                Gate = !Gate;
            
            if (key.Code == Keyboard.Key.Num2)
                Conveyor = !Conveyor;
            
            if (key.Code == Keyboard.Key.Num3)
                TrapDoor1 = !TrapDoor1;
            
            if (key.Code == Keyboard.Key.Num4)
                TrapDoor2 = !TrapDoor2;
            
            if (key.Code == Keyboard.Key.Num5)
                TrapDoor3 = !TrapDoor3;
        }
        
        /// Shows overall game state
        public override string ToString()
        {
            return string.Join("\n", new string[]
            {
                $"TrapDoor1 {TrapDoor1}",
                $"TrapDoor1Open {TrapDoor1Open}",
                $"TrapDoor2 {TrapDoor2}",
                $"TrapDoor2Open {TrapDoor2Open}",
                $"TrapDoor3 {TrapDoor3}",
                $"TrapDoor3Open {TrapDoor3Open}",
                $"Gate {Gate}",
                $"GateClosed {GateClosed}",
                $"Conveyor {Conveyor}",
                $"MotionSensor {ConveyorMotionSensor}",
                $"WeightSensor {PressureSensor}",
                $"ColorSensor {ColorSensor}"
            });
        }
    }
}