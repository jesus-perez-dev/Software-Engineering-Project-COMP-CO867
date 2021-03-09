using System;
using System.Linq;
using System.Timers;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //
            // Console.WriteLine("==========DEMO============");
            // Console.WriteLine("Press ESC key to close window");
            // //========= TRAPDOOR/SENSOR DEMO =============/
            // /**
            // var window = new SimpleWindow();
            // window.Run();
            // */
            // //========= MARBLE SORTER GAME DEMO ============/
            // AssetBundleLoader assetBundle = new AssetBundleLoader("assets/");
            // MarbleSorter marbleSorterGame = new MarbleSorter(assetBundle);
            //
            // // Print game configuration loaded from JSON files
            // // foreach (var preset in assetBundle.GameConfiguration.Presets)
            // //    Console.WriteLine(preset.ToString());
            //
            // marbleSorterGame.Run();
            //
            // Console.WriteLine("All done");

            
            MarbleSorterGame msg = new MarbleSorterGame();
            msg.Run();
           
        }

    }

    /// <summary>
    /// sample class for testing basic shapes and collision
    /// </summary>
    class CircleEntity
    {
        public CircleShape Circle { get;  }
        public Vector2f Velocity { get; set; }

        public CircleEntity(CircleShape circle)
        {
            Circle = circle;
            Velocity = new Vector2f(0, 0);
        }

        public bool Overlaps(Shape other)
        {
            return Circle.GetGlobalBounds()
                .Intersects(other.GetGlobalBounds());
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
        
        static Font font = new Font("Assets/OpenSans-Regular.ttf");

        
        public void Run()
        {
            var mode = new SFML.Window.VideoMode(800, 600);
            var window = new RenderWindow(mode, "Marble Sorter Game - PLC Training Tool");
            window.KeyPressed += Window_KeyPressed;

            float circleSize = 25f;

            int rollSpeed = 1; // movement speed right
            int fallSpeed = 1; // movement speed down
            
            Vector2f rollVelocity = new Vector2f(rollSpeed, 0);
            Vector2f fallVelocity = new Vector2f(0, fallSpeed);

            Vector2f redStart = new Vector2f(0, 300);

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
            Vector2f greenCurrentVelocity = rollVelocity;
            red.SetPosition(redStart);

            RectangleShape conveyer = new RectangleShape();
            RectangleShape sensor = new RectangleShape();
            RectangleShape bucket = new RectangleShape();
            RectangleShape trapdoor = new RectangleShape();

            bool falling = false;
            bool trapdoorOpen = false;

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();
                Thread.Sleep(16);
                window.Clear();

                //=================== KEYBOARD FEEDBACK ================
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    fallVelocity = new Vector2f(fallVelocity.X, Math.Max(fallVelocity.Y+1, 0));
                    rollVelocity = new Vector2f(Math.Max(rollVelocity.X+1, 0), rollVelocity.Y);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    fallVelocity = new Vector2f(fallVelocity.X, Math.Max(fallVelocity.Y-1, 0));
                    rollVelocity = new Vector2f(Math.Max(rollVelocity.X-1, 0), rollVelocity.Y);
                }
                //reset ball
                else if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                {
                    falling = false;
                    red.SetPosition(redStart);
                    green.SetPosition(new Vector2f(1000, 1000));
                }
                //new ball
                else if (Keyboard.IsKeyPressed(Keyboard.Key.G))
                {
                    falling = false;
                    red.SetPosition(new Vector2f(1000, 1000));
                    green.SetPosition(redStart);
                }
                

                //================= GAME STATS TEXT ==================
                Text text1 = new Text("Roll Velocity: " + rollVelocity, font);
                Text text2 = new Text("Fall Velocity: " + fallVelocity, font);
                Text text3 = new Text("Red Current Velocity: " + redCurrentVelocity, font);
                Text text4 = new Text("Green Current Velocity: " + greenCurrentVelocity, font);

                text1.CharacterSize = 10;
                text2.CharacterSize = 10;
                text3.CharacterSize = 10;
                text4.CharacterSize = 10;

                text1.FillColor = SFML.Graphics.Color.White;
                text2.FillColor = SFML.Graphics.Color.White;
                text3.FillColor = SFML.Graphics.Color.White;
                text4.FillColor = SFML.Graphics.Color.White;

                text1.Position = new Vector2f(0, 0);
                text2.Position = new Vector2f(0, 30);
                text3.Position = new Vector2f(0, 60);
                text4.Position = new Vector2f(0, 90);
                
                window.Draw(text1);
                window.Draw(text2);
                window.Draw(text3);
                window.Draw(text4);

                //CONVEYER
                conveyer.Position = new Vector2f(0, 350);
                conveyer.Size = new Vector2f(1000, 0);
                conveyer.OutlineColor = SFML.Graphics.Color.White;
                conveyer.OutlineThickness = 2f;
                window.Draw(conveyer);

                //SENSOR
                sensor.Position = new Vector2f(500, 300);
                sensor.Size = new Vector2f(30, 30);
                sensor.OutlineColor = SFML.Graphics.Color.Red;
                sensor.OutlineThickness = 1f;
                window.Draw(sensor);
                
                if (red.Overlaps(sensor))
                {
                    falling = true;
                    trapdoorOpen = true;

                    //set trapdoor shut after 5s
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Interval = 7000;
                    timer.Enabled = true;
                    timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) => { trapdoorOpen = false; };

                }

                if (green.Overlaps(sensor) && trapdoorOpen)
                {
                    falling = true;
                }

                if (trapdoorOpen)
                {
                    //TRAPDOOR (FILL)
                    trapdoor.Position = new Vector2f(450, 340);
                    trapdoor.Size = new Vector2f(100, 20);
                    trapdoor.OutlineColor = SFML.Graphics.Color.Black;
                    trapdoor.FillColor = SFML.Graphics.Color.Black;
                    trapdoor.OutlineThickness = 1f;
                    window.Draw(trapdoor);
                }

                foreach (var circle in circles)
                {
                    circle.UpdatePosition();
                    circle.Draw(window);
 
                //BUCKET
                bucket.Position = new Vector2f(450, 500);
                bucket.Size = new Vector2f(100, 200);
                bucket.FillColor = SFML.Graphics.Color.Blue;
                bucket.OutlineThickness = 1f;
                window.Draw(bucket);
               }
                
                if (falling)
                {
                    redCurrentVelocity = fallVelocity;
                    greenCurrentVelocity = fallVelocity;
                }
                else
                {
                    redCurrentVelocity = rollVelocity;
                    greenCurrentVelocity = rollVelocity;
                }
                
                red.Velocity = redCurrentVelocity;
                green.Velocity = greenCurrentVelocity;

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
