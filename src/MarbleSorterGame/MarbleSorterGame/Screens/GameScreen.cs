using System;
using System.Linq;
using System.Collections.Generic;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame.Screens
{
    /// <summary>
    /// The game itself
    /// </summary>
    public class GameScreen : IDisposable
    {
        private Drawable[] _drawables;
        private GameEntity[] _entities;

        private IIODriver _driver;

        // Sensors
        private MotionSensor _motionSensorConveyor;
        private MotionSensor _motionSensorBucket;
        private ColorSensor _colorSensor;
        private PressureSensor _pressureSensor;

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
        public GameScreen(RenderWindow window, IAssetBundle bundle, uint screenWidth, uint screenHeight, IIODriver driver, int presetIndex)
        {
            _driver = driver;
            
            Font font = bundle.Font;
            Sizer sizer = new Sizer(screenWidth, screenHeight);

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

            Vector2f gateEntranceSize = sizer.Percent(0.5f, 9);
            Vector2f signalSize = sizer.Percent(3, 8);

            _marbles = bundle.GameConfiguration.Presets[presetIndex].Marbles
                .Select(mc => new Marble(sizer, new Vector2f(40, _conveyor.Position.Y), mc.Color, mc.Weight))
                .Reverse()
                .ToArray();
            
            // Set marble initialal positions based on screen dimensions
            Vector2f offset = new Vector2f(0,0);
            foreach (var marble in _marbles.Reverse())
            {
                //marble.Position = sizer.Percent(0, 0);
                marble.Position = new Vector2f(marble.Position.X, _conveyor.Position.Y - marble.Size.Y);
                marble.Position -= offset;
                offset = new Vector2f(offset.X + marble.Size.X, 0);
            }

            int bucketCount = bundle.GameConfiguration.Presets[presetIndex].Buckets.Count;
            float bucketHorizontalSpaceIncrement = 100.0f / (bucketCount + 2);
            _buckets = bundle.GameConfiguration.Presets[presetIndex].Buckets
                .Select((bc, i) => new Bucket(
                    sizer.Percent(bucketHorizontalSpaceIncrement * (i + 1), 100),
                    sizer.Percent(10, 20),
                    bc.Color,
                    bc.Weight,
                    bc.Capacity
                ))
                .ToArray();
            
            _trapDoors = _buckets
                .Select((b, i) =>
                    new Trapdoor(
                        sizer.Percent(bucketHorizontalSpaceIncrement * (i + 1), 60), 
                        new Vector2f(b.Size.X, _conveyor.Size.Y)))
                .ToArray();
            
            // Adjust bucket positions to be at very bottom of screen
            foreach (var bucket in _buckets)
            {
                bucket.Position -= new Vector2f(0, bucket.Size.Y);
            }
            
            _gateEntrance = new Gate( sizer.Percent(13, 52), gateEntranceSize );
            
            Vector2f sensorSize = new Vector2f(MarbleSorterGame.WINDOW_WIDTH/40, MarbleSorterGame.WINDOW_WIDTH/40); // Size half of the largest marble size
            Vector2f gateSensorPosition = _gateEntrance.Position - new Vector2f(sensorSize.X, -1 * (_gateEntrance.Size.Y - sensorSize.Y - _conveyor.Size.Y));
            _pressureSensor = new PressureSensor(gateSensorPosition, sensorSize);
            _colorSensor = new ColorSensor(gateSensorPosition, sensorSize) ;
            _motionSensorConveyor = new MotionSensor(gateSensorPosition, sensorSize);

            // Position half-way between conveyer and top of buckets
            Vector2f motionSensorPosition = new Vector2f(MarbleSorterGame.WINDOW_WIDTH - sensorSize.X, _buckets[0].Position.Y - sensorSize.Y);
            _motionSensorBucket = new MotionSensor(motionSensorPosition, sensorSize );

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
                sizer.Percent(115, 100),
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
            
            _entities = new GameEntity[]
            {
                _buttonStart,
                _buttonReset,
                _buttonExit,
                _conveyor,
                _pressureSensor,
                _colorSensor,
                signalColor1,
                signalColor2,
                signalPressure1,
                _motionSensorBucket,
                _motionSensorConveyor,
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
                .Concat(new [] { _gateEntrance })
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
                // TODO: Toggle the simulation (?)
                if (_driver is S7IODriver s7driver)
                {
                    s7driver.SetRunState(true);
                }
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
            
            //////////////////////////////////////////////////////////// 
            /////// BEGIN: TODO FIXME HACK

            // This is code that gets the sensors working for the demo presentation
            // It simply checks if a marble overlaps the sensor, if yes, write the value to the driver
            // In the final version this should be re-architected, perhaps using a Sensor.Update(Marble, IIODriver) methods?

            _driver.ConveyorMotionSensor = false;
            _driver.BucketMotionSensor = false;
            _driver.ColorSensor = 0;
            _driver.PressureSensor = 0;
            foreach (var marble in _marbles)
            {
                // Write color of overlapping marble
                if (_colorSensor.Overlaps(marble))
                    _driver.ColorSensor = (byte) marble.Color;
                
                // Write pressure of overlapping marble
                if (_pressureSensor.Overlaps(marble))
                    _driver.PressureSensor = (byte) marble.Weight;
                
                // If marble position is past the gate and its X-value is on the conveyer belt (not falling)
                // write to the conveyer motion sensor
                bool marbleOnConveyor = marble.Position.Y == _conveyor.Position.Y - marble.Size.Y; // TODO: This should be its own method
                bool marblePastGate = marble.Position.X > _gateEntrance.Position.X; // TODO: Should we use radius or size here?
                _driver.ConveyorMotionSensor |= marbleOnConveyor && marblePastGate;

                // If a straight horizontal line drawn from the marble across the screen touches the bucket motion sensor, fire-off the sensor
                bool marbleVerticalMatches = _motionSensorBucket.OverlapsVertical(marble);
                _driver.BucketMotionSensor |= marbleVerticalMatches;
            }
            
            //////// END: TODO FIXME HACK
            //////////////////////////////////////////////////////////// 

            foreach (var trapdoor in _trapDoors)
            {
                trapdoor.Update();
            }
            
            foreach (var marble in _marbles)
            {
                // By default, marble should roll right
                marble.SetState(MarbleState.Rolling);

                // If marble is touching gate and gate is closed, do not move
                // Marble can clip through if more than half of marble is past gate
                if (!_gateEntrance.IsFullyOpen && _gateEntrance.Overlaps(marble) 
                                               && marble.Position.X + marble.Size.X/2 < _gateEntrance.Position.X)
                {
                    marble.SetState(MarbleState.Still);
                }
                
                // If marble has started falling, keep it falling
                if (marble.Position.Y + marble.Size.Y > _conveyor.Position.Y)
                    marble.SetState(MarbleState.Falling);
            }
            
            // If marbles are touching/overlapping each other, the marble farthest to left should stop moving
            // NOTE: This assumes marble order is placed from left-to-right!
            for (int i = 0; i < _marbles.Length - 1; i++)
            {
                if (_marbles[i].MarbleOverlaps(_marbles[i + 1]))
                {
                    _marbles[i].SetState(MarbleState.Still);
                }
            }
            
            // If marble is overtop a trap-door, and the trap door is open, switch velocity to falling
            foreach (var marble in _marbles)
            {
                foreach (var trapdoor in _trapDoors)
                {
                    if (marble.InsideHorizontal(trapdoor) && trapdoor.IsOpen)
                    {
                        marble.SetState(MarbleState.Falling);
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