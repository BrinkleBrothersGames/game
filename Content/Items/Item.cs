using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items
{
    public class Item
    {
        // TODO - Should have seperate display name and name.
        public string name;
        
        public Item(string name)
        {
            this.name = name;
        }

        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Point return false.
            Item item = obj as Item;
            if ((System.Object)item == null)
            {
                return false;
            }

            if (this.name == item.name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
    }
}