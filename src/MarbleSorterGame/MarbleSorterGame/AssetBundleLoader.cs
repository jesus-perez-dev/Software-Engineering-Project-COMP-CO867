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
        
        public Sound BucketDropSuccess { get; set; }
        public Sound BucketDropFail { get; set; }
        public Texture BucketTexture { get; set; }
        public Texture MarbleRedTexture { get; set; }
        public Texture MarbleBlueTexture { get; set; }
        public Texture MarbleGreenTexture { get; set; }
        public Texture SensorTexture { get; set; }
        public Sound SensorActivate { get; set; }
        public MarbleGameConfiguration GameConfiguration { get; set; }

        public AssetBundleLoader(String assetDirectoryPath)
        {
            this.assetDirectoryPath = assetDirectoryPath;

            try
            {

                //SFML.Audio.Music music = new Music(assetDirectoryPath + "/BackgroundMusic");

                //Sound BucketDropSuccess = new Sound(new SoundBuffer(this.assetDirectoryPath + "/BucketDropSuccess.ogg"));
                //Sound BucketDropFail = new Sound(new SoundBuffer(this.assetDirectoryPath + "/BucketDropFail.ogg"));
                //Sound SensorActivate = new Sound(new SoundBuffer(this.assetDirectoryPath + "/SensorActivate.ogg"));

                SensorTexture = new Texture(this.assetDirectoryPath + "sensor.jpg");
                BucketTexture = new Texture(this.assetDirectoryPath + "bucket.png");
                GameConfiguration = ConfigurationLoader.Load(this.assetDirectoryPath + "presets.json");
                
                /**
                 * marble textures do not load, do not know why
                Texture MarbleRedTexture = new Texture(this.assetDirectoryPath + "marble_red_texture.jpg");
                Texture MarbleGreenTexture = new Texture(this.assetDirectoryPath + "marble_green_texture.jpg");
                Texture MarbleBlueTexture = new Texture(this.assetDirectoryPath + "marble_blue_texture.jpg");
                */

                //this.BucketDropSuccess = BucketDropSuccess;
                //this.BucketDropFail = BucketDropFail;
                //this.SensorActivate = SensorActivate;

                this.SensorTexture = SensorTexture;
                this.BucketTexture = BucketTexture;
                this.MarbleRedTexture = MarbleRedTexture;
                this.MarbleGreenTexture = MarbleGreenTexture;
                this.MarbleBlueTexture = MarbleBlueTexture;
            }
            catch (SFML.System.LoadingFailedException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
