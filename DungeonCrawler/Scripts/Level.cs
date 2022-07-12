using SFML.Graphics;
using SFML.System;

namespace DungeonCrawler
{
    public class Level
    {
        #region Variables
        /// <summary>
        /// A 2D array that describes the level data.
        /// The type is Tile, which holds a sprite and an index.
        /// </summary>
        Tile[,] _grid;

        /// <summary>
        /// A List of all the sprites in the level.
        /// </summary>
        List<Sprite> _tileSprites = new List<Sprite>();

        /// <summary>
        /// The position of the level relative to the window.
        /// This is to the top-left of the level grid.
        /// </summary>
        Vector2i _origin = new(x: 0, y: 0);

        int _floorNumber = 1;

        /// <summary>
        /// The floor number that the player is currently on.
        /// </summary>
        public int FloorNumber => _floorNumber;

        int _roomNumber = 0;

        /// <summary>
        /// The room number that the player is currently in.
        /// </summary>
        public int RoomNumber => _roomNumber;

        /// <summary>
        /// A 2D array that contains the room layout for the current floor.
        /// </summary>
        int[,] _roomLayout = new int[3, 10];

        /// <summary>
        /// An array containing all texture IDs of the level tiles.
        /// </summary>
        int[] _textureIDs = new int[(int)TILE.Count];

        /// <summary>
        /// The indices of the tile containing the levels door.
        /// </summary>
        Vector2i _doorTileIndices = new(x: 0, y: 0);

        /// <summary>
        /// A List of all tiles in the level.
        /// </summary>
        List<Torch> _torches = new();

        /// <summary>
        /// The game grids width.
        /// </summary>
        public const int GRID_WIDTH = 19;

        /// <summary>
        /// The game grids height.
        /// </summary>
        public const int GRID_HEIGHT = 19;

        /// <summary>
        /// The width and height of each tile in pixels.
        /// </summary>
        public const int TILE_SIZE = 50;
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Level() { }

        /// <summary>
        /// A renderWindow is needed in order for the level to calculate its position.
        /// </summary>
        /// <param name="window">The game window</param>
        public Level(RenderWindow window)
        {
            _grid = new Tile[GRID_WIDTH, GRID_HEIGHT];

            // Load all Tiles.
            #region tile_floor
            AddTile(fileName: "Resources/Textures/tiles/spr_tile_floor.png", tileType: TILE.Floor);
            #endregion

            #region wall_top
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_top.png",
                tileType: TILE.Wall_Top
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_top_left.png",
                tileType: TILE.Wall_Top_Left
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_top_right.png",
                tileType: TILE.Wall_Top_Right
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_top_t.png",
                tileType: TILE.Wall_Top_T
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_top_end.png",
                tileType: TILE.Wall_Top_End
            );
            #endregion

            #region wall_bottom
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_bottom_left.png",
                tileType: TILE.Wall_Bottom_Left
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_bottom_right.png",
                tileType: TILE.Wall_Bottom_Right
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_bottom_t.png",
                tileType: TILE.Wall_Bottom_T
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_bottom_end.png",
                tileType: TILE.Wall_Bottom_End
            );
            #endregion

