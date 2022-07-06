namespace DungeonCrawler
{
    public class Heart : Item
    {
        /// <summary>
        /// The amount of health the heart gives.
        /// </summary>
        public int HealthValue { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Heart()
        {
            // Set item sprite.
            SetSprite(
                texture: TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "../Resources/Textures/loot/heart/spr_pickup_heart.png"
                    )
                ),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );

            // Set health value.
            HealthValue = 15;

            // Set item type.
            Type = ITEM.Heart;
        }
    }
}
