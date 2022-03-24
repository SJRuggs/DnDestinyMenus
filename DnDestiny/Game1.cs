using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        private Texture2D proficient;
        private Texture2D expert;

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
        List<StringField> sheetStrings;
        List<EditableField> editableFields;
        string text;
        List<string> skillNames;
        List<int> skillScores;
        List<string> scoreNames;
        int currentShields;
        int currentHealth;
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
            img = new FileStream("../../../Assets/AppImages/proficient.png", FileMode.Open, FileAccess.Read);
            proficient = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream("../../../Assets/AppImages/expert.png", FileMode.Open, FileAccess.Read);
            expert = Texture2D.FromStream(GraphicsDevice, img);

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
            sheetStrings = new List<StringField>();
            characters = new List<Character>();
            editableFields = new List<EditableField>();
            while (text != null)
            {
                characters.Add(new Character(text));
                text = txt.ReadLine();
            }
            txt.Close();
            currentChar = characters[0];

            skillNames = new List<string>();
            skillScores = new List<int>();
            scoreNames = new List<string>();
            scoreNames.Add("Strength");
            scoreNames.Add("Dexterity");
            scoreNames.Add("Constitution");
            scoreNames.Add("Intelligence");
            scoreNames.Add("Wisdom");
            scoreNames.Add("Charisma");
            skillNames.Add("Acrobatics");
            skillScores.Add(1);
            skillNames.Add("Animal Handling");
            skillScores.Add(4);
            skillNames.Add("Arcana");
            skillScores.Add(3);
            skillNames.Add("Athletics");
            skillScores.Add(0);
            skillNames.Add("Deception");
            skillScores.Add(5);
            skillNames.Add("History");
            skillScores.Add(3);
            skillNames.Add("Insight");
            skillScores.Add(4);
            skillNames.Add("Intimidation");
            skillScores.Add(5);
            skillNames.Add("Investigation");
            skillScores.Add(3);
            skillNames.Add("Medicine");
            skillScores.Add(3);
            skillNames.Add("Nature");
            skillScores.Add(3);
            skillNames.Add("Perception");
            skillScores.Add(4);
            skillNames.Add("Performance");
            skillScores.Add(5);
            skillNames.Add("Persuasion");
            skillScores.Add(5);
            skillNames.Add("Religion");
            skillScores.Add(4);
            skillNames.Add("Sleight of Hand");
            skillScores.Add(1);
            skillNames.Add("Stealth");
            skillScores.Add(1);
            skillNames.Add("Survival");
            skillScores.Add(4);
            skillNames.Add("Technology");
            skillScores.Add(3);

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
            for (int i = 0; i < editableFields.Count; i++)
                editableFields[i].CalcInput(mState, prevMState, kbState, prevKbState);

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
                sheetStrings[i].Draw(_spriteBatch, mState);
            DrawProfs(currentChar);
            for (int i = 0; i < sheetFields.Count; i++)
                sheetFields[i].DrawPopup(_spriteBatch, mState);
            for (int i = 0; i < editableFields.Count; i++)
                editableFields[i].Draw(_spriteBatch, mState);
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
            for (int i = 0; i < 6; i++) cha.Stats[i] = int.Parse(chaReader.ReadLine());
            for (int i = 0; i < 6; i++) cha.Saves[i] = bool.Parse(chaReader.ReadLine());
            for (int i = 0; i < 19; i++) cha.Profs[i] = bool.Parse(chaReader.ReadLine());
            for (int i = 0; i < 19; i++) cha.Experts[i] = bool.Parse(chaReader.ReadLine());
            cha.OtherProfs = chaReader.ReadLine().Split('$').ToList();
            cha.EquippedArmor = chaReader.ReadLine();
            cha.Speed = int.Parse(chaReader.ReadLine());
            cha.Glimmer = int.Parse(chaReader.ReadLine());
            chaReader.Close();
            #endregion
            
            #region Character Sheet Fields
            sheetFields.Clear();
            sheetStrings.Clear();

            #region Ability Scores
            readStrings = new List<string>();
            for (int i = 0; i < scoreNames.Count; i++)
            {
                txt = new StreamReader(string.Format("../../../Assets/AppData/{0}.txt", scoreNames[i]));
                text = txt.ReadLine();
                readStrings.Clear();
                while (text != null)
                {
                    readStrings.Add(text);
                    text = txt.ReadLine();
                }
                sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, scoreNames[i].ToUpper(), "Ability Score", 
                    readStrings, new Rectangle(100, 100 + 150 * i, 100, 100)));
                img = new FileStream(string.Format("../../../Assets/AppImages/{0} active.png", scoreNames[i].ToLower()), FileMode.Open, FileAccess.Read);
                sheetFields[i].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
                img = new FileStream(string.Format("../../../Assets/AppImages/{0} inactive.png", scoreNames[i].ToLower()), FileMode.Open, FileAccess.Read);
                sheetFields[i].InactiveT = Texture2D.FromStream(GraphicsDevice, img);

                // label
                sheetStrings.Add(new StringField(factor, smallFont, sheetFields[i].Title, new Vector2(
                    sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - smallFont.MeasureString(sheetFields[i].Title).X / 2,
                    sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height + 10)));

                // stat
                sheetStrings.Add(new StringField(factor, titleFont, cha.Stats[i].ToString(), new Vector2(
                    sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - titleFont.MeasureString(cha.Stats[i].ToString()).X / 2,
                    sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height / 2 - titleFont.MeasureString(cha.Stats[i].ToString()).Y)));

                // bonus
                if ((cha.Stats[i] - 10) / 2 > -1)
                {
                    sheetStrings.Add(new StringField(factor, titleFont, "(+" + ((cha.Stats[i] - 10) / 2) + ")", new Vector2(
                        sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - titleFont.MeasureString("(+" + ((cha.Stats[i] - 10) / 2) + ")").X / 2,
                        sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height / 2)));
                }
                else
                {
                    sheetStrings.Add(new StringField(factor, titleFont, "(" + ((cha.Stats[i] - 10) / 2) + ")", new Vector2(
                        sheetFields[i].StartPosition.X + sheetFields[i].StartPosition.Width / 2 - titleFont.MeasureString("(" + ((cha.Stats[i] - 10) / 2) + ")").X / 2,
                        sheetFields[i].StartPosition.Y + sheetFields[i].StartPosition.Height / 2)));
                }
            }
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
                    "SAVING THROWS", "Character Attribute", readStrings, new Rectangle(250, 100 + 32 * i, 300, 32)));

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

            sheetStrings.Add(new StringField(factor, boldFont, "        Strength",      new Vector2(sheetFields[6].StartPosition.X + 40, sheetFields[6].StartPosition.Y + 3)));
            sheetStrings.Add(new StringField(factor, boldFont, "        Dexterity",     new Vector2(sheetFields[6].StartPosition.X + 40, sheetFields[7].StartPosition.Y + 3)));
            sheetStrings.Add(new StringField(factor, boldFont, "        Constitution",  new Vector2(sheetFields[6].StartPosition.X + 40, sheetFields[8].StartPosition.Y + 3)));
            sheetStrings.Add(new StringField(factor, boldFont, "        Intelligence",  new Vector2(sheetFields[6].StartPosition.X + 40, sheetFields[9].StartPosition.Y + 3)));
            sheetStrings.Add(new StringField(factor, boldFont, "        Wisdom",        new Vector2(sheetFields[6].StartPosition.X + 40, sheetFields[10].StartPosition.Y + 3)));
            sheetStrings.Add(new StringField(factor, boldFont, "        Charisma",      new Vector2(sheetFields[6].StartPosition.X + 40, sheetFields[11].StartPosition.Y + 3)));
            int stat;
            for (int i = 0; i < 6; i++)
            {
                if ((cha.Stats[i] - 10) / 2 > -1 && (!cha.Saves[i] || (cha.Stats[i] - 10 + cha.ProfBonus) / 2 > -1))
                    sheetStrings.Add(new StringField(factor, boldFont, "+", new Vector2(sheetFields[i + 6].StartPosition.X + 40, sheetFields[i + 6].StartPosition.Y + 3)));
                else
                    sheetStrings.Add(new StringField(factor, boldFont, "-", new Vector2(sheetFields[i + 6].StartPosition.X + 40, sheetFields[i + 6].StartPosition.Y + 3)));
                stat = (cha.Stats[i] - 10) / 2;
                if (cha.Saves[i]) stat += cha.ProfBonus;
                sheetStrings[sheetStrings.Count - 1].Text = sheetStrings[sheetStrings.Count - 1].Text + Math.Abs(stat);
            }

            sheetStrings.Add(new StringField(factor, smallFont, "SAVING THROWS", new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 -
                smallFont.MeasureString("SAVING THROWS").X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));
            #endregion

            #region Skill Proficiencies
            for (int i = 0; i < skillNames.Count; i++)
            {
                txt = new StreamReader(String.Format("../../../Assets/AppData/{0}.txt", skillNames[i].ToLower()));
                text = txt.ReadLine();
                readStrings.Clear();
                while (text != null)
                {
                    readStrings.Add(text);
                    text = txt.ReadLine();
                }
                sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack,
                    skillNames[i].ToUpper(), scoreNames[skillScores[i]] + " Skill", readStrings, new Rectangle(250, 344 + 32 * i, 300, 32)));

                if (i == 0) 
                    img = new FileStream(string.Format("../../../Assets/AppImages/proficienciesTop.png"), FileMode.Open, FileAccess.Read);
                if (i == 1) 
                    img = new FileStream(string.Format("../../../Assets/AppImages/proficienciesMiddle.png"), FileMode.Open, FileAccess.Read);
                if (i == skillNames.Count - 1) 
                    img = new FileStream(string.Format("../../../Assets/AppImages/proficienciesBottom.png"), FileMode.Open, FileAccess.Read);
                sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
                sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
                sheetStrings.Add(new StringField(factor, boldFont, "        " + skillNames[i], new Vector2(sheetFields[12 + i].StartPosition.X + 40, sheetFields[12 + i].StartPosition.Y + 3)));
                if ((cha.Stats[skillScores[i]] - 10) / 2 > -1 && (!cha.Profs[0] || (cha.Stats[skillScores[i]] - 10 + cha.ProfBonus) / 2 > -1))
                    sheetStrings.Add(new StringField(factor, boldFont, "+", new Vector2(sheetFields[12 + i].StartPosition.X + 40, sheetFields[12 + i].StartPosition.Y + 3)));
                else
                    sheetStrings.Add(new StringField(factor, boldFont, "-", new Vector2(sheetFields[12 + i].StartPosition.X + 40, sheetFields[12 + i].StartPosition.Y + 3)));
                stat = (cha.Stats[skillScores[i]] - 10) / 2;
                if (cha.Profs[i]) stat += cha.ProfBonus;
                if (cha.Experts[i]) stat += cha.ProfBonus;
                sheetStrings[sheetStrings.Count - 1].Text = sheetStrings[sheetStrings.Count - 1].Text + Math.Abs(stat);
            }
            sheetStrings.Add(new StringField(factor, smallFont, "SKILLS", new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 -
                smallFont.MeasureString("SKILLS").X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));
            #endregion

            #region Name Plate
            text = string.Format("Level {0} {1} {2}", cha.Level, cha.Race, cha.Class);
            if (cha.Foundation != null) { text = text + ", " + cha.Foundation; }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack,
                    cha.Name, text, null, new Rectangle(600, 100, 500, 100)));
            img = new FileStream(string.Format("../../../Assets/AppImages/namePlate active.png"), FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream(string.Format("../../../Assets/AppImages/namePlate inactive.png"), FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);
            sheetStrings.Add(new StringField(factor, smallFont, "CHARACTER NAME", new Vector2(
                    sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - 
                    smallFont.MeasureString("CHARACTER NAME").X / 2,
                    sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));
            sheetStrings.Add(new StringField(factor, titleFont, cha.Name, new Vector2(
                    sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2
                    - titleFont.MeasureString(cha.Name).X / 2,
                    sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height / 2
                    - titleFont.MeasureString(cha.Name).Y / 2)));
            #endregion

            #region Proficiency Bonus
            txt = new StreamReader("../../../Assets/AppData/proficiencyBonus.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "PROFICIENCY", "Character Attribute",
                readStrings, new Rectangle(600, 250, 100, 100)));
            img = new FileStream("../../../Assets/AppImages/proficiencyBonus active.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream("../../../Assets/AppImages/proficiencyBonus inactive.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);

            // label
            sheetStrings.Add(new StringField(factor, smallFont, sheetFields[sheetFields.Count - 1].Title, new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - smallFont.MeasureString(sheetFields[sheetFields.Count - 1].Title).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));

            // stat
            sheetStrings.Add(new StringField(factor, titleFont, "+" + cha.ProfBonus.ToString(), new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - titleFont.MeasureString("+" + cha.ProfBonus.ToString()).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height / 2 - titleFont.MeasureString("+" + cha.ProfBonus.ToString()).Y / 2)));
            #endregion

            #region Armor Class
            txt = new StreamReader("../../../Assets/AppData/armorClass.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "ARMOR CLASS", "Character Attribute",
                readStrings, new Rectangle(733, 250, 100, 100)));
            img = new FileStream("../../../Assets/AppImages/armorClass active.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream("../../../Assets/AppImages/armorClass inactive.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);

            // label
            sheetStrings.Add(new StringField(factor, smallFont, sheetFields[sheetFields.Count - 1].Title, new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - smallFont.MeasureString(sheetFields[sheetFields.Count - 1].Title).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));

            // stat
            sheetStrings.Add(new StringField(factor, titleFont, cha.AC.ToString(), new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - titleFont.MeasureString(cha.AC.ToString()).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height / 2 - titleFont.MeasureString(cha.AC.ToString()).Y / 2)));
            #endregion

            #region Initiative
            txt = new StreamReader("../../../Assets/AppData/initiative.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "INITIATIVE", "Character Attribute",
                readStrings, new Rectangle(867, 250, 100, 100)));
            img = new FileStream("../../../Assets/AppImages/initiative active.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream("../../../Assets/AppImages/initiative inactive.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);

            // label
            sheetStrings.Add(new StringField(factor, smallFont, sheetFields[sheetFields.Count - 1].Title, new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - smallFont.MeasureString(sheetFields[sheetFields.Count - 1].Title).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));

            // stat
            sheetStrings.Add(new StringField(factor, titleFont, "+" + cha.Initiative.ToString(), new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - titleFont.MeasureString("+" + cha.Initiative.ToString()).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height / 2 - titleFont.MeasureString("+" + cha.Initiative.ToString()).Y / 2)));
            #endregion

            #region Speed
            txt = new StreamReader("../../../Assets/AppData/speed.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "SPEED", "Character Attribute",
                readStrings, new Rectangle(1000, 250, 100, 100)));
            img = new FileStream("../../../Assets/AppImages/speed active.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream("../../../Assets/AppImages/speed inactive.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);

            // label
            sheetStrings.Add(new StringField(factor, smallFont, sheetFields[sheetFields.Count - 1].Title, new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - smallFont.MeasureString(sheetFields[sheetFields.Count - 1].Title).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));

            // stat
            sheetStrings.Add(new StringField(factor, titleFont, "+" + cha.Speed.ToString(), new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - titleFont.MeasureString("+" + cha.Speed.ToString()).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height / 2 - titleFont.MeasureString("+" + cha.Speed.ToString()).Y / 2)));
            #endregion

            #region SP
            txt = new StreamReader("../../../Assets/AppData/SP.txt");
            text = txt.ReadLine();
            readStrings.Clear();
            while (text != null)
            {
                readStrings.Add(text);
                text = txt.ReadLine();
            }
            sheetFields.Add(new Field(factor, defaultFont, boldFont, titleFont, panelPixelWhite, panelPixelBlack, "SHIELD POINTS", "Character Attribute",
                readStrings, new Rectangle(600, 400, 233, 233)));
            img = new FileStream("../../../Assets/AppImages/SP active.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].ActiveT = Texture2D.FromStream(GraphicsDevice, img);
            img = new FileStream("../../../Assets/AppImages/SP inactive.png", FileMode.Open, FileAccess.Read);
            sheetFields[sheetFields.Count - 1].InactiveT = Texture2D.FromStream(GraphicsDevice, img);

            // label
            sheetStrings.Add(new StringField(factor, smallFont, sheetFields[sheetFields.Count - 1].Title, new Vector2(
                sheetFields[sheetFields.Count - 1].StartPosition.X + sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - smallFont.MeasureString(sheetFields[sheetFields.Count - 1].Title).X / 2,
                sheetFields[sheetFields.Count - 1].StartPosition.Y + sheetFields[sheetFields.Count - 1].StartPosition.Height + 10)));

            // max
            currentShields = cha.MaxShield;
            sheetStrings.Add(new StringField(factor, defaultFont, "Max Shield Points: " + currentShields, new Vector2(sheetFields[sheetFields.Count - 1].StartPosition.X +
                sheetFields[sheetFields.Count - 1].StartPosition.Width / 2 - defaultFont.MeasureString("Max Shield Points: " + currentShields).X / 2, 405)));

            // current
            editableFields.Add(new EditableField(factor, titleFont, new Rectangle(600, 400, 233, 233), cha.MaxShield.ToString()));
            #endregion

            #endregion
        }

        private void DrawProfs(Character cha)
        {
            for (int i = 0; i < 6; i++)
                if (cha.Saves[i]) { _spriteBatch.Draw(proficient, new Vector2(sheetFields[6 + i].Position(mState).X + 6, sheetFields[6 + i].Position(mState).Y + 6), Color.White); }
            for (int i = 0; i < skillNames.Count; i++)
            {
                if (cha.Profs[i]) { _spriteBatch.Draw(proficient, new Vector2(sheetFields[12 + i].Position(mState).X + 6, sheetFields[12 + i].Position(mState).Y + 6), Color.White); }
                if (cha.Experts[i])   { _spriteBatch.Draw(expert, new Vector2(sheetFields[12 + i].Position(mState).X + 6, sheetFields[12 + i].Position(mState).Y + 6), Color.White); }
            }
        }
        #endregion
    }
}
