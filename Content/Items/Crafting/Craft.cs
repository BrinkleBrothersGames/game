using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Content.World;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items.Crafting
{
    public class Craft
    {

        Recipe recipe;
        Player player;
        Map map;

        public void CreateRecipe(Recipe recipe, Player player, Map map)
        {
            this.recipe = recipe;
            this.player = player;
            this.map = map;

            bool craftingSucceeded = false;

            if (HasIngredients() && HasCatalyst())
            {
                // Player can try to craft - consume items
                ConsumeCraftItems();

                if (CraftingSucceeds())
                {
                    craftingSucceeded = true;

                    // Add output to player inv and inform them they succeeded.
                    foreach(Item producedItem in recipe.createdItems.Keys)
                    {
                        player.inv.AddItemToInventory(producedItem, recipe.createdItems[producedItem]);
                    }

                    // TODO - add skill increment here - could be based on chance player had of failing?
                    IncrementSkillLevel(craftingSucceeded);

                    Console.WriteLine("Crafting succeeded! - NEEDS SKILL INCREASE IMPLEMENTATION");
                }
                else
                {
                    // TODO - add skill increment here - could be based on chance player had of failing?
                    IncrementSkillLevel(craftingSucceeded);
                    // TODO - should produce some 'failed' attempt?
                    // TODO - always waste ingredients? Could that be luck based?
                    Console.WriteLine("Crafting failed. You wasted the ingredients! - NEEDS SKILL INCREASE IMPLEMENTATION");
                }
            }
            else
            {
                Console.WriteLine("You are unable to craft that.");
            }
        }

        public bool HasIngredients()
        {
            foreach(Item requiredItem in recipe.requiredItems.Keys)
            {
                // If player doesn't have item in inv OR has less than the required number, return false
                if (!player.inv.Contains(requiredItem) || player.inv.inventory[requiredItem] < recipe.requiredItems[requiredItem])
                {
                    return false;
                }
            }

            // If player has all ingredients, return true
            return true;
        }

        public bool HasCatalyst()
        {
            foreach(Item catalyst in recipe.catalysts)
            {
                // If player doesn't have item in inv OR has less than the required number, return false
                if (!player.inv.Contains(catalyst) && !TileContains(catalyst, map.layout[player.playerXCoord, player.playerYCoord]) && !AdjacentTilesContain(catalyst))
                {
                    return false;
                }
            }
            return true;
        }

        public bool TileContains(Item catalyst, Tile tile)
        {
            if(tile.contentsItems.Contains(catalyst))
            {
                return true;
            }

            return false;
        }
        
        public bool AdjacentTilesContain(Item catalyst)
        {
            foreach(Coords adjacentTileCoords in MapUtils.GetAdjacentTiles(map.layout, player.playerCoords))
            {
                if (TileContains(catalyst, map.layout[adjacentTileCoords.x, adjacentTileCoords.y]))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CraftingSucceeds()
        {
            // TODO - implement / Should roll based on player skill for success. Recipe needs 'difficulty' level?
            return true;
        }

        public void ConsumeCraftItems()
        {
            // Removes the neccessary number of each item from player inv
            foreach(Item consumedItem in recipe.requiredItems.Keys)
            {
                player.inv.RemoveItemFromInventory(consumedItem, recipe.requiredItems[consumedItem]);
            }
        }

        // TODO - this should call a method in Skill that increases the skill a certian proportion based on a 'reward level'. EG, high reward is ~ 10% of a level, low is ~1%
        public void IncrementSkillLevel(bool succeeded)
        {
            // TODO -implement this
        }
    }
}
