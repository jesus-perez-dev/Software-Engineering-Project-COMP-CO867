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
        private RenderWindow _window;
        private Drawable[] _drawables;
        private GameEntity[] _entities;
        
        private IIODriver _driver;

        // Sensors
        private MotionSensor _motionSensorConveyor;
        private MotionSensor _motionSensorBucket;
        private ColorSensor _colorSensor;
        private PressureSensor _pressureSensor;

        // Other Entities
        private Gate _gateEntrance;
        private Conveyor _conveyor;
        private Label _legend;
        private Marble[] _marbles;
        private Trapdoor[] _trapDoors;
        private Bucket[] _buckets;
        private GameEntities.Sensor[] _sensors;

        // UI Buttons
        private Button _winPopup;
        private Button _losePopup;
        private Button _buttonStart;
        private Button _buttonReset;
        private Button _buttonExit;
        private Button[] _buttons;

        // Legend helper text
        private GameEntity _hoveredEntity;
        private Dictionary<string, string> _legendData;
        
        // Game screen state
        private GameState _gameState;
        private int _marblesTotal;
        private int _marblesRemaining;

        
        // TODO: Pass a game configuration structure in here instead of width/height uints
        public GameScreen(RenderWindow window, IAssetBundle bundle, uint screenWidth, uint screenHeight, IIODriver driver, int presetIndex)
        {
            // Used for positioning by percentage relative to screen
            var screen = GameLoop.WINDOW_RECT;
            
            _driver = driver;
            _window = window;
            var font = bundle.Font;

            _legendData = new Dictionary<string, string>() { };
            _marblesTotal = bundle.GameConfiguration.Presets[presetIndex].Marbles.Count;
            _marblesRemaining = _marblesTotal;
            _gameState = GameState.Progress;

            //================= Event Handlers ====================//
            _window.MouseButtonPressed += GameMouseClickEventHandler;
            _window.KeyPressed += GameKeyEventHandler;
            _window.MouseMoved += GameMouseMoveEventHandler;

            //================= Background widgets ====================//
            // Menu bar background (slight gray recantgle behind buttons)
            RectangleShape menuBarBackground = new RectangleShape
            {
                Position = new Vector2f(),
                Size = screen.Percent(100f, 6f),
                FillColor = new SFML.Graphics.Color(89, 105, 115) // dark-blue-gray ish color
            };

            Vector2f popupSize = screen.Percent(15f, 10f);
            Vector2f popupPosition = screen.Percent(15f, 10f);
            
            _winPopup = new Button("YOU WIN!", 20f, font, popupPosition, popupSize);
            _winPopup.FillColor = new SFML.Graphics.Color(102, 255, 51); // green
            
            _losePopup = new Button ("YOU LOSE!", 20f, font, popupPosition, popupSize);
            _losePopup.FillColor = new SFML.Graphics.Color(255, 80, 80); // red

            //================= Buttons ====================//

            Vector2f menuButtonSize = screen.Percent(13, 5);
            float menuButtonFontScale = 0.4f;
            _buttonStart = new Button("Start Simulation", menuButtonFontScale, font, screen.Percent(60, 3), menuButtonSize);
            _buttonReset = new Button("Reset Game", menuButtonFontScale, font, screen.Percent(75, 3), menuButtonSize);
            _buttonExit = new Button("Exit Game", menuButtonFontScale, font, screen.Percent(90, 3), menuButtonSize);

            _buttonStart.ClickEvent += StartSimulationButtonClickHandler;
            _buttonReset.ClickEvent += ResetButtonClickHandler;
            _buttonExit.ClickEvent += ExitButtonClickHandler;
            
            _buttons = new[] { _buttonExit, _buttonStart, _buttonReset, _winPopup, _losePopup };

            //================= Labels ====================//
            String instructionsText = "Use the Input/Output Addresses shown below to create a working \nPLC for the marble sorter, based on the requirements on the buckets below.";

            Label instructions = new Label(
                instructionsText,
                screen.Percent(29.5f, 3),
                14,
                SFML.Graphics.Color.Black,
                font);

            var legendBackgroundSize = screen.Percent(30.5f, 40f);
            RectangleShape legendBackground = new RectangleShape
            {
                Size = legendBackgroundSize,
                FillColor = new SFML.Graphics.Color(89, 105, 115), // dark-blue-gray ish color
                Position = menuBarBackground
                            .PositionRelative(Joint.End, Joint.End)
                            .ShiftX(-legendBackgroundSize.X)
            };

            _legend = new Label(String.Empty, legendBackground.Position.Shift(new Vector2f(5, 5)), 14, SFML.Graphics.Color.Black, font);

            //================= Game Entities ====================//

            _conveyor = new Conveyor(
                screen.Percent(0, 60),
                screen.Percent(100, 1),
                new Vector2f(1, 0)
                );

            Vector2f gateEntranceSize = screen.Percent(0.5f, 9);
            Vector2f signalSize = screen.Percent(3, 8);

            _marbles = bundle.GameConfiguration.Presets[presetIndex].Marbles
                .Select(mc => new Marble(screen, new Vector2f(40, _conveyor.Position.Y), mc.Color, mc.Weight))
                .Reverse()
                .ToArray();

            // Set marble initialal positions based on screen dimensions
            float offset = 0;
            foreach (var marble in _marbles.Reverse())
            {
                marble.Position = marble.Position
                    .PositionRelativeToY(_conveyor.Box, Joint.Start)
                    .ShiftY(-marble.GlobalBounds.Height)
                    .ShiftX(-offset);
                
                offset += marble.Size.X;
            }

            int bucketCount = bundle.GameConfiguration.Presets[presetIndex].Buckets.Count;
            float bucketHorizontalSpaceIncrement = 100.0f / (bucketCount + 2);
            _buckets = bundle.GameConfiguration.Presets[presetIndex].Buckets
                .Select((bc, i) => new Bucket(
                    screen.Percent(bucketHorizontalSpaceIncrement * (i + 1), 100),
                    screen.Percent(10, 20),
                    bc.Color,
                    bc.Weight,
                    bc.Capacity
                ))
                .ToArray();

            _trapDoors = _buckets
                .Select((b, i) =>
                    new Trapdoor(
                        screen.Percent(bucketHorizontalSpaceIncrement * (i + 1), 60),
                        new Vector2f(b.Size.X, _conveyor.Size.Y)))
                .ToArray();

            // Adjust bucket positions to be at very bottom of screen
            foreach (var bucket in _buckets)
            {
                bucket.Position -= new Vector2f(0, bucket.Size.Y);
            }

            _gateEntrance = new Gate(screen.Percent(13, 52), gateEntranceSize);

            Vector2f sensorSize = new Vector2f(MarbleSorterGame.WINDOW_WIDTH / 40, MarbleSorterGame.WINDOW_WIDTH / 40); // Size half of the largest marble size
            
            Vector2f gateSensorPosition = _gateEntrance.Box
                .PositionRelative(Joint.Start, Joint.End)
                .ShiftY(-_conveyor.GlobalBounds.Height)
                .ShiftY(-sensorSize.Y)
                .ShiftX(-sensorSize.X);
            
            _pressureSensor = new PressureSensor(gateSensorPosition, sensorSize);
            _colorSensor = new ColorSensor(gateSensorPosition, sensorSize);
            
            // Difference between middle of motion sensor and right-edge of gate
            float motionSensorXPosition = MarbleSorterGame.WINDOW_WIDTH - sensorSize.X;
            float motionSensorLaserRange = (_gateEntrance.Position.X + _gateEntrance.Size.X) - (motionSensorXPosition + sensorSize.X / 2);
            _motionSensorConveyor = new MotionSensor(motionSensorLaserRange, new Vector2f(motionSensorXPosition, gateSensorPosition.Y), sensorSize);
            _gateEntrance = new Gate(screen.Percent(13, 52), gateEntranceSize);

            PressureSensor sensorPressureStart = new PressureSensor(screen.Percent(3, 55), sensorSize);
            ColorSensor sensorColorStart = new ColorSensor(screen.Percent(6, 55), sensorSize);

            // Position half-way between conveyer and top of buckets
            Vector2f motionSensorPosition = new Vector2f(MarbleSorterGame.WINDOW_WIDTH - sensorSize.X, _buckets[0].Position.Y - sensorSize.Y);
            _motionSensorBucket = new MotionSensor(motionSensorLaserRange, motionSensorPosition, sensorSize);

            var signalColor1 = new SignalLight(screen.Percent(30, 20), signalSize );
            var signalColor2 = new SignalLight(new Vector2f(signalColor1.Position.X + signalSize.X + 10, signalColor1.Position.Y), signalSize );
            var signalPressure1 = new SignalLight(new Vector2f(signalColor2.Position.X + signalSize.X + 10, signalColor1.Position.Y), signalSize );
            var signalMotion1 = new SignalLight(screen.Percent(95, 50), signalSize );
            var gateOpen = new SignalLight(screen.Percent(10, 74), signalSize );
            var gateClosed = new SignalLight(new Vector2f(gateOpen.Position.X + signalSize.X + 10, gateOpen.Position.Y), signalSize );
            var conveyerOn = new SignalLight(screen.Percent(5, 75), signalSize );
            var bucketDropped = new SignalLight(screen.Percent(115, 100), signalSize );
            var trapdoorOpen1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X, _trapDoors[0].Position.Y + 150), signalSize );
            var trapdoorClosed1 = new SignalLight(new Vector2f(trapdoorOpen1.Position.X + signalSize.X + 10, trapdoorOpen1.Position.Y), signalSize );
            var trapdoorOpen2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X + 100, _trapDoors[1].Position.Y + 150), signalSize );
            var trapdoorClosed2 = new SignalLight(new Vector2f(trapdoorOpen2.Position.X + signalSize.X + 10, trapdoorOpen2.Position.Y), signalSize );
            var trapdoorOpen3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X + 200, _trapDoors[2].Position.Y + 150), signalSize );
            var trapdoorClosed3 = new SignalLight(new Vector2f(trapdoorOpen3.Position.X + signalSize.X + 10, trapdoorOpen3.Position.Y), signalSize );

            _entities = new GameEntity[]
            {
                _buttonStart,
                _buttonReset,
                _buttonExit,
                /**
                _winPopup,
                _losePopup,
                */
                _conveyor,
                _pressureSensor,
                _colorSensor,
                /*
                signalColor1,
                signalColor2,
                signalPressure1,
                */
                _motionSensorBucket,
                _motionSensorConveyor,
                /*
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
                */
                // signalMotion1,
            };

            _entities = _entities.ToList()
                .Concat(_marbles)
                .Concat(_trapDoors)
                .Concat(_buckets)
                .Concat(new[] { _gateEntrance })
                .ToArray();

            foreach (GameEntity entity in _entities)
            {
                entity.Load(bundle);
            }

            _drawables = new Drawable[]
            {
                menuBarBackground,
                legendBackground,
                instructions,
                _legend
            };

            //gameentity addresses infotext
            _trapDoors[0].InfoText = "Trapdoor 1 \n %Q0.0 Bool";
            _trapDoors[1].InfoText = "Trapdoor 2 \n %Q0.1 Bool";
            _trapDoors[2].InfoText = "Trapdoor 3 \n %Q0.2 Bool";
            sensorColorStart.InfoText = "Color Sensor \n %Q0.2 Bool";
            sensorPressureStart.InfoText = "Pressure Sensor \n %Q0.2 Bool";
            _motionSensorBucket.InfoText = "Motion Sensor \n %Q0.2 Bool";

            sensorColorStart.InfoText = "Color Sensor \n %Q0.2 Bool";
            _buckets[0].InfoText = "Bucket 1 \n %I1.0 Bool";
            _buckets[1].InfoText = "Bucket 2 \n %I1.0 Bool";
            _buckets[2].InfoText = "Bucket 3 \n %I1.0 Bool";
        }

        private void GameMouseMoveEventHandler(object? sender, MouseMoveEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromMouseEvent(_window, _buttons, mouse);
        }

        private void GameMouseClickEventHandler(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }

        private void GameKeyEventHandler(object? sender, KeyEventArgs key)
        {
            if (_driver is KeyboardIODriver kbdriver)
                kbdriver.UpdateByKey(key);
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
            }
            
            // If marble position is past the gate and its X-value is on the conveyer belt (not falling)
            // write to the conveyer motion sensor
            _motionSensorConveyor.Update(_marbles);
            _driver.ConveyorMotionSensor |= _motionSensorConveyor.Detected;

            // If a straight horizontal line drawn from the marble across the screen touches the bucket motion sensor, fire-off the sensor
            _motionSensorBucket.Update(_marbles);
            _driver.BucketMotionSensor |= _motionSensorBucket.Detected;
            
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
                        marble.Position = new Vector2f(50, 50000);
                        bool success = bucket.InsertMarble(marble);
                    }
                }
            }

            // Update entrace gate position
            _gateEntrance.Update(_marbles);
            
            // Update IIODriver instance
            _driver.Update();

            // Update legend text
            _marblesRemaining = _marblesTotal - _buckets.Select(b => b.TotalMarbles).Sum();
            int marblesCorrectDrop = _buckets.Select(b => b.TotalCorrect).Sum();
            int marblesIncorrectDrop = _buckets.Select(b => b.TotalIncorrect).Sum();

            //check if marble that passed through all trapdoors was supposed to be in a bucket
            foreach (var marble in _marbles)
            {
                if (marble.Position.X - marble.Radius > _window.Size.X)
                {
                    bool marbleSkipped = false;

                    foreach(var bucket in _buckets)
                    {
                        if (bucket.ValidateMarble(marble))
                        {
                            marbleSkipped = true;
                        }
                    }

                    if (marbleSkipped)
                    {
                        marblesCorrectDrop++;
                    } else
                    {
                        marblesIncorrectDrop++;
                    }

                    _marblesRemaining--;
                }

            }

            //check game win state
            if (_marblesRemaining == 0 && marblesIncorrectDrop == 0)
            {
                _gameState = GameState.Win;
            }
            else if (marblesIncorrectDrop > 0)
            {
                _gameState = GameState.Lose;
            }

            _legendData["Game Data"] = "";
            _legendData["Marbles Remaining Total"] = _marblesRemaining.ToString();
            _legendData["Marbles Correctly Dropped"] = marblesCorrectDrop.ToString();
            _legendData["Marbles Incorrectly Dropped"] = marblesIncorrectDrop.ToString();
            _legendData["PLC Devices I/O"] = "";
            _legendData["Conveyor State"] = _driver.Conveyor.ToString();
            _legendData["Trapdoor 1 Opening"] = _driver.TrapDoor1.ToString();
            _legendData["Trapdoor 2 Opening"] = _driver.TrapDoor2.ToString();
            _legendData["Trapdoor 3 Opening"] = _driver.TrapDoor3.ToString();
            _legendData["Entrance Gate Opening"] = _driver.Gate.ToString();

            var legendBuilder = new System.Text.StringBuilder();

            if (_hoveredEntity != null)
                legendBuilder.AppendLine(_hoveredEntity.InfoText);

            foreach (var stat in _legendData)
                legendBuilder.AppendLine(String.Format("{0,-30}: {1}", stat.Key, stat.Value));
            
            _legend.Text = legendBuilder.ToString();
        }
        
        /// Method that gets called when the screen is to be redrawn
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

        public void StartSimulationButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            if (_driver is S7IODriver s7driver)
                s7driver.SetRunState(true);
        }

        public void ResetButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
        }

        public void ExitButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            Dispose();
            _window.Close();
        }

        public void Dispose()
        {
            _buttonStart.ClickEvent -= StartSimulationButtonClickHandler;
            _buttonReset.ClickEvent -= ResetButtonClickHandler;
            _buttonExit.ClickEvent -= ExitButtonClickHandler;
            _window.MouseButtonPressed -= GameMouseClickEventHandler;
            _window.KeyPressed -= GameKeyEventHandler;
            _window.MouseMoved -= GameMouseMoveEventHandler;
        }
    }
}