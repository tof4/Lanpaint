using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint.Elements
{
    public class DebugLog
    {
        private readonly SpriteFont _font;
        private readonly Rectangle _size;
        private readonly List<string> _logItems = new();
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _background;

        public DebugLog(
            SpriteBatch spriteBatch, 
            SpriteFont font,
            GraphicsDevice graphicsDevice, 
            Rectangle size)
        {
            _spriteBatch = spriteBatch;
            _font = font;
            _size = size;
            _background = new Texture2D(graphicsDevice, 1, 1);
            _background.SetData(new[] { Color.White });
        }

        public bool Show { get; set; }

        public void AddToLog(string message)
        {
            if (_logItems.Count == 20) _logItems.RemoveAt(0);
            _logItems.Add(message);
        }

        public void Draw()
        {
            if (!Show) return;
            
            var y = 0;
            _spriteBatch.Draw(_background, new Rectangle(0, y, _size.Width, 5), new Color(Color.Black, 0.5f));
            y = 5;
            _logItems.ToList().ForEach(x =>
            {
                _spriteBatch.Draw(_background, new Rectangle(0, y, _size.Width, 20), new Color(Color.Black, 0.5f));
                _spriteBatch.DrawString(_font, x, new Vector2(5, y), Color.White);
                y += 20;
            });
        }
    }
}