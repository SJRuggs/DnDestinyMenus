using System;
using System.Collections.Generic;
using System.Text;

namespace DnDestiny
{
    class Character
    {
        #region Fields
        Random rng;

        private string name;
        private string clas;
        private int level;
        private string foundation;
        private string race;
        private string alignment;
        private int[] stats;
        private bool[] saves;
        private bool[] profs;
        private bool[] experts;
        private List<string> otherProfs;
        private string equippedArmor;
        private int speed;
        private int glimmer;
        private int initiative;
        #endregion

        #region Constructors
        public Character(string name)
        {
            this.name = name;
            rng = new Random();
            initiative = 0;
            stats = new int[6];
            saves = new bool[6];
            profs = new bool[19];
            experts = new bool[19];
            otherProfs = new List<string>();
        }
        #endregion

        #region Properties

        // simple return properties
        public string Name { get { return name; } set { name = value; } }
        public string Class { get { return clas; } set { clas = value; } }
        public int Level { get { return level; } set { level = value; } }
        public string Foundation { get { return foundation; } set { foundation = value; } }
        public string Race { get { return race; } set { race = value; } }
        public string Alignment { get { return alignment; } set { alignment = value; } }
        public int[] Stats { get { return stats; } set { stats = value; } }
        public bool[] Saves { get { return saves; } set { saves = value; } }
        public bool[] Profs { get { return profs; } set { profs = value; } }
        public bool[] Experts { get { return experts; } set { experts = value; } }
        public List<string> OtherProfs { get { return otherProfs; } set { otherProfs = value; } }
        public string EquippedArmor { get { return equippedArmor; } set { equippedArmor = value; } }
        public int Speed { get { return speed; } set { speed = value; } }
        public int Glimmer { get { return glimmer; } set { glimmer = value; } }

        // calculated properties
        public int HitDie
        {
            get
            {
                switch (Class)
                {
                    case "Bladedancer": return 8;
                    case "Defender": return 12;
                    case "Gunslinger": return 8;
                    case "Nightstalker": return 8;
                    case "Stormcaller": return 8;
                    case "Striker": return 8;
                    case "Sunbreaker": return 10;
                    case "Sunsinger": return 8;
                    case "Voidwalker": return 6;
                    default: return 0;
                }
            }
        }
        public int MaxHP { get { return HitDie + (Level - 1) * Math.Max((Stats[2] - 10) / 2, 0); } }
        public int MaxShield { get { return HitDie + (Level - 1) * (HitDie / 2 + 1); } }
        public int Initiative { get { return Math.Max((Stats[1] - 10) / 2, initiative); } set { initiative = value; } }
        public int ProfBonus { get { return (level - 1) / 4 + 2; } }
        public int AC 
        { 
            get 
            {
                switch (equippedArmor)
                {
                    case "Padded": return 11 + (stats[1] - 10) / 2;
                    case "Leather": return 11 + (stats[1] - 10) / 2;
                    case "Spinweave": return 12 + (stats[1] - 10) / 2;
                    case "Makeshift": return 12 + Math.Max((stats[1] - 10) / 2, 2);
                    case "Spinwire": return 13 + Math.Max((stats[1] - 10) / 2, 2);
                    case "Reinforced": return 14 + Math.Max((stats[1] - 10) / 2, 2);
                    case "Plastwire": return 14 + Math.Max((stats[1] - 10) / 2, 2);
                    case "Spinplate": return 15 + Math.Max((stats[1] - 10) / 2, 2);
                    case "Half-Plast": return 14;
                    case "Plasteel": return 16;
                    case "Fortified": return 17;
                    case "Relic": return 18;
                    default: return 10 + (stats[1] - 10) / 2;
                }
            } 
        }

        #endregion

        #region Methods
        #endregion
    }
}
