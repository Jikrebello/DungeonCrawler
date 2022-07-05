namespace DungeonCrawler
{
    public class Enemy : Entity
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Enemy()
        {
            var rand = new Random();

            // Set stats.
            Health = rand.Next(minValue: 40, maxValue: 81);
            Attack = rand.Next(minValue: 5, maxValue: 11);
            Defense = rand.Next(minValue: 5, maxValue: 11);
            Strength = rand.Next(minValue: 5, maxValue: 11);
            Dexterity = rand.Next(minValue: 5, maxValue: 11);
            Stamina = rand.Next(minValue: 5, maxValue: 11);

            // Set speed.
            Speed = rand.Next(minValue: 50, maxValue: 151);
        }

        /// <summary>
        /// Applies the given amount of damage to the enemy.
        /// </summary>
        /// <param name="damage">The amount of damage to deal to the enemy.</param>
        public void Damage(int damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Checks if the enemy has taken enough damage that they are now dead.
        /// </summary>
        /// <returns>True if the enemy is dead.</returns>
        public bool IsDead()
        {
            return Health <= 0;
        }
    }
}
