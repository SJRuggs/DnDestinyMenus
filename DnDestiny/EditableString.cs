using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DnDestiny
{
    class EditableString
    {
        private Vector2 factor;
        private SpriteFont font;
        private Rectangle startPosition;
        private string value;
        private int backSpaceFrames;
        private bool toggle;

        public EditableString(Vector2 factor, SpriteFont font, Rectangle startPosition, string startingValue)
        {
            this.factor = factor;
            this.font = font;
            this.startPosition = startPosition;
            this.value = startingValue;
            toggle = false;
        }
        public Rectangle Position(MouseState mState)
        {
            return new Rectangle(startPosition.X - (int)((mState.X - Game1.screenWidth / 2) * factor.X),
            startPosition.Y - (int)((mState.Y - Game1.screenHeight / 2) * factor.Y), startPosition.Width, startPosition.Height);
        }

        public string Value { get { return value; } set { this.value = value; } }
        public bool Toggle { get { return toggle; } set { toggle = value; } }

        public void Draw(SpriteBatch _spriteBatch, MouseState mState)
        {
            _spriteBatch.DrawString(font, value, new Vector2(
                Position(mState).X + Position(mState).Width / 2 - font.MeasureString(value).X / 2,
                Position(mState).Y + Position(mState).Height / 2 - font.MeasureString(value).Y / 2),
                Color.White);
        }

        public bool CalcInput(MouseState ms, MouseState pms, KeyboardState kb, KeyboardState pkb)
        {
            if (Position(ms).Contains(ms.Position) && ms.LeftButton == ButtonState.Pressed && pms.LeftButton == ButtonState.Released)
                toggle = true;
            if (toggle)
            {
                if (kb.IsKeyDown(Keys.Back))
                {
                    if (kb.IsKeyDown(Keys.LeftControl) || kb.IsKeyDown(Keys.LeftControl)) { value = ""; }
                    else
                    {
                        backSpaceFrames++;
                        if (!pkb.IsKeyDown(Keys.Back) || backSpaceFrames > 30)
                            if (value.Length > 0)
                                value = value.Substring(0, value.Length - 1);
                    }
                }
                else
                {
                    backSpaceFrames = 0;
                }
                foreach (Keys key in kb.GetPressedKeys())
                {
                    if (!pkb.IsKeyDown(key))
                        switch (key)
                        {
                            case Keys.NumPad1:
                                value = value + "1";
                                break;
                            case Keys.NumPad2:
                                value = value + "2";
                                break;
                            case Keys.NumPad3:
                                value = value + "3";
                                break;
                            case Keys.NumPad4:
                                value = value + "4";
                                break;
                            case Keys.NumPad5:
                                value = value + "5";
                                break;
                            case Keys.NumPad6:
                                value = value + "6";
                                break;
                            case Keys.NumPad7:
                                value = value + "7";
                                break;
                            case Keys.NumPad8:
                                value = value + "8";
                                break;
                            case Keys.NumPad9:
                                value = value + "9";
                                break;
                            case Keys.NumPad0:
                                value = value + "0";
                                break;
                            case Keys.D1:
                                value = value + "1";
                                break;
                            case Keys.D2:
                                value = value + "2";
                                break;
                            case Keys.D3:
                                value = value + "3";
                                break;
                            case Keys.D4:
                                value = value + "4";
                                break;
                            case Keys.D5:
                                value = value + "5";
                                break;
                            case Keys.D6:
                                value = value + "6";
                                break;
                            case Keys.D7:
                                value = value + "7";
                                break;
                            case Keys.D8:
                                value = value + "8";
                                break;
                            case Keys.D9:
                                value = value + "9";
                                break;
                            case Keys.D0:
                                value = value + "0";
                                break;
                            case Keys.Q:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "Q"; }
                                else { value = value + "q"; }
                                break;
                            case Keys.W:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "W"; }
                                else { value = value + "w"; }
                                break;
                            case Keys.E:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "E"; }
                                else { value = value + "e"; }
                                break;
                            case Keys.R:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "R"; }
                                else { value = value + "r"; }
                                break;
                            case Keys.T:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "T"; }
                                else { value = value + "t"; }
                                break;
                            case Keys.Y:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "Y"; }
                                else { value = value + "y"; }
                                break;
                            case Keys.U:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "U"; }
                                else { value = value + "u"; }
                                break;
                            case Keys.I:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "I"; }
                                else { value = value + "i"; }
                                break;
                            case Keys.O:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "O"; }
                                else { value = value + "o"; }
                                break;
                            case Keys.P:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "P"; }
                                else { value = value + "p"; }
                                break;
                            case Keys.A:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "A"; }
                                else { value = value + "a"; }
                                break;
                            case Keys.S:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "S"; }
                                else { value = value + "s"; }
                                break;
                            case Keys.D:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "D"; }
                                else { value = value + "d"; }
                                break;
                            case Keys.F:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "F"; }
                                else { value = value + "f"; }
                                break;
                            case Keys.G:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "G"; }
                                else { value = value + "g"; }
                                break;
                            case Keys.H:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "H"; }
                                else { value = value + "h"; }
                                break;
                            case Keys.J:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "J"; }
                                else { value = value + "j"; }
                                break;
                            case Keys.K:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "K"; }
                                else { value = value + "k"; }
                                break;
                            case Keys.L:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "L"; }
                                else { value = value + "l"; }
                                break;
                            case Keys.Z:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "Z"; }
                                else { value = value + "z"; }
                                break;
                            case Keys.X:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "X"; }
                                else { value = value + "x"; }
                                break;
                            case Keys.C:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "C"; }
                                else { value = value + "c"; }
                                break;
                            case Keys.V:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "V"; }
                                else { value = value + "v"; }
                                break;
                            case Keys.B:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "B"; }
                                else { value = value + "b"; }
                                break;
                            case Keys.N:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "N"; }
                                else { value = value + "n"; }
                                break;
                            case Keys.M:
                                if (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) { value = value + "M"; }
                                else { value = value + "m"; }
                                break;
                            case Keys.Space:
                                value = value + " ";
                                break;
                            case Keys.OemPlus:
                                value = value + "+";
                                break;
                            case Keys.OemMinus:
                                value = value + "-";
                                break;
                            case Keys.OemComma:
                                value = value + ",";
                                break;
                            case Keys.OemPeriod:
                                value = value + ".";
                                break;
                        }
                }
                return true;
            }
            return false;
        }
    }
}
