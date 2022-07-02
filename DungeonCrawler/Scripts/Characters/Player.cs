using SFML.Graphics;
using SFML.System;

namespace DungeonCrawler
{
    public class Player : Entity
    {
        /// <summary>
        /// The time since the player's last attack.
        /// </summary>
        float _attackDelta;

        /// <summary>
        /// The time since the player last took damage.
        /// </summary>
        float _damageDelta;

        /// <summary>
        /// The time since the last mana regeneration.
        /// </summary>
        float _manaDelta;

        /// <summary>
        /// The sprite for the player's aim cross-hair.
        /// </summary>
        public Sprite AimSprite { get; }

        /// <summary>
        /// Is the player attacking.
        /// </summary>
        public bool IsAttacking { get; set; }

        /// <summary>
        /// Can the player take damage.
        /// </summary>
        public bool CanTakeDamage { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Player() { }

        /// <summary>
        /// Checks if the given movement will result in a collision.
        /// </summary>
        /// <param name="movement">The movement to check.</param>
        /// <param name="level">A reference to the level object.</param>
        /// <returns>True if the given movement will result in a collision.</returns>
        // bool CausesCollision(Vector2f movement, Level level) { }

        /// <summary>
        /// Updates the player object.
        /// The main purpose of this function is to update the players position.
        /// </summary>
        /// <param name="deltaTime">The time, in MS, since the last game tick.</param>
        /// <param name="level">A reference to the level object.</param>
        public void Update(float deltaTime, ref Level level) { }

        /// <summary>
        /// Apply the given amount of damage to the player.
        /// </summary>
        /// <param name="damage">The amount of damage to deal to the player.</param>
        public void Damage(int damage) { }
    }
}
