using SFML.Window;

namespace MarbleSorterGame
{
    public class KeyboardIODriver : IIODriver
    {
        public bool TrapDoor1 { get; set; }
        public bool TrapDoor1Open { get; set; }
        public bool TrapDoor1Closed { get; set; }
        public bool TrapDoor2 { get; set; }
        public bool TrapDoor2Open { get; set; }
        public bool TrapDoor2Closed { get; set; }
        public bool TrapDoor3 { get; set; }
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
            // This implementation does not update programatically, it uses keyboard input. See: UpdateByKey()
        }

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
        
        public override string ToString()
        {
            return string.Join("\n", new string[]
            {
                $"TrapDoor1 {TrapDoor1}",
                $"TrapDoor1Open {TrapDoor1Open}",
                $"TrapDoor1Closed {TrapDoor1Closed}",
                $"TrapDoor2 {TrapDoor2}",
                $"TrapDoor2Open {TrapDoor2Open}",
                $"TrapDoor2Closed {TrapDoor2Closed}",
                $"TrapDoor3 {TrapDoor3}",
                $"TrapDoor3Open {TrapDoor3Open}",
                $"TrapDoor3Closed {TrapDoor3Closed}",
                $"BucketSensor1 {BucketSensor1}",
                $"BucketSensor2 {BucketSensor2}",
                $"BucketSensor3 {BucketSensor3}",
                $"Gate {Gate}",
                $"GateOpened {GateOpened}",
                $"GateClosed {GateClosed}",
                $"Conveyor {Conveyor}",
                $"MotionSensor {MotionSensor}",
                $"WeightSensor {WeightSensor}",
                $"ColorSensor {ColorSensor}"
            });
        }
    }
}