using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint.Elements
{
    public class Board
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _texture;

        public bool Show { get; set; }
        
        public Board(SpriteBatch spriteBatch, Texture2D texture)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
        }
        
        public void Draw()
        {
            if (!Show)
            {
                return;
            }
            

            _spriteBatch.Draw(_texture, new Vector2(298, 0), null, Color.White);
        }
    }
}