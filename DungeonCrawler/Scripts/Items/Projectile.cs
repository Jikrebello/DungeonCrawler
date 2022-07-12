using SFML.Graphics;
using SFML.System;

namespace DungeonCrawler
{
    public class Projectile : Object
    {
        /// <summary>
        /// The velocity of the projectile.
        /// </summary>
        Vector2f _velocity;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="texture">The texture of the projectile.</param>
        /// <param name="origin">The location that the projectile should be created at.</param>
        /// <param name="screenCenter">The center of the screen. Used to calculate direction.</param>
        /// <param name="target">The target location of the projectile.</param>
        public Projectile(Texture texture, Vector2f origin, Vector2f screenCenter, Vector2f target)
        {
            // Create the sprite.
            SetSprite(texture: texture, isSmooth: false);

            // Set the sprite position.
            Position = origin;

            // Calculate the velocity of the object.
            _velocity = target - screenCenter;

            float length = (float)
                Math.Sqrt(
                    d: Convert.ToDouble(value: _velocity.X * _velocity.X)
                        + Convert.ToDouble(value: _velocity.Y * _velocity.Y)
                );
            _velocity.X /= length;
            _velocity.Y /= length;
        }

        /// <summary>
        /// Update the projectile.
        /// </summary>
        /// <param name="deltaTime">The time in seconds since the last update.</param>
        public override void Update(float deltaTime)
        {
            // Update rotation.
            Sprite.Rotation += 400f * deltaTime;

            // Update position.
            Sprite.Position = new Vector2f(
                x: Sprite.Position.X + (_velocity.X * (500 * deltaTime)),
                y: Sprite.Position.Y + (_velocity.Y * (500 * deltaTime))
            );
            Position = Sprite.Position;
        }
    }
}
