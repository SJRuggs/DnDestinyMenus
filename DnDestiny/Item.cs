using System;
using System.Collections.Generic;
using System.Text;

namespace DnDestiny
{
    public enum Type
    {
        ability,
        feature,
        gear,
        ghostFeature
    }

    class Item
    {
        #region Fields
        private string name;
        private Type type;
        private List<string> description;
        #endregion

        #region Constructor
        public Item(string name, int type) 
        { 
            this.name = name;
            if (type == 0) this.type = DnDestiny.Type.ability;
            else if (type == 0) this.type = DnDestiny.Type.feature;
            else if (type == 0) this.type = DnDestiny.Type.gear;
            else this.type = DnDestiny.Type.ghostFeature;
        }
        #endregion

        #region Properties
        public string Name { get  {return name; } set { name = value; } }
        public Type Type { get { return type; } set { type = value; } }
        public List<string> Description { get { return description; } set { description = value; } }
        #endregion

        #region Methods
        #endregion
    }
}
