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
using MarbleSorterGame.Drivers;

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
        private Marble[] _marbles;
        private Trapdoor[] _trapDoors;
        private SignalLight[] _trapDoorsOpen;
        private SignalLight[] _trapDoorsClosed;
        private Bucket[] _buckets;
        private List<GameEntity> _sensors;
        private List<GameEntity> _hoverable;
        private List<GameEntity> _mappable;
        private List<GameEntity> _signalLights;
        private SignalLight[] _signalLightsColors;
        private SignalLight[] _signalLightsWeights;

        // UI Buttons
        private Button _buttonPause;
        private Button _buttonReset;
        private Button _buttonMainMenu;
        private Button[] _buttons;

        // Win/Lose boxes
        private RectangleShape _winPopupBox;
        private RectangleShape _losePopupBox;
        private Label _winPopup;
        private Label _losePopup;

        // Infobox helper text
        private Vector2f _infoBoxBackgroundSize;
        private GameEntity _hoveredEntity;
        private RectangleShape _infoBoxBackground;
        private Label _infoLabel;
        private Dictionary<string, string> _infoData;

        // Legend area
        private Dictionary<string, string> _IOMapping;
        private Label _legendGame;
        private Label _legendMapping;
        private RectangleShape _legendGameBackground;
        private RectangleShape _legendMappingBackground;
        private Dictionary<string, string> _legendGameData;
        private Dictionary<string, string> _legendMappingData;
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

            _IOMapping = new Dictionary<string, string>();
            _legendGameData = new Dictionary<string, string>();
            _marblesTotal = preset.Marbles.Count;
            _marblesRemaining = _marblesTotal;
            _gameState = GameState.Progress;

            _infoData = new Dictionary<string, string>();

            //================= Event Handlers ====================//
            _window.MouseButtonPressed += GameMouseClickEventHandler;
            _window.KeyPressed += GameKeyEventHandler;
            _window.MouseMoved += GameMouseMoveEventHandler;

            //================= Background widgets ====================//
            // Color of the menu and legend background
            var chromeColor = new SFML.Graphics.Color(218, 224, 226);

            // Menu bar background (slight gray recantgle behind buttons)
            RectangleShape menuBarBackground = new RectangleShape
            {
                Position = new Vector2f(),
                Size = screen.Percent(100f, 6f),
                FillColor = chromeColor,
                OutlineColor = Color.Black,
                OutlineThickness = 2
            };

            //================= Buttons ====================//

            Vector2f menuButtonSize = screen.Percent(8, 4);
            float menuButtonFontScale = 0.4f;
            
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
            
            _buttons = new[] { _buttonMainMenu, _buttonPause, _buttonReset};

            //================= Labels ====================//
            // string instructionsText = "Use the Input/Output Addresses shown below to create a working \nPLC for the marble sorter, based on the requirements on the buckets below.";
            // //Label instructions = new Label(instructionsText,screen.Percent(0, 3), 14, SFML.Graphics.Color.Black, font);
            // var instructions = QuickShape.Label(instructionsText, new Vector2f(0, 0), font, Color.Black);
            // instructions.Scale = new Vector2f(0.5f, 0.5f);
            // var gameScreenTitle = QuickShape.Label("Marble Sorter", new Vector2f(0, 0), font, Color.Black);

            var legendBackgroundSize = screen.Percent(50f, 40f);
            _legendGameBackground = new RectangleShape
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
            _legendGame = new Label(string.Empty, _legendGameBackground.Position.Shift(new Vector2f(_legendPadding, _legendPadding)), 14, SFML.Graphics.Color.Black, font);

            _legendMappingBackground = new RectangleShape
            {
                Size = legendBackgroundSize,
                FillColor = chromeColor,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                Position = _legendGameBackground.PositionRelative(Joint.End, Joint.Start)
            };
            _legendMapping = new Label(string.Empty, _legendMappingBackground.Position.Shift(new Vector2f(_legendPadding, _legendPadding)), 14, SFML.Graphics.Color.Black, font);

            _infoBoxBackgroundSize = screen.Percent(30f, 15f);
            _infoBoxBackground = new RectangleShape
            {
                Size = new Vector2f(0,0),
                FillColor = chromeColor,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                Position = menuBarBackground.PositionRelative(Joint.End, Joint.End)
                    .ShiftX(-_infoBoxBackgroundSize.X - _legendPadding * 2)
                    .ShiftY(_legendPadding)
            };
            _infoLabel = new Label(string.Empty, _infoBoxBackground.Position.Shift(new Vector2f(_legendPadding, _legendPadding)), 14, SFML.Graphics.Color.Black, font);

            //win/lose screens
            Vector2f popupSize = screen.Percent(15f, 10f);

            _winPopupBox = new RectangleShape
            {
                Size = new Vector2f(0f, 0f),
                FillColor = SFML.Graphics.Color.Green,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                Position = _infoBoxBackground.PositionRelative(Joint.Start, Joint.End)
                    .ShiftY(_legendPadding)
            };

            _losePopupBox = new RectangleShape
            {
                Size = new Vector2f(0f, 0f),
                FillColor = SFML.Graphics.Color.Red,
                OutlineColor = Color.Black,
                OutlineThickness = 2,
                Position = _infoBoxBackground.PositionRelative(Joint.Start, Joint.End)
                    .ShiftY(_legendPadding)
            };

            _winPopup = new Label(string.Empty, _winPopupBox.Position.Shift(new Vector2f(_legendPadding, _legendPadding)), 14, SFML.Graphics.Color.Black, font);
            _losePopup = new Label(string.Empty, _losePopupBox.Position.Shift(new Vector2f(_legendPadding, _legendPadding)), 14, SFML.Graphics.Color.Black, font);

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

            _trapDoors[0].Name = IOKeys.Q.TrapDoor1;
            _trapDoors[1].Name = IOKeys.Q.TrapDoor2;
            _trapDoors[2].Name = IOKeys.Q.TrapDoor3;

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

            // Position half-way between conveyer and top of buckets
            Vector2f motionSensorPosition = new Vector2f(MarbleSorterGame.WINDOW_WIDTH - sensorSize.X, _buckets[0].Position.Y - sensorSize.Y);
            _motionSensorBucket = new MotionSensor(motionSensorLaserRange, motionSensorPosition, sensorSize);

            // Signal Lights
            var temp = screen.Percent(2f, 2f);
            Vector2f signalSize = new Vector2f(temp.X, temp.X);

            var trapDoorSizeX = _trapDoors[0].Size.X;
            var signalColor1 = new SignalLight(_gateEntrance.Box.PositionRelative(Joint.Start, Joint.Start).Shift(-signalSize * 1.5f).ShiftX(-signalSize.X * 2f), signalSize);
            var signalColor2 = new SignalLight(signalColor1.Position.ShiftX(-signalSize.X * 2f), signalSize);
            var signalPressure1 = new SignalLight(signalColor1.Position.ShiftY(-signalSize.Y * 3f), signalSize);
            var signalPressure2 = new SignalLight(signalColor2.Position.ShiftY(-signalSize.Y * 3f), signalSize);
            var bucketDropped = new SignalLight(screen.Percent(115, 100), signalSize );
            var trapdoorOpen1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X + trapDoorSizeX, _trapDoors[0].Position.Y - 50), signalSize);
            var trapdoorOpen2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X + trapDoorSizeX, _trapDoors[1].Position.Y - 50), signalSize);
            var trapdoorOpen3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X + trapDoorSizeX, _trapDoors[2].Position.Y - 50), signalSize);
            var trapdoorClosed1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X - signalSize.X, _trapDoors[0].Position.Y + 50), signalSize);
            var trapdoorClosed2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X - signalSize.X, _trapDoors[1].Position.Y + 50), signalSize);
            var trapdoorClosed3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X - signalSize.X, _trapDoors[2].Position.Y + 50), signalSize);
            _gateOpen = new SignalLight(new Vector2f(_gateEntrance.Position.X + gateEntranceSize.X + 10, _trapDoors[0].Position.Y - 50), signalSize);
            _gateClosed = new SignalLight(new Vector2f(_gateEntrance.Position.X + gateEntranceSize.X + 10, _trapDoors[0].Position.Y + 25), signalSize);

            signalColor1.Name = IOKeys.I.ColorSensor;
            signalColor2.Name = IOKeys.I.ColorSensor;
            signalPressure1.Name = IOKeys.I.WeightSensor;
            signalPressure2.Name = IOKeys.I.WeightSensor;
            trapdoorOpen1.Name = IOKeys.I.TrapDoor1Open;
            trapdoorOpen2.Name = IOKeys.I.TrapDoor2Open;
            trapdoorOpen3.Name = IOKeys.I.TrapDoor3Open;
            trapdoorClosed1.Name = IOKeys.I.TrapDoor1Closed;
            trapdoorClosed2.Name = IOKeys.I.TrapDoor2Closed;
            trapdoorClosed3.Name = IOKeys.I.TrapDoor3Closed;
            bucketDropped.Name = IOKeys.I.BucketMotionSensor;
            _pressureSensor.Name = IOKeys.I.WeightSensor;
            _colorSensor.Name = IOKeys.I.ColorSensor;
            _motionSensorBucket.Name = IOKeys.I.BucketMotionSensor;
            _motionSensorConveyor.Name = IOKeys.I.ConveyorMotionSensor;
            _gateOpen.Name = IOKeys.I.GateOpen;
            _gateClosed.Name = IOKeys.I.GateClosed;
            _gateEntrance.Name = IOKeys.Q.Gate;
            _conveyor.Name = IOKeys.Q.Conveyor;

            _trapDoorsOpen = new[] {trapdoorOpen1, trapdoorOpen2, trapdoorOpen3};
            _trapDoorsClosed = new[] {trapdoorClosed1, trapdoorClosed2, trapdoorClosed3};

            _sensors = new List<GameEntity>
            {
                _motionSensorBucket,
                _motionSensorConveyor,
                _pressureSensor,
                _colorSensor
            };

            _signalLights = new List<GameEntity>
            {
                trapdoorOpen1,
                trapdoorOpen2,
                trapdoorOpen3,
                trapdoorClosed1,
                trapdoorClosed2,
                trapdoorClosed3,
                bucketDropped,
                _gateOpen,
                _gateClosed,
                signalColor1,
                signalColor2,
                signalPressure1,
                signalPressure2,
            };

            _signalLightsColors = new []
            {
                signalColor1,
                signalColor2,
            };

            _signalLightsWeights = new []
            {
                signalPressure1,
                signalPressure2,
            };


            _entities = new GameEntity[]
            {
                _buttonPause,
                _buttonReset,
                _buttonMainMenu,
                _conveyor,
                _pressureSensor,
                _colorSensor,
                _motionSensorBucket,
                _motionSensorConveyor,
                _gateOpen,
                _gateClosed,
                bucketDropped,
                trapdoorOpen1,
                trapdoorOpen2,
                trapdoorOpen3,
                trapdoorClosed1,
                trapdoorClosed2,
                trapdoorClosed3,
            };

            _entities = _entities.ToList()
                .Concat(_signalLights)
                .Concat(_marbles)
                .Concat(_trapDoors)
                .Concat(_marbles)
                .Concat(_buckets)
                .Concat(new[] { _gateEntrance })
                .ToArray();

            _hoverable = new List<GameEntity>() { };
            _hoverable.AddRange(_trapDoors);
            _hoverable.AddRange(_signalLights);
            _hoverable.AddRange(_sensors);
            _hoverable.AddRange(_marbles);

            _mappable = new List<GameEntity>() { };
            _mappable.AddRange(_trapDoors);
            _mappable.Add(_gateEntrance);
            _mappable.Add(_conveyor);

            _mappable.AddRange(_trapDoorsOpen);
            _mappable.AddRange(_trapDoorsClosed);
            _mappable.Add(_motionSensorBucket);
            _mappable.Add(_gateOpen);
            _mappable.Add(_gateClosed);
            _mappable.Add(_motionSensorConveyor);
            _mappable.Add(_pressureSensor);
            _mappable.Add(_colorSensor);

            //load assets for all entities
            foreach (GameEntity entity in _entities)
                entity.Load(_bundle);

            _drawables = new Drawable[]
            {
                //instructions,
                //gameScreenTitle,
                menuBarBackground,
                _legendGameBackground,
                _legendGame,
                _legendMappingBackground,
                _legendMapping,
                _infoBoxBackground,
                _infoLabel,
                _winPopupBox,
                _winPopup,
                _losePopupBox,
                _losePopup
            };
        }

        // Mouse movement event handler for features involving hover-over
        private void GameMouseMoveEventHandler(object? sender, MouseMoveEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromMouseEvent(_window, _buttons, mouse);
            UpdateHoveredEntityMouseEvent(_window, _entities, mouse);
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

        // Mouse move event for any entity that is hovered over
        public void UpdateHoveredEntityMouseEvent(RenderWindow window, GameEntity[] entities, MouseMoveEventArgs mouse)
        {
            _hoveredEntity = null;
            window.SetMouseCursor(MarbleSorterGame.Cursors.Arrow);

            foreach (GameEntity entity in _hoverable)
            {
                if (entity.MouseHovered(new Vector2f(mouse.X, mouse.Y)))
                {
                    _hoveredEntity = entity;
                    window.SetMouseCursor(MarbleSorterGame.Cursors.Help);
                    break;
                }
            }
        }

        //sets signal state from driver
        public void UpdateSignals()
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

            //set weight/color sensor lights
            var c = _driver.ColorSensor;
            _signalLightsColors[0].SetState(c == (int)Enums.Color.Blue || c == (int)Enums.Color.Red);
            _signalLightsColors[1].SetState(c == (int)Enums.Color.Blue || c == (int)Enums.Color.Green);

            var w = _driver.PressureSensor;
            _signalLightsWeights[0].SetState(w == (int)Enums.Weight.Large || w == (int)Enums.Weight.Small);
            _signalLightsWeights[1].SetState(w == (int)Enums.Weight.Large || w == (int)Enums.Weight.Medium);
        }

        // Writes driver values to the game and reads in game state 
        private void UpdateDriver()
        {
            _driver.TrapDoor1Open = _trapDoors[0].IsFullyOpen;
            _driver.TrapDoor2Open = _trapDoors[1].IsFullyOpen;
            _driver.TrapDoor3Open = _trapDoors[2].IsFullyOpen;

            _driver.TrapDoor1Closed = _trapDoors[0].IsFullyClosed;
            _driver.TrapDoor2Closed = _trapDoors[1].IsFullyClosed;
            _driver.TrapDoor3Closed = _trapDoors[2].IsFullyClosed;

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
            {
                float spacing = Marble.MarbleSizeSmall * 2;
                float prevMarbleEdge = _marbles[i].GlobalBounds.Left + _marbles[i].GlobalBounds.Width;
                if (prevMarbleEdge + spacing > _marbles[i + 1].GlobalBounds.Left)
                    _marbles[i].SetState(MarbleState.Still);

            }
                
            
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
            //IoMapConfiguration config = _bundle.IoMapConfiguration();
            _legendGameData["Marbles Remaining Total"] = _marblesRemaining.ToString();
            _legendGameData["Marbles Correctly Dropped"] = _buckets.Select(b => b.TotalCorrect).Sum().ToString();
            _legendGameData["Marbles Incorrectly Dropped"] = _buckets.Select(b => b.TotalIncorrect).Sum().ToString();

            _IOMapping[IOKeys.I.TrapDoor1Open.ToString()] = _driver.TrapDoor1Open.ToString();
            _IOMapping[IOKeys.I.TrapDoor2Open.ToString()] = _driver.TrapDoor2Open.ToString();
            _IOMapping[IOKeys.I.TrapDoor3Open.ToString()] = _driver.TrapDoor3Open.ToString();
            _IOMapping[IOKeys.I.TrapDoor1Closed.ToString()] = _driver.TrapDoor1Closed.ToString();
            _IOMapping[IOKeys.I.TrapDoor2Closed.ToString()] = _driver.TrapDoor2Closed.ToString();
            _IOMapping[IOKeys.I.TrapDoor3Closed.ToString()] = _driver.TrapDoor3Closed.ToString();
            _IOMapping[IOKeys.I.BucketMotionSensor.ToString()] = _driver.BucketMotionSensor.ToString();
            _IOMapping[IOKeys.I.WeightSensor.ToString()] = _driver.PressureSensor.ToString();
            _IOMapping[IOKeys.I.ColorSensor.ToString()] = _driver.ColorSensor.ToString();
            _IOMapping[IOKeys.I.ConveyorMotionSensor.ToString()] = _driver.ConveyorMotionSensor.ToString();
            _IOMapping[IOKeys.I.GateClosed.ToString()] = _driver.GateClosed.ToString();
            _IOMapping[IOKeys.I.GateOpen.ToString()] = _driver.GateOpen.ToString();
            _IOMapping[IOKeys.Q.TrapDoor1.ToString()] = _driver.TrapDoor1.ToString();
            _IOMapping[IOKeys.Q.TrapDoor2.ToString()] = _driver.TrapDoor2.ToString();
            _IOMapping[IOKeys.Q.TrapDoor3.ToString()] = _driver.TrapDoor3.ToString();
            _IOMapping[IOKeys.Q.Gate.ToString()] = _driver.Gate.ToString();
            _IOMapping[IOKeys.Q.Conveyor.ToString()] = _driver.Conveyor.ToString();

            var legendGameBuilder = new System.Text.StringBuilder();
            var legendMappingBuilder = new System.Text.StringBuilder();

            foreach (var stat in _legendGameData)
                legendGameBuilder.AppendLine(string.Format("{0,-40}: {1}", stat.Key, stat.Value));

            foreach (var entity in _mappable)
            {
                IoMapConfiguration entityConfig = _bundle.IoMapConfiguration.Find(e => e.EntityName == entity.Name);

                legendMappingBuilder.AppendLine(string.Format("{0}-{1,-30}: {2}",
                    "%" + entityConfig.MemoryArea + entityConfig.Byte.ToString() + "." + entityConfig.Bit.ToString(),
                    entityConfig.Description,
                    _IOMapping[entity.Name] 
                    ));

            }
            
            _legendGame.Text = legendGameBuilder.ToString();
            _legendMapping.Text = legendMappingBuilder.ToString();

            // Automatically adjust background height and width according to height of the text label
            var legendBounds = _legendGame.LabelText.GetGlobalBounds();
            _legendGameBackground.Size = new Vector2f(legendBounds.Width + _legendPadding*4, legendBounds.Height + _legendPadding*2);
            legendBounds = _legendMapping.LabelText.GetGlobalBounds();
            _legendMappingBackground.Size = new Vector2f(legendBounds.Width + _legendPadding*4, legendBounds.Height + _legendPadding*2);
            _legendMappingBackground.Position = _legendGameBackground.PositionRelative(Joint.End, Joint.Start).ShiftX(_legendPadding);

            _legendMapping.LabelPosition = _legendMappingBackground.Position.Shift(new Vector2f(_legendPadding, _legendPadding));

        }

        // Update info box data for hover-over entities
        private void UpdateInfoBox()
        {
            if (_hoveredEntity == null)
            {
                _infoLabel.Text = string.Empty;
                _infoBoxBackground.Size = default;
                return;
            }

            string hoveredEntityAddress = null;
            string hoveredEntityDescription = null;

            if (_hoveredEntity.GetType() == _marbles[0].GetType())
            {
                hoveredEntityDescription = _hoveredEntity.ToString();
            } else
            {
                IoMapConfiguration hoveredEntityConfig = _bundle.IoMapConfiguration.Find(entity => entity.EntityName == _hoveredEntity.Name);
                if (hoveredEntityConfig != null)
                {
                    hoveredEntityAddress = "%" + hoveredEntityConfig.MemoryArea + hoveredEntityConfig.Byte.ToString() + "." + hoveredEntityConfig.Bit.ToString();
                    hoveredEntityDescription = hoveredEntityConfig.Description;
                }
            }

            _infoData["Currently Hovered Item"] = _hoveredEntity == null ? "N/A" : hoveredEntityDescription;
            _infoData["Item Address"] = _hoveredEntity == null ? "N/A" : hoveredEntityAddress;

            var legendBuilder = new System.Text.StringBuilder();
            foreach (var stat in _infoData)
                legendBuilder.AppendLine(string.Format("{0,-23}: {1}", stat.Key, stat.Value));
            
            _infoLabel.Text = legendBuilder.ToString();
            // Automatically adjust background height and width according to height of the text label
            var bounds = _infoLabel.LabelText.GetGlobalBounds();

            //show info box background
            _infoBoxBackground.Size = _infoBoxBackgroundSize;
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
            UpdateInfoBox();

            if (_gameState == GameState.Progress)
            {
                UpdateDriver();
                UpdateSignals();
                UpdateMarbles();
                UpdateDoors();
                UpdateGameState(); // Win, lose, or pause
            }

            UpdateWinState();
        }

        //show win/lose popup box if win/lose
        public void UpdateWinState()
        {
            Vector2f popupSize = new Vector2f(100f, 30f);

            if (_gameState == GameState.Win)
            {
                _winPopupBox.Size = popupSize;
                _winPopup.Text = "YOU WIN";
            } 
            else if (_gameState == GameState.Lose)
            {
                _losePopupBox.Size = popupSize;
                _losePopup.Text = "YOU LOSE";
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
