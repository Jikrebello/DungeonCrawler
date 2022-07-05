using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using System.Numerics;

namespace DungeonCrawler
{
    public class Game
    {
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
        bool _isRunning;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="window">The main render window.</param>
        public Game(RenderWindow window)
        {
            _window = window;
        }

        /// <summary>
        /// Initializes the game object by initializing all objects the main game uses.
        /// </summary>
        public void Initialize() { }

        /// <summary>
        /// The main game loop. This loop in turn updates the game, and draws all objects to screen.
        /// </summary>
        public void Run() { }

        /// <summary>
        /// Returns true if the game is currently running.
        /// </summary>
        /// <returns>True if the game is running.</returns>
        public bool IsRunning() { }

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
        /// Populates the current game room with items and enemies.
        /// </summary>
        void PopulateLevel() { }

        /// <summary>
        /// Loads all sprites needed for the UI.
        /// </summary>
        void LoadUI() { }

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
        void ConstructLightGrid() { }

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
