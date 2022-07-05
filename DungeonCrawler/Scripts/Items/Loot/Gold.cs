namespace DungeonCrawler
{
    public class Gold : Item
    {
        /// <summary>
        /// Gets the amount of gold this pickup has
        /// </summary>
        /// <value>The value of this gold pickup</value>
        public int GoldValue { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Gold()
        {
            // Set the gold value.
            GoldValue = 15;

            // Set the sprite.
            int textureID = TextureManager.AddTexture(
                filePath: "../Resources/Textures/loot/gold/spr_pickup_gold_medium.png"
            );

            SetSprite(
                TextureManager.GetTexture(textureID),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );

            // Set the item type.
            Type = ITEM.Gold;
        }
    }
}
