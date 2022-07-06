namespace DungeonCrawler
{
    public class Potion : Item
    {
        /// <summary>
        /// The attack value of the potion.
        /// </summary>
        public int AttackValue { get; set; }

        /// <summary>
        /// The defense value of the potion.
        /// </summary>
        public int DefenseValue { get; set; }

        /// <summary>
        /// The stamina value of the potion.
        /// </summary>
        public int StaminaValue { get; set; }

        /// <summary>
        /// The strength value of the potion.
        /// </summary>
        public int StrengthValue { get; set; }

        /// <summary>
        /// The dexterity value of the potion.
        /// </summary>
        public int DexterityValue { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Potion()
        {
            // Load and set sprite.
            SetSprite(
                texture: TextureManager.GetTexture(
                    textureID: TextureManager.AddTexture(
                        filePath: "../Resources/Textures/loot/potions/spr_potion_stamina.png"
                    )
                ),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );

            // Set the item type.
            Type = ITEM.Potion;
        }
    }
}
