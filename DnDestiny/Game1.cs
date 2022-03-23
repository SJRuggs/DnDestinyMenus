using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DnDestiny
{
    public enum ShowState
    {
        charSelect,
        charView,
        levelUp
    }

    public class Game1 : Game
    {
        #region Fields

        #region Fonts and Graphics
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D background;
        public static int screenWidth;
        public static int screenHeight;
        private Vector2 factor;
        private Texture2D cursor;

        // field graphics
        private Texture2D panelPixelWhite;
        private Texture2D panelPixelBlack;

        // fonts
        private SpriteFont titleFont;
        private SpriteFont boldFont;
        private SpriteFont defaultFont;
        private SpriteFont smallFont;
        #endregion

        #region Character Fields
        List<Character> characters;
        Character currentChar;
        List<Field> sheetFields;
        List<string> sheetStrings;
        List<SpriteFont> sheetFonts;
        List<Vector2> sheetStringsLocations;
        string text;
        #endregion

        #region Readers
        private StreamReader txt;
        private FileStream img;
        private List<string> readStrings;
        #endregion

        #region Input
        private MouseState mState;
        private MouseState prevMState;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        #endregion

        #endregion

        #region Main Methods
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            #region Fonts and Graphics

            // set background
            img = new FileStream("../../../Assets/AppImages/background.png", FileMode.Open, FileAccess.Read);
            background = Texture2D.FromStream(GraphicsDevice, img);
            IsMouseVisible = false;

            // screen size
            screenWidth = 1920;
            screenHeight = 1080;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            // fields
            factor = new Vector2(0.03f, 0.03f);
            panelPixelWhite = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            panelPixelWhite.SetData(new[] { Color.White });
            panelPixelBlack = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            panelPixelBlack.SetData(new[] { Color.Black });

            // fonts
            titleFont = Content.Load<SpriteFont>("TitleFont");
            boldFont = Content.Load<SpriteFont>("BoldFont");
            defaultFont = Content.Load<SpriteFont>("DefaultFont");
            smallFont = Content.Load<SpriteFont>("SmallFont");

            #endregion

            #region Characters
            // retrieve names
            txt = new StreamReader("../../../Characters/MetaData.txt");
            text = txt.ReadLine();
            sheetFields = new List<Field>();
            sheetStrings = new List<string>();
            sheetFonts = new List<SpriteFont>();
            sheetStringsLocations = new List<Vector2>();
            characters = new List<Character>();
            while (text != null)
            {
                characters.Add(new Character(text));
                text = txt.ReadLine();
            }
            currentChar = characters[0];
            LoadCharacter(currentChar);
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            img = new FileStream(string.Format("../../../Assets/AppImages/cursor.png"), FileMode.Open, FileAccess.Read);
            cursor = Texture2D.FromStream(GraphicsDevice, img);
        }

        protected override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();
            kbState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevKbState = kbState;
            prevMState = mState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            #region Character Sheet
            for (int i = 0; i < sheetFields.Count; i++)
                sheetFields[i].Draw(_spriteBatch, mState);
            for (int i = 0; i < sheetStrings.Count; i++)
                DrawString(sheetStrings[i], sheetFonts[i], sheetStringsLocations[i]);
            for (int i = 0; i < sheetFields.Count; i++)
                sheetFields[i].DrawPopup(_spriteBatch, mState);
            #endregion

            // draw cursor over mouse
            _spriteBatch.Draw(cursor, mState.Position.ToVector2(), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

        #region Helper Methods
        private void LoadCharacter(Character cha)
        {
            #region Construct Character
            currentChar = cha;
            StreamReader chaReader = new StreamReader(string.Format("../../../Characters/{0}.txt", cha.Name));
            cha.Class = chaReader.ReadLine();
            cha.Level = int.Parse(chaReader.ReadLine());
            cha.Foundation = chaReader.ReadLine();
            cha.Race = chaReader.ReadLine();
            cha.Alignment = chaReader.ReadLine();
            for (int i = 0; i < 6; i++) cha.Stats[i] = int.Parse(chaReader.ReadLine());
            for (int i = 0; i < 6; i++) cha.Saves[i] = bool.Parse(chaReader.ReadLine());
            for (int i = 0; i < 19; i++) cha.Profs[i] = bool.Parse(chaReader.ReadLine());
            for (int i = 0; i < 19; i++) cha.Experts[i] = bool.Parse(chaReader.ReadLine());
            cha.OtherProfs = chaReader.ReadLine().Split('$').ToList();
            cha.EquippedArmor = chaReader.ReadLine();
            cha.Speed = int.Parse(chaReader.ReadLine());
            cha.Glimmer = int.Parse(chaReader.ReadLine());
            sheetStrings.Clear();
            #endregion
            
            #region Character Sheet Fields
            sheetFields.Clear();
            sheetStrings.Clear();
            sheetFonts.Clear();
            sheetStringsLocations.Clear();

            #region Strength
            txt = new StreamReader("../../../Assets/AppData/strengthDescription.txt");
            text = txt.ReadLine();
            readStrings = new List<string>();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "STRENGTH", "Ability Score", readStrings, new Rectangle(100, 100, 100, 100)));
            #endregion

            #region Dexterity
            txt = new StreamReader("../../../Assets/AppData/dexterityDescription.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "DEXTERITY", "Ability Score", readStrings, new Rectangle(100, 250, 100, 100)));
            #endregion

            #region Constitution
            txt = new StreamReader("../../../Assets/AppData/constitutionDescription.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "CONSTITUTION", "Ability Score", readStrings, new Rectangle(100, 400, 100, 100)));
            #endregion

            #region Intelligence
            txt = new StreamReader("../../../Assets/AppData/intelligenceDescription.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "INTELLIGENCE", "Ability Score", readStrings, new Rectangle(100, 550, 100, 100)));
            #endregion

            #region Wisdom
            txt = new StreamReader("../../../Assets/AppData/wisdomDescription.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "WISDOM", "Ability Score", readStrings, new Rectangle(100, 700, 100, 100)));
            #endregion

            #region Charisma
            txt = new StreamReader("../../../Assets/AppData/charismaDescription.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "CHARISMA", "Ability Score", readStrings, new Rectangle(100, 850, 100, 100)));
            #endregion

            #region Saves
            txt = new StreamReader("../../../Assets/AppData/savesToolTip.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            for (int i = 0; i < 6; i++)
                sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack,
                    "SAVING THROWS", "Character Attribute", readStrings, new Rectangle(250, 100 + 36 * i, 400, 36)));
            #endregion

            #region Strength
            img = new FileStream(string.Format("../../../Assets/AppImages/strength active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[0].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/strength inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[0].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #region Dexterity
            img = new FileStream(string.Format("../../../Assets/AppImages/dexterity active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/dexterity inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #region Constitution
            img = new FileStream(string.Format("../../../Assets/AppImages/constitution active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[2].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/constitution inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[2].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #region Intelligence
            img = new FileStream(string.Format("../../../Assets/AppImages/intelligence active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[3].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/intelligence inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[3].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #region Wisdom
            img = new FileStream(string.Format("../../../Assets/AppImages/intelligence active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[4].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/intelligence inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[4].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #region Charisma
            img = new FileStream(string.Format("../../../Assets/AppImages/intelligence active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[5].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/intelligence inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[5].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #region Saves
            img = new FileStream(string.Format("../../../Assets/AppImages/proficienciesTop.png"), FileMode.Open, FileAccess.Read);
            sheetFields[6].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            sheetFields[6].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/proficienciesMiddle.png"), FileMode.Open, FileAccess.Read);
            for (int i = 7; i < 11; i++)
            {
                sheetFields[i].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
                sheetFields[i].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            }
            img = new FileStream(string.Format("../../../Assets/AppImages/proficienciesBottom.png"), FileMode.Open, FileAccess.Read);
            sheetFields[11].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            sheetFields[11].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            #endregion

            #endregion

            #region Stats
            for (int i = 0; i < 6; i++)
            {
                sheetStrings.Add(sheetFields[i].Title);
                sheetFonts.Add(smallFont);
                sheetStringsLocations.Add(new Vector2(
                    sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - sheetFonts[i * 3].MeasureString(sheetStrings[i * 3]).X / 2,
                    sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height + 10));
                sheetStrings.Add(cha.Stats[i].ToString());
                sheetFonts.Add(titleFont);
                sheetStringsLocations.Add(new Vector2(
                    sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - sheetFonts[i * 3 + 1].MeasureString(sheetStrings[i * 3 + 1]).X / 2,
                    sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height / 2 - sheetFonts[i * 3 + 1].MeasureString(sheetStrings[i * 3 + 1]).Y));
                if ((cha.Stats[i] - 10) / 2 > -1) { sheetStrings.Add("(+" + ((cha.Stats[i] - 10) / 2) + ")"); }
                else { sheetStrings.Add("(" + ((cha.Stats[i] - 10) / 2) + ")"); }
                sheetFonts.Add(titleFont);
                sheetStringsLocations.Add(new Vector2(
                    sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - sheetFonts[i * 3 + 2].MeasureString(sheetStrings[i * 3 + 2]).X / 2,
                    sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height / 2));
            }
            #endregion

            #region Saves
            sheetStrings.Add(string.Format("{0}  Strength",      sheetStrings[2].Substring(1, 2)));
            sheetStrings.Add(string.Format("{0}  Dexterity",     sheetStrings[5].Substring(1, 2)));
            sheetStrings.Add(string.Format("{0}  Constitution",  sheetStrings[8].Substring(1, 2)));
            sheetStrings.Add(string.Format("{0}  Intelligence",  sheetStrings[11].Substring(1, 2)));
            sheetStrings.Add(string.Format("{0}  Wisdom",        sheetStrings[14].Substring(1, 2)));
            sheetStrings.Add(string.Format("{0}  Charisma",      sheetStrings[17].Substring(1, 2)));
            for (int i = 0; i < 6; i++)
            {
                sheetFonts.Add(boldFont);
                sheetStringsLocations.Add(new Vector2(
                    sheetFields[i + 6].StartPosition.X + 40,
                    sheetFields[i + 6].StartPosition.Y + 3));
            }
            sheetStrings.Add("SAVING THROWS");
            sheetFonts.Add(smallFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - 
                sheetFonts[sheetFonts.Count - 1].MeasureString(sheetStrings[sheetStrings.Count - 1]).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10));
            #endregion

            // update visibility
            for (int i = 0; i < sheetFields.Count; i++)
                sheetFields[i].IsVisible = true;
        }

        private void DrawString(string str, SpriteFont font, Vector2 position)
        {
            _spriteBatch.DrawString(font, str, new Vector2((int)(position.X - (position.X + mState.X) * factor.X),
                (int)(position.Y - (position.Y + mState.Y) * factor.Y)), Color.White);
        }
        #endregion
    }
}
