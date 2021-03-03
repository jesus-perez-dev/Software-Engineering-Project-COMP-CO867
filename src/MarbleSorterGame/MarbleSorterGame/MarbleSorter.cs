using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

class MarbleSorter
{
    private List<GameEntity> _entities;
    private Window _window;
    //private SimulationClient _client;
    private IAssetBundle _assets;

    /**
    MarbleSorter(SimulationClient client, IAssetBundle assetBundle)
    {
        this._client = client;
        this._assets = assetBundle;
    }
    */

    public void Run()
    {
        while (window.IsOpen)
        {
            window.Display();
        }
    }

}
