using System;
using System.Linq;
using System.Collections.Generic;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace MarbleSorterGame.Screens
{
    // The game itself
    public class GameScreen : Screen, IDisposable
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
        private SignalLight _gateOpen;
        private SignalLight _gateClosed;
        private Conveyor _conveyor;
        private Label _legend;
        private Marble[] _marbles;
        private Trapdoor[] _trapDoors;
        private SignalLight[] _trapDoorsOpen;
        private SignalLight[] _trapDoorsClosed;
        private Bucket[] _buckets;
        private GameEntities.Sensor[] _sensors;

        // UI Buttons
        private Button _winPopup;
        private Button _losePopup;
        private Button _buttonPause;
        private Button _buttonReset;
        private Button _buttonMainMenu;
        private Button[] _buttons;
        private RectangleShape _legendBackground;

        // Legend helper text
        private GameEntity _hoveredEntity;
        private Dictionary<string, string> _legendData;
        private readonly float _legendPadding = 5;
        
        // Game screen state
        private GameState _gameState;
        private int _marblesTotal;
        private int _marblesRemaining;
        
        // Config options
        private IAssetBundle _bundle;
        
        public GameScreen(RenderWindow window, IAssetBundle bundle)
        {
            _window = window;
            _bundle = bundle;
            _driver = bundle.GameConfiguration.Driver switch
            {
                DriverType.Keyboard => new KeyboardIODriver(),
                DriverType.Simulation => new S7IODriver(bundle.GameConfiguration.DriverOptions, bundle.IoMapConfiguration),
                _ => throw new ArgumentException($"Unknown IO driver: {bundle.GameConfiguration.Driver}")
            };
            
            Reset();
        }

        // Initialize all game entity objects and reset values to default states
        private void Reset()
        {
            // Used for positioning by percentage relative to screen
            var screen = GameLoop.WINDOW_RECT;
            var font = _bundle.Font;
            var preset = _bundle.GameConfiguration.Preset;

            _legendData = new Dictionary<string, string>();
            _marblesTotal = preset.Marbles.Count;
            _marblesRemaining = _marblesTotal;
            _gameState = GameState.Progress;

            //================= Event Handlers ====================//
            _window.MouseButtonPressed += GameMouseClickEventHandler;
            _window.KeyPressed += GameKeyEventHandler;
            _window.MouseMoved += GameMouseMoveEventHandler;
            
            // Color of the menu and legend background
            var chromeColor = new SFML.Graphics.Color(218, 224, 226);

            //================= Background widgets ====================//
            // Menu bar background (slight gray recantgle behind buttons)
            RectangleShape menuBarBackground = new RectangleShape
            {
                Position = new Vector2f(),
                Size = screen.Percent(100f, 6f),
                FillColor = chromeColor,
                OutlineColor = Color.Black,
                OutlineThickness = 2
            };

            Vector2f popupSize = screen.Percent(15f, 10f);
            Vector2f popupPosition = screen.Percent(15f, 10f);
            
            _winPopup = new Button("YOU WIN!", 20f, font, popupPosition, popupSize);
            _winPopup.FillColor = new SFML.Graphics.Color(102, 255, 51); // green
            
            _losePopup = new Button ("YOU LOSE!", 20f, font, popupPosition, popupSize);
            _losePopup.FillColor = new SFML.Graphics.Color(255, 80, 80); // red

            //================= Buttons ====================//

            Vector2f menuButtonSize = screen.Percent(8, 4);
            float menuButtonFontScale = 0.4f;
            //_buttonPause = new Button("Pause", menuButtonFontScale, font, screen.Percent(60, 3), menuButtonSize);
            //_buttonReset = new Button("Reset Game", menuButtonFontScale, font, screen.Percent(75, 3), menuButtonSize);
            //_buttonMainMenu = new Button("Main Menu", menuButtonFontScale, font, screen.Percent(90, 3), menuButtonSize);
            
            _buttonPause = new Button("Pause", menuButtonFontScale, font, default, menuButtonSize);
            _buttonReset = new Button("Reset Game", menuButtonFontScale, font, default, menuButtonSize);
            _buttonMainMenu = new Button("Main Menu", menuButtonFontScale, font, default, menuButtonSize);

            // NOTE: Button is centered by *Origin = Center*, which affects our shift values
            _buttonMainMenu.Position = screen
                .PositionRelative(Joint.End, Joint.Start) // Position top corner of screen
                .ShiftX(-menuButtonSize.X/2) // Shift left so its not over screen edge
                .ShiftX(-new Vector2f().PercentOfX(screen, 1f).X) // Add a 1% spacer from screen edge
                .ShiftY((menuButtonSize.Y)/2 + Math.Max(0, menuBarBackground.Size.Y - menuButtonSize.Y)/2); // Shift down so its vertically centered in menu bar background

            _buttonReset.Position = _buttonMainMenu.Box
                .PositionRelative(Joint.Start, Joint.Start)
                .ShiftX(-menuButtonSize.X)
                .ShiftX(-new Vector2f().PercentOfX(screen, 1f).X); // Add a 1% spacer from button edge
            
            _buttonPause.Position = _buttonReset.Box
                .PositionRelative(Joint.Start, Joint.Start)
                .ShiftX(-menuButtonSize.X)
                .ShiftX(-new Vector2f().PercentOfX(screen, 1f).X); // Add a 1% spacer from button edge

            _buttonPause.ClickEvent += PauseButtonClickHandler;
            _buttonReset.ClickEvent += ResetButtonClickHandler;
            _buttonMainMenu.ClickEvent += MainMenuButtonClickHandler;
            
            _buttons = new[] { _buttonMainMenu, _buttonPause, _buttonReset, _winPopup, _losePopup };

            //================= Labels ====================//
            // string instructionsText = "Use the Input/Output Addresses shown below to create a working \nPLC for the marble sorter, based on the requirements on the buckets below.";
            // //Label instructions = new Label(instructionsText,screen.Percent(0, 3), 14, SFML.Graphics.Color.Black, font);
            // var instructions = QuickShape.Label(instructionsText, new Vector2f(0, 0), font, Color.Black);
            // instructions.Scale = new Vector2f(0.5f, 0.5f);
            // var gameScreenTitle = QuickShape.Label("Marble Sorter", new Vector2f(0, 0), font, Color.Black);

            var legendBackgroundSize = screen.Percent(50f, 40f);
            _legendBackground = new RectangleShape
            {
                Size = legendBackgroundSize,
                FillColor = chromeColor,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                //Position = menuBarBackground.PositionRelative(Joint.End, Joint.End).ShiftX(-legendBackgroundSize.X)
                Position = menuBarBackground.PositionRelative(Joint.Start, Joint.End)
                    .ShiftX(screen.Percent(0, 1).Y)
                    .ShiftY(screen.Percent(0, 1).Y)
            };

            _legend = new Label(String.Empty, _legendBackground.Position.Shift(new Vector2f(_legendPadding, _legendPadding)), 14, SFML.Graphics.Color.Black, font);

            //================= Game Entities ====================//
            _conveyor = new Conveyor(screen.Percent(0, 60), screen.Percent(100, 1), new Vector2f(1, 0));

            _marbles = preset.Marbles
                .Select(mc => new Marble(screen, new Vector2f(40, _conveyor.Position.Y), mc.Color.ToGameColor(), mc.Weight.ToGameWeight()))
                .Reverse()
                .ToArray();

            // Set marble initial positions based on screen dimensions
            float offset = 0;
            foreach (var marble in _marbles.Reverse())
            {
                marble.Position = marble.Position
                    .PositionRelativeToY(_conveyor.Box, Joint.Start)
                    .ShiftY(-marble.GlobalBounds.Height)
                    .ShiftX(-offset);
                
                offset += marble.Size.X;
            }

            int bucketCount = preset.Buckets.Count;
            float bucketHorizontalSpaceIncrement = 125.0f / (bucketCount + 2);
            _buckets = preset.Buckets
                .Select((bc, i) => new Bucket(
                    screen.Percent(bucketHorizontalSpaceIncrement * (i + 1), 100),
                    screen.Percent(10, 20),
                    bc.Color?.ToGameColor(),
                    bc.Weight?.ToGameWeight(),
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
            
            Vector2f gateEntranceSize = screen.Percent(0.5f, 0);
            gateEntranceSize.Y = Marble.MarbleSizeLarge;
            // screen.Percent(13, 52)
            _gateEntrance = new Gate(
                _conveyor.Box
                    .PositionRelative(Joint.Start, Joint.Start)
                    .ShiftX(screen.Percent(15, 0).X)
                    .ShiftY(-gateEntranceSize.Y), gateEntranceSize);

            Vector2f sensorSize = new Vector2f(Marble.MarbleSizeSmall, Marble.MarbleSizeSmall);
            
            Vector2f gateSensorPosition = _gateEntrance.Box
                .PositionRelative(Joint.Start, Joint.End)
                .ShiftY(-sensorSize.Y)
                .ShiftX(-sensorSize.X);
            
            _pressureSensor = new PressureSensor(gateSensorPosition, sensorSize);
            _colorSensor = new ColorSensor(gateSensorPosition, sensorSize);
            
            // Difference between middle of motion sensor and right-edge of gate
            float motionSensorXPosition = MarbleSorterGame.WINDOW_WIDTH - sensorSize.X;
            float motionSensorLaserRange = (_gateEntrance.Position.X + _gateEntrance.Size.X) - (motionSensorXPosition + sensorSize.X / 2);
            _motionSensorConveyor = new MotionSensor(motionSensorLaserRange, new Vector2f(motionSensorXPosition, gateSensorPosition.Y), sensorSize);

            PressureSensor sensorPressureStart = new PressureSensor(screen.Percent(3, 55), sensorSize);
            ColorSensor sensorColorStart = new ColorSensor(screen.Percent(6, 55), sensorSize);

            // Position half-way between conveyor and top of buckets
            Vector2f motionSensorPosition = new Vector2f(MarbleSorterGame.WINDOW_WIDTH - sensorSize.X, _buckets[0].Position.Y - sensorSize.Y);
            _motionSensorBucket = new MotionSensor(motionSensorLaserRange, motionSensorPosition, sensorSize);

            Vector2f signalSize = screen.Percent(3, 8);
            var signalColor1 = new SignalLight(screen.Percent(30, 20), signalSize );
            var signalColor2 = new SignalLight(new Vector2f(signalColor1.Position.X + signalSize.X + 10, signalColor1.Position.Y), signalSize );
            var signalPressure1 = new SignalLight(new Vector2f(signalColor2.Position.X + signalSize.X + 10, signalColor1.Position.Y), signalSize );
            var signalMotion1 = new SignalLight(screen.Percent(95, 50), signalSize );
            _gateOpen = new SignalLight(new Vector2f(_gateEntrance.Position.X + gateEntranceSize.X + 10, _trapDoors[0].Position.Y - 50), signalSize);
            _gateClosed = new SignalLight(new Vector2f(_gateEntrance.Position.X + gateEntranceSize.X + 10, _trapDoors[0].Position.Y + 25), signalSize);
            var conveyorOn = new SignalLight(screen.Percent(5, 75), signalSize );
            var bucketDropped = new SignalLight(screen.Percent(115, 100), signalSize );
            
            var trapDoorSizeX = _trapDoors[0].Size.X;
            
            var trapdoorOpen1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X + trapDoorSizeX, _trapDoors[0].Position.Y - 50), signalSize);
            var trapdoorOpen2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X + trapDoorSizeX, _trapDoors[1].Position.Y - 50), signalSize);
            var trapdoorOpen3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X + trapDoorSizeX, _trapDoors[2].Position.Y - 50), signalSize);
            _trapDoorsOpen = new[] {trapdoorOpen1, trapdoorOpen2, trapdoorOpen3};
            
            var trapdoorClosed1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X - signalSize.X, _trapDoors[0].Position.Y + 50), signalSize);
            var trapdoorClosed2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X - signalSize.X, _trapDoors[1].Position.Y + 50), signalSize);
            var trapdoorClosed3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X - signalSize.X, _trapDoors[2].Position.Y + 50), signalSize);
            _trapDoorsClosed = new[] {trapdoorClosed1, trapdoorClosed2, trapdoorClosed3};

            _entities = new GameEntity[]
            {
                _buttonPause,
                _buttonReset,
                _buttonMainMenu,
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
                _gateOpen,
                _gateClosed,
                // conveyorOn,
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
                .Concat(_trapDoors)
                .Concat(_marbles)
                .Concat(_buckets)
                .Concat(new[] { _gateEntrance })
                .ToArray();

            //load assets for all entities
            foreach (GameEntity entity in _entities)
                entity.Load(_bundle);

            _drawables = new Drawable[]
            {
                menuBarBackground,
                _legendBackground,
                //instructions,
                //gameScreenTitle,
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

        // Mouse movement event handler for features involving hover-over
        private void GameMouseMoveEventHandler(object? sender, MouseMoveEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromMouseEvent(_window, _buttons, mouse);
        }

        // Mouse click event handler for button clicking
        private void GameMouseClickEventHandler(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }

        // Keyboard event handlers for keyboard driver, for debug purposes
        private void GameKeyEventHandler(object? sender, KeyEventArgs key)
        {
            if (_driver is KeyboardIODriver kbdriver)
                kbdriver.UpdateByKey(key);
        }

        // Writes driver values to the game and reads in game state 
        private void UpdateDriver()
        {
            _gateEntrance.SetState(_driver.Gate);
            
            _gateOpen.SetState(_gateEntrance.IsFullyOpen);
            _gateClosed.SetState(_gateEntrance.IsFullyClosed);
            
            _conveyor.SetState(_driver.Conveyor);

            _trapDoors[0].SetState(_driver.TrapDoor1);
            _trapDoors[1].SetState(_driver.TrapDoor2);
            _trapDoors[2].SetState(_driver.TrapDoor3);

            _trapDoorsOpen[0].SetState(_trapDoors[0].IsFullyOpen);
            _trapDoorsOpen[1].SetState(_trapDoors[1].IsFullyOpen);
            _trapDoorsOpen[2].SetState(_trapDoors[2].IsFullyOpen);
            
            _trapDoorsClosed[0].SetState(_trapDoors[0].IsFullyClosed);
            _trapDoorsClosed[1].SetState(_trapDoors[1].IsFullyClosed);
            _trapDoorsClosed[2].SetState(_trapDoors[2].IsFullyClosed);
            
            //////////////////////////////////////// 
            ///// BEGIN: TODO FIXME HACK

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
            
            // If marble position is past the gate and its X-value is on the conveyor belt (not falling)
            // write to the conveyor motion sensor
            _motionSensorConveyor.Update(_marbles);
            _driver.ConveyorMotionSensor |= _motionSensorConveyor.Detected;

            // If a straight horizontal line drawn from the marble across the screen touches the bucket motion sensor, fire-off the sensor
            _motionSensorBucket.Update(_marbles);
            _driver.BucketMotionSensor |= _motionSensorBucket.Detected;
            
            ////// END: TODO FIXME HACK
            ////////////////////////////////////////

            // Update IIODriver instance
            _driver.Update();
        }

        // Update marble positioning
        private void UpdateMarbles()
        {
            foreach (var marble in _marbles)
            {
                // By default, marble should roll right
                marble.SetState(MarbleState.Rolling);

                // If marble is touching gate and gate is closed, do not move
                // Marble can clip through if more than half of marble is past gate
                if (!_gateEntrance.IsFullyOpen && _gateEntrance.Overlaps(marble) 
                                               && marble.Position.X < _gateEntrance.Position.X)
                {
                    marble.SetState(MarbleState.Still);
                }
                
                // If the right edge of the marble is passed the left edge of the gate, keep it going
                if (marble.Position.X + (marble.Size.X) > _gateEntrance.Position.X + _gateEntrance.Size.X)
                    marble.SetState(MarbleState.Rolling);
                
                // If conveyor is off, marbles stay still
                if (!_conveyor.ConveyorOn)
                    marble.SetState(MarbleState.Still);
                
                // If marble has started falling, keep it falling
                if (marble.Position.Y + marble.Size.Y > _conveyor.Position.Y)
                    marble.SetState(MarbleState.Falling);
            }
            
            // If marbles are touching/overlapping each other, the marble farthest to left should stop moving
            // NOTE: This assumes marble order is placed from left-to-right!
            for (int i = 0; i < _marbles.Length - 1; i++)
                if (_marbles[i].MarbleOverlaps(_marbles[i + 1]))
                    _marbles[i].SetState(MarbleState.Still);
            
            // If marble is overtop a trap-door, and the trap door is open, switch velocity to falling
            foreach (var marble in _marbles)
                foreach (var trapdoor in _trapDoors)
                    if (marble.InsideHorizontal(trapdoor) && trapdoor.IsOpen)
                        marble.SetState(MarbleState.Falling);

            // Now actually update marble coordinates
            foreach (var m in _marbles)
                m.Update();
            
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
        }

        // Update door positions
        private void UpdateDoors()
        {
            foreach (var trapdoor in _trapDoors)
                trapdoor.Update();

            // Update entrace gate position
            _gateEntrance.Update(_marbles);
        }

        // Update legend text
        private void UpdateLegend()
        {
            _legendData["Currently Hovered Item"] = _hoveredEntity == null ? "N/A" : _hoveredEntity.InfoText;
            _legendData["Marbles Remaining Total"] = _marblesRemaining.ToString();
            _legendData["Marbles Correctly Dropped"] = _buckets.Select(b => b.TotalCorrect).Sum().ToString();
            _legendData["Marbles Incorrectly Dropped"] = _buckets.Select(b => b.TotalIncorrect).Sum().ToString();
            _legendData["PLC Devices I/O"] = "";
            _legendData["Conveyor State"] = _driver.Conveyor.ToString();
            _legendData["Trapdoor 1 Opening"] = _driver.TrapDoor1.ToString();
            _legendData["Trapdoor 2 Opening"] = _driver.TrapDoor2.ToString();
            _legendData["Trapdoor 3 Opening"] = _driver.TrapDoor3.ToString();
            _legendData["Entrance Gate Opening"] = _driver.Gate.ToString();

            var legendBuilder = new System.Text.StringBuilder();
            foreach (var stat in _legendData)
                legendBuilder.AppendLine(String.Format("{0,-40}: {1}", stat.Key, stat.Value));
            
            _legend.Text = legendBuilder.ToString();
            // Automatically adjust background height and width according to height of the text label
            var legendBounds = _legend.LabelText.GetGlobalBounds();
            _legendBackground.Size = new Vector2f(legendBounds.Width + _legendPadding*4, legendBounds.Height + _legendPadding*2);
        }

        // Update game state from key events such as bucket handling and win state
        public void UpdateGameState()
        {
            _marblesRemaining = _marblesTotal - _buckets.Select(b => b.TotalMarbles).Sum();
            int marblesCorrectDrop = _buckets.Select(b => b.TotalCorrect).Sum();
            int marblesIncorrectDrop = _buckets.Select(b => b.TotalIncorrect).Sum();

            //check if marble that passed through all trapdoors was supposed to be in a bucket
            foreach (var marble in _marbles)
            {
                if (marble.Position.X - marble.Radius > _window.Size.X)
                {
                    if (_buckets.Any(b => b.ValidateMarble(marble)))
                        marblesCorrectDrop++;
                    else
                        marblesIncorrectDrop++;

                    _marblesRemaining--;
                }
            }

            //check game win state
            if (_marblesRemaining == 0 && marblesIncorrectDrop == 0)
                _gameState = GameState.Win;
            else if (marblesIncorrectDrop > 0)
                _gameState = GameState.Lose;
        }

        // Update all game entity positions, game state and driver input/output
        public override void Update()
        {
            UpdateLegend();

            if (_gameState == GameState.Progress)
            {
                UpdateDriver();
                UpdateMarbles();
                UpdateDoors();
                UpdateGameState(); // Win, lose, or pause
            }
        }
        
        // Redraw screen in preparation for next update loop
        public override void Draw(RenderWindow window)
        {
            foreach (var drawable in _drawables)
                window.Draw(drawable);
            
            foreach (var entity in _entities) 
                entity.Render(window);
        }

        // Event handler for pause button click
        private void PauseButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            if (_gameState == GameState.Pause)
                _gameState = GameState.Progress;
            else if (_gameState == GameState.Progress)
                _gameState = GameState.Pause;
            
            //if (_driver is S7IODriver s7driver)
            //   s7driver.SetRunState(true);
        }

        // Event handler for reset button click
        private void ResetButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            Dispose();
            Reset();
        }

        // Event handler for main menu button click
        private void MainMenuButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            Dispose();
            MarbleSorterGame.ActiveMenu = Menu.Main;
        }

        // Dispose all event handlers associated with the game screen
        public void Dispose()
        {
            _buttonPause.ClickEvent -= PauseButtonClickHandler;
            _buttonReset.ClickEvent -= ResetButtonClickHandler;
            _buttonMainMenu.ClickEvent -= MainMenuButtonClickHandler;
            _window.MouseButtonPressed -= GameMouseClickEventHandler;
            _window.KeyPressed -= GameKeyEventHandler;
            _window.MouseMoved -= GameMouseMoveEventHandler;
        }
    }
}