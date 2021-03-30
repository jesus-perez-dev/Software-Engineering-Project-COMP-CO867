using System;
using System.Collections.Generic;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace MarbleSorterGame
{
    // interface for all available bundled assets required for game
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
        public MarbleGameConfiguration GameConfiguration { get; set; }
        public List<IoMapConfiguration> IoMapConfiguration { get; set; }
        public Font Font { get; set;  }
        public string Error { get;  }
    }
}
