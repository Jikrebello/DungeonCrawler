using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DungeonCrawler
{
    public class Game
    {
        #region Variables
        /// <summary>
        /// Constant for fixed time - step loop. We'll lock it at 60fps.
        /// </summary>
        static readonly float FPS = 60f;

        /// <summary>
        /// Roughly (0.017) @ 60fps.
        /// </summary>
        static readonly float MS_PER_STEP = 1f / FPS;

        /// <summary>
        /// The main application window.
        /// </summary>
        RenderWindow _window;

        /// <summary>
        /// An Array of  the different view the game needs.
        /// </summary>
        View[] _views = new View[(int)VIEW.Count];

        /// <summary>
        /// Used in the main game time step.
        /// </summary>
        Clock _timestepClock;

        /// <summary>
        /// The default font to be used when drawing text.
        /// </summary>
        Font _font;

        /// <summary>
        /// The game state.
        /// </summary>
        GAME_STATE _gameState;

        /// <summary>
        /// A list that holds all items within the level.
        /// </summary>
        List<Item> _items;

        /// <summary>
        /// A list that holds all the enemies within the level.
        /// </summary>
        List<Enemy> _enemies;

        /// <summary>
        /// A bool that tracks the running state of the game. It's used in the main loop.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// The main level object. All data and functionally regarding the level lives in this class/object.
        /// </summary>
        Level _level;

        /// <summary>
        /// The main player object. Only one instance of this object should be created at any one time.
        /// </summary>
        Player _player;

        /// <summary>
        /// String stream used by the DrawText() function.
        /// </summary>
        StreamReader _stringStream;

        /// <summary>
        /// String used by the DrawText() function.
        /// </summary>
        string _string;

        /// <summary>
        /// Text used by the DrawText() function.
        /// </summary>
        Text _text;

        /// <summary>
        /// A list containing all sprites that make up the lighting grid.
        /// </summary>
        List<Sprite> _lightGrid = new List<Sprite>();

        /// <summary>
        /// The size of the screen and window.
        /// </summary>
        Vector2u _screenSize;

        /// <summary>
        /// The center of the screen.
        /// </summary>
        Vector2f _screenCenter;

        /// <summary>
        /// The current game score.
        /// </summary>
        int _scoreTotal;

        /// <summary>
        /// The amount of gold that the player currently has.
        /// </summary>
        int _goldTotal;

        /// <summary>
        /// The sprite that shows the player class in the UI.
        /// </summary>
        Sprite _playerUISprite;

        /// <summary>
        /// The sprite used to show how many coins the player has.
        /// </summary>
        Sprite _coinUISprite;

        /// <summary>
        /// The sprite used to show how much score the player has.
        /// </summary>
        Sprite _gemUISprite;

        /// <summary>
        /// Key ui sprite.
        /// </summary>
        Sprite _keyUISprite;

        /// <summary>
        /// The sprite for the players attack stat.
        /// </summary>
        Sprite _attackStatSprite;

        /// <summary>
        /// The texture IDs for the attack stat textures.
        /// </summary>
        int[] _attackStatTextureIDs = new int[2];

        /// <summary>
        /// The sprite for the players defense stat.
        /// </summary>
        Sprite _defenseStatSprite;

        /// <summary>
        /// The texture IDs for the defense stat textures.
        /// </summary>
        int[] _defenseStatTextureIDs = new int[2];

        /// <summary>
        /// The sprite for the players strength stat.
        /// </summary>
        Sprite _strengthStatSprite;

        /// <summary>
        /// The texture IDs for the strength stat textures.
        /// </summary>
        int[] _strengthStatTextureIDs = new int[2];

        /// <summary>
        /// The sprite for the players dexterity stat.
        /// </summary>
        Sprite _dexterityStatSprite;

        /// <summary>
        /// The texture IDs for the dexterity stat textures.
        /// </summary>
        int[] _dexterityStatTextureIDs = new int[2];

        /// <summary>
        /// The sprite for the players stamina stat.
        /// </summary>
        Sprite _staminaStatSprite;

        /// <summary>
        /// The texture IDs for the stamina stat textures.
        /// </summary>
        int[] _staminaStatTextureIDs = new int[2];

        /// <summary>
        /// A vector of all the player's projectiles.
        /// </summary>
        List<Projectile> _playerProjectiles;

        /// <summary>
        /// The ID of the player's projectile texture.
        /// </summary>
        int _projectileTextureID;

        /// <summary>
        /// A boolean denoting if a new level was generated.
        /// </summary>
        bool _levelWasGenerated;

        /// <summary>
        /// Sprite for the health bar.
        /// </summary>
        Sprite _healthBarSprite;

        /// <summary>
        /// Sprite for the health bar outline.
        /// </summary>
        Sprite _healthBarOutlineSprite;

        /// <summary>
        /// Sprite for the mana bar.
        /// </summary>
        Sprite _manaBarSprite;

        /// <summary>
        /// Sprite for the mana bar outline.
        /// </summary>
        Sprite _manaBarOutlineSprite;

        /// <summary>
        /// A list of all ui sprites.
        /// </summary>
        List<Sprite> _uiSprites = new();
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="window">The main render window.</param>
        public Game(RenderWindow window)
        {
            _window = window;
            _gameState = GAME_STATE.Playing;
            IsRunning = true;

            // Enable VSync.
            _window.SetVerticalSyncEnabled(enable: true);

            // Hide the mouse cursor.
            _window.SetMouseCursorVisible(visible: false);

            // Calculate and store the center of the screen.
            _screenCenter = new Vector2f(x: _window.Size.X / 2f, y: _window.Size.Y / 2f);

            // Create the level object.
            _level = new Level(window: _window);

            // Create the game font.
            _font = new Font(filename: "Resources/fonts/ADDSBP__.TTF");
        }

        /// <summary>
        /// Initializes the game object by initializing all objects the main game uses.
        /// </summary>
        public void Initialize()
        {
            // Get the screen size.
            _screenSize = _window.Size;

            // Load the correct projectile texture.
            _projectileTextureID = TextureManager.AddTexture(
                filePath: "Resources/Textures/projectiles/spr_sword.png"
            );

            // Initialize the UI.
            LoadUI();

            // Builds the light grid.
            ConstructLightGrid();

            // Define the game views.
            _views[(int)VIEW.Main] = _window.DefaultView;
            _views[(int)VIEW.Main].Zoom(factor: 0.5f);
            _views[(int)VIEW.UI] = _window.DefaultView;

            // Load the level.
            _level.LoadLevelFromFile(fileName: "Resources/data/level_data2.txt");

            // Set the position of the player.
            _player.Position = new Vector2f(x: _screenCenter.X + 197f, y: _screenCenter.Y + 410f);

            // Populate the level.
            PopulateLevel();
        }

        /// <summary>
        /// Loads and prepares all UI assets.
        /// </summary>
        void LoadUI()
        {
            // Initialize the player UI Texture and sprite.
            _playerUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "Resources/Textures/ui/spr_warrior_ui.png"
                    )
                ),
                Position = new Vector2f(x: 45f, y: 45f),
                Origin = new Vector2f(x: 30f, y: 30f)
            };
            _uiSprites.Add(item: _playerUISprite);

            // Bar outlines.
            Texture barOutlineTexture = TextureManager.GetTexture(
                textureID: TextureManager.AddTexture(
                    filePath: "Resources/Textures/ui/spr_bar_outline.png"
                )
            );
            Vector2f barOutlineTextureOrigin =
                new(x: barOutlineTexture.Size.X / 2f, y: barOutlineTexture.Size.Y / 2f);

            _healthBarOutlineSprite = new Sprite
            {
                Texture = barOutlineTexture,
                Position = new Vector2f(x: 205f, y: 35f),
                Origin = new Vector2f(x: barOutlineTextureOrigin.X, y: barOutlineTextureOrigin.Y)
            };
            _uiSprites.Add(item: _healthBarOutlineSprite);

            _manaBarOutlineSprite = new Sprite
            {
                Texture = barOutlineTexture,
                Position = new Vector2f(x: 205f, y: 55f),
                Origin = new Vector2f(x: barOutlineTextureOrigin.X, y: barOutlineTextureOrigin.Y)
            };
            _uiSprites.Add(item: _manaBarOutlineSprite);

            // Bars.
            Texture healthBarTexture = TextureManager.GetTexture(
                textureID: TextureManager.AddTexture(
                    filePath: "Resources/Textures/ui/spr_health_bar.png"
                )
            );
            Vector2f barTextureOrigin =
                new(x: healthBarTexture.Size.X / 2f, y: healthBarTexture.Size.Y / 2f);

            // Health Bar.
            _healthBarSprite = new Sprite
            {
                Texture = healthBarTexture,
                Position = new Vector2f(x: 205f, y: 35f),
                Origin = new Vector2f(x: barTextureOrigin.X, y: barOutlineTextureOrigin.Y)
            };

            // Mana Bar.
            _manaBarSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "Resources/Textures/ui/spr_mana_bar.png"
                    )
                ),
                Position = new Vector2f(x: 205f, y: 55f),
                Origin = new Vector2f(x: barOutlineTextureOrigin.X, y: barOutlineTextureOrigin.Y)
            };

            // Initialize the gem UI sprites.
            _gemUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "Resources/Textures/ui/spr_gem_ui.png"
                    )
                ),
                Position = new Vector2f(x: _screenCenter.X - 260f, y: 50f),
                Origin = new Vector2f(x: 42f, y: 36f)
            };
            _uiSprites.Add(item: _gemUISprite);

            // Initialize the coin UI sprites.
            _coinUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "Resources/Textures/ui/spr_coin_ui.png"
                    )
                ),
                Position = new Vector2f(x: _screenCenter.X + 60f, y: 50f),
                Origin = new Vector2f(x: 48f, y: 24f)
            };
            _uiSprites.Add(item: _coinUISprite);

            // Key pickup sprite.
            _keyUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "Resources/Textures/ui/spr_key_ui.png"
                    )
                ),
                Position = new Vector2f(x: _screenSize.X - 120f, y: _screenSize.Y - 70f),
                Origin = new Vector2f(x: 90f, y: 45f),
                Color = new Color(red: 255, green: 255, blue: 255, alpha: 60)
            };
            _uiSprites.Add(item: _keyUISprite);

            // Load attack stats.
            _attackStatTextureIDs[0] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_attack_ui.png"
            );
            _attackStatTextureIDs[1] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_attack_ui_alt.png"
            );

            _attackStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _attackStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 270f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _attackStatSprite);

            // Load defense stats.
            _defenseStatTextureIDs[0] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_defense_ui.png"
            );
            _defenseStatTextureIDs[1] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_defense_ui_alt.png"
            );

            _defenseStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _defenseStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 150f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _defenseStatSprite);

            // Load strength stats.
            _strengthStatTextureIDs[0] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_strength_ui.png"
            );
            _strengthStatTextureIDs[1] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_strength_ui_alt.png"
            );

            _strengthStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _strengthStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 30f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 22f, y: 12f)
            };
            _uiSprites.Add(item: _strengthStatSprite);

            // Load dexterity stats.
            _dexterityStatTextureIDs[0] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_dexterity_ui.png"
            );
            _dexterityStatTextureIDs[1] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_dexterity_ui_alt.png"
            );

            _dexterityStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _dexterityStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 90f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _dexterityStatSprite);

            // Load stamina stats.
            _staminaStatTextureIDs[0] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_stamina_ui.png"
            );
            _staminaStatTextureIDs[1] = TextureManager.AddTexture(
                filePath: "Resources/Textures/ui/spr_stamina_ui_alt.png"
            );

            _staminaStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _staminaStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 210f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _staminaStatSprite);
        }

        /// <summary>
        /// Populates the current game room with items and enemies.
        /// </summary>
        void PopulateLevel()
        {
            // Create a gold object.
            var gold = new Gold
            {
                // Set the gold position.
                Position = new Vector2f(x: _screenCenter.X - 50f, y: _screenCenter.Y)
            };

            // Add the gold item to our collection of all object.
            _items.Add(item: gold);
        }

        void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                _window.Close();
            }
        }

        /// <summary>
        /// The main game loop. This loop in turn updates the game, and draws all objects to screen.
        /// </summary>
        public void Run()
        {
            float currentTime = _timestepClock.Restart().AsSeconds();
            float deltaTime = 0f;

            _window.Closed += (sender, args) => _window.Close();
            _window.KeyPressed += KeyPressed;

            while (_window.IsOpen)
            {
                float newTime = _timestepClock.ElapsedTime.AsSeconds();
                deltaTime = Math.Max(val1: 0f, val2: newTime - currentTime);
                currentTime = newTime;

                if (!_levelWasGenerated)
                {
                    Update(deltaTime: deltaTime);

                    Draw(deltaTime: deltaTime);
                }
                else
                {
                    _levelWasGenerated = false;
                }

                _window.DispatchEvents();
                _window.Clear();

                _window.Display();
            }
        }

        /// <summary>
        /// The main update loop. This loop in turns calls the update loops of all game objects.
        /// </summary>
        /// <param name="deltaTime">The time, in MS, since the last update call.</param>
        public void Update(float deltaTime)
        {
            // Check what state the game is in.
            switch (_gameState)
            {
                case GAME_STATE.Main_Menu:
                    // Main Menu code.
                    break;

                case GAME_STATE.Playing:

                    // First check if the player is at the exit. If so theres no need to update anything.
                    Tile playerTile = _level.GetTile(position: _player.Position);

                    if (playerTile.Type == TILE.Wall_Door_Unlocked)
                    {
                        // ...
                    }
                    else
                    {
                        // Update the player.
                        _player.Update(deltaTime: deltaTime, level: ref _level);

                        // Store the player position as it's used many times.
                        Vector2f playerPosition = _player.Position;

                        if (_player.IsAttacking())
                        {
                            if (_player.Mana >= 2)
                            {
                                Vector2f target =
                                    new(x: Mouse.GetPosition().X, y: Mouse.GetPosition().Y);
                                Projectile projectile =
                                    new(
                                        texture: TextureManager.GetTexture(
                                            textureID: _projectileTextureID
                                        ),
                                        origin: playerPosition,
                                        screenCenter: _screenCenter,
                                        target: target
                                    );
                                _playerProjectiles.Add(item: projectile);

                                // Reduce the players Mana.
                                _player.Mana -= 2;
                            }
                        }

                        // Update all the items.
                        UpdateItems(playerPosition: playerPosition);

                        // Update level light.
                        UpdateLight(playerPosition: playerPosition);

                        // Update all enemies.
                        UpdateEnemies(playerPosition: playerPosition, deltaTime: deltaTime);

                        // Update all projectiles.
                        UpdateProjectiles(deltaTime: deltaTime);

                        // Center the view.
                        _views[(int)VIEW.Main].Center = playerPosition;
                    }
                    break;

                case GAME_STATE.Game_Over:
                    // Game over code.
                    break;
            }
        }

        /// <summary>
        /// Draws all game objects to screen.
        /// </summary>
        /// <param name="deltaTime">The time, in MS, since the last draw call.</param>
        public void Draw(float deltaTime)
        {
            // Clear the screen.
            _window.Clear(color: new Color(red: 3, green: 3, blue: 3, alpha: 255));

            // Check what state the game is in.
            switch (_gameState)
            {
                case GAME_STATE.Main_Menu:
                    // Draw Main Menu.
                    break;

                case GAME_STATE.Playing:
                    // Set the main game view.
                    _window.SetView(view: _views[(int)VIEW.Main]);

                    // Draw the level.
                    _level.Draw(window: _window, deltaTime: deltaTime);

                    // Draw all the objects.
                    foreach (var item in _items)
                    {
                        item.Draw(_window: _window, _deltaTime: deltaTime);
                    }

                    // Draw for all enemies.
                    foreach (var enemy in _enemies)
                    {
                        enemy.Draw(window: _window, deltaTime: deltaTime);
                    }

                    // Draw all projectiles.
                    foreach (var projectile in _playerProjectiles)
                    {
                        _window.Draw(drawable: projectile.GetSprite());
                    }

                    // Draw the player.
                    _player.Draw(window: _window, deltaTime: deltaTime);

                    // Draw the level light.
                    foreach (var sprite in _lightGrid)
                    {
                        _window.Draw(drawable: sprite);
                    }

                    // Switch to UI.
                    _window.SetView(view: _views[(int)VIEW.UI]);

                    // Draw player stats.
                    DrawString(
                        text: _player.Attack.ToString(),
                        position: new Vector2f(x: _screenCenter.X - 210f, y: _screenCenter.Y - 30f),
                        size: 25
                    );
                    DrawString(
                        text: _player.Defense.ToString(),
                        position: new Vector2f(x: _screenCenter.X - 90f, y: _screenCenter.Y - 30f),
                        size: 25
                    );
                    DrawString(
                        text: _player.Strength.ToString(),
                        position: new Vector2f(x: _screenCenter.X + 30f, y: _screenCenter.Y - 30f),
                        size: 25
                    );
                    DrawString(
                        text: _player.Dexterity.ToString(),
                        position: new Vector2f(x: _screenCenter.X + 150f, y: _screenCenter.Y - 30f),
                        size: 25
                    );
                    DrawString(
                        text: _player.Stamina.ToString(),
                        position: new Vector2f(x: _screenCenter.X + 270f, y: _screenCenter.Y - 30f),
                        size: 25
                    );

                    // Draw player score.
                    string scoreString;

                    if (_scoreTotal > 99999)
                        scoreString = _scoreTotal.ToString();
                    else if (_scoreTotal > 9999)
                        scoreString = "0" + _scoreTotal;
                    else if (_scoreTotal > 999)
                        scoreString = "00" + _scoreTotal;
                    else if (_scoreTotal > 99)
                        scoreString = "000" + _scoreTotal;
                    else if (_scoreTotal > 9)
                        scoreString = "0000" + _scoreTotal;
                    else
                        scoreString = "00000" + _scoreTotal;

                    DrawString(
                        text: scoreString,
                        position: new Vector2f(x: _screenCenter.X - 120f, y: 40f),
                        size: 40
                    );

                    // Draw gold total.
                    string goldString;

                    if (_goldTotal > 99999)
                        goldString = _goldTotal.ToString();
                    else if (_goldTotal > 9999)
                        goldString = "0" + _goldTotal;
                    else if (_goldTotal > 999)
                        goldString = "00" + _goldTotal;
                    else if (_goldTotal > 99)
                        goldString = "000" + _goldTotal;
                    else if (_goldTotal > 9)
                        goldString = "0000" + _goldTotal;
                    else
                        goldString = "00000" + _goldTotal;

                    DrawString(
                        text: goldString,
                        position: new Vector2f(x: _screenCenter.X + 220f, y: 40f),
                        size: 40
                    );

                    // Draw rest of the UI.
                    foreach (var sprite in _uiSprites)
                    {
                        _window.Draw(drawable: sprite);
                    }

                    // Draw the current room and floor.
                    DrawString(
                        text: "Floor " + _level.GetFloorNumber(),
                        position: new Vector2f(x: 70f, y: _screenSize.Y - 65f),
                        size: 25
                    );
                    DrawString(
                        text: "Room " + _level.GetRoomNumber(),
                        position: new Vector2f(x: 70f, y: _screenSize.Y - 30f),
                        size: 25
                    );

                    // Draw health and mana bars.
                    _healthBarSprite.TextureRect = new IntRect(
                        left: 0,
                        top: 0,
                        width: (int)(213f / _player.MaxHealth * _player.Health),
                        height: 8
                    );
                    _window.Draw(drawable: _healthBarSprite);

                    _manaBarSprite.TextureRect = new IntRect(
                        left: 0,
                        top: 0,
                        width: (int)(213f / _player.MaxMana * _player.Mana),
                        height: 8
                    );
                    _window.Draw(drawable: _manaBarSprite);
                    break;

                case GAME_STATE.Game_Over:
                    // Draw game over screen.
                    break;
            }
            // Present the back-buffer to the screen.
            _window.Display();
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="position1">The position of the first point.</param>
        /// <param name="position2">The position of the second item.</param>
        /// <returns>The distance between the two points.</returns>
        static float DistanceBetweenPoints(Vector2f position1, Vector2f position2)
        {
            return (float)
                Math.Abs(
                    value: Math.Sqrt(
                        d: ((position1.X - position2.X) * (position1.X - position2.X))
                            + ((position1.Y - position2.Y) * (position1.Y - position2.Y))
                    )
                );
        }

        /// <summary>
        /// Draws text at a given location on the screen.
        /// </summary>
        /// <param name="text">The string you wish to draw.</param>
        /// <param name="position">The top-left position of the string.</param>
        /// <param name="size">(Optional) The font-size to use. Default value is 10.</param>
        void DrawString(string text, Vector2f position, uint size = 10)
        {
            // Clear the old data.
            _text.DisplayedString = "";

            _text.DisplayedString = text;
            _text.Font = _font;
            _text.CharacterSize = size;
            _text.Position = new Vector2f(
                x: position.X - (_text.GetLocalBounds().Width / 2f),
                y: position.Y - (_text.GetLocalBounds().Height / 2f)
            );

            _window.Draw(drawable: _text);
        }

        /// <summary>
        /// Constructs the grid of sprites that are used to draw the game light system.
        /// </summary>
        void ConstructLightGrid()
        {
            // Load the light tile texture and store a reference.
            int textureID = TextureManager.AddTexture(
                filePath: "Resources/Textures/spr_light_grid.png"
            );
            var lightTexture = TextureManager.GetTexture(textureID: textureID);

            // Calculate the number of tiles in the grid. Each light tile is 25px square.
            IntRect levelArea;

            // Define the bounds of the level.
            levelArea.Left = (int)_level.GetPosition().X;
            levelArea.Top = (int)_level.GetPosition().Y;
            levelArea.Width = Level.GetSize().X * Level.GetTileSize();
            levelArea.Height = Level.GetSize().Y * Level.GetTileSize();

            int width,
                height,
                lightTotal;

            width = levelArea.Width / 25;
            height = levelArea.Height / 25;

            lightTotal = width * height;

            // Create all tiles.
            for (int i = 0; i < lightTotal; i++)
            {
                // Create the tile.
                Sprite lightSprite = new Sprite
                {
                    Texture = lightTexture,
                    Position = new Vector2f(
                        x: levelArea.Left + i % width * 25,
                        y: levelArea.Top + i / width * 25
                    )
                };
                _lightGrid.Add(item: lightSprite);
            }
        }

        /// <summary>
        /// Updates the level light.
        /// </summary>
        /// <param name="playerPosition">The position of the players within the level.</param>
        void UpdateLight(Vector2f playerPosition)
        {
            foreach (var sprite in _lightGrid)
            {
                float tileAlpha = 255f; // Tile alpha.

                // Calculate distance between tile and player.
                float distance = DistanceBetweenPoints(
                    position1: sprite.Position,
                    position2: playerPosition
                );

                // Calculate tile transparency.
                if (distance < 200f)
                {
                    tileAlpha = 0f;
                }
                else if (distance < 250f)
                {
                    tileAlpha = 51f * (distance - 200f) / 10f;
                }

                // Get all the torches from the level.
                var torches = _level.GetTorches();

                // If there are torches.
                if (torches != null)
                {
                    // Update the light surrounding each torch.
                    foreach (var torch in torches)
                    {
                        // If the light tile is within range of the torch.
                        distance = DistanceBetweenPoints(
                            position1: sprite.Position,
                            position2: torch.Position
                        );
                        if (distance < 100f)
                        {
                            // Edit its alpha.
                            tileAlpha -=
                                (tileAlpha - (tileAlpha / 100f * distance)) * torch.Brightness;
                        }
                    }

                    // Ensure alpha does not go negative.
                    if (tileAlpha < 0)
                    {
                        tileAlpha = 0;
                    }
                }

                // Set the sprite transparency.
                sprite.Color = new Color(red: 255, green: 255, blue: 255, alpha: (byte)tileAlpha);
            }
        }

        /// <summary>
        /// Updates all items in the level.
        /// </summary>
        /// <param name="playerPosition">The position of the players within the level.</param>
        void UpdateItems(Vector2f playerPosition)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (
                    DistanceBetweenPoints(
                        position1: _items[index: i].Position,
                        position2: playerPosition
                    ) < 40f
                )
                {
                    // Check what type of object it is.
                    switch (_items[index: i].Type)
                    {
                        case ITEM.Gold:
                        {
                            // Cast item as a gold object so we can access its properties.
                            Gold gold = _items[index: i] as Gold;
                            // Add to the gold total.
                            _goldTotal = gold.GoldValue;

                            // Finally, delete the object.
                            _items.RemoveAt(index: i);
                            break;
                        }

                        case ITEM.Gem:
                        {
                            // Cast to a Gem and add to score.
                            Gem gem = _items[index: i] as Gem;
                            _scoreTotal = gem.ScoreValue;

                            // Finally, delete the object.
                            _items.RemoveAt(index: i);
                            break;
                        }

                        case ITEM.Key:
                        {
                            // Unlock the door.
                            _level.UnlockDoor();
                            // Set the key as collected.
                            _keyUISprite.Color = new Color(color: Color.White);

                            // Finally, delete the object.
                            _items.RemoveAt(index: i);
                            break;
                        }

                        case ITEM.Potion:
                        {
                            // ...

                            // Finally, delete the object.
                            _items.RemoveAt(index: i);
                            break;
                        }

                        case ITEM.Heart:
                        {
                            // Cast to heart and get health.
                            Heart heart = _items[index: i] as Heart;
                            _player.Health += heart.HealthValue;

                            // Finally, delete the object.
                            _items.RemoveAt(index: i);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates all enemies in the level.
        /// </summary>
        /// <param name="playerPosition">The position of the players within the level.</param>
        /// <param name="deltaTime">The amount of time that has passed since the last update.</param>
        void UpdateEnemies(Vector2f playerPosition, float deltaTime)
        {
            // Store player tile.
            var playerTile = _level.GetTile(position: playerPosition);

            var rand = new Random();

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                // Get the enemy.
                var enemy = _enemies[index: i];

                // Create a bool so we can check if an enemy was deleted.
                bool enemyWasDeleted = false;

                //Get the tile that the enemy is on.
                var enemyTile = _level.GetTile(position: enemy.Position);

                // Check for collisions with projectiles.
                for (int j = _playerProjectiles.Count - 1; j >= 0; j--)
                {
                    var projectile = _playerProjectiles[index: j];

                    // If the enemy and projectile occupy the same tile they have collided.
                    if (enemyTile == _level.GetTile(position: projectile.Position))
                    {
                        // Delete the projectile.
                        _playerProjectiles.RemoveAt(index: j);

                        // Damage the enemy.
                        enemy.Damage(damage: 25);

                        // If the enemy is dead remove it.
                        if (enemy.IsDead())
                        {
                            // Get the enemy position.
                            var enemyPosition = enemy.Position;

                            // Spawn loot.
                            for (int k = 0; k < 5; k++)
                            {
                                enemyPosition.X += rand.Next(minValue: 15, maxValue: 31);
                                enemyPosition.Y += rand.Next(minValue: 15, maxValue: 31);
                                var item = new Item();

                                switch (rand.Next(minValue: 0, maxValue: 2))
                                {
                                    // Spawn Gold.
                                    case 0:
                                        Gold gold = item as Gold;
                                        gold.Position = enemyPosition;
                                        _items.Add(item: gold);
                                        break;

                                    // Spawn Gem.
                                    case 1:
                                        Gem gem = item as Gem;
                                        gem.Position = enemyPosition;
                                        _items.Add(item: gem);
                                        break;
                                }
                            }

                            // 1 in 5 chance of spawning health.
                            if (rand.Next(minValue: 0, maxValue: 5) == 0)
                            {
                                enemyPosition.X += rand.Next(minValue: 15, maxValue: 31);
                                enemyPosition.Y += rand.Next(minValue: 15, maxValue: 31);
                                var heart = new Heart { Position = enemyPosition };
                                _items.Add(item: heart);
                            }
                            // 1 in 5 chance of spawning potion.
                            else if (rand.Next(minValue: 0, maxValue: 5) == 1)
                            {
                                enemyPosition.X += rand.Next(minValue: 15, maxValue: 31);
                                enemyPosition.Y += rand.Next(minValue: 15, maxValue: 31);
                                var potion = new Potion { Position = enemyPosition };
                                _items.Add(item: potion);
                            }

                            // Delete the enemy.
                            _enemies.RemoveAt(index: i);
                            enemyWasDeleted = true;
                        }
                    }
                }
                // If the enemy was not deleted.
                if (!enemyWasDeleted)
                {
                    enemy.Update(deltaTime: deltaTime);
                }

                if (enemyTile == playerTile)
                {
                    if (_player.CanTakeDamage)
                    {
                        _player.Damage(damage: 10);
                    }
                }
            }
        }

        /// <summary>
        /// Updates all projectiles in the level.
        /// </summary>
        /// <param name="deltaTime">The amount of time that has passed since the last update.</param>
        void UpdateProjectiles(float deltaTime)
        {
            for (int i = _playerProjectiles.Count - 1; i >= 0; i--)
            {
                var projectile = _playerProjectiles[index: i];
                var projectileTileType = _level.GetTile(position: projectile.Position).Type;

                // If the tile the projectile is on is not the floor, delete it.
                if (projectileTileType != TILE.Floor && projectileTileType != TILE.Floor_Alt)
                {
                    _playerProjectiles.RemoveAt(index: i);
                }
                else
                {
                    // Update the projectile and move it to the next one.
                    projectile.Update(deltaTime: deltaTime);
                }
            }
        }
    }
}
