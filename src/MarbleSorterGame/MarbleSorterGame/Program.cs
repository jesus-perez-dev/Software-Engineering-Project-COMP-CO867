using System;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press ESC key to close window");
            var window = new SimpleWindow();
            window.Run();

            Console.WriteLine("All done");
        }
    }

    class CircleEntity
    {
        public CircleShape Circle { get;  }
        public Vector2f Velocity { get; set; }

        public CircleEntity(CircleShape circle)
        {
            Circle = circle;
            Velocity = new Vector2f(0, 0);
        }

        public bool Overlaps(CircleEntity other)
        {
            return Circle.GetGlobalBounds()
                .Intersects(other.Circle.GetGlobalBounds());
        }

        public void UpdatePosition()
        {
            Circle.Position = Circle.Position + Velocity;
        }

        public void SetPosition(Vector2f position)
        {
            Circle.Position = position;
        }

        /*
        public void Shift(int x, int y)
        {
            _circle.Position = new Vector2f(
                _circle.Position.X + x,
                _circle.Position.Y + y
            );
        }
        */

        public void Draw(RenderWindow window)
        {
            window.Draw(Circle);
        }
    }
    
    
    class SimpleWindow
    {
        static int height = 720;
        static int width = 1280;
        
        static Font font = new Font("assets/OpenSans-Regular.ttf");

        
        public void Run()
        {
            var mode = new SFML.Window.VideoMode(800, 600);
            var window = new RenderWindow(mode, "SFML works!");
            window.KeyPressed += Window_KeyPressed;

            float circleSize = 25f;

            int rollSpeed = 1; // movement speed right
            int fallSpeed = 1; // movement speed down
            
            Vector2f rollVelocity = new Vector2f(rollSpeed, 0);
            Vector2f fallVelocity = new Vector2f(0, fallSpeed);

            Vector2f redStart = new Vector2f(0, circleSize);

            var circleRed = new CircleShape(circleSize)
            {
                FillColor = SFML.Graphics.Color.Red,
                Position = redStart
            };
            
            var circleYellow = new CircleShape(circleSize)
            {
                FillColor = SFML.Graphics.Color.Yellow,
                Position = new Vector2f(width/2, circleSize)
            };
            
            var circleGreen = new CircleShape(circleSize)
            {
                FillColor = SFML.Graphics.Color.Green,
                Position = new Vector2f(width/2, height-circleSize)
            };

            CircleEntity red = new CircleEntity(circleRed);
            CircleEntity yellow = new CircleEntity(circleYellow);
            CircleEntity green = new CircleEntity(circleGreen);
            
            CircleEntity[] circles = {red, yellow, green};

            Vector2f redCurrentVelocity = rollVelocity;
            red.SetPosition(redStart);

            bool falling = false;

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();
                Thread.Sleep(16);
                window.Clear();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    fallVelocity = new Vector2f(fallVelocity.X, Math.Max(fallVelocity.Y+1, 0));
                    rollVelocity = new Vector2f(Math.Max(rollVelocity.X+1, 0), rollVelocity.Y);
                }
                
                if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    fallVelocity = new Vector2f(fallVelocity.X, Math.Max(fallVelocity.Y-1, 0));
                    rollVelocity = new Vector2f(Math.Max(rollVelocity.X-1, 0), rollVelocity.Y);
                }

                Text text1 = new Text("Roll Velocity: " + rollVelocity, font);
                Text text2 = new Text("Fall Velocity: " + fallVelocity, font);
                Text text3 = new Text("Red Current Velocity: " + fallVelocity, font);

                text1.CharacterSize = 10;
                text2.CharacterSize = 10;
                text3.CharacterSize = 10;
                text1.Position = new Vector2f(0, height-text1.GetGlobalBounds().Height*3);
                text2.Position = new Vector2f(0, height-text1.GetGlobalBounds().Height*2);
                text3.Position = new Vector2f(0, height-text1.GetGlobalBounds().Height*1);
                
                window.Draw(text1);
                window.Draw(text2);
                window.Draw(text3);

                foreach (var circle in circles)
                {
                    circle.UpdatePosition();
                    circle.Draw(window);
                }
                
                // If red overlaps yellow, stop moving right and start falling down
                if (red.Overlaps(yellow))
                {
                    falling = true;
                }

                // If red overlaps green, set default velocity and go to starting point
                if (red.Overlaps(green))
                {
                    falling = false;
                    red.SetPosition(redStart);
                }
                
                if (falling)
                {
                    redCurrentVelocity = fallVelocity;
                }
                else
                {
                    redCurrentVelocity = rollVelocity;
                }
                
                red.Velocity = redCurrentVelocity;
                
                // TODO: Implement catchup/slowdown game loop as described here:
                // https://www.gameprogrammingpatterns.com/game-loop.html

                // Finally, display the rendered frame on screen
                window.Display();
            }
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}
