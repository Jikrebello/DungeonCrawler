namespace DungeonCrawler
{
    public class Humanoid : Enemy
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Humanoid()
        {
            // Load textures.
            textureIDs[(int)ANIMATION_STATE.Walk_Up] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_walk_up.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Down] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_walk_down.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Right] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_walk_right.png"
            );
            textureIDs[(int)ANIMATION_STATE.Walk_Left] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_walk_left.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Up] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_idle_up.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Down] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_idle_down.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Right] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_idle_right.png"
            );
            textureIDs[(int)ANIMATION_STATE.Idle_Left] = TextureManager.AddTexture(
                filePath: "../Resources/Textures/enemies/skeleton/spr_skeleton_idle_left.png"
            );

            // Set initial sprite.
            SetSprite(
                TextureManager.GetTexture(textureID: textureIDs[(int)ANIMATION_STATE.Walk_Up]),
                isSmooth: false,
                frames: 8,
                frameSpeed: 12
            );
        }
    }
}
