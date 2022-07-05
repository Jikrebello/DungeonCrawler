namespace DungeonCrawler
{
    public class Key : Item
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Key()
        {
            // Set the sprite.
            int textureID = TextureManager.AddTexture(
                filePath: "../Resources/Textures/loot/key/spr_pickup_key.png"
            );
            SetSprite(
                TextureManager.GetTexture(textureID),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );

            // Set the item name.
            Name = "Key";

            // Set the item type.
            Type = ITEM.Key;
        }
    }
}
