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
        private readonly Texture2D _background;
        private string _textInputBuffer;

        public bool Show { get; set; } = true;

        public Chat(
            SpriteBatch spriteBatch,
            GameWindow window,
            IP2P network,
            SpriteFont font,
            Rectangle size,
            GraphicsDevice graphicsDevice)
        {
            _spriteBatch = spriteBatch;
            _network = network;
            _font = font;
            _size = size;
            _background = new Texture2D(graphicsDevice, 1, 1);
            _background.SetData(new[] { Color.White });
            window.TextInput += GameWindowOnTextInput;

            _chatHistory.Add("F1 - Chat");
            _chatHistory.Add("F2 - Paper soccer board");
            _chatHistory.Add("F3 - Debug log");
            _chatHistory.Add("Left mouse button - draw");
            _chatHistory.Add("Right mouse button - erase");
        }

        public void Draw()
        {
            if (!Show) return;

            _spriteBatch.Draw(_background, new Rectangle(0, 295, _size.Width, 305), new Color(Color.Black, 0.5f));

            var y = 40;
            Enumerable.Reverse(_chatHistory).ToList().ForEach(x =>
            {
                _spriteBatch.DrawString(_font, x, new Vector2(5, _size.Bottom - y), Color.White);
                y += 20;
            });
            
            _spriteBatch.DrawString(_font,
                _textInputBuffer == null
                    ? $"{Program.Storage.Config.Nickname}: "
                    : $"{Program.Storage.Config.Nickname}: {_textInputBuffer}",
                new Vector2(5, _size.Bottom - 20), Color.White);
        }
        
        public void AddMessage(string nickname, string content)
        {
            if (_chatHistory.Count == 14) _chatHistory.RemoveAt(0);
            _chatHistory.Add($"{nickname}: {content}");
        }
        
        private void GameWindowOnTextInput(object sender, TextInputEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Enter when !string.IsNullOrWhiteSpace(_textInputBuffer):
                    _network.Broadcast.SendMessage(_textInputBuffer);
                    AddMessage(Program.Storage.Config.Nickname, _textInputBuffer);
                    _textInputBuffer = string.Empty;
                    return;

                case Keys.Back:
                    if (!string.IsNullOrEmpty(_textInputBuffer))
                        _textInputBuffer = _textInputBuffer.Remove(_textInputBuffer.Length - 1);
                    return;

                case Keys.Tab:
                    return;

                case Keys.Delete:
                    return;

                default:
                    _textInputBuffer += e.Character;
                    break;
            }
        }
    }
}