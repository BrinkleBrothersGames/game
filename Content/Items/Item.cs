using SurvivalGame.Content.Characters;
using SurvivalGame.Utils;
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

        //TODO - Put here for simplicity, but might have to change.
        public Dictionary<BodyPart, bool> equippableTo = new Dictionary<BodyPart, bool>()
            {
                {new BodyPart("hand", 1), true},
                {new BodyPart("back", 1), false},
                {new BodyPart("torso", 2), false}
            };

        public Item(string name)
        {
            this.name = name;
        }

        //TODO - might have to update in the container subclass
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

        //TODO - extend to actor.
        /// <summary>
        /// Equip item to player's bodypart.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="bodyPartToBeEquipped"></param>
        public void Equip(Player player, BodyPart bodyPart)
        {
            if (!player.bodyParts.Contains(bodyPart) || !equippableTo[bodyPart] || !player.inv.inventory.ContainsKey(this))
            {
                Console.WriteLine(bodyPart.name + " isn't a body part belonging to the player," + name + "isn't an item in the player's inventory, or " + name + " can't be equipped to" + bodyPart.name);
            }
            else if (bodyPart.inv.CountOfAllItemsInInventory() >= bodyPart.inv.capacity)
            {
                bool invalidResoponse = true;
                Console.WriteLine(bodyPart.name + " already has the maximum number of items equipped, remove item(s) and equip " + name + "?");
                while (invalidResoponse)
                {
                    string remove = Console.ReadLine();
                    switch (remove)
                    {
                        case "y":
                        case "yes":
                            foreach (Item item in bodyPart.inv.inventory.Keys)
                            {
                                bodyPart.inv.inventory.Remove(item);
                            }

                            if (bodyPart.inv.AddItemToInventory(this))
                            {
                                player.inv.RemoveItemFromInventory(this);
                            }
                            invalidResoponse = false;
                            break;
                        case "n":
                        case "no":
                            invalidResoponse = false;
                            break;
                    }
                }
            }
            else
            {
                if (bodyPart.inv.AddItemToInventory(this))
                {
                    player.inv.RemoveItemFromInventory(this);
                }
            }
        }
    }
}