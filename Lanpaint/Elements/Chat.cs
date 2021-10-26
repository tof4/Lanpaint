using System.Collections.Generic;
using System.Linq;
using Lanchat.Core.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint.Elements
{
    public class Chat
    {
        private readonly List<string> _chatHistory = new();
        private readonly SpriteFont _font;
        private readonly IP2P _network;
        private readonly Rectangle _size;
        private readonly SpriteBatch _spriteBatch;
        private string _textInputBuffer;

        public Chat(
            SpriteBatch spriteBatch,
            GameWindow window,
            IP2P network,
            SpriteFont font,
            Rectangle size)
        {
            _spriteBatch = spriteBatch;
            _network = network;
            _font = font;
            _size = size;
            window.TextInput += GameWindowOnTextInput;
        }

        public void Draw()
        {
            var y = 40;
            _chatHistory.ToList().ForEach(x =>
            {
                _spriteBatch.DrawString(_font, x, new Vector2(5, _size.Bottom - y), Color.Yellow);
                y += 20;
            });

            if (_textInputBuffer != null)
                _spriteBatch.DrawString(_font, _textInputBuffer, new Vector2(5, _size.Bottom - 20), Color.White);
        }

        public void MessagingOnMessageReceived(object sender, string e)
        {
            _chatHistory.Insert(0, e);
        }

        private void GameWindowOnTextInput(object sender, TextInputEventArgs e)
        {
            if (e.Key != Keys.Enter)
            {
                _textInputBuffer += e.Character;
                return;
            }

            _network.Broadcast.SendMessage(_textInputBuffer);
            _chatHistory.Insert(0, _textInputBuffer);
            _textInputBuffer = string.Empty;
        }
    }
}