using System.IO;
using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using MarbleSorterGame.Utilities;

namespace MarbleSorterGame
{
    /// <summary>
    /// Loads all available bundled assets
    /// </summary>
    public class AssetBundleLoader : IAssetBundle
    {
        private String _assetDirectoryPath;
        public Sound BucketDrop { get; set; }
        public Sound BucketDropSuccess { get; set; }
        public Sound BucketDropFail { get; set; }
        public Texture BucketTexture { get; set; }
        public Texture MarbleRedTexture { get; set; }
        public Texture MarbleBlueTexture { get; set; }
        public Texture MarbleGreenTexture { get; set; }
        public Texture SensorTexture { get; set; }
        public Texture SensorSignalOffTexture { get; set; }
        public Texture SensorSignalOnTexture { get; set; }
        public Sound SensorActivate { get; set; }
        public MarbleGameConfiguration GameConfiguration { get; set; }

        public Font Font { get; set; }

        public string Error { get;  }
        
        public AssetBundleLoader(String assetDirectoryPath)
        {
            _assetDirectoryPath = Path.Join(Directory.GetCurrentDirectory(), assetDirectoryPath);
            try
            {
                // Load + Validate Configuration
                GameConfiguration = ConfigurationLoader.Load(Path.Join(_assetDirectoryPath + "Config", "game.json"));
                GameConfiguration.Validate(); // May throw MarbleGameConfigException

                // Load Fonts
                Font = new Font(Path.Join(_assetDirectoryPath, "Fonts", "DejaVuSansMono.ttf"));

                // Load Sounds
                string GetSound(string file) => Path.Join(_assetDirectoryPath, "Sounds", file);
                BucketDropSuccess = new Sound(new SoundBuffer(GetSound("bucketDropSuccess.ogg")));
                BucketDropFail = new Sound(new SoundBuffer(GetSound("bucketDropFail.ogg")));
                SensorActivate = new Sound(new SoundBuffer(GetSound("sensorActivate.ogg")));

                // Load Images
                string GetImage(string file) => Path.Join(_assetDirectoryPath, "Images", file);
                BucketTexture = new Texture(GetImage("bucket3.png"));
                SensorTexture = new Texture(GetImage("sensor.png"));
                SensorSignalOffTexture = new Texture(GetImage("sensorSignalOff1.png"));
                SensorSignalOnTexture = new Texture(GetImage("sensorSignalOn1.png"));
                MarbleRedTexture = new Texture(GetImage("marbleRed.png"));
                MarbleGreenTexture = new Texture(GetImage("marbleGreen.png"));
                MarbleBlueTexture = new Texture(GetImage("marbleBlue.png"));
            }
            catch (SFML.LoadingFailedException e)
            {
                Console.WriteLine(e);
                var lines = new Dictionary<string, string>();
                lines["Exception"] = e.GetType().FullName;
                lines["Message"] = e.Message;
                lines["Asset Path"] = _assetDirectoryPath;
                Error = FormatErrorString("Failed to load game resources", lines);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e);
                var lines = new Dictionary<string, string>();
                lines["Exception"] = e.GetType().FullName;
                lines["LineNumber"] = e.LineNumber.ToString();
                lines["Message"] = e.Message;
                lines["Asset Path"] = _assetDirectoryPath;
                Error = FormatErrorString("Failed to load 'game.json' configuration file", lines);
            }
            catch (MarbleGameConfigException e)
            {
                Console.WriteLine(e);
                var lines = new Dictionary<string, string>();
                lines["Exception"] = e.GetType().FullName;
                lines["Message"] = e.Message;
                Error = FormatErrorString("Error found in 'game.json' configuration file", lines);
            }
        }
    
        private static string FormatErrorString(string title, Dictionary<string, string> errorFields)
        {
            int columns = 120;
            List<string> lines = new List<string>();
            lines.Add(title);
            lines.Add(new String('=', columns-1));
            foreach (var (key, value) in errorFields)
                lines.Add($"- {key}: {value}");
            return string.Join("\n", lines).ColumnWrap(columns);
        }
    }
}
