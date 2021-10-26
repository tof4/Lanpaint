using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint.Elements
{
    public class PaperSoccer
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _texture;

        public PaperSoccer(SpriteBatch spriteBatch, Texture2D texture)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
        }

        public bool Show { get; set; }

        public void Draw()
        {
            if (Show) _spriteBatch.Draw(_texture, new Vector2(300, 2), Color.White);
        }
    }
}