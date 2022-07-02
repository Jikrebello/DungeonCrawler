using SFML.Graphics;
using SFML.System;

namespace DungeonCrawler
{
    public class Object
    {
        /// <summary>
        /// The animation speed of the image if applicable.
        /// Value is frames per second.
        /// </summary>
        int _animationSpeed;

        /// <summary>
        /// The current frame of the sprite.
        /// </summary>
        int _currentFrame;

        /// <summary>
        /// The width of each frame of the animated sprite if applicable.
        /// </summary>
        int _frameWidth;

        /// <summary>
        /// The height of each frame of the animated sprite if applicable.
        /// </summary>
        int _frameHeight;

        /// <summary>
        /// An aggregate of the time passed between draw calls.
        /// </summary>
        float _deltaTime;

        /// <summary>
        /// Used to determine if the given sprite is animated.
        /// </summary>
        bool _animated;

        /// <summary>
        /// Sets the animation state of the object.
        /// </summary>
        /// <param name="animated">The new animation state of the object.</param>
        /// <returns>The current animation state of the object.</returns>
        public bool Animated
        {
            get { return _animated; }
            set
            {
                _animated = value;
                if (value)
                {
                    _currentFrame = 0;
                }
                else
                {
                    // set the texture of the rect of the first frame
                    sprite.TextureRect = new IntRect(
                        left: 0,
                        top: 0,
                        width: _frameWidth,
                        height: _frameHeight
                    );
                }
            }
        }

        /// <summary>
        /// The total number of frames the sprite has.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// The position of the object in the game window.
        /// </summary>
        Vector2f _position;

        /// <summary>
        /// The position of the object in the game window.
        /// </summary>
        /// <param name="position">Sets the position of the object.</param>
        /// <returns>Returns the position of the object.</returns>
        public Vector2f Position
        {
            get { return _position; }
            set
            {
                _position.X = value.X;
                _position.Y = value.Y;
                sprite.Position = new Vector2f(x: value.X, y: value.Y);
            }
        }

        /// <summary>
        /// The object's sprite.
        /// </summary>
        internal Sprite sprite;

        /// <summary>
        /// Creates and sets the object sprite.
        /// This function takes the location to a resource, and from that create a texture and sprite.
        /// You can optionally specify animation properties. If set the frame dimensions will be calculated automatically.
        /// If left blank, the whole resource will be used.
        /// </summary>
        /// <param name="texture">The path to the resource that you wish to load, relative to the project.</param>
        /// <param name="frames">The number of frames in the sprite. Defaults to 1.</param>
        /// <param name="frameSpeed">The speed that the animation plays at. Defaults to 0.</param>
        /// <returns></returns>
        internal bool SetSprite(
            Texture texture,
            bool isSmooth,
            int frames = 1,
            int frameSpeed = 0
        )
        {
            // Create a sprite from the loaded texture.
            sprite.Texture = texture;

            // Set animation speed.
            _animationSpeed = frameSpeed;

            // Store the number of frames.
            FrameCount = frames;

            // Calculate frame dimensions.
            Vector2u textureSize = sprite.Texture.Size;
            _frameWidth = Convert.ToInt32(value: textureSize.X) / FrameCount;
            _frameHeight = Convert.ToInt32(value: textureSize.Y);

            // Check if animated or static
            if (frames > 1)
            {
                // Set sprite as animated
                _animated = true;

                // Set the texture rect of the first frame
                sprite.TextureRect = new IntRect(
                    left: 0,
                    top: 0,
                    width: _frameWidth,
                    height: _frameHeight
                );
            }
            else
            {
                // Set the sprite as non animated
                _animated = false;
            }

            // Set up the origin of the sprite
            sprite.Origin = new Vector2f(x: _frameWidth / 2f, y: _frameHeight / 2f);

            return true;
        }

        internal Sprite GetSprite()
        {
            return sprite;
        }

        /// <summary>
        ///Default constructor.
        /// </summary>
        public Object()
        {
            _position = new Vector2f(x: 0f, y: 0f);
            _animationSpeed = 0;
            _animated = false;
            FrameCount = 0;
            _currentFrame = 0;
            _frameWidth = 0;
            _frameHeight = 0;
            _deltaTime = 0f;
        }

        /// <summary>
        /// Updates the game object. Called once per tick.
        /// This is a pure virtual function, and must be implemented by extending classes.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last tick in Milliseconds.</param>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// Draws the object to the screen at its current position.
        /// </summary>
        /// <param name="window">The render window to draw the object to.</param>
        /// <param name="deltaTime">The time, in MS, since the last draw call.</param>
        public virtual void Draw(RenderWindow window, float deltaTime)
        {
            // Check if the sprite is animated
            if (_animated)
            {
                // add the elapsed time since the last draw call to the aggregate
                _deltaTime += deltaTime;

                if (_deltaTime >= (1 / _animationSpeed))
                {
                    NextFrame();
                    _deltaTime = 0;
                }
            }
            window.Draw(drawable: sprite);
        }

        /// <summary>
        /// Advances the sprite by a frame.
        /// </summary>
        void NextFrame()
        {
            // check if we reached the last frame
            if (_currentFrame == (FrameCount - 1))
            {
                _currentFrame = 0;
            }
            else
            {
                _currentFrame++;
            }

            // update the texture rect
            sprite.TextureRect = new IntRect(
                left: _frameWidth * _currentFrame,
                top: 0,
                width: _frameWidth,
                height: _frameHeight
            );
        }
    }
}
