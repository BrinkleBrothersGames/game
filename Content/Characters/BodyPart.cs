using SurvivalGame.Content.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class BodyPart
    {
        public string name;
        public Inventory inv;

        public BodyPart(string name, int invCapacity)
        {
            this.name = name;
            inv = new Inventory();
            inv.capacity = invCapacity;
        }

        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Point return false.
            BodyPart bodyPart = obj as BodyPart;
            if ((System.Object)bodyPart == null)
            {
                return false;
            }

            if (this.name == bodyPart.name)
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
