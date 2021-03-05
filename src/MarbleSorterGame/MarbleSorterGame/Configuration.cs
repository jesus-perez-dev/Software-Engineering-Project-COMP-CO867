using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

/* Example Configuration JSON:
{
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
          ],
          "Sensors": [
            { "Type": "color" },
            { "Type": "weight" },
            { "Type": "motion" }
          ]
        }
      ]
   }
}
*/

namespace MarbleSorterGame
{
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

    public class SensorConfig
    {
        public string Type { get; set; }
        public override string ToString() => $"SensorConfig: Type = {Type}";
    }

    public class MarbleGameConfiguration
    {
        public List<MarbleGamePreset> Presets { get; set; }
    }

    public class MarbleGamePreset
    {
        public List<MarbleConfig> Marbles { get; set; }
        public List<BucketConfig> Buckets { get; set; }
        public List<SensorConfig> Sensors { get; set; }

        public override string ToString() => string.Join("\n",
            new string[]
            {
                "Marbles:", string.Join("\n\t", Marbles), 
                "Buckets:", string.Join("\n\t", Buckets), 
                "Sensors:", string.Join("\n\t", Sensors), 
            }
        );
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