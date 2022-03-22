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

            #region Character Sheet Fields
            sheetFields = new List<Field>();
            sheetStrings = new List<string>();
            sheetFonts = new List<SpriteFont>();
            sheetStringsLocations = new List<Vector2>();
            characters = new List<Character>();

            #region Strength
            txt = new StreamReader("../../../Assets/AppData/strengthDescription.txt");
            string text = txt.ReadLine();
            readStrings = new List<string>();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "STRENGTH", "Ability Score", readStrings, new Rectangle(150, 200, 100, 100)));
            #endregion

            #region Dexterity
            txt = new StreamReader("../../../Assets/AppData/dexterityDescription.txt");
            text = txt.ReadLine();
            readStrings = new List<string>();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "DEXTERITY", "Ability Score", readStrings, new Rectangle(150, 350, 100, 100)));
            #endregion

            #region Constitution
            txt = new StreamReader("../../../Assets/AppData/constitutionDescription.txt");
            text = txt.ReadLine();
            readStrings = new List<string>();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "CONSTITUTION", "Ability Score", readStrings, new Rectangle(150, 500, 100, 100)));
            #endregion

            #endregion

            #region Characters
            // retrieve names
            txt = new StreamReader("../../../Characters/MetaData.txt");
            text = txt.ReadLine();
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

            #region Character Sheet Fields

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


            #endregion
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
                sheetFields[i].DrawString(_spriteBatch, mState);
            #endregion

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
            for (int i = 0; i < 18; i++) cha.Profs[i] = bool.Parse(chaReader.ReadLine());
            for (int i = 0; i < 18; i++) cha.Experts[i] = bool.Parse(chaReader.ReadLine());
            cha.OtherProfs = chaReader.ReadLine().Split('$').ToList();
            cha.EquippedArmor = chaReader.ReadLine();
            cha.Speed = int.Parse(chaReader.ReadLine());
            cha.Glimmer = int.Parse(chaReader.ReadLine());
            sheetStrings.Clear();
            #endregion

            #region Strength
            sheetStrings.Add("STRENGTH");
            sheetFonts.Add(smallFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[0].StartPosition.X + sheetFields[0].StartPosition.Width / 2 - sheetFonts[0].MeasureString(sheetStrings[0]).X / 2,
                sheetFields[0].StartPosition.Y + sheetFields[0].StartPosition.Height + 10));
            sheetStrings.Add(cha.Stats[0].ToString());
            sheetFonts.Add(titleFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[0].StartPosition.X + sheetFields[0].StartPosition.Width / 2 - sheetFonts[1].MeasureString(sheetStrings[1]).X / 2,
                sheetFields[0].StartPosition.Y + sheetFields[0].StartPosition.Height / 2 - sheetFonts[1].MeasureString(sheetStrings[1]).Y));
            if ((cha.Stats[0] - 10) / 2 > -1) { sheetStrings.Add("(+" + ((cha.Stats[0] - 10) / 2) + ")"); }
            else { sheetStrings.Add("(" + ((cha.Stats[0] - 10) / 2) + ")"); }
            sheetFonts.Add(titleFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[0].StartPosition.X + sheetFields[0].StartPosition.Width / 2 - sheetFonts[2].MeasureString(sheetStrings[2]).X / 2,
                sheetFields[0].StartPosition.Y + sheetFields[0].StartPosition.Height / 2));
            #endregion

            #region Dexterity
            sheetStrings.Add("DEXTERITY");
            sheetFonts.Add(smallFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[1].StartPosition.X + sheetFields[1].StartPosition.Width / 2 - sheetFonts[3].MeasureString(sheetStrings[3]).X / 2,
                sheetFields[1].StartPosition.Y + sheetFields[1].StartPosition.Height + 10));
            sheetStrings.Add(cha.Stats[1].ToString());
            sheetFonts.Add(titleFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[1].StartPosition.X + sheetFields[1].StartPosition.Width / 2 - sheetFonts[4].MeasureString(sheetStrings[4]).X / 2,
                sheetFields[1].StartPosition.Y + sheetFields[1].StartPosition.Height / 2 - sheetFonts[4].MeasureString(sheetStrings[4]).Y));
            if ((cha.Stats[1] - 10) / 2 > -1) { sheetStrings.Add("(+" + ((cha.Stats[1] - 10) / 2) + ")"); }
            else { sheetStrings.Add("(" + ((cha.Stats[1] - 10) / 2) + ")"); }
            sheetFonts.Add(titleFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[1].StartPosition.X + sheetFields[1].StartPosition.Width / 2 - sheetFonts[5].MeasureString(sheetStrings[5]).X / 2,
                sheetFields[1].StartPosition.Y + sheetFields[1].StartPosition.Height / 2));
            #endregion

            #region Constitution
            sheetStrings.Add("CONSTITUTION");
            sheetFonts.Add(smallFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[2].StartPosition.X + sheetFields[2].StartPosition.Width / 2 - sheetFonts[6].MeasureString(sheetStrings[6]).X / 2,
                sheetFields[2].StartPosition.Y + sheetFields[2].StartPosition.Height + 10));
            sheetStrings.Add(cha.Stats[2].ToString());
            sheetFonts.Add(titleFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[2].StartPosition.X + sheetFields[2].StartPosition.Width / 2 - sheetFonts[7].MeasureString(sheetStrings[7]).X / 2,
                sheetFields[2].StartPosition.Y + sheetFields[2].StartPosition.Height / 2 - sheetFonts[7].MeasureString(sheetStrings[7]).Y));
            if ((cha.Stats[2] - 10) / 2 > -1) { sheetStrings.Add("(+" + ((cha.Stats[2] - 10) / 2) + ")"); }
            else { sheetStrings.Add("(" + ((cha.Stats[2] - 10) / 2) + ")"); }
            sheetFonts.Add(titleFont);
            sheetStringsLocations.Add(new Vector2(
                sheetFields[2].StartPosition.X + sheetFields[2].StartPosition.Width / 2 - sheetFonts[8].MeasureString(sheetStrings[8]).X / 2,
                sheetFields[2].StartPosition.Y + sheetFields[2].StartPosition.Height / 2));
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
