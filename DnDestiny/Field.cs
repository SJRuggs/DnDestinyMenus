using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace DnDestiny
{
    class Field
    {
        #region Fields

        // position
        private Vector2 factor;
        private Rectangle startPosition;

        // text
        private SpriteFont[] fonts;
        private string title;
        private string type;
        private List<string> text;
        private int textLength;

        // status
        private bool isActive;

        // graphics
        private Texture2D activeT;
        private Texture2D inactiveT;
        private Texture2D white;
        private Texture2D black;

        #endregion

        #region Constructors
        public Field(Vector2 factor, SpriteFont defaultFont, SpriteFont boldFont, SpriteFont titleFont,
            Texture2D white, Texture2D black, string title, string type, List<string> text, Rectangle startPosition)
        {
            this.factor = new Vector2(factor.X * 0.8f, factor.Y * 0.8f);
            this.startPosition = startPosition;

            this.fonts = new SpriteFont[3];
            this.fonts[0] = titleFont;
            this.fonts[1] = boldFont;
            this.fonts[2] = defaultFont;
            this.title = title;
            this.type = type;
            this.textLength = 450;
            this.text = FormatText(text, textLength);

            this.isActive = false;
            this.white = white;
            this.black = black;
        }
        #endregion

        #region Properties
        public Rectangle StartPosition
        { get { return startPosition; } set { startPosition = value; } }
        public bool IsActive
        { get { return isActive; } set { isActive = value; } }
        public string Title
        { get { return title; } set { title = value; } }
        public string Type
        { get { return type; } set { type = value; } }
        public List<string> Text
        { get { return text; } set { text = FormatText(value, textLength); } }
        public Texture2D ActiveT
        { set { activeT = value; } }
        public Texture2D InactiveT
        { set { inactiveT = value; } }
        #endregion

        #region Methods
        public List<string> FormatText(List<string> text, int textLength)
        {
            if (text != null)
            {
                List<string> splitText = new List<string>();
                float lineLength;
                string remainingWords;
                string movingWords;
                for (int i = 0; i < text.Count; i++)
                    if (text[i].Contains("$$"))
                    { 
                        if (!checkCharacters(text[i]).Split("$$")[0].Trim().ToUpper().Equals(""))
                            splitText.Add("0" + checkCharacters(text[i]).Split("$$")[0].Trim().ToUpper());
                        if (!checkCharacters(text[i]).Split("$$")[1].Trim().Equals(""))
                            splitText.Add("2" + checkCharacters(text[i]).Split("$$")[1].Trim());
                    }
                    else if (text[i].Contains('$'))
                    {
                        if (!checkCharacters(text[i]).Split("$")[0].Trim().ToUpper().Equals(""))
                            splitText.Add("1" + checkCharacters(text[i]).Split('$')[0].Trim().ToUpper());
                        if (!checkCharacters(text[i]).Split("$")[1].Trim().Equals(""))
                            splitText.Add("2" + checkCharacters(text[i]).Split('$')[1].Trim());
                    }
                    else
                    { splitText.Add("2" + checkCharacters(text[i]).Trim()); }
                for (int i = 0; i < splitText.Count; i++)
                    if (splitText[i].Substring(0, 1).Equals("2"))
                        if (fonts[2].MeasureString(splitText[i]).X > textLength)
                        {
                            lineLength = 0;
                            remainingWords = "";
                            movingWords = "";
                            for (int j = 0; j < splitText[i].Split(" ").Length; j++)
                            {
                                lineLength += fonts[2].MeasureString(splitText[i].Split(" ")[j]).X;
                                if (lineLength > textLength)
                                    movingWords += splitText[i].Split(" ")[j] + " ";
                                else
                                    remainingWords += splitText[i].Split(" ")[j] + " ";
                            }
                            splitText[i] = remainingWords;
                            splitText.Insert(i + 1, "2" + movingWords);
                        }
                return splitText;
            }
            return null;
        }

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

        public Rectangle Position(MouseState mState)
        {
            return new Rectangle(startPosition.X - (int)((mState.X - Game1.screenWidth / 2) * factor.X),
            startPosition.Y - (int)((mState.Y - Game1.screenHeight / 2) * factor.Y), startPosition.Width, startPosition.Height);
        }

        public void DrawPopup(SpriteBatch _spriteBatch, MouseState mState)
        {
            if (Position(mState).Contains(mState.Position))
            {
                float textHeight = 0;
                int columnHeight = 800;
                int columnWidth = 500;
                List<List<string>> columns = new List<List<string>>();
                columns.Add(new List<string>());
                Point textPosition = mState.Position;
                float drawHeight = 0;

                if (text != null)
                {
                    // calculate columns
                    for (int i = 0; i < text.Count; i++)
                    {
                        if (drawHeight + fonts[int.Parse(text[i].Substring(0, 1))].LineSpacing > 800 - 100)
                        {
                            columns.Add(new List<string>());
                            textHeight = Math.Max(textHeight, drawHeight);
                            drawHeight = 0;
                        }
                        columns[columns.Count - 1].Add(text[i].Trim());
                        drawHeight += fonts[int.Parse(text[i].Substring(0, 1))].LineSpacing;
                    }
                    textHeight = Math.Max(textHeight, drawHeight);

                    // remove redundant lines
                    for (int i = 0; i < columns.Count; i++)
                    {
                        for (int j = columns[i].Count - 1; j > 0; j--)
                            if (columns[i][j].Substring(columns[i][j].Length - 1).Equals("2"))
                            { textHeight -= fonts[int.Parse(columns[i][j].Substring(0, 1))].LineSpacing; columns[i].RemoveAt(j); }
                            else { j = 0; }
                        for (int j = 0; j < columns[i].Count - 1; j++)
                            if (columns[i][j].Equals("2") && columns[i][j + 1].Equals("2")) 
                            { textHeight -= fonts[2].LineSpacing; columns[i].RemoveAt(j); }
                        for (int j = 0; j < columns[i].Count; j++)
                            if(columns[i][j].Length > 1)
                                if (columns[i][j][1].Equals(" "))
                                    columns[i][j] = columns[i][j].Substring(0, 1) + columns[i][j].Substring(2);
                    }
                }

                // move textPosition
                if (mState.Position.X > Game1.screenWidth / 2) { textPosition.X -= columnWidth * columns.Count; }
                if (mState.Position.Y > Game1.screenHeight / 2) { textPosition.Y -= (int)(textHeight / columns.Count + 100); }
                if (mState.Position.X > Game1.screenWidth / 2) { textPosition.X -= 15; }
                else { textPosition.X += 50; }
                if (mState.Position.Y > Game1.screenHeight / 2) { textPosition.Y -= 15; }
                else { textPosition.Y += 50; }
                textPosition.X = Math.Max(textPosition.X, 30);
                textPosition.Y = Math.Max(textPosition.Y, 30);
                textPosition.X = Math.Min(textPosition.X, Game1.screenWidth - 30 - columns.Count * columnWidth);
                textPosition.Y = Math.Min(textPosition.Y, Game1.screenHeight - 170 - (int)textHeight);

                // draw background title and type
                _spriteBatch.Draw(white, new Rectangle(textPosition.X, textPosition.Y, columnWidth * columns.Count + 5, 6), Color.White);
                _spriteBatch.Draw(black, new Rectangle(textPosition.X, textPosition.Y + 6, columnWidth * columns.Count + 5, 94), Color.White);
                _spriteBatch.DrawString(fonts[0], title, new Vector2(textPosition.X + 10, textPosition.Y + 16), Color.White);
                _spriteBatch.DrawString(fonts[1], type, new Vector2(textPosition.X + 10, textPosition.Y + 16 + fonts[0].LineSpacing), Color.White);

                if (text != null)
                {
                    _spriteBatch.Draw(black, new Rectangle(textPosition.X, textPosition.Y + 100, columnWidth * columns.Count + 5,
                           (int)Math.Min(columnHeight, textHeight + 20)), Color.White * 0.8f);
                    for (int i = 0; i < columns.Count; i++)
                    {
                        drawHeight = 0;
                        for (int j = 0; j < columns[i].Count; j++)
                        {
                            _spriteBatch.DrawString(fonts[int.Parse(columns[i][j].Substring(0, 1))], columns[i][j].Substring(1),
                                new Vector2(textPosition.X + 10 + columnWidth * i, textPosition.Y + 110 + drawHeight), Color.White);
                            drawHeight += fonts[int.Parse(columns[i][j].Substring(0, 1))].LineSpacing;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch, MouseState mState)
        {
            if (Position(mState).Contains(mState.Position) || isActive) { _spriteBatch.Draw(activeT, Position(mState), Color.White); }
            else { _spriteBatch.Draw(inactiveT, Position(mState), Color.White); }
        }
        #endregion
    }
}
