namespace DungeonCrawler
{
    public class Slime : Enemy
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Slime()
        {
            // Load Textures.
            textureIDs[(int)ANIMATION_STATE.Walk_Up] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_walk_up.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Down] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_walk_down.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Right] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_walk_right.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Left] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_walk_left.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Up] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_idle_up.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Down] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_idle_down.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Right] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_idle_right.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Left] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/slime/spr_slime_idle_left.png"
            );

            // Set the initial sprite.
            SetSprite(
                texture: TextureManager.GetTexture(
                    textureID: textureIDs[(int)ANIMATION_STATE.Walk_Down]
                ),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );
        }
    }
}
