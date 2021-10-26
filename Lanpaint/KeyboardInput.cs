using Lanpaint.Elements;
using Microsoft.Xna.Framework.Input;

namespace Lanpaint
{
    public class KeyboardInput
    {
        private readonly DebugLog _debugLog;
        private readonly Chat _chat;
        private readonly PaperSoccer _paperSoccer;
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        public KeyboardInput(Chat chat, DebugLog debugLog, PaperSoccer paperSoccer)
        {
            _chat = chat;
            _paperSoccer = paperSoccer;
            _debugLog = debugLog;
        }

        public void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            if (CheckKey(Keys.F1))
                _chat.Show = !_chat.Show;
            
            else if (CheckKey(Keys.F2))
                _debugLog.Show = !_debugLog.Show;
            
            else if (CheckKey(Keys.F3)) 
                _paperSoccer.Show = !_paperSoccer.Show;
        }

        private bool CheckKey(Keys key)
        {
            return _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);
        }
    }
}