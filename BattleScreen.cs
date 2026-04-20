// BattleScreen.cs - Clean & Working Version
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class BattleScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        // Layout rectangles
        private Rectangle _turnOrderRect;
        private Rectangle _playerTeamRect;
        private Rectangle _enemyTeamRect;
        private Rectangle _actionsRect;
        private Rectangle _combatLogRect;
        private Rectangle _startBattleButtonRect;

        // Battle data
        private List<Entity> _playerTeam = new List<Entity>();
        private List<Enemy> _enemyTeam = new List<Enemy>();
        private List<Entity> _turnOrder = new List<Entity>();
        private int _currentTurnIndex = 0;

        private List<string> _combatLog = new List<string>();
        private bool _battleStarted = false;

        public BattleScreen(Game1 game)
            : base(game) { }

        public override void LoadContent()
        {
            _whitePixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _whitePixel.SetData(new Color[] { Color.White });

            try
            {
                _font = Game.Content.Load<SpriteFont>("DefaultFont");
                Console.WriteLine("[BattleScreen] Font loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] BattleScreen font failed: {ex.Message}");
            }

            _turnOrderRect = new Rectangle(0, 0, 1920, 140);
            _playerTeamRect = new Rectangle(40, 160, 620, 580);
            _enemyTeamRect = new Rectangle(1260, 160, 620, 580);
            _actionsRect = new Rectangle(40, 760, 720, 280);
            _combatLogRect = new Rectangle(780, 760, 1100, 280);
            _startBattleButtonRect = new Rectangle(800, 400, 320, 80);
        }

        public void StartTestBattle()
        {
            Console.WriteLine("[Battle] StartTestBattle called - initializing...");

            _playerTeam.Clear();
            _playerTeam.Add(new PlayerHero("You", "Sharpshooter") { CurrentHP = 100, MaxHP = 100 });
            _playerTeam.Add(new HelperHero("Helper A", "Brawler") { CurrentHP = 100, MaxHP = 100 });
            _playerTeam.Add(new HelperHero("Helper B", "Healer") { CurrentHP = 100, MaxHP = 100 });

            _enemyTeam.Clear();
            for (int i = 1; i <= 6; i++)
            {
                _enemyTeam.Add(new Enemy($"Enemy {i}") { CurrentHP = 25, MaxHP = 25 });
            }

            _turnOrder.Clear();
            _turnOrder.Add(_enemyTeam[0]);
            _turnOrder.Add(_enemyTeam[1]);
            _turnOrder.Add(_playerTeam[0]);
            _turnOrder.Add(_enemyTeam[2]);
            _turnOrder.Add(_enemyTeam[3]);
            _turnOrder.Add(_playerTeam[1]);
            _turnOrder.Add(_enemyTeam[4]);
            _turnOrder.Add(_enemyTeam[5]);
            _turnOrder.Add(_playerTeam[2]);

            _currentTurnIndex = 0;
            _battleStarted = true;
            _combatLog.Clear();

            _combatLog.Add("=== BATTLE STARTED ===");
            _combatLog.Add("Click bottom area or press SPACE to advance turns.");
            Console.WriteLine("[Battle] Battle started successfully!");
        }

        private void ExecuteNextAttack()
        {
            if (!_battleStarted || _turnOrder.Count == 0)
                return;

            var attacker = _turnOrder[_currentTurnIndex];
            Entity? target = _playerTeam.Contains(attacker)
                ? _enemyTeam.Find(e => e.CurrentHP > 0)
                : _playerTeam.Find(e => e.CurrentHP > 0);

            if (target != null)
            {
                target.CurrentHP = Math.Max(0, target.CurrentHP - 1);
                _combatLog.Add($"{attacker.Name} attacks {target.Name} for 1 damage.");
                if (target.CurrentHP <= 0)
                    _combatLog.Add($"{target.Name} was defeated!");
            }
            else
            {
                _combatLog.Add($"{attacker.Name} has no targets.");
            }

            if (_combatLog.Count > 15)
                _combatLog.RemoveAt(0);

            _currentTurnIndex = (_currentTurnIndex + 1) % _turnOrder.Count;
        }

        public override void Update(GameTime gameTime)
        {
            HandleDevNavigation(gameTime);

            if (Game.InputManager.IsKeyJustPressed(Keys.Space))
            {
                ExecuteNextAttack();
            }
        }

        // IMPORTANT: Must be public override because Screen.OnMouseClick is now public
        public override void OnMouseClick(Point mousePosition)
        {
            Console.WriteLine(
                $"[BattleScreen] Mouse clicked at ({mousePosition.X}, {mousePosition.Y})"
            );

            if (!_battleStarted && _startBattleButtonRect.Contains(mousePosition))
            {
                Console.WriteLine("[BattleScreen] START BATTLE BUTTON CLICKED!");
                StartTestBattle();
                return;
            }

            if (_battleStarted && _actionsRect.Contains(mousePosition))
            {
                Console.WriteLine("[BattleScreen] Actions area clicked - next turn");
                ExecuteNextAttack();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whitePixel, new Rectangle(0, 0, 1920, 1080), new Color(18, 16, 32));

            if (_font == null)
                return;

            // Turn Order Bar
            spriteBatch.Draw(_whitePixel, _turnOrderRect, new Color(30, 25, 50));
            spriteBatch.DrawString(_font, "TURN ORDER", new Vector2(80, 35), Color.Cyan);

            if (_battleStarted)
            {
                for (int i = 0; i < Math.Min(6, _turnOrder.Count); i++)
                {
                    int idx = (_currentTurnIndex + i) % _turnOrder.Count;
                    var entity = _turnOrder[idx];
                    int x = 320 + i * 190;
                    Color color = (i == 0) ? new Color(100, 220, 100) : new Color(55, 50, 85);
                    spriteBatch.Draw(_whitePixel, new Rectangle(x, 55, 170, 60), color);
                    spriteBatch.DrawString(
                        _font,
                        entity.Name,
                        new Vector2(x + 15, 68),
                        Color.White
                    );
                }
            }
            else
            {
                spriteBatch.DrawString(
                    _font,
                    "(Battle not started)",
                    new Vector2(400, 70),
                    Color.Gray
                );
            }

            // Player Team
            spriteBatch.Draw(_whitePixel, _playerTeamRect, new Color(35, 30, 60));
            spriteBatch.DrawString(_font, "YOUR TEAM", new Vector2(80, 170), Color.Cyan);
            for (int i = 0; i < _playerTeam.Count; i++)
            {
                var unit = _playerTeam[i];
                int y = 240 + i * 140;
                Color boxColor = unit.CurrentHP > 0 ? new Color(50, 45, 75) : new Color(40, 20, 20);
                spriteBatch.Draw(_whitePixel, new Rectangle(80, y, 520, 110), boxColor);
                spriteBatch.DrawString(_font, unit.Name, new Vector2(100, y + 25), Color.White);
                spriteBatch.DrawString(
                    _font,
                    $"HP: {unit.CurrentHP}/{unit.MaxHP}",
                    new Vector2(100, y + 65),
                    Color.LightGreen
                );
            }

            // Enemy Team
            spriteBatch.Draw(_whitePixel, _enemyTeamRect, new Color(60, 25, 25));
            spriteBatch.DrawString(_font, "ENEMIES", new Vector2(1300, 170), Color.Red);
            for (int i = 0; i < _enemyTeam.Count; i++)
            {
                var enemy = _enemyTeam[i];
                int y = 240 + i * 85;
                Color boxColor =
                    enemy.CurrentHP > 0 ? new Color(80, 30, 30) : new Color(50, 15, 15);
                spriteBatch.Draw(_whitePixel, new Rectangle(1300, y, 520, 70), boxColor);
                spriteBatch.DrawString(_font, enemy.Name, new Vector2(1320, y + 20), Color.White);
                spriteBatch.DrawString(
                    _font,
                    $"HP: {enemy.CurrentHP}/25",
                    new Vector2(1320, y + 45),
                    Color.LightGreen
                );
            }

            // Actions Area
            spriteBatch.Draw(_whitePixel, _actionsRect, new Color(40, 35, 65));
            spriteBatch.DrawString(_font, "BATTLE CONTROLS", new Vector2(80, 800), Color.Cyan);

            if (!_battleStarted)
            {
                spriteBatch.Draw(_whitePixel, _startBattleButtonRect, new Color(70, 140, 70));
                spriteBatch.DrawString(_font, "START BATTLE", new Vector2(860, 425), Color.White);
            }
            else
            {
                spriteBatch.DrawString(
                    _font,
                    "Click here or press SPACE for Next Attack",
                    new Vector2(100, 860),
                    Color.LightGreen
                );
            }

            // Combat Log
            spriteBatch.Draw(_whitePixel, _combatLogRect, new Color(25, 22, 40));
            spriteBatch.DrawString(_font, "COMBAT LOG", new Vector2(820, 800), Color.LightGray);
            for (int i = 0; i < Math.Min(12, _combatLog.Count); i++)
            {
                spriteBatch.DrawString(
                    _font,
                    _combatLog[i],
                    new Vector2(840, 840 + i * 28),
                    Color.LightGreen
                );
            }

            DrawDevMenu(spriteBatch, _font);
        }
    }
}
