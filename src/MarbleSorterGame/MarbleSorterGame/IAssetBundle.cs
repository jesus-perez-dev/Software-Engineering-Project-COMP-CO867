using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace MarbleSorterGame
{
    /// <summary>
    /// interface for all available bundled assets required for game
    /// </summary>
    public interface IAssetBundle
    {
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

        public Font Font { get; set;  }
    }
}
