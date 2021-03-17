using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using MarbleSorterGame.Enums;
using Color = SFML.Graphics.Color;

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
  "Presets": [
    {
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
  ]
}
*/

namespace MarbleSorterGame
{

    public enum DriverType
    {
        Keyboard,
        Simulation
    }
    
    public class SimulationDriverOptions
    {
        public string SimulationName { get; set; }
        public override string ToString() => $"SimulationDriverOptions: SimulationName = {SimulationName}";
    }
    
    public class MarbleConfig
    {
        public Enums.Color Color { get; set; }
        public Weight Weight { get; set; }
        
        public override string ToString() => $"MarbleConfig: Color = {Color}, Weight = {Weight}";
    }

    public class BucketConfig
    {
        public int Capacity { get; set; }
        public Enums.Color? Color { get; set; }
        public Weight? Weight { get; set; }
        
        public override string ToString() => $"BucketConfig: Capacity = {Capacity}, Color = {Color}, Weight = {Weight}";
    }

    public class MarbleGameConfiguration
    {
        public List<MarbleGamePreset> Presets { get; set; }
        
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
            $"Presets:",
            string.Join("\n",Presets.Select((p,i) => $"#{i}:\n{p}"))
        });

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