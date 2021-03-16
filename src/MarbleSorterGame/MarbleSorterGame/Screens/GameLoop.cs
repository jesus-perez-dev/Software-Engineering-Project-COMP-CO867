using System;
using System.Threading;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Abstract class for the game loop. This is the general structure of how the game needs to be implemented.
    /// </summary>
    public abstract class GameLoop
    {
        public const int FPS = 60;
        
        /// <summary>
        /// Display window for the game
        /// </summary>
        public RenderWindow Window
        {
            get;
            protected set;
        }

        /// <summary>
        /// Color for when the window get cleared
        /// </summary>
        public  SFML.Graphics.Color WindowClearColor
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowTitle"></param>
        /// <param name="windowClearColor"></param>
        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, SFML.Graphics.Color windowClearColor)
        {
            WindowClearColor = windowClearColor;
            Window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            
            Window.Closed += Window_Closed;
            Window.Resized += Window_Resized;
        }

        /// <summary>
        /// Structure for the game. Assets are first loaded then objects that do not need to change get initialized. The game then loops.
        /// Inside the loop events get handled, then data gets updated followed by a window clear, redrawing all the object, and then
        /// displaying new ones. 
        /// </summary>
        public void Run()
        {
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Thread.Sleep(1000 / FPS);
                Update();
                Window.Clear(WindowClearColor);
                Draw();
                Window.Display();
            }
        }

        /// <summary>
        /// Abstract method for inheritors to implement
        /// </summary>
        public abstract void Update();
        public abstract void Draw();

        /// <summary>
        /// Event for when users click the x button to close the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }
        
        private void Window_Resized(object sender, SizeEventArgs e)
        {
            Window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }
    }
}