using System;
using System.Linq;
using System.Collections.Generic;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    /// <summary>
    /// The game itself
    /// </summary>
    public class GameScreen : IDisposable
    {
        private Vector2f MarbleFallVelocity = default;
        private Vector2f MarbleRollVelocity = default;
        
        private Drawable[] _drawables;
        private GameEntity[] _entities;

        private IIODriver _driver;

        private Gate _gateEntrance;
        private Conveyor _conveyor;
        private Marble[] _marbles;
        private Trapdoor[] _trapDoors;
        private Bucket[] _buckets;
        
        private RenderWindow _window;
        
        // UI Buttons
        private Button _buttonStart;
        private Button _buttonReset;
        private Button _buttonExit;
        
        // TODO: Pass a game configuration structure in here instead of width/height uints
        public GameScreen(RenderWindow window, AssetBundleLoader bundle, uint screenWidth, uint screenHeight, IIODriver driver)
        {
            _driver = driver;
            
            Font font = bundle.Font;
            Sizer sizer = new Sizer(screenWidth, screenHeight);

            // Stick marble roll velocity to resolution size
            // 300 ticks = 5 seconds
            MarbleRollVelocity = new Vector2f((float)screenWidth / 800, 0);
            MarbleFallVelocity = new Vector2f(0, (float)screenHeight / 300);

            _window = window;
            _window.MouseButtonPressed += GameMouseClickEventHandler;
            _window.KeyPressed += GameKeyEventHandler;
            _window.MouseMoved += GameMouseMoveEventHandler;
            
            //================= Background widgets ====================//
            // Menu bar background (slight gray recantgle behind buttons)
            RectangleShape menuBarBackground = new RectangleShape
            {
                Position = sizer.Percent(0f, 0f),
                Size = sizer.Percent(100f, 6f),
                FillColor = new SFML.Graphics.Color(89, 105, 115) // dark-blue-gray ish color
            };

            //================= Buttons ====================//
            _buttonStart = new Button(
                "Start Simulation",
                0.4f,
                font,
                sizer.Percent(60, 3),
                sizer.Percent(13, 5)
            );

            _buttonReset = new Button(
                "Reset Game",
                0.4f,
                font,
                sizer.Percent(75, 3),
                sizer.Percent(13, 5)
                );

            _buttonExit = new Button(
                "Exit Game",
                0.4f,
                font,
                sizer.Percent(90, 3),
                sizer.Percent(13, 5)
            );

            //================= Labels ====================//
            String instructionsText = "Use the Input/Output Addresses shown below to create a working \nPLC for the marble sorter, based on the requirements on the buckets below.";

            Label instructions = new Label(
                instructionsText,
                sizer.Percent(30, 3),
                10,
                SFML.Graphics.Color.Black,
                font);

            Label infobar = new Label(
                instructionsText,
                sizer.Percent(30, 3),
                10,
                SFML.Graphics.Color.Black,
                font);

            //================= Game Entities ====================//
            
            _conveyor = new Conveyor(
                sizer.Percent(0, 60),
                sizer.Percent(100, 1),
                new Vector2f(1, 0)
                );

            Vector2f trapdoorSize = new Vector2f(MarbleSorterGame.WINDOW_WIDTH/15, _conveyor.Size.Y); //sizer.Percent(8, 1);
            Vector2f gateEntranceSize = sizer.Percent(1, 9);
            Vector2f signalSize = sizer.Percent(3, 8);
            Vector2f sensorSize = new Vector2f(20, 20);

            Vector2f helperPopupSize = new Vector2f(70, 15);
            
            // TODO: Populate this list based on game configuration
            _trapDoors = new[]
            {
                new Trapdoor( sizer.Percent(27, 60), trapdoorSize ), // Left-most
                new Trapdoor( sizer.Percent(51, 60), trapdoorSize ), 
                new Trapdoor( sizer.Percent(76, 60), trapdoorSize ) // Right-most
            };

            Gate gateEntrance = new Gate(
                sizer.Percent(13, 52),
                gateEntranceSize
                );

            _gateEntrance = gateEntrance;

            PressureSensor sensorPressureStart = new PressureSensor(
                sizer.Percent(3, 55),
                sensorSize
                );

            ColorSensor sensorColorStart = new ColorSensor(
                sizer.Percent(6, 55),
                sensorSize
                ) ;

            MotionSensor sensorMotionEnd = new MotionSensor(
                sizer.Percent(94, 55),
                sensorSize
                );

            MotionSensor sensorMotionBucket1 = new MotionSensor(
                sizer.Percent(25, 100),
                sizer.Percent(0, 0)
                );

            MotionSensor sensorMotionBucket2 = new MotionSensor(
                sizer.Percent(45, 100),
                sizer.Percent(0, 0)
                );

            MotionSensor sensorMotionBucket3 = new MotionSensor(
                sizer.Percent(65, 100),
                sizer.Percent(0, 0)
                );

            SignalLight signalColor1 = new SignalLight(
                sizer.Percent(30, 20),
                signalSize
                );

            SignalLight signalColor2 = new SignalLight(
                new Vector2f(signalColor1.Position.X + signalSize.X + 10, signalColor1.Position.Y),
                signalSize
                );

            SignalLight signalPressure1 = new SignalLight(
                new Vector2f(signalColor2.Position.X + signalSize.X + 10, signalColor1.Position.Y),
                signalSize
                );

            // SignalLight signalMotion1 = new SignalLight(
            //     sizer.Percent(95, 50),
            //     signalSize
            //     );

            SignalLight gateOpen = new SignalLight(
                sizer.Percent(10, 74),
                signalSize
                );
            
            SignalLight gateClosed = new SignalLight(
                new Vector2f(gateOpen.Position.X + signalSize.X + 10, gateOpen.Position.Y),
                signalSize
                );

            // SignalLight conveyerOn = new SignalLight(
            //     sizer.Percent(5, 75),
            //     signalSize
            //     );

            SignalLight bucketDropped = new SignalLight(
                sizer.Percent(95, 90),
                signalSize
                );

            SignalLight trapdoorOpen1 = new SignalLight(
                new Vector2f(_trapDoors[0].Position.X, _trapDoors[0].Position.Y + 150),
                signalSize
                );

            SignalLight trapdoorClosed1 = new SignalLight(
                new Vector2f (trapdoorOpen1.Position.X + signalSize.X + 10, trapdoorOpen1.Position.Y),
                signalSize
                );

            SignalLight trapdoorOpen2 = new SignalLight(
                new Vector2f(_trapDoors[1].Position.X + 100, _trapDoors[1].Position.Y + 150),
                signalSize
                );

            SignalLight trapdoorClosed2 = new SignalLight(
                new Vector2f(trapdoorOpen2.Position.X + signalSize.X + 10, trapdoorOpen2.Position.Y),
                signalSize
                );

            SignalLight trapdoorOpen3 = new SignalLight(
                new Vector2f(_trapDoors[2].Position.X + 200, _trapDoors[2].Position.Y + 150),
                signalSize
                );

            SignalLight trapdoorClosed3 = new SignalLight(
                new Vector2f(trapdoorOpen3.Position.X + signalSize.X + 10, trapdoorOpen3.Position.Y),
                signalSize
                );
            
            // TODO: Populate list based on file configuration
            _marbles = new[]
            {
                new Marble(sizer, new Vector2f(40, _conveyor.Position.Y), Color.Red, Weight.Large),
                new Marble(sizer, new Vector2f(40, _conveyor.Position.Y), Color.Blue, Weight.Medium),
                new Marble(sizer, new Vector2f(40, _conveyor.Position.Y), Color.Green, Weight.Small),
            };

            _buckets = new[]
            {
                new Bucket(sizer.Percent(27, 100), sizer.Percent(10, 20), Color.Red, Weight.Small, 20),
                new Bucket(sizer.Percent(51,100), sizer.Percent(10, 20), Color.Green, Weight.Medium, 20),
                new Bucket(sizer.Percent(76,100), sizer.Percent(10, 20), Color.Blue, Weight.Large, 20),
            };
            
            // Adjust bucket positions to be at very bottom of screen
            foreach (var bucket in _buckets)
            {
                bucket.Position -= new Vector2f(0, bucket.Size.Y);
            }
            
            // Set marble initialal positions based on screen dimensions
            Vector2f offset = new Vector2f(0,0);
            foreach (var marble in _marbles.Reverse())
            {
                //marble.Position = sizer.Percent(0, 0);
                marble.Position = new Vector2f(marble.Position.X, _conveyor.Position.Y - marble.Size.Y);
                marble.Position -= offset;
                offset = new Vector2f(offset.X + marble.Size.X, 0);
            }
            
            _entities = new GameEntity[]
            {
                _buttonStart,
                _buttonReset,
                _buttonExit,
                _conveyor,
                sensorColorStart,
                sensorPressureStart,
                signalColor1,
                signalColor2,
                signalPressure1,
                sensorMotionEnd,
                sensorMotionBucket1,
                sensorMotionBucket2,
                sensorMotionBucket3,
                gateOpen,
                gateClosed,
                // conveyerOn,
                bucketDropped,
                trapdoorOpen1,
                trapdoorClosed1,
                trapdoorOpen2,
                trapdoorClosed2,
                trapdoorOpen3,
                trapdoorClosed3,
                // signalMotion1,
            };

            _entities = _entities.ToList()
                .Concat(_marbles)
                .Concat(_trapDoors)
                .Concat(_buckets)
                .Concat(new [] { gateEntrance })
                .ToArray();

            foreach (GameEntity entity in _entities)
            {
                entity.Load(bundle);
            }
            
            _drawables = new Drawable[]
            {
                menuBarBackground,
                instructions,
                infobar
            };
        }

        public void GameMouseMoveEventHandler(object? sender, SFML.Window.MouseMoveEventArgs mouse)
        {
            // TODO: Implement on-hover logic for game entities
        }

        public void GameMouseClickEventHandler(object? sender, MouseButtonEventArgs mouse)
        {
            if (_buttonStart.IsPressed(mouse.X, mouse.Y))
            {
                // TODO: Start the simulation (?)
            }
            else if (_buttonReset.IsPressed(mouse.X, mouse.Y))
            {
                // TODO: Reset the game
            }
            else if (_buttonExit.IsPressed(mouse.X, mouse.Y))
            {
                Dispose();
                _window.Close();
            }
        }

        public void GameKeyEventHandler(object? sender, KeyEventArgs key)
        {
            if (_driver is KeyboardIODriver kbdriver)
            {
                kbdriver.UpdateByKey(key);
            }
        }
        
        public void Update()
        {
            _gateEntrance.SetState(_driver.Gate);
            _trapDoors[0].SetState(_driver.TrapDoor1);
            _trapDoors[1].SetState(_driver.TrapDoor2);
            _trapDoors[2].SetState(_driver.TrapDoor3);

            foreach (var trapdoor in _trapDoors)
            {
                trapdoor.Update();
            }
            
            foreach (var marble in _marbles)
            {
                // By default, marble should roll right
                marble.Velocity = MarbleRollVelocity;
                
                // If marble is touching gate and gate is closed, do not move
                if (_gateEntrance.Overlaps(marble) && !_gateEntrance.IsFullyOpen)
                    marble.Velocity = new Vector2f(0f, 0);
                
                // If marble has started falling, keep it falling
                if (marble.Position.Y > _conveyor.Position.Y)
                    marble.Velocity = MarbleFallVelocity;
            }
            
            // If marbles are touching/overlapping each other, the marble farthest to left should stop moving
            // NOTE: This assumes marble order is placed from left-to-right!
            for (int i = 0; i < _marbles.Length - 1; i++)
            {
                if (_marbles[i].Overlaps(_marbles[i + 1]))
                {
                    _marbles[i].Velocity = new Vector2f(0f, _marbles[i].Velocity.Y);
                }
            }
            
            // If marble is overtop a trap-door, and the trap door is open, switch velocity to falling
            foreach (var marble in _marbles)
            {
                foreach (var trapdoor in _trapDoors)
                {
                    if (marble.InsideHorizontal(trapdoor) && trapdoor.IsOpen)
                    {
                        marble.Velocity = MarbleFallVelocity;
                    }
                }
            }

            // Now actually update marble coordinates
            foreach (var m in _marbles)
            {
                m.Update();
            }
            
            // When the marble is complete inside the bucket, insert marble and teleport it offscreen
            foreach (var marble in _marbles)
            {
                foreach (var bucket in _buckets)
                {
                    if (bucket.Inside(marble))
                    {
                        // Set marble position offscreen and update bucket counters
                        marble.Position = new Vector2f(50000, 50000);
                        bool success = bucket.InsertMarble(marble);
                    }
                }
            }

            _gateEntrance.Update();
            
            // Update IIODriver instance
            _driver.Update();
        }
        
        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public void Draw(RenderWindow window)
        {
            foreach (var drawable in _drawables)
            {
                window.Draw(drawable);
            }
            
            foreach (var entity in _entities) 
            {
                entity.Render(window);
            }
        }

        public void Dispose()
        {
            _window.MouseButtonPressed -= GameMouseClickEventHandler;
            _window.KeyPressed -= GameKeyEventHandler;
            _window.MouseMoved -= GameMouseMoveEventHandler;
        }
    }
}