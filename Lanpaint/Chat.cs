using System.Collections.Generic;
using System.Linq;
using Lanchat.Core.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint
{
    public class Chat
    {
        private readonly Drawing _drawing;
        private readonly IP2P _network;
        private readonly List<string> _chatHistory = new();
        private string _textInputBuffer;

        public Chat(GameWindow window, Drawing drawing, IP2P network)
        {
            _drawing = drawing;
            _network = network;
            window.TextInput += GameWindowOnTextInput;
        }

        public void Draw()
        {
            var y = 40;
            _chatHistory.ToList().ForEach(x =>
            {
                _drawing.DrawText(x, new Vector2(5, _drawing.Size.Bottom - y), Color.Yellow);
                y += 20;
            });
            
            if (_textInputBuffer != null)
            {
               _drawing.DrawText(_textInputBuffer, 
                   new Vector2(5, _drawing.Size.Bottom - 20), Color.White);
            }
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