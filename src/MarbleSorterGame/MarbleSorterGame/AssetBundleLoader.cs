using System.IO;
using SFML.Audio;
using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Loads all available bundled assets
    /// </summary>
    public class AssetBundleLoader : IAssetBundle
    {
        private String assetDirectoryPath;
        
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

        public AssetBundleLoader(String assetDirectoryPath)
        {
            this.assetDirectoryPath = assetDirectoryPath;

            try
            {
                GameConfiguration = ConfigurationLoader.Load(this.assetDirectoryPath + "game.json");
                Font = new Font(assetDirectoryPath + "OpenSans-Regular.ttf");

                BucketTexture = new Texture(this.assetDirectoryPath + "bucket3.png");
                BucketDropSuccess = new Sound(new SoundBuffer(this.assetDirectoryPath + "bucketDropSuccess.ogg"));
                BucketDropFail = new Sound(new SoundBuffer(this.assetDirectoryPath + "bucketDropFail.ogg"));

                SensorTexture = new Texture(this.assetDirectoryPath + "sensor.png");
                SensorSignalOffTexture = new Texture(this.assetDirectoryPath + "sensorSignalOff1.png");
                SensorSignalOnTexture = new Texture(this.assetDirectoryPath + "sensorSignalOn1.png");
                SensorActivate = new Sound(new SoundBuffer(this.assetDirectoryPath + "sensorActivate.ogg"));

                MarbleRedTexture = new Texture(this.assetDirectoryPath + "marbleRed.png");
                MarbleGreenTexture = new Texture(this.assetDirectoryPath + "marbleGreen.png");
                MarbleBlueTexture = new Texture(this.assetDirectoryPath + "marbleBlue.png");

            }
            catch (SFML.System.LoadingFailedException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
