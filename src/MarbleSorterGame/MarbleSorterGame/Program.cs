#nullable enable
using System;

namespace MarbleSorterGame
{
    public static class Program
    {
        
        public static void Main(string[] args)
        {
            // Attempt to load assets and driver. Most likely if anything goes wrong, it will occur here
            IAssetBundle? bundle = null;
            IIODriver? driver = null;
            Exception? exception = null;
            try
            {
                bundle = new AssetBundleLoader("Assets");
                driver = bundle.GameConfiguration.Driver switch
                {
                    DriverType.Keyboard => new KeyboardIODriver(),
                    DriverType.Simulation => new S7IODriver(bundle.GameConfiguration.DriverOptions, bundle.IoMapConfiguration),
                    _ => throw new ArgumentException($"Unknown IO driver: {bundle.GameConfiguration.Driver}")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                exception = ex;
            }
            
            MarbleSorterGame msg = new MarbleSorterGame(bundle, driver, exception);
            msg.Run();
        }
    }
}
