using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DungeonCrawler
{
    public class Player : Entity
    {
        /// <summary>
        /// The time since the player's last attack.
        /// </summary>
        float _attackDelta = 0f;

        /// <summary>
        /// The time since the player last took damage.
        /// </summary>
        float _damageDelta = 0f;

        /// <summary>
        /// The time since the last mana regeneration.
        /// </summary>
        float _manaDelta = 0f;

        /// <summary>
        /// The sprite for the player's aim cross-hair.
        /// </summary>
        public Sprite AimSprite { get; }

        bool _isAttacking;

        /// <summary>
        /// Checks if the player is attacking.
        /// </summary>
        public bool IsAttacking()
        {
            if (_isAttacking)
            {
                _isAttacking = false;
                _attackDelta = 0f;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Can the player take damage.
        /// </summary>
        public bool CanTakeDamage { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Player()
        {
            CanTakeDamage = true;

            AimSprite = new Sprite();

            // Load textures
            textureIDs[(int)ANIMATION_STATE.Walk_Up] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_walk_up.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Down] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_walk_down.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Right] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_walk_right.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Left] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_walk_left.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Up] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_idle_up.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Down] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_idle_down.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Right] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_idle_right.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Left] = TextureManager.AddTexture(
                filePath: "./Resources/Textures/players/warrior/spr_warrior_idle_left.png"
            );

            // Set the initial sprite
            SetSprite(
                texture: TextureManager.GetTexture(
                    textureID: textureIDs[(int)ANIMATION_STATE.Walk_Up]
                ),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );
            currentTextureIndex = (int)ANIMATION_STATE.Walk_Up;
            Sprite.Origin = new Vector2f(x: 13f, y: 18f);

            // Create the players aim-sprite
            int textureID = TextureManager.AddTexture(
                filePath: "./Resources/Textures/ui/spr_aim.png"
            );
            AimSprite.Texture = new Texture(copy: TextureManager.GetTexture(textureID: textureID));
            AimSprite.Origin = new Vector2f(x: 16.5f, y: 16.5f);
            AimSprite.Scale = new Vector2f(x: 2f, y: 2f);

            // Set the stats
            Health = MaxHealth = 100;
            Mana = MaxMana = 50;
            Speed = 200;

            Attack = 10;
            Defense = 10;
            Strength = 10;
            Dexterity = 10;
            Stamina = 10;
        }

        /// <summary>
        /// Checks if the given movement will result in a collision.
        /// </summary>
        /// <param name="movement">The movement to check.</param>
        /// <param name="level">A reference to the level object.</param>
        /// <returns>True if the given movement will result in a collision.</returns>
        bool CausesCollision(Vector2f movement, Level level)
        {
            // Get the tiles that the four corners other players are overlapping with.
            Tile[] overlappingTiles = new Tile[4];

            Vector2f newPosition = Position + movement;

            // Top left.
            overlappingTiles[0] = level.GetTile(
                position: new Vector2f(x: newPosition.X - 14f, y: newPosition.Y - 14f)
            );

            // Top right.
            overlappingTiles[1] = level.GetTile(
                position: new Vector2f(x: newPosition.X + 14f, y: newPosition.Y - 14f)
            );

            // Bottom left.
            overlappingTiles[2] = level.GetTile(
                position: new Vector2f(x: newPosition.X - 14f, y: newPosition.Y + 14f)
            );

            // Bottom right.
            overlappingTiles[3] = level.GetTile(
                position: new Vector2f(x: newPosition.X + 14f, y: newPosition.Y + 14f)
            );

            // If any of the overlapping tiles are solid there was a collision.
            for (int i = 0; i < 4; i++)
            {
                if (
                    level.IsSolid(
                        column: overlappingTiles[i].ColumnIndex,
                        row: overlappingTiles[i].RowIndex
                    )
                )
                {
                    return true;
                }
            }

            // If we've not returned yet no collisions were found.
            return false;
        }

        /// <summary>
        /// Updates the player object.
        /// The main purpose of this function is to update the players position.
        /// </summary>
        /// <param name="deltaTime">The time, in MS, since the last game tick.</param>
        /// <param name="level">A reference to the level object.</param>
        public void Update(float deltaTime, ref Level level)
        {
            // Calculate the movement speed based on the deltaTime since the last update.
            Vector2f movementSpeed = new();
            Vector2f previousPosition = Position;

            // Calculate where the current movement will put us.
            ANIMATION_STATE animState = (ANIMATION_STATE)currentTextureIndex;

            // Handle Input horizontally.
            if (Input.IsKeyPressed(keycode: KEY.Key_Left))
            {
                // Set movement speed.
                movementSpeed.X = -Speed * deltaTime;

                // Chose animation state.
                animState = ANIMATION_STATE.Walk_Left;
            }
            else if (Input.IsKeyPressed(keycode: KEY.Key_Right))
            {
                // Set movement speed.
                movementSpeed.X = Speed * deltaTime;

                // Chose animation state.
                animState = ANIMATION_STATE.Walk_Right;
            }

            // Handle Input vertically.
            if (Input.IsKeyPressed(keycode: KEY.Key_Up))
            {
                // Set movement speed.
                movementSpeed.Y = -Speed * deltaTime;

                // Chose animation state.
                animState = ANIMATION_STATE.Walk_Up;
            }
            else if (Input.IsKeyPressed(keycode: KEY.Key_Down))
            {
                // Set movement speed.
                movementSpeed.Y = Speed * deltaTime;

                // Chose animation state.
                animState = ANIMATION_STATE.Walk_Down;
            }

            // Calculate horizontal movement.
            if (CausesCollision(movement: new Vector2f(x: movementSpeed.X, y: 0f), level: level))
            {
                Position = previousPosition;
            }
            else
            {
                Position += new Vector2f(x: movementSpeed.X, y: Position.Y);
            }

            // Calculate vertical movement.
            if (CausesCollision(movement: new Vector2f(x: 0f, y: movementSpeed.Y), level: level))
            {
                Position = previousPosition;
            }
            else
            {
                Position += new Vector2f(x: Position.X, y: movementSpeed.Y);
            }

            // Update the sprite position.
            Sprite.Position = Position;

            // Set the Sprite.
            if (currentTextureIndex != (int)animState)
            {
                currentTextureIndex = (int)animState;
                Sprite.Texture = TextureManager.GetTexture(
                    textureID: textureIDs[currentTextureIndex]
                );
            }

            // Set the animation sprite.
            if ((movementSpeed.X == 0) && (movementSpeed.Y == 0))
            {
                // The character is still.
                if (Animated)
                {
                    // Update sprite to idle version.
                    // In our enum we have 4 walking sprites followed by 4 idle sprites.
                    // Given this, we can simply add 4 to a walking sprite to get its idle counterpart.
                    currentTextureIndex += 4;
                    Sprite.Texture = TextureManager.GetTexture(
                        textureID: textureIDs[currentTextureIndex]
                    );

                    // Stop movement animations.
                    Animated = false;
                }
            }
            else
            {
                // The character is moving.
                if (!Animated)
                {
                    // Update sprite to walking version.
                    currentTextureIndex -= 4;
                    Sprite.Texture = TextureManager.GetTexture(
                        textureID: textureIDs[currentTextureIndex]
                    );

                    // Start movement animations.
                    Animated = true;
                }
            }

            // Calculate aim based on mouse.
            Vector2i mousePosition = Mouse.GetPosition();
            AimSprite.Position = new Vector2f(x: mousePosition.X, y: mousePosition.Y);

            // Check if shooting.
            if ((_attackDelta += deltaTime) > 0.25f)
            {
                if (Input.IsKeyPressed(keycode: KEY.Key_Attack))
                {
                    // Mark player as attacking.
                    _isAttacking = true;
                }
            }

            // Determine if the player can take damage.
            if (!CanTakeDamage)
            {
                if ((_damageDelta += deltaTime) > 1f)
                {
                    CanTakeDamage = true;
                    _damageDelta = 0f;
                }
            }

            // Increase player mana.
            if ((_manaDelta += deltaTime) > 0.20f)
            {
                if (Mana < MaxMana)
                {
                    Mana += 1;
                }

                _manaDelta = 0f;
            }
        }

        /// <summary>
        /// Apply the given amount of damage to the player.
        /// </summary>
        /// <param name="damage">The amount of damage to deal to the player.</param>
        public void Damage(int damage)
        {
            Health -= damage;

            if (Health < 0)
            {
                Health = 0;
            }

            CanTakeDamage = false;
        }
    }
}
