using Microsoft.Xna.Framework.Input;

namespace Lanpaint
{
    public class KeyboardInput
    {
        private KeyboardState _previousState;
        private KeyboardState _currentState;
        
        public void UpdateState()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }
        
        public bool CheckKey(Keys key)
        {
            return _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);
        }
    }
}