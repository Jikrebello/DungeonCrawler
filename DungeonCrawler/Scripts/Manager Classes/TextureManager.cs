using SFML.Graphics;

namespace DungeonCrawler
{
    static class TextureManager
    {
        /// <summary>
        /// A Dictionary of each texture name with its ID.
        /// </summary>
        static Dictionary<string, Tuple<int, Texture>> Textures;

        /// <summary>
        /// The current key value.
        /// </summary>
        static int CurrentID = 0;

        /// <summary>
        /// Adds a texture to the manager and returns its id in the map.
        /// </summary>
        /// <param name="filePath">The path to the image to load.</param>
        /// <returns>The id to the texture created, or the id in the map if it already exists.</returns>
        public static int AddTexture(string filePath)
        {
            // First check if the texture has already been created. If so, return that one.
            if (Textures.ContainsKey(key: filePath))
            {
                Textures[filePath].Item2;
            }
        }

        /// <summary>
        /// Removes a texture from the manager from a given id.
        /// </summary>
        /// <param name="textureID">The id of the texture to be removed.</param>
        public static void RemoveTexture(int textureID) { }

        /// <summary>
        /// Gets a texture from the texture manager from an ID.
        /// </summary>
        /// <param name="textureID">The id of the texture to return.</param>
        /// <returns>A reference to the texture.</returns>
        public static Texture GetTexture(int textureID) { }
    }
}
