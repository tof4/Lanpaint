using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lanpaint.Elements
{
    public class DebugLog
    {
        private readonly List<string> _logItems = new();
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;

        public bool Show { get; set; }
        
        public DebugLog(SpriteBatch spriteBatch, SpriteFont font)
        {
            _spriteBatch = spriteBatch;
            _font = font;
        }

        public void AddToLog(string message)
        {
            if (_logItems.Count > 20)
            {
                _logItems.RemoveAt(0);
            }
            _logItems.Add(message);
        }

        public void Draw()
        {
            if (!Show)
            {
                return;
            }
            
            var y = 0;
            _logItems.ToList().ForEach(x =>
            {
                _spriteBatch.DrawString(_font, x, new Vector2(0, y), Color.Yellow);
                y += 20;
            });
        }
    }
}