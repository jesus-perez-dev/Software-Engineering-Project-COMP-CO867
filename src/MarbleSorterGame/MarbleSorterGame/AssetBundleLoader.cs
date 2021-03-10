﻿using System.IO;
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
                GameConfiguration = ConfigurationLoader.Load(this.assetDirectoryPath + "presets.json");

                /**
                BucketDropSuccess = new Sound(new SoundBuffer(this.assetDirectoryPath + "/BucketDropSuccess.ogg"));
                BucketDropFail = new Sound(new SoundBuffer(this.assetDirectoryPath + "/BucketDropFail.ogg"));
                SensorActivate = new Sound(new SoundBuffer(this.assetDirectoryPath + "/SensorActivate.ogg"));

                */
                Font = new Font(assetDirectoryPath + "OpenSans-Regular.ttf");
                //SensorTexture = new Texture(this.assetDirectoryPath + "sensor.jpg");
                BucketTexture = new Texture(this.assetDirectoryPath + "bucket.png");
                GameConfiguration = ConfigurationLoader.Load(this.assetDirectoryPath + "presets.json");
                
                MarbleRedTexture = new Texture(this.assetDirectoryPath + "marbleRed.png");
                MarbleGreenTexture = new Texture(this.assetDirectoryPath + "marbleGreen.png");
                MarbleBlueTexture = new Texture(this.assetDirectoryPath + "marbleBlue.png");

                //BucketDropSuccess = BucketDropSuccess;
                //BucketDropFail = BucketDropFail;
                //SensorActivate = SensorActivate;

            }
            catch (SFML.System.LoadingFailedException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
