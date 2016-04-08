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

        public ConsumableItem(string name) : base(name)
        {
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

        public void OnConsumption(Creature creature)
        {
            creature.UpdateCreatureNeeds(needChange);
            creature.inv.RemoveItemFromInventory(this);
        }

        public int GetNeedValue(string need)
        {
            int value = 0;

            this.needChange.TryGetValue(need, out value);

            return value;
        }

    }
}
