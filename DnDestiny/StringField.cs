using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DnDestiny
{
    class StringField
    {
        #region Fields

        // position
        private Vector2 factor;
        private Vector2 startPosition;

        // text
        private SpriteFont font;
        private string title;
        private string type;
        private string text;

        // status
        private bool isActive;

        #endregion

        #region Constructors
        public StringField(Vector2 factor, SpriteFont font, string text, Vector2 startPosition)
        {
            this.factor = factor;
            this.startPosition = startPosition;
            this.font = font;
            this.text = checkCharacters(text);

            this.isActive = false;
        }
        #endregion

        #region Properties
        public Vector2 StartPosition
        { get { return startPosition; } set { startPosition = value; } }
        public bool IsActive
        { get { return isActive; } set { isActive = true; } }
        public string Title
        { get { return title; } set { title = value; } }
        public string Type
        { get { return type; } set { type = value; } }
        public string Text
        { get { return text; } set { text = checkCharacters(value); } }
        #endregion

        #region Methods

        private string checkCharacters(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '’') { text = text.Substring(0, i) + "'" + text.Substring(i + 1); }
                if (text[i] == '—') { text = text.Substring(0, i) + "-" + text.Substring(i + 1); }
                if (text[i] == '½') { text = text.Substring(0, i) + "1/2" + text.Substring(i + 1); }
            }
            return text;
        }

        public Vector2 Position(MouseState mState)
        {
            return new Vector2(startPosition.X - (int)((mState.X - Game1.screenWidth / 2) * factor.X),
            (int)startPosition.Y - ((mState.Y - Game1.screenHeight / 2) * factor.Y));
        }

        public void Draw(SpriteBatch _spriteBatch, MouseState mState)
        {
            _spriteBatch.DrawString(font, text, new Vector2(Position(mState).X, Position(mState).Y), Color.White);
        }
        #endregion
    }
}
