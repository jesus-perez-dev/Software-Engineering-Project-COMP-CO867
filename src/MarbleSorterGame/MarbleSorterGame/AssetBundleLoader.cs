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

        public AssetBundleLoader(String assetDirectoryPath)
        {
            try
            {

                //SFML.Audio.Music music = new Music(assetDirectoryPath + "/BackgroundMusic");

                Sound BucketDropSuccess = new Sound(new SoundBuffer(this.assetDirectoryPath + "/BucketDropSuccess.ogg"));
                Sound BucketDropFail = new Sound(new SoundBuffer(this.assetDirectoryPath + "/BucketDropFail.ogg"));
                Sound SensorActivate = new Sound(new SoundBuffer(this.assetDirectoryPath + "/SensorActivate.ogg"));

                Texture SensorTexture = new Texture(this.assetDirectoryPath + "/Sensor.png");
                Texture BucketTexture = new Texture(this.assetDirectoryPath + "/Bucket.png");
                Texture MarbleRedTexture = new Texture(this.assetDirectoryPath + "/MarbleRed.png");
                Texture MarbleGreenTexture = new Texture(this.assetDirectoryPath + "/MarbleGreen.png");
                Texture MarbleBlueTexture = new Texture(this.assetDirectoryPath + "/MarbleBlue.png");

                this.BucketDropSuccess = BucketDropSuccess;
                this.BucketDropFail = BucketDropFail;
                this.SensorActivate = SensorActivate;

                this.SensorTexture = SensorTexture;
                this.BucketTexture = BucketTexture;
                this.MarbleRedTexture = MarbleRedTexture;
                this.MarbleGreenTexture = MarbleGreenTexture;
                this.MarbleBlueTexture = MarbleBlueTexture;
            }
            catch (FileNotFoundException e)
            {
                //add exception
            }
        }

        public Sound BucketDropSuccess { get; set; }
        public Sound BucketDropFail { get; set; }
        public Texture BucketTexture { get; set; }
        public Texture MarbleRedTexture { get; set; }
        public Texture MarbleBlueTexture { get; set; }
        public Texture MarbleGreenTexture { get; set; }
        public Texture SensorTexture { get; set; }
        public Sound SensorActivate { get; set; }

    }
}
