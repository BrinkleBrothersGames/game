using SurvivalGame.Content.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items
{
    public class ConsumableItem : Item
    {
        public Dictionary<string, int> needChange = new Dictionary<string, int>();
        public int healthChange = 0;
        public int hungerChange = 0;
        public int thirstChange = 0;
        public int tirednessChange = 0;

        public ConsumableItem(string name) : base(name)
        {
        }

        // TODO - Is this really the best way to do this?
        public ConsumableItem(string name, int healthChange, int hungerChange, int thirstChange, int tirednessChange, Dictionary<string, int> needChange) : base(name)
        {
            this.healthChange = healthChange;
            this.hungerChange = hungerChange;
            this.thirstChange = thirstChange;
            this.tirednessChange = tirednessChange;
            this.needChange = needChange;
        }

        // TODO - Is this really the best way to do this?
        public ConsumableItem(string name, Dictionary<string, int> needChange) : base(name)
        {
            this.needChange = needChange;
        }

        public void OnConsumption(Player player)
        {
            player.UpdatePlayerNeeds(needChange);
            player.inv.RemoveItemFromInventory(this);
        }

        public int GetNeedValue(string need)
        {
            int value = 0;

            switch (need)
            {
                case ("hunger"):
                    return this.hungerChange;
                case ("thirst"):
                    return this.thirstChange;
                case ("tiredness"):
                    return this.tirednessChange;
                case ("health"):
                    return this.healthChange;
            }

            return value

        }

    }
}
