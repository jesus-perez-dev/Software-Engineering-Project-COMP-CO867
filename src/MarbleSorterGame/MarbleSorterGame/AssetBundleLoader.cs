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
    // Organizes and stores paths to required assets
    public class AssetBundleLoader : IAssetBundle
    {
        public Sound BucketDrop { get; set; }
        public Sound BucketDropSuccess { get; set; }
        public Sound BucketDropFail { get; set; }
        public Texture BucketTexture { get; set; }
        public Texture MarbleRedTexture { get; set; }
        public Texture MarbleBlueTexture { get; set; }
        public Texture MarbleGreenTexture { get; set; }
        public Texture SensorTexture { get; set; }
        public MarbleGameConfiguration GameConfiguration { get; set; }
        public List<IoMapConfiguration> IoMapConfiguration { get; set; }
        public Font Font { get; set; }
        public string AbsoluteAssetDirectoryPath { get; }
        
        public AssetBundleLoader(String assetDirectoryRelativePath)
        {
            AbsoluteAssetDirectoryPath = Path.Join(Directory.GetCurrentDirectory(), assetDirectoryRelativePath);
            string configFile = "";
            try 
            {
                // Load + Validate Game Configuration
                string GetConfig(string file) => Path.Join(AbsoluteAssetDirectoryPath, "Config", file);
                configFile = GetConfig("game.json");
                GameConfiguration = ConfigurationLoader.LoadGameConfiguration(configFile);
                GameConfiguration.Validate(); // May throw MarbleGameConfigException
                
                // TODO: Validate IOMap configuration
                configFile = GetConfig("iomap.json");
                IoMapConfiguration = ConfigurationLoader.LoadIoMapConfiguration(configFile); 

                // Load Fonts
                Font = new Font(Path.Join(AbsoluteAssetDirectoryPath, "Fonts", "DejaVuSansMono.ttf"));

                // Load Sounds
                string GetSound(string file) => Path.Join(AbsoluteAssetDirectoryPath, "Sounds", file);
                BucketDropSuccess = new Sound(new SoundBuffer(GetSound("bucketDropSuccess.ogg")));
                BucketDropFail = new Sound(new SoundBuffer(GetSound("bucketDropFail.ogg")));

                // Load Images
                string GetImage(string file) => Path.Join(AbsoluteAssetDirectoryPath, "Images", file);
                BucketTexture = new Texture(GetImage("bucket3.png"));
                SensorTexture = new Texture(GetImage("sensor.png"));
                MarbleRedTexture = new Texture(GetImage("marbleRed.png"));
                MarbleGreenTexture = new Texture(GetImage("marbleGreen.png"));
                MarbleBlueTexture = new Texture(GetImage("marbleBlue.png"));
            }
            catch (JsonException e)
            {
                Console.WriteLine(e);
                var lines = new Dictionary<string, string>();
                lines["Exception"] = e.GetType().FullName;
                lines["LineNumber"] = e.LineNumber.ToString();
                lines["Message"] = e.Message;
                lines["File"] = configFile;
                throw new JsonException(FormatErrorString($"Error loading '{configFile}'", lines));
            }
            
            catch (SFML.LoadingFailedException e)
            {
                Console.WriteLine(e);
                var lines = new Dictionary<string, string>();
                lines["Exception"] = e.GetType().FullName;
                lines["Message"] = e.Message;
                lines["Asset Path"] = AbsoluteAssetDirectoryPath;
                throw new SFML.LoadingFailedException(FormatErrorString("Failed to load game resources", lines));
            }
            catch (ConfigValidationException e)
            {
                Console.WriteLine(e);
                var lines = new Dictionary<string, string>();
                lines["Exception"] = e.GetType().FullName;
                lines["Message"] = e.Message;
                lines["File"] = configFile;
                throw new ConfigValidationException(FormatErrorString($"Validation error found in '{configFile}'", lines));
            }
        }
    
        private static string FormatErrorString(string title, Dictionary<string, string> errorFields)
        {
            List<string> lines = new List<string>();
            lines.Add(title);
            //lines.Add(new String('=', title.Length));
            //lines.Add(new String('=', columns-1));
            foreach (var (key, value) in errorFields)
                lines.Add($"- {key}: {value}\n");
            return string.Join("\n", lines);
        }
    }
}
