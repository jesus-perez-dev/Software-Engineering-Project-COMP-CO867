using System;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Abstract class for the game loop. 
    /// </summary>
    public abstract class GameLoop
    {
        public const int FPS = 60;
        
        public RenderWindow Window
        {
            get;
            protected set;
        }

        public  SFML.Graphics.Color WindowClearColor
        {
            get;
            protected set;
        }

        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, SFML.Graphics.Color windowClearColor)
        {
            this.WindowClearColor = windowClearColor;
            this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            
            Window.Closed += Window_Closed;
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Update();
                
                Window.Clear(WindowClearColor);
                Draw();
                Window.Display();
            }
        }

        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update();
        public abstract void Draw();

        private void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}