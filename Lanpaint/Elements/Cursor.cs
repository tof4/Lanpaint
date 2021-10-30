using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint.Elements
{
    public class Cursor
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Color _brushColor;
        private readonly Color _brushBorderColor;
        private readonly Texture2D _cursorTexture;
        private Point _position;
        private bool _erasing;

        public Cursor(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color brushColor)
        {
            _spriteBatch = spriteBatch;
            _brushColor = brushColor;

            _brushBorderColor = _brushColor.R * 0.2126 + _brushColor.G * 0.7152 + _brushColor.B * 0.0722 < 127
                ? Color.White
                : Color.Black;

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
            if (_erasing)
            {
                _spriteBatch.Draw(
                    _cursorTexture,
                    new Rectangle(_position.X - 11, _position.Y - 11, 22, 22),
                    Color.Black);

                _spriteBatch.Draw(
                    _cursorTexture,
                    new Rectangle(_position.X - 10, _position.Y - 10, 20, 20),
                    Color.White);
            }
            else
            {
                _spriteBatch.Draw(
                    _cursorTexture,
                    new Rectangle(_position.X - 4, _position.Y - 4, 8, 8),
                    _brushBorderColor);

                _spriteBatch.Draw(
                    _cursorTexture,
                    new Rectangle(_position.X - 3, _position.Y - 3, 6, 6),
                    _brushColor);
            }
        }
    }
}