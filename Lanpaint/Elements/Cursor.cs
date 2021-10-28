using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint.Elements
{
    public class Cursor
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Color _brushColor;
        private readonly Texture2D _cursorTexture;
        private Point _position;
        private bool _erasing;

        public Cursor(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color brushColor)
        {
            _spriteBatch = spriteBatch;
            _brushColor = brushColor;
            _cursorTexture = new Texture2D(graphicsDevice, 1, 1);
            _cursorTexture.SetData(new[] { Color.White });
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            _position = mouseState.Position;
            _erasing = mouseState.RightButton == ButtonState.Pressed;
        }

        public void Draw()
        {
            Rectangle rect;
            Color color;
            
            if (_erasing)
            {
                rect = new Rectangle(_position.X - 10, _position.Y - 10, 20, 20);
                color = Color.Black;
            }
            else
            {
                rect = new Rectangle(_position.X - 3, _position.Y - 3, 6, 6);
                color = _brushColor;
            }
            
            _spriteBatch.Draw(_cursorTexture, rect, color);

        }
    }
}