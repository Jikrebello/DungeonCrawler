namespace DungeonCrawler
{
    public class Gem : Item
    {
        public int ScoreValue { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Gem()
        {
            // Set the sprite.
            SetSprite(
                TextureManager.GetTexture(
                    TextureManager.AddTexture(
                        filePath: "../Resources/Textures/loot/gem/spr_pickup_gem.png"
                    )
                ),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );

            // Set the value of the gem.
            ScoreValue = 50;

            // Set the item type.
            Type = ITEM.Gem;
        }
    }
}
