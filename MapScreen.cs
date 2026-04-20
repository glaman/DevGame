// MapScreen.cs - Loads both map backgrounds and switches correctly
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedRPG
{
    public class MapScreen : Screen
    {
        private Texture2D _whitePixel;
        private SpriteFont? _font;

        // Map backgrounds
        private Texture2D? _map1Background; // Forest
        private Texture2D? _map2Background; // Island Nation

        private List<string> _mapTabs = new List<string> { "Map 1", "Map 2" };
        private int _currentMapIndex = 0;

        private Rectangle[] _tabRects = new Rectangle[2];
        private Rectangle _backButtonRect;

        private class MapNode
        {
            public string Name { get; set; } = "";
            public Vector2 Position { get; set; }
            public Rectangle ClickRect { get; set; }
        }

        private List<MapNode> _nodes = new List<MapNode>();

        private bool _isMiniMode = false;
        private Rectangle _miniBounds = Rectangle.Empty;

        public MapScreen(Game1 game)
            : base(game) { }

        public void SetMiniMode(Rectangle bounds)
        {
            _isMiniMode = true;
            _miniBounds = bounds;
        }

        public void SetFullMode()
        {
            _isMiniMode = false;
        }

        public int GetMapCount() => _mapTabs.Count;

        public string GetMapName(int index) => index < _mapTabs.Count ? _mapTabs[index] : "Unknown";

        public int GetCurrentMapIndex() => _currentMapIndex;

        public void SetCurrentMap(int index)
        {
            if (index >= 0 && index < _mapTabs.Count)
            {
                _currentMapIndex = index;
                GenerateNodes();
            }
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

            // Load both map backgrounds
            LoadMapBackgrounds();

            _tabRects[0] = new Rectangle(300, 80, 260, 60);
            _tabRects[1] = new Rectangle(600, 80, 260, 60);
            _backButtonRect = new Rectangle(1500, 80, 300, 80);

            GenerateNodes();
        }

        private void LoadMapBackgrounds()
        {
            // Map 1 - Forest
            string map1Path = Path.Combine(Game.Content.RootDirectory, "Maps/Map1_ForestRuins.png");
            if (File.Exists(map1Path))
            {
                try
                {
                    _map1Background = Texture2D.FromFile(Game.GraphicsDevice, map1Path);
                }
                catch
                {
                    _map1Background = null;
                }
            }

            // Map 2 - Island Nation
            string map2Path = Path.Combine(
                Game.Content.RootDirectory,
                "Maps/Map2_IslandNation.png"
            );
            if (File.Exists(map2Path))
            {
                try
                {
                    _map2Background = Texture2D.FromFile(Game.GraphicsDevice, map2Path);
                }
                catch
                {
                    _map2Background = null;
                }
            }

            Console.WriteLine("Map backgrounds loaded.");
        }

        private void GenerateNodes()
        {
            _nodes.Clear();

            if (_currentMapIndex == 0) // Map 1 - Forest
            {
                _nodes.Add(new MapNode { Name = "Start", Position = new Vector2(380, 480) });
                _nodes.Add(new MapNode { Name = "Forest Edge", Position = new Vector2(620, 320) });
                _nodes.Add(
                    new MapNode { Name = "River Crossing", Position = new Vector2(540, 580) }
                );
                _nodes.Add(
                    new MapNode { Name = "Ancient Ruins", Position = new Vector2(850, 390) }
                );
                _nodes.Add(new MapNode { Name = "Hidden Grove", Position = new Vector2(720, 650) });
                _nodes.Add(
                    new MapNode { Name = "Overgrown Camp", Position = new Vector2(980, 510) }
                );
                _nodes.Add(
                    new MapNode { Name = "Old Watchtower", Position = new Vector2(1180, 340) }
                );
                _nodes.Add(
                    new MapNode { Name = "Final Clearing", Position = new Vector2(1290, 560) }
                );
            }
            else // Map 2 - Island Nation
            {
                _nodes.Add(
                    new MapNode { Name = "Harbor Village", Position = new Vector2(420, 520) }
                );
                _nodes.Add(
                    new MapNode { Name = "Cherry Blossom Shrine", Position = new Vector2(680, 380) }
                );
                _nodes.Add(new MapNode { Name = "Stone Bridge", Position = new Vector2(920, 450) });
                _nodes.Add(
                    new MapNode { Name = "Mountain Temple", Position = new Vector2(1150, 320) }
                );
                _nodes.Add(new MapNode { Name = "Hidden Cove", Position = new Vector2(580, 620) });
                _nodes.Add(
                    new MapNode { Name = "Pagoda Island", Position = new Vector2(1050, 580) }
                );
                _nodes.Add(
                    new MapNode { Name = "Ancient Gateway", Position = new Vector2(780, 280) }
                );
            }

            foreach (var node in _nodes)
            {
                node.ClickRect = new Rectangle(
                    (int)node.Position.X - 35,
                    (int)node.Position.Y - 35,
                    70,
                    70
                );
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput(gameTime);

            if (!_isMiniMode)
            {
                Game.Window.Title =
                    $"DevGame - {_mapTabs[_currentMapIndex]} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            }
        }

        public override void OnMouseClick(Point mousePosition)
        {
            if (_isMiniMode)
            {
                Game.ChangeState(GameState.Map);
                return;
            }

            // Tab clicks - switch maps
            for (int i = 0; i < _mapTabs.Count; i++)
            {
                if (_tabRects[i].Contains(mousePosition))
                {
                    if (_currentMapIndex != i)
                    {
                        _currentMapIndex = i;
                        GenerateNodes();
                    }
                    return;
                }
            }

            // Node clicks → Battle
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].ClickRect.Contains(mousePosition))
                {
                    Console.WriteLine($"Battle started at: {_nodes[i].Name}");
                    Game.ChangeState(GameState.Battle);
                    return;
                }
            }

            // Back button
            if (_backButtonRect.Contains(mousePosition))
            {
                Game.ChangeState(GameState.MainHub);
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_isMiniMode)
            {
                spriteBatch.Draw(_whitePixel, _miniBounds, new Color(25, 38, 52));
                if (_font != null)
                {
                    spriteBatch.DrawString(
                        _font,
                        _mapTabs[_currentMapIndex],
                        new Vector2(_miniBounds.X + 40, _miniBounds.Y + 25),
                        Color.Cyan
                    );
                }
                return;
            }

            // Draw the correct background based on current map
            Texture2D? currentBackground =
                _currentMapIndex == 0 ? _map1Background : _map2Background;

            if (currentBackground != null)
            {
                spriteBatch.Draw(currentBackground, new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            else
            {
                // Fallback
                spriteBatch.Draw(
                    _whitePixel,
                    new Rectangle(0, 0, 1920, 1080),
                    new Color(20, 30, 45)
                );
            }

            if (_font != null)
            {
                // Tabs
                for (int i = 0; i < _mapTabs.Count; i++)
                {
                    Color color =
                        (i == _currentMapIndex) ? new Color(90, 70, 130) : new Color(45, 40, 75);
                    spriteBatch.Draw(_whitePixel, _tabRects[i], color);
                    spriteBatch.DrawString(
                        _font,
                        _mapTabs[i],
                        new Vector2(_tabRects[i].X + 30, _tabRects[i].Y + 18),
                        Color.White
                    );
                }

                // Current map title
                spriteBatch.DrawString(
                    _font,
                    $"CURRENT MAP: {_mapTabs[_currentMapIndex].ToUpper()}",
                    new Vector2(200, 160),
                    Color.Cyan
                );

                // Draw nodes
                foreach (var node in _nodes)
                {
                    spriteBatch.Draw(_whitePixel, node.ClickRect, new Color(100, 80, 140));
                    spriteBatch.DrawString(
                        _font,
                        node.Name,
                        new Vector2(node.Position.X - 35, node.Position.Y + 45),
                        Color.White
                    );
                }

                // Back button
                spriteBatch.Draw(_whitePixel, _backButtonRect, new Color(100, 30, 30));
                spriteBatch.DrawString(_font, "BACK TO HUB", new Vector2(1540, 105), Color.White);

                DrawDevMenu(spriteBatch, _font);
            }
        }
    }
}