            #region wall_side
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_side.png",
                tileType: TILE.Wall_Side
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_side_left_t.png",
                tileType: TILE.Wall_Side_Left_T
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_side_left_end.png",
                tileType: TILE.Wall_Side_Left_End
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_side_right_t.png",
                tileType: TILE.Wall_Side_Right_T
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_side_right_end.png",
                tileType: TILE.Wall_Side_Right_End
            );
            #endregion

            #region wall_intersection
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_intersection.png",
                tileType: TILE.Wall_Intersection
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_single.png",
                tileType: TILE.Wall_Single
            );
            #endregion

            #region wall_entrance
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_wall_entrance.png",
                tileType: TILE.Wall_Entrance
            );
            #endregion

            #region wall_door
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_door_locked.png",
                tileType: TILE.Wall_Door_Locked
            );
            AddTile(
                fileName: "Resources/Textures/tiles/spr_tile_door_unlocked.png",
                tileType: TILE.Wall_Door_Unlocked
            );
            #endregion

            // Calculate the top left of the grid.
            _origin.X = (int)(window.Size.X - GRID_WIDTH * TILE_SIZE);
            _origin.X /= 2;

            _origin.Y = (int)(window.Size.Y - GRID_WIDTH * TILE_SIZE);
            _origin.Y /= 2;

            // Store the column and row information for each node.
            for (int i = 0; i < GRID_WIDTH; i++)
            {
                for (int j = 0; j < GRID_HEIGHT; j++)
                {
                    _grid[i, j] = new Tile();
                    var cell = _grid[i, j];
                    cell.ColumnIndex = i;
                    cell.RowIndex = j;
                }
            }
        }

        /// <summary>
        /// Checks if a given tile is a wall block.
        /// </summary>
        /// <param name="column">The column that the tile is in.</param>
        /// <param name="row">The column that the row is in.</param>
        /// <returns>True if the given tile is a wall tile.</returns>
        private bool IsWall(int column, int row)
        {
            if (TileIsValid(column: column, row: row))
            {
                return _grid[column, row].Type <= TILE.Wall_Intersection;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the given tile index is solid.
        /// </summary>
        /// <param name="column">The tile's column index.</param>
        /// <param name="row">The tile's row index.</param>
        /// <returns>True if the given tile is solid.</returns>
        public bool IsSolid(int column, int row)
        {
            // Check that the tile is valid
            if (TileIsValid(column: column, row: row))
            {
                int tileIndex = (int)_grid[column, row].Type;
                return (tileIndex != (int)TILE.Floor)
                    && (tileIndex != (int)TILE.Floor_Alt)
                    && (tileIndex != (int)TILE.Wall_Door_Unlocked);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the index of a given tile in the 2D game grid.
        /// This also changes the tile sprite, and is how tiles should be changed and set manually.
        /// </summary>
        /// <param name="column">The tile's column index.</param>
        /// <param name="row">The tile's row index.</param>
        /// <param name="tileType">The new index of the tile.</param>
        public void SetTile(int column, int row, TILE tileType)
        {
            // Check that the provided tile index is valid.
            if ((column >= GRID_WIDTH) || (row >= GRID_HEIGHT))
            {
                return;
            }

            // Check that the sprite index is valid.
            if (tileType >= TILE.Count)
            {
                return;
            }

            // Change the tiles sprite to a new index.
            _grid[column, row].Type = tileType;
            _grid[column, row].Sprite = new Sprite(
                texture: TextureManager.GetTexture(textureID: _textureIDs[(int)tileType])
            );
        }

        /// <summary>
        /// Draws the level grid to the provided render window.
        /// </summary>
        /// <param name="window">The render window to draw the level to.</param>
        /// <param name="deltaTime">The time that has elapsed since the last update.</param>
        public void Draw(RenderWindow window, float deltaTime)
        {
            // Draw the level tiles.
            for (int i = 0; i < GRID_WIDTH; i++)
            {
                for (int j = 0; j < GRID_HEIGHT; j++)
                {
                    window.Draw(drawable: _grid[i, j].Sprite);
                }
            }

            // Draw all torches.
            foreach (var torch in _torches)
            {
                torch.Draw(window: window, deltaTime: deltaTime);
            }
        }

        /// <summary>
        /// Gets the index of the given tile.
        /// </summary>
        /// <param name="column">The column index of the tile to check.</param>
        /// <param name="row">The row index of the tile to check.</param>
        /// <returns>The index of the given tile.</returns>
        public TILE GetTileType(int column, int row)
        {
            // Check that the parameters are valid.
            if ((column >= GRID_WIDTH) || (row >= GRID_HEIGHT))
            {
                return TILE.Empty; // Failed
            }

            return _grid[column, row].Type;
        }

        /// <summary>
        /// Loads a level from a text file
        /// </summary>
        /// <param name="fileName">The path to the level file to load.</param>
        /// <returns>True if the level loaded successfully.</returns>
        public bool LoadLevelFromFile(string fileName)
        {
            var reader = new StreamReader(stream: File.OpenRead(path: fileName));
            if (!reader.EndOfStream)
            {
                while (!reader.EndOfStream)
                {
                    for (int j = 0; j < GRID_HEIGHT; j++)
                    {
                        for (int i = 0; i < GRID_WIDTH; i++)
                        {
                            // Get the cell that we're working on.
                            var cell = _grid[i, j];

                            //Read the character. Out of 4 characters we only want 2nd and 3rd.
                            string input = "";

                            int value = reader.Read();

                            input += Convert.ToChar(value: value);

                            if (!(input == "[" || input == "]" || input == "\n" || input == "\r"))
                            {
                                input += Convert.ToChar(value: reader.Read());

                                // Convert string to int.
                                int tileID = int.Parse(s: input);

                                // Set type, sprite and position.
                                cell.Type = (TILE)tileID;
                                cell.Sprite.Texture = new Texture(
                                    copy: TextureManager.GetTexture(textureID: _textureIDs[tileID])
                                );
                                cell.Sprite.Position = new Vector2f(
                                    x: _origin.X + (TILE_SIZE * i),
                                    y: _origin.Y + (TILE_SIZE * j)
                                );

                                // Check for entry/exit nodes.
                                if (cell.Type == TILE.Wall_Door_Locked)
                                {
                                    // Save the location of the exit door.
                                    _doorTileIndices = new Vector2i(x: i, y: 0);
                                }
                            }
                        }
                    }

                    // Create torches at specific locations.
                    Vector2i[] locations = new Vector2i[5];

                    locations[0] = new Vector2i(
                        x: _origin.X + (3 * TILE_SIZE) + (TILE_SIZE / 2),
                        y: _origin.Y + (9 * TILE_SIZE) + (TILE_SIZE / 2)
                    );
                    locations[1] = new Vector2i(
                        x: _origin.X + (7 * TILE_SIZE) + (TILE_SIZE / 2),
                        y: _origin.Y + (7 * TILE_SIZE) + (TILE_SIZE / 2)
                    );
                    locations[2] = new Vector2i(
                        x: _origin.X + (11 * TILE_SIZE) + (TILE_SIZE / 2),
                        y: _origin.Y + (11 * TILE_SIZE) + (TILE_SIZE / 2)
                    );
                    locations[3] = new Vector2i(
                        x: _origin.X + (13 * TILE_SIZE) + (TILE_SIZE / 2),
                        y: _origin.Y + (15 * TILE_SIZE) + (TILE_SIZE / 2)
                    );
                    locations[4] = new Vector2i(
                        x: _origin.X + (15 * TILE_SIZE) + (TILE_SIZE / 2),
                        y: _origin.Y + (3 * TILE_SIZE) + (TILE_SIZE / 2)
                    );

                    // Spawn torches.
                    for (int i = 0; i < 5; i++)
                    {
                        Torch torch =
                            new() { Position = new Vector2f(x: locations[i].X, y: locations[i].Y) };
                        _torches.Add(item: torch);
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the tile at the given position.
        /// </summary>
        /// <param name="position">The coordinates of the position to check.</param>
        /// <returns>A reference to the tile at the given location.</returns>
        public Tile GetTile(Vector2f position)
        {
            // Convert the position to the relative to the level grid.
            position.X -= _origin.X;
            position.Y -= _origin.Y;

            // Convert to a tile position.
            int tileColumn,
                tileRow;

            tileColumn = (int)position.X / TILE_SIZE;
            tileRow = (int)position.Y / TILE_SIZE;

            return _grid[tileColumn, tileRow];
        }

        /// <summary>
        /// Gets the tile at the given position in the level array.
        /// </summary>
        /// <param name="column">The column that the tile is in.</param>
        /// <param name="row">The row that the tile is in.</param>
        /// <returns>A pointer to the tile if valid.</returns>
        public Tile GetTile(int column, int row)
        {
            if (TileIsValid(column: column, row: row))
            {
                return _grid[column, row];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the position of the level grid relative to the window.
        /// </summary>
        /// <returns>The position of the top-left of the level grid.</returns>
        public Vector2f GetPosition()
        {
            return new Vector2f(x: _origin.X, y: _origin.Y);
        }

        /// <summary>
        /// Gets a list of all torches in the level.
        /// </summary>
        /// <returns>A list containing all torches in the level.</returns>
        public List<Torch> GetTorches()
        {
            return _torches;
        }

        /// <summary>
        /// Checks if a given tile is valid.
        /// </summary>
        /// <param name="column">The column that the tile is in.</param>
        /// <param name="row">The column that the row is in.</param>
        /// <returns>True if the tile is valid.</returns>
        public static bool TileIsValid(int column, int row)
        {
            bool validColumn,
                validRow;

            validColumn = (column >= 0) && (column < GRID_WIDTH);
            validRow = (row >= 0) && (row < GRID_HEIGHT);

            return validColumn && validRow;
        }

        /// <summary>
        /// Gets the current floor number.
        /// </summary>
        /// <returns>The current floor.</returns>
        public int GetFloorNumber() => FloorNumber;

        /// <summary>
        /// Gets the current room number.
        /// </summary>
        /// <returns>The current room.</returns>
        public int GetRoomNumber() => RoomNumber;

        /// <summary>
        /// Gets the size of the level in terms of tiles.
        /// </summary>
        /// <returns>The size of the level grid.</returns>
        public static Vector2i GetSize()
        {
            return new(x: GRID_WIDTH, y: GRID_HEIGHT);
        }

        /// <summary>
        /// Spawns a given number of torches in the level.
        /// </summary>
        /// <param name="torchCount">The number of torches to create.</param>
        public static void SpawnTorches(int torchCount) { }

        /// <summary>
        /// Unlocks the door in the level.
        /// </summary>
        public void UnlockDoor()
        {
            SetTile(
                column: _doorTileIndices.X,
                row: _doorTileIndices.Y,
                tileType: TILE.Wall_Door_Unlocked
            );
        }

        /// <param name="column">The column that the tile is in.</param>
        /// <param name="rowIndex">The column that the row is in.</param>
        /// <returns>True if the given tile is a floor tile.</returns>
        public bool IsFloor(int column, int row)
        {
            Tile tile = _grid[column, row];

            return (tile.Type == TILE.Floor) || (tile.Type == TILE.Floor_Alt);
        }

        /// <param name="tile">The tile to check</param>
        /// <returns>True if the given tile is a floor tile.</returns>
        public static bool IsFloor(ref Tile tile)
        {
            return tile.Type == TILE.Floor || tile.Type == TILE.Floor_Alt;
        }

        /// <returns>The size of the tiles in the level.</returns>
        public static int GetTileSize()
        {
            return TILE_SIZE;
        }

        /// <summary>
        /// Adds a tile to the level.
        /// These tiles are essentially sprites with a unique index. Once added, they can be loaded via the LoadLevelFromFile() function by including its index in the level data.
        /// </summary>
        /// <param name="fileName">The path to the sprite resource, relative to the project directory.</param>
        /// <param name="tileType">The type of tile that is being added.</param>
        /// <returns>The index of the tile. This is used when building levels.</returns>
        public int AddTile(string fileName, TILE tileType)
        {
            // Add the texture to the texture manager.
            int textureID = TextureManager.AddTexture(filePath: fileName);

            if (textureID < 0)
            {
                return -1; // Failed
            }
            else
            {
                _textureIDs[(int)tileType] = textureID;
            }

            // Return the ID of the tile.
            return textureID;
        }
    }
}
