using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint
{
    public class Drawing
    {
        public SpriteBatch SpriteBatch { get; }
        public SpriteFont Font { get; }
        public Rectangle Size { get; }

        public Drawing(SpriteBatch spriteBatch, SpriteFont font, Rectangle size)
        {
            SpriteBatch = spriteBatch;
            Font = font;
            Size = size;
        }

        public void DrawText(string text, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(Font, text, position, color);
        }
    }
}