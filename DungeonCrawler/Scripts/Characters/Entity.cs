using SFML.Graphics;
using SFML.System;

namespace DungeonCrawler
{
    public class Entity : Object
    {
        /// <summary>
        /// An array of all texture IDs.
        /// </summary>
        public int[] textureIDs = new int[(int)ANIMATION_STATE.Count];

        /// <summary>
        /// The index of the current texture.
        /// </summary>
        public int currentTextureIndex;

        int _health;

        /// <summary>
        /// The entities current health
        /// </summary>
        public int Health
        {
            get { return _health; }
            set => _health = value;
        }

        int _maxHealth;

        /// <summary>
        /// The entities maximum health.
        /// </summary>
        public int MaxHealth
        {
            get { return _maxHealth; }
            set => _maxHealth = value;
        }

        int _mana;

        /// <summary>
        /// The entities current mana.
        /// </summary>
        public int Mana
        {
            get { return _mana; }
            set => _mana = value;
        }

        int _maxMana;

        /// <summary>
        /// The entities maximum mana.
        /// </summary>
        public int MaxMana
        {
            get { return _maxMana; }
            set => _maxMana = value;
        }

        int _attack;

        /// <summary>
        /// The entities attack stat. Effects damage dealt.
        /// </summary>
        public int Attack
        {
            get { return _attack; }
            set => _attack = value;
        }

        int _defense;

        /// <summary>
        /// The entities defense stat. Effects damage taken.
        /// </summary>
        public int Defense
        {
            get { return _defense; }
            set => _defense = value;
        }

        int _strength;

        /// <summary>
        /// The entities strength. Effects damage dealt.
        /// </summary>
        public int Strength
        {
            get { return _strength; }
            set => _strength = value;
        }

        int _dexterity;

        /// <summary>
        /// The entities dexterity. Effects movement speed.
        /// </summary>
        public int Dexterity
        {
            get { return _dexterity; }
            set => _dexterity = value;
        }

        int _stamina;

        /// <summary>
        /// The entities stamina. Effects health.
        /// </summary>
        public int Stamina
        {
            get { return _stamina; }
            set => _stamina = value;
        }

        int _speed;

        /// <summary>
        /// The entities movement speed.
        /// </summary>
        public int Speed
        {
            get { return _speed; }
            set => _speed = value;
        }

        Vector2f _velocity;

        /// <summary>
        /// The entities current velocity.
        /// </summary>
        public Vector2f Velocity
        {
            get { return _velocity; }
            set => _velocity = value;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Entity()
        {
            currentTextureIndex = (int)ANIMATION_STATE.Walk_Down;
            _health = 0;
            _maxHealth = 0;
            _mana = 0;
            _maxMana = 0;
            _attack = 0;
            _defense = 0;
            _strength = 0;
            _dexterity = 0;
            _stamina = 0;
            _speed = 0;
            _velocity = new Vector2f(x: 0f, y: 0f);
        }

        /// <summary>
        /// Override the default Object.Update() function
        /// </summary>
        /// <param name="deltaTime">The time that has elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            ANIMATION_STATE animState = (ANIMATION_STATE)currentTextureIndex;

            // Choose animation state.
            if ((_velocity.X != 0) || (_velocity.Y != 0))
            {
                if (Math.Abs(value: _velocity.X) > Math.Abs(value: _velocity.Y))
                {
                    if (_velocity.X <= 0)
                    {
                        animState = ANIMATION_STATE.Walk_Left;
                    }
                    else
                    {
                        animState = ANIMATION_STATE.Walk_Right;
                    }
                }
                else
                {
                    if (_velocity.Y <= 0)
                    {
                        animState = ANIMATION_STATE.Walk_Up;
                    }
                    else
                    {
                        animState = ANIMATION_STATE.Walk_Down;
                    }
                }
            }

            // Set animation speed.
            if ((_velocity.X == 0) && (_velocity.Y == 0))
            {
                // The character is still.
                if (Animated)
                {
                    // Update sprite to idle version.
                    currentTextureIndex += 4;

                    // Stop the movement animations.
                    Animated = false;
                }
            }
            else
            {
                // The Character is moving.
                if (!Animated)
                {
                    // Update sprite to walking version.
                    currentTextureIndex -= 4;

                    // Start movement animations.
                    Animated = true;
                }
            }

            // Set the Sprite.
            if (currentTextureIndex != (int)animState)
            {
                currentTextureIndex = (int)animState;
                Sprite.Texture = new Texture(
                    copy: TextureManager.GetTexture(textureID: textureIDs[currentTextureIndex])
                );
            }
        }
    }
}
