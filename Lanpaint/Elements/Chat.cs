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
                _spriteBatch.DrawString(_font, x, new Vector2(5, _size.Bottom - y), Color.White);
                y += 20;
            });

            _spriteBatch.DrawString(_font,
                _textInputBuffer == null
                    ? $"{Program.Config.Nickname}: "
                    : $"{Program.Config.Nickname}: {_textInputBuffer}",
                new Vector2(5, _size.Bottom - 20), Color.White);
        }

        public void MessagingOnMessageReceived(object sender, string e)
        {
            var node = (INode)sender;
            AddMessage(node.User.Nickname, e);
        }

        private void GameWindowOnTextInput(object sender, TextInputEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Enter when !string.IsNullOrWhiteSpace(_textInputBuffer):
                    _network.Broadcast.SendMessage(_textInputBuffer);
                    AddMessage(Program.Config.Nickname, _textInputBuffer);
                    _textInputBuffer = string.Empty;
                    return;
                
                case Keys.Back:
                    if (!string.IsNullOrEmpty(_textInputBuffer))
                    {
                        _textInputBuffer = _textInputBuffer.Remove(_textInputBuffer.Length - 1);
                    }
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

        private void AddMessage(string nickname, string content)
        {
            _chatHistory.Insert(0, $"{nickname}: {content}");
        }
    }
}