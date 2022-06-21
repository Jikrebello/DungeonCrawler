using SFML.Graphics;
using SFML.Audio;
using SFML.System;
using System.Numerics;

namespace DungeonCrawler
{
    public class Game
    {
        /// <summary>
        /// The main application window.
        /// </summary>
        RenderWindow window;

        /// <summary>
        /// An Array of  the different view the game needs.
        /// </summary>
        View[] views = new View[(int)VIEW.Count];

        /// <summary>
        /// Used in the main game time step.
        /// </summary>
        Clock timestepClock;

        /// <summary>
        /// The default font to be used when drawing text.
        /// </summary>
        Font font;

        /// <summary>
        /// The game state.
        /// </summary>
        GAME_STATE gameState;

        /// <summary>
        /// A vector that holds all items within the level.
        /// </summary>
        // Vector<Item> items = new Vector<Item>();


    }
}
