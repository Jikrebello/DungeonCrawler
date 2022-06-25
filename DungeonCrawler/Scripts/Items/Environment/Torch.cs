namespace DungeonCrawler
{
    public class Torch : Object
    {
        Random _numberGen;
        int _randNum;

        float _brightness = 1f;

        /// <summary>
        /// The brightness modifier of the torch. This is used to denote flicker.
        /// </summary>
        public float Brightness
        {
            get { return _brightness; }
            set { }
        }

        public Torch()
        {
            // Set the sprite
            int textureID = TextureManager.AddTexture(
                filePath: "../Resources/Textures/spr_torch.png"
            );
            SetSprite(
                texture: TextureManager.GetTexture(textureID: textureID),
                isSmooth: false,
                frames: 5,
                frameSpeed: 12
            );
        }

        public override void Update(float _deltaTime)
        {
            _numberGen = new Random();
            _randNum = _numberGen.Next(minValue: 80, maxValue: 121) / 100;
            Brightness = _randNum;
        }
    }
}
