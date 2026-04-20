// SaveSlotSelectionScreen.cs - Complete version with ExecuteSlotAction
using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class SaveSlotSelectionScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        private Rectangle _backButtonRect;
        private Rectangle[] _slotRects = new Rectangle[8];

        private bool _isSaveMode = false;

        // Confirmation system
        private bool _showingConfirmation = false;
        private int _pendingSlot = -1;

        // Status message
        private string _statusMessage = "";
        private Color _statusColor = Color.White;
        private double _messageTimer = 0;

        public SaveSlotSelectionScreen(Game1 game)
            : base(game) { }

        public void SetMode(bool isSaveMode)
        {
            _isSaveMode = isSaveMode;
            _showingConfirmation = false;
            _pendingSlot = -1;
            _statusMessage = "";
        }

        public override void LoadContent()
        {
            _whitePixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new Color[] { Color.White });

            try
            {
                _font = Game.Content.Load<SpriteFont>("DefaultFont");
            }
            catch
            {
                _font = null;
            }

            _backButtonRect = new Rectangle(80, 80, 260, 70);

            for (int i = 0; i < 8; i++)
            {
                _slotRects[i] = new Rectangle(400, 200 + i * 80, 1100, 65);
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            // Number keys 1-8
            var input = Game.InputManager;

            if (input.IsKeyJustPressed(Keys.D1))
                ExecuteSlotAction(1);
            if (input.IsKeyJustPressed(Keys.D2))
                ExecuteSlotAction(2);
            if (input.IsKeyJustPressed(Keys.D3))
                ExecuteSlotAction(3);
            if (input.IsKeyJustPressed(Keys.D4))
                ExecuteSlotAction(4);
            if (input.IsKeyJustPressed(Keys.D5))
                ExecuteSlotAction(5);
            if (input.IsKeyJustPressed(Keys.D6))
                ExecuteSlotAction(6);
            if (input.IsKeyJustPressed(Keys.D7))
                ExecuteSlotAction(7);
            if (input.IsKeyJustPressed(Keys.D8))
                ExecuteSlotAction(8);

            // Message timer
            if (_messageTimer > 0)
            {
                _messageTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_messageTimer <= 0)
                    _statusMessage = "";
            }

            string modeText = _isSaveMode ? "SAVE" : "LOAD";
            Game.Window.Title = $"DevGame - {modeText} Slot - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        public override void OnMouseClick(Point mousePosition)
        {
            if (_backButtonRect.Contains(mousePosition))
            {
                if (_showingConfirmation)
                {
                    _showingConfirmation = false;
                    _pendingSlot = -1;
                }
                else
                {
                    Game.ChangeState(GameState.Title);
                }
                return;
            }

            // Confirmation dialog
            if (_showingConfirmation)
            {
                if (new Rectangle(1100, 500, 200, 70).Contains(mousePosition))
                {
                    ConfirmSave();
                    return;
                }
                if (new Rectangle(700, 500, 200, 70).Contains(mousePosition))
                {
                    _showingConfirmation = false;
                    _pendingSlot = -1;
                    _statusMessage = "Save cancelled.";
                    _statusColor = Color.Orange;
                    _messageTimer = 1.5;
                    return;
                }
                return;
            }

            // Normal slot selection
            for (int i = 0; i < 8; i++)
            {
                if (_slotRects[i].Contains(mousePosition))
                {
                    if (_isSaveMode)
                    {
                        string filePath = Path.Combine(
                            Game.SaveManager.GetSaveDirectory(),
                            $"PlayerSave{i + 1}.json"
                        );
                        if (File.Exists(filePath))
                        {
                            _showingConfirmation = true;
                            _pendingSlot = i + 1;
                            _statusMessage = $"Slot {i + 1} already exists. Overwrite?";
                            _statusColor = Color.Yellow;
                        }
                        else
                        {
                            ExecuteSave(i + 1);
                        }
                    }
                    else
                    {
                        ExecuteLoad(i + 1);
                    }
                    return;
                }
            }
        }

        private void ConfirmSave()
        {
            if (_pendingSlot > 0)
            {
                ExecuteSave(_pendingSlot);
                _showingConfirmation = false;
                _pendingSlot = -1;
            }
        }

        // This is the method that was missing in previous versions
        private void ExecuteSlotAction(int slotNumber)
        {
            if (_isSaveMode)
            {
                string filePath = Path.Combine(
                    Game.SaveManager.GetSaveDirectory(),
                    $"PlayerSave{slotNumber}.json"
                );
                if (File.Exists(filePath))
                {
                    _showingConfirmation = true;
                    _pendingSlot = slotNumber;
                    _statusMessage = $"Slot {slotNumber} already exists. Overwrite?";
                    _statusColor = Color.Yellow;
                }
                else
                {
                    ExecuteSave(slotNumber);
                }
            }
            else
            {
                ExecuteLoad(slotNumber);
            }
        }

        private void ExecuteSave(int slotNumber)
        {
            bool success = Game.SaveCurrentGame(slotNumber);
            if (success)
            {
                _statusMessage = $"Successfully saved to Slot {slotNumber}";
                _statusColor = Color.LightGreen;
                _messageTimer = 2.5;
            }
            else
            {
                _statusMessage = "Save failed.";
                _statusColor = Color.Red;
                _messageTimer = 2.0;
            }
        }

        private void ExecuteLoad(int slotNumber)
        {
            bool success = Game.LoadGame(slotNumber);
            if (success)
            {
                Game.ChangeState(GameState.MainHub);
            }
            else
            {
                _statusMessage = $"Slot {slotNumber} is empty.";
                _statusColor = Color.Orange;
                _messageTimer = 2.0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(20, 18, 40));

            if (_font != null)
            {
                string title = _isSaveMode ? "SAVE GAME - Choose Slot" : "LOAD GAME - Choose Slot";
                spriteBatch.DrawString(_font, title, new Vector2(680, 80), Color.White);

                for (int i = 0; i < 8; i++)
                {
                    var rect = _slotRects[i];
                    string saveName = $"PlayerSave{i + 1}";
                    string filePath = Path.Combine(
                        Game.SaveManager.GetSaveDirectory(),
                        saveName + ".json"
                    );

                    bool hasSave = File.Exists(filePath);
                    string displayText = $"Slot {i + 1} - Empty";
                    Color textColor = Color.LightGray;

                    if (hasSave)
                    {
                        string playerName = "Unknown Player";
                        string lastSaved = "";

                        try
                        {
                            string json = File.ReadAllText(filePath);
                            using var doc = JsonDocument.Parse(json);
                            var root = doc.RootElement;

                            if (root.TryGetProperty("Player", out var playerElement))
                            {
                                if (playerElement.TryGetProperty("Name", out var nameElement))
                                {
                                    playerName = nameElement.GetString() ?? "Unknown Player";
                                }
                            }

                            if (root.TryGetProperty("LastSaved", out var dateElement))
                            {
                                if (DateTime.TryParse(dateElement.GetString(), out var savedDate))
                                    lastSaved = savedDate.ToString("yyyy-MM-dd HH:mm");
                            }
                        }
                        catch
                        {
                            playerName = "Corrupted Save";
                        }

                        displayText = $"Slot {i + 1} - {playerName}";
                        if (!string.IsNullOrEmpty(lastSaved))
                            displayText += $" ({lastSaved})";

                        textColor = Color.LightGreen;
                    }

                    Color slotColor = hasSave ? new Color(60, 80, 100) : new Color(40, 40, 55);

                    spriteBatch.Draw(_whitePixel, rect, slotColor);
                    spriteBatch.DrawString(
                        _font,
                        displayText,
                        new Vector2(rect.X + 40, rect.Y + 18),
                        textColor
                    );
                }

                // Confirmation dialog
                if (_showingConfirmation)
                {
                    spriteBatch.Draw(
                        _whitePixel,
                        new Rectangle(580, 380, 760, 220),
                        new Color(30, 30, 55)
                    );
                    spriteBatch.DrawString(
                        _font,
                        $"Overwrite Slot {_pendingSlot}?",
                        new Vector2(700, 440),
                        Color.Yellow
                    );

                    spriteBatch.Draw(
                        _whitePixel,
                        new Rectangle(1100, 520, 200, 70),
                        new Color(0, 140, 0)
                    );
                    spriteBatch.DrawString(
                        _font,
                        "YES - OVERWRITE",
                        new Vector2(1130, 545),
                        Color.White
                    );

                    spriteBatch.Draw(
                        _whitePixel,
                        new Rectangle(700, 520, 200, 70),
                        new Color(140, 0, 0)
                    );
                    spriteBatch.DrawString(
                        _font,
                        "NO - CANCEL",
                        new Vector2(750, 545),
                        Color.White
                    );
                }

                // Status message
                if (!string.IsNullOrEmpty(_statusMessage))
                {
                    spriteBatch.Draw(
                        _whitePixel,
                        new Rectangle(580, 700, 760, 50),
                        new Color(0, 0, 0, 160)
                    );
                    spriteBatch.DrawString(
                        _font,
                        _statusMessage,
                        new Vector2(620, 715),
                        _statusColor
                    );
                }

                spriteBatch.Draw(_whitePixel, _backButtonRect, new Color(100, 30, 30));
                spriteBatch.DrawString(_font, "BACK TO TITLE", new Vector2(120, 100), Color.White);

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}
