using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items
{
    public class Container : Item
    {
        public Inventory inv;

        public Container(int capacity, string name) : base(name)
        {
            this.inv.capacity = capacity;
        }
    }

}
