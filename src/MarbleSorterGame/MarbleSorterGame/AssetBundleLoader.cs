using System.IO;
using SFML.Audio;
using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Organizes and stores paths to required assets
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
        public MarbleGameConfiguration GameConfiguration { get; set; }

        public Font Font { get; set; }

        /// <summary>
        /// Constructor for asset bundle
        /// </summary>
        /// <param name="assetDirectoryPath">File path to the assets folder</param>
        public AssetBundleLoader(String assetDirectoryPath)
        {
            _assetDirectoryPath = assetDirectoryPath;

            try
            {
                GameConfiguration = ConfigurationLoader.Load(assetDirectoryPath + "Config/game.json");
                Font = new Font(assetDirectoryPath + "Fonts/DejaVuSansMono.ttf");
                BucketTexture = new Texture(assetDirectoryPath + "Images/bucket3.png");
                BucketDropSuccess = new Sound(new SoundBuffer(assetDirectoryPath + "Sounds/bucketDropSuccess.ogg"));
                BucketDropFail = new Sound(new SoundBuffer(assetDirectoryPath + "Sounds/bucketDropFail.ogg"));
                SensorTexture = new Texture(assetDirectoryPath + "Images/sensor.png");
                MarbleRedTexture = new Texture(assetDirectoryPath + "Images/marbleRed.png");
                MarbleGreenTexture = new Texture(assetDirectoryPath + "Images/marbleGreen.png");
                MarbleBlueTexture = new Texture(assetDirectoryPath + "Images/marbleBlue.png");

            }
            catch (SFML.System.LoadingFailedException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
