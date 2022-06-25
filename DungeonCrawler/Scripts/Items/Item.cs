using SFML.Graphics;
using SFML.System;

namespace DungeonCrawler
{
    public class Item : Object
    {
        string _name = "";

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                // Store a new name.
                _name = value;

                // Set the item name.
                text = new Text(str: value, font: font, characterSize: 12);

                // Store the text offset needed when drawing.
                textOffset = new Vector2f(
                    x: text.GetLocalBounds().Width / 2,
                    y: text.GetLocalBounds().Height / 2
                );
            }
        }

        /// <summary>
        /// The type of item.
        /// </summary>
        public ITEM Type;

        /// <summary>
        /// A text object storing the name of the item.
        /// </summary>
        public Text text;

        /// <summary>
        /// A font object storing the name of the item.
        /// </summary>
        public Font font;

        /// <summary>
        /// A font object storing the name of the item.
        /// </summary>
        public Vector2f textOffset = new(x: 0f, y: 0f);

        public Item()
        {
            font = new Font(filename: "../Resources/fonts/ADDSBP__.TTF");
        }

        /// <summary>
        /// Draws the item name to screen if it has one. The drawing of the object is done in the parent function which is called.
        /// </summary>
        /// <param name="_window">The render window to draw to.</param>
        /// <param name="_deltaTime">The font to use when drawing the item name.</param>
        public override void Draw(RenderWindow _window, float _deltaTime)
        {
            base.Draw(_window: _window, _deltaTime: _deltaTime);

            // Draw the item Name.
            text.Position = new Vector2f(
                x: Position.X - textOffset.X,
                y: (Position.Y - 30f) - textOffset.Y
            );

            _window.Draw(drawable: text);
        }
    }
}
