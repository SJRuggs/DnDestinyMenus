using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DnDestiny
{
    class EditableField
    {
        private Vector2 factor;
        private Rectangle startPosition;
        private bool toggle;
        private Texture2D activeT;
        private Texture2D inactiveT;

        public EditableField(Vector2 factor, Rectangle startPosition)
        {
            this.factor = factor;
            this.startPosition = startPosition;
            toggle = false;
        }
        public Rectangle Position(MouseState mState)
        {
            return new Rectangle(startPosition.X - (int)((mState.X - Game1.screenWidth / 2) * factor.X),
            startPosition.Y - (int)((mState.Y - Game1.screenHeight / 2) * factor.Y), startPosition.Width, startPosition.Height);
        }
        public bool Toggle { get { return toggle; } set { toggle = value; } }
        public Texture2D ActiveT
        { set { activeT = value; } }
        public Texture2D InactiveT
        { set { inactiveT = value; } }

        public void Draw(SpriteBatch _spriteBatch, MouseState mState)
        {
            if (toggle)
                _spriteBatch.Draw(activeT, Position(mState), Color.White);
            else
                _spriteBatch.Draw(inactiveT, Position(mState), Color.White);
        }

        public void DetectToggle(MouseState mState, MouseState prevMState)
        {
            if (Position(mState).Contains(mState.Position) && mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released) toggle = !toggle;
        }
    }
}
