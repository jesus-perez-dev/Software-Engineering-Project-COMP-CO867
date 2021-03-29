using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using MarbleSorterGame.Enums;

/* Example Configuration JSON:
{
  "ScreenWidth": 1920,
  "ScreenHeight": 1080,
  "MarblePeriod": 5.0,
  "GatePeriod": 5.0,
  "TrapDoorPeriod": 5.0,
  "Driver": "Simulation",
  "DriverOptions": {
    "SimulationName": "demo-simulation"
  },
  "Presets": {
      "Marbles": [
        { "Color": "Red", "Weight": "Small" },
        { "Color": "Green", "Weight": "Large" },
        { "Color": "Blue", "Weight": "Medium" }
      ],
      "Buckets": [
        { "Capacity": 1, "Weight": null,     "Color": "blue" },
        { "Capacity": 1, "Weight": "Medium", "Color": null },
        { "Capacity": 1, "Weight": null,     "Color": "Green" }
      ]
   }
}
*/

namespace MarbleSorterGame
{
    public enum DriverType
    {
        Keyboard,
        Simulation
    }
    
    public class MarbleGameConfigException: System.Exception
    {
       public MarbleGameConfigException() { }
       public MarbleGameConfigException(string message): base(message) { }
    }
    
    
    public class SimulationDriverOptions
    {
        public string SimulationName { get; set; }
        public override string ToString() => $"SimulationDriverOptions: SimulationName = {SimulationName}";
    }
    
    public class MarbleConfig
    {
        public Color Color { get; set; }
        public Weight Weight { get; set; }
        
        public override string ToString() => $"MarbleConfig: Color = {Color}, Weight = {Weight}";
    }

    public class BucketConfig
    {
        public int Capacity { get; set; }
        public Color? Color { get; set; }
        public Weight? Weight { get; set; }
        
        public override string ToString() => $"BucketConfig: Capacity = {Capacity}, Color = {Color}, Weight = {Weight}";
    }

    public class MarbleGameConfiguration
    {
        public MarbleGamePreset Preset { get; set; }
        
        public uint ScreenWidth { get; set; }
        public uint ScreenHeight { get; set; }
        
        // Time it takes for a marble to cross the entire screen width
        public float MarblePeriod { get; set; }
        
        // Time it takes for the marble gate to open (or close) completely from the opposite state 
        public float GatePeriod { get; set; }

        // Time it takes for a trap door to (or close) completely from the opposite state 
        public float TrapDoorPeriod { get; set; }

        // The kind of IIODriver implementation to use for game entity IO
        public DriverType Driver { get; set; }

        // Additional driver-specific options. Currently only SimulationDriverOptions are available
        public SimulationDriverOptions? DriverOptions { get; set; }
        public override string ToString() => string.Join("\n", new[]
        {
            $"ScreenWidth: {ScreenWidth}",
            $"ScreenHeight: {ScreenHeight}",
            $"MarblePeriod: {MarblePeriod}",
            $"GatePeriod: {GatePeriod}",
            $"TrapDoorPeriod: {TrapDoorPeriod}",
            $"Driver: {Driver}",
            $"DriverOptions: {DriverOptions}",
            $"Preset: {Preset}"
        });

        // Throw MarbleGameConfigException if anything looks off
        private void ValidateProperty(bool failCondition, string prop, string message)
        {
            if (failCondition)
                throw new MarbleGameConfigException($"Invalid or missing property '{prop}': {message}");
        }

        public void Validate()
        {
            ValidateProperty(ScreenHeight < 600, "ScreenHeight", "Must be >= 600");
            ValidateProperty(ScreenWidth < 800, "ScreenWidth", "Must be >= 800");
            ValidateProperty(MarblePeriod <= 0, "MarblePeriod", "Must be > 0");
            ValidateProperty(GatePeriod <= 0, "GatePeriod", "Must be > 0");
            ValidateProperty(TrapDoorPeriod <= 0, "TrapDoorPeriod", "Must be > 0");
            if (Driver == DriverType.Simulation)
            {
                ValidateProperty(DriverOptions == null, "DriverOptions.SimulationName", "Cannot be null when driver is 'Simulation'");
                ValidateProperty(DriverOptions?.SimulationName == null, "DriverOptions.SimulationName", "Cannot be null");
            }
        }
    }

    public class MarbleGamePreset
    {
        public List<MarbleConfig> Marbles { get; set; }
        public List<BucketConfig> Buckets { get; set; }
        
        public override string ToString() => string.Join("\n", new[] 
        {
            "Marbles:", string.Join("\n\t", Marbles), 
            "Buckets:", string.Join("\n\t", Buckets), 
        });
    }
    
    public class ConfigurationLoader
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
         {
             // Convert json string "Red" to Color.Red, etc...
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
         };

         public static MarbleGameConfiguration Load(string filePath)
         {
            return JsonSerializer.Deserialize<MarbleGameConfiguration>(File.ReadAllText(filePath), _options);
         }
    }
}