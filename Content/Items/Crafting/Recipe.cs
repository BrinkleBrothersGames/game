using SurvivalGame.Content.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items.Crafting
{
    public class Recipe
    {
        public Dictionary<Item, int> requiredItems = new Dictionary<Item, int>();
        public Dictionary<Item, int> createdItems = new Dictionary<Item, int>();
        public List<Item> catalysts = new List<Item>();
        public Skill associatedSkill;

        public Recipe(Dictionary<Item, int> requiredItems, Dictionary<Item, int> createdItems, List<Item> catalysts, Skill associatedSkill)
        {
            this.requiredItems = requiredItems;
            this.createdItems = createdItems;
            this.catalysts = catalysts;
            this.associatedSkill = associatedSkill;
        }
    }
}
