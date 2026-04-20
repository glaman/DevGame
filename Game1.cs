// Game1.cs - Fixed mouse click forwarding + battle support
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private UIManager _uiManager;
        private InputManager _inputManager;
        private SaveManager _saveManager;

        // All screens
        private TitleScreen _titleScreen;
        private MainHubScreen _mainHubScreen;
        private MapScreen _mapScreen;
        private BattleScreen _battleScreen;
        private EquipmentScreen _equipmentScreen;
        private HeroGalleryScreen _heroGalleryScreen;
        private HeroDetailScreen _heroDetailScreen;
        private PlayerProfileScreen _playerProfileScreen;
        private SaveSlotSelectionScreen _saveSlotSelectionScreen;

        // Game state data
        public PlayerHero? Player { get; set; }
        public List<Hero> ActiveParty { get; private set; } = new List<Hero>();
        private Stack<Screen> _screenHistory = new Stack<Screen>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            _inputManager = new InputManager();
            _uiManager = new UIManager(this);
            _saveManager = new SaveManager();

            // Create all screens
            _titleScreen = new TitleScreen(this);
            _mainHubScreen = new MainHubScreen(this);
            _mapScreen = new MapScreen(this);
            _battleScreen = new BattleScreen(this);
            _equipmentScreen = new EquipmentScreen(this);
            _heroGalleryScreen = new HeroGalleryScreen(this);
            _heroDetailScreen = new HeroDetailScreen(this);
            _playerProfileScreen = new PlayerProfileScreen(this);
            _saveSlotSelectionScreen = new SaveSlotSelectionScreen(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            InitializeTestData();
            PushScreen(_titleScreen);
        }

        private void InitializeTestData()
        {
            Player = new PlayerHero("Clint", "Sharpshooter");
            Player.Might = 12;
            Player.Finesse = 18;
            Player.Wit = 14;
            Player.Vigor = 15;
            Player.Speed = 16;

            var handgun = new EquipmentItem("9mm Handgun", "Weapons/Icons/handgun_9mm_icon");
            Player.EquipItem(0, handgun);

            var brawler = new HelperHero("Grom", "Brawler");
            brawler.Might = 20;
            brawler.Finesse = 10;
            brawler.Wit = 8;
            brawler.Vigor = 18;
            brawler.Speed = 12;

            var healer = new HelperHero("Lira", "Healer");
            healer.Might = 9;
            healer.Finesse = 12;
            healer.Wit = 19;
            healer.Vigor = 16;
            healer.Speed = 13;

            ActiveParty.Clear();
            ActiveParty.Add(Player);
            ActiveParty.Add(brawler);
            ActiveParty.Add(healer);
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update();

            if (_inputManager.IsKeyJustPressed(Keys.F4))
            {
                Exit();
                return;
            }

            // CRITICAL: Forward mouse clicks to the current screen
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (_uiManager.CurrentScreen != null)
                {
                    Point mousePos = new Point(mouse.X, mouse.Y);
                    _uiManager.CurrentScreen.OnMouseClick(mousePos);
                }
            }

            _uiManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _uiManager.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // ====================== SCREEN MANAGEMENT ======================
        public void ChangeState(GameState state)
        {
            Screen? newScreen = state switch
            {
                GameState.Title => _titleScreen,
                GameState.MainHub => _mainHubScreen,
                GameState.Map => _mapScreen,
                GameState.Battle => _battleScreen,
                GameState.Equipment => _equipmentScreen,
                GameState.HeroGallery => _heroGalleryScreen,
                GameState.PlayerProfile => _playerProfileScreen,
                GameState.SaveSlotSelection => _saveSlotSelectionScreen,
                _ => _mainHubScreen,
            };

            if (newScreen != null)
            {
                _screenHistory.Clear();
                PushScreen(newScreen);
            }
        }

        public void PushScreen(Screen screen)
        {
            _screenHistory.Push(screen);
            _uiManager.ChangeScreen(screen);
        }

        public void HandleEscape()
        {
            if (_screenHistory.Count > 1)
            {
                _screenHistory.Pop();
                var previous = _screenHistory.Peek();
                _uiManager.ChangeScreen(previous);
            }
            else
            {
                ChangeState(GameState.MainHub);
            }
        }

        public void ShowHeroDetail(Hero hero)
        {
            _heroDetailScreen.SetHero(hero);
            PushScreen(_heroDetailScreen);
        }

        // ====================== SAVE / LOAD ======================
        public bool SaveCurrentGame(int slotNumber = 1)
        {
            return _saveManager.SaveGame(this, slotNumber);
        }

        public bool LoadGame(int slotNumber = 1)
        {
            bool success = _saveManager.LoadGame(this, slotNumber);
            if (success && _uiManager.CurrentScreen != null)
            {
                _uiManager.CurrentScreen.LoadContent();
            }
            return success;
        }

        public SaveManager SaveManager => _saveManager;

        // ====================== PUBLIC ACCESSORS ======================
        public InputManager InputManager => _inputManager;
        public EquipmentScreen EquipmentScreen => _equipmentScreen;
        public MapScreen MapScreen => _mapScreen;
        public PlayerProfileScreen PlayerProfileScreen => _playerProfileScreen;
        public SaveSlotSelectionScreen SaveSlotSelectionScreen => _saveSlotSelectionScreen;
        public BattleScreen BattleScreen => _battleScreen; // Added for easy access if needed
    }
}
