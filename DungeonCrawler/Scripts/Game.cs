using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using System.Numerics;
using SFML.Window;

namespace DungeonCrawler
{
    public class Game
    {
        #region Variables
        /// <summary>
        /// Constant for fixed time - step loop. We'll lock it at 60fps.
        /// </summary>
        static float FPS = 60f;

        /// <summary>
        /// Roughly (0.017) @ 60fps.
        /// </summary>
        static float MS_PER_STEP = 1f / FPS;

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
        List<Sprite> _lightGrid;

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
        int _goldScore;

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
        List<Sprite> _uiSprites;
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
            _font = new Font(filename: "../Resources/fonts/ADDSBP__.TTF");
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
                filePath: "../Resources/Textures/projectiles/spr_sword.png"
            );

            // Initialize the UI.
            LoadUI();

            // Define the game views.
            _views[(int)VIEW.Main] = _window.DefaultView;
            _views[(int)VIEW.Main].Zoom(factor: 0.5f);
            _views[(int)VIEW.UI] = _window.DefaultView;

            // Load the level.
            _level.LoadLevelFromFile(fileName: "../Resources/data/level_data.txt");

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
                        filePath: "../Resources/Textures/ui/spr_warrior_ui.png"
                    )
                ),
                Position = new Vector2f(x: 45f, y: 45f),
                Origin = new Vector2f(x: 30f, y: 30f)
            };
            _uiSprites.Add(item: _playerUISprite);

            // Bar outlines.
            Texture barOutlineTexture = TextureManager.GetTexture(
                textureID: TextureManager.AddTexture(
                    filePath: "../Resources/Textures/ui/spr_bar_outline.png"
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
                    filePath: "../Resources/Textures/ui/spr_health_bar.png"
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
                        filePath: "../Resources/Textures/ui/spr_mana_bar.png"
                    )
                ),
                Position = new Vector2f(x: 205f, y: 55f),
                Origin = new Vector2f(x: barOutlineTextureOrigin.X, y: barOutlineTextureOrigin.Y)
            };

            // Initialize the gem UI sprites.
            _gemUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(filePath: "../")
                ),
                Position = new Vector2f(x: _screenCenter.X - 260f, y: 50f),
                Origin = new Vector2f(x: 42f, y: 36f)
            };
            _uiSprites.Add(item: _gemUISprite);

            // Initialize the coin UI sprites.
            _coinUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(filePath: "../")
                ),
                Position = new Vector2f(x: _screenCenter.X + 60f, y: 50f),
                Origin = new Vector2f(x: 48f, y: 24f)
            };
            _uiSprites.Add(item: _coinUISprite);

            // Key pickup sprite.
            _keyUISprite = new Sprite
            {
                Texture = TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(filePath: "../")
                ),
                Position = new Vector2f(x: _screenSize.X - 120f, y: _screenSize.Y - 70f),
                Origin = new Vector2f(x: 90f, y: 45f),
                Color = new Color(red: 255, green: 255, blue: 255, alpha: 60)
            };
            _uiSprites.Add(item: _keyUISprite);

            // Load attack stats.
            _attackStatTextureIDs[0] = TextureManager.AddTexture(filePath: "../");
            _attackStatTextureIDs[1] = TextureManager.AddTexture(filePath: "../");

            _attackStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _attackStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 270f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _attackStatSprite);

            // Load defense stats.
            _defenseStatTextureIDs[0] = TextureManager.AddTexture(filePath: "../");
            _defenseStatTextureIDs[1] = TextureManager.AddTexture(filePath: "../");

            _defenseStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _defenseStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 150f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _defenseStatSprite);

            // Load strength stats.
            _strengthStatTextureIDs[0] = TextureManager.AddTexture(filePath: "../");
            _strengthStatTextureIDs[1] = TextureManager.AddTexture(filePath: "../");

            _strengthStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _strengthStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 30f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 22f, y: 12f)
            };
            _uiSprites.Add(item: _strengthStatSprite);

            // Load dexterity stats.
            _dexterityStatTextureIDs[0] = TextureManager.AddTexture(filePath: "../");
            _dexterityStatTextureIDs[1] = TextureManager.AddTexture(filePath: "../");

            _dexterityStatSprite = new Sprite
            {
                Texture = TextureManager.GetTexture(textureID: _dexterityStatTextureIDs[0]),
                Position = new Vector2f(x: _screenCenter.X - 90f, y: _screenCenter.Y - 30f),
                Origin = new Vector2f(x: 16f, y: 16f)
            };
            _uiSprites.Add(item: _dexterityStatSprite);

            // Load stamina stats.
            _staminaStatTextureIDs[0] = TextureManager.AddTexture(filePath: "../");
            _staminaStatTextureIDs[1] = TextureManager.AddTexture(filePath: "../");

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

        /// <summary>
        /// The main game loop. This loop in turn updates the game, and draws all objects to screen.
        /// </summary>
        public void Run()
        {
            float currentTime = _timestepClock.Restart().AsSeconds();
            float deltaTime = 0f;

            // Loop until there is quite message from the window or the user pressed escape.
            while (IsRunning)
            {
                // Check if the game was closed.
            }
        }

        /// <summary>
        /// The main update loop. This loop in turns calls the update loops of all game objects.
        /// </summary>
        /// <param name="deltaTime">The time, in MS, since the last update call.</param>
        public void Update(float deltaTime) { }

        /// <summary>
        /// Draws all game objects to screen.
        /// </summary>
        /// <param name="deltaTime">The time, in MS, since the last draw call.</param>
        public void Draw(float deltaTime) { }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="position1">The position of the first point.</param>
        /// <param name="position2">The position of the second item.</param>
        /// <returns>The distance between the two points.</returns>
        float DistanceBetweenPoints(Vector2f position1, Vector2f position2) { }

        /// <summary>
        /// Draws text at a given location on the screen.
        /// </summary>
        /// <param name="text">The string you wish to draw.</param>
        /// <param name="position">The top-left position of the string.</param>
        /// <param name="size">(Optional) The font-size to use. Default value is 10.</param>
        void DrawString(string text, Vector2f position, uint size = 10);

        /// <summary>
        /// Constructs the grid of sprites that are used to draw the game light system.
        /// </summary>
        void ConstructLightGrid()
        {
            // Load the light tile texture and store a reference.
            int textureID = TextureManager.AddTexture(filePath: "../");
            var lightTexture = TextureManager.GetTexture(textureID: textureID);

            // Calculate the number of tiles in the grid. Each light tile is 25px square.
            IntRect levelArea;

            // Define the bounds of the level.
            levelArea.Left = (int)_level.GetPosition().X;
            levelArea.Top = (int)_level.GetPosition().Y;
            levelArea.Width = _level.GetSize().X * _level.GetTileSize();
            levelArea.Height = _level.GetSize().Y * _level.GetTileSize();

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
        void UpdateLight(Vector2f playerPosition) { }

        /// <summary>
        /// Updates all items in the level.
        /// </summary>
        /// <param name="playerPosition">The position of the players within the level.</param>
        void UpdateItems(Vector2f playerPosition) { }

        /// <summary>
        /// Updates all enemies in the level.
        /// </summary>
        /// <param name="playerPosition">The position of the players within the level.</param>
        /// <param name="timeDelta">The amount of time that has passed since the last update.</param>
        void UpdateEnemies(Vector2f playerPosition, float timeDelta) { }

        /// <summary>
        /// Updates all projectiles in the level.
        /// </summary>
        /// <param name="timeDelta">The amount of time that has passed since the last update.</param>
        void UpdateProjectiles(float timeDelta) { }
    }
}
