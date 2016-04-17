using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items
{
    public class Inventory
    {
        // TODO - should have method for adding many items. Should take input is Dictionary<Item, int>()

        // Contains item and the number of it the player has in their inventory

        public Dictionary<Item, int> inventory = new Dictionary<Item, int>() { };

        public Inventory()
        {

        }

        public Inventory(Dictionary<Item, int> contents)
        {
            this.inventory = contents;
        }

        public Item GetItemByName(string wantedItemName)
        {
            foreach(Item item in inventory.Keys)
            {
                if (item.name.Equals(wantedItemName))
                {
                    return item;
                }
            }
            return null;         
        }

        public void AddItemToInventory(Item item)
        {
            // If dictionary contains item, increase count by 1. Else add item to inventory.
            if (this.inventory.ContainsKey(item))
            {
                this.inventory[item] += 1;
            }
            else
            {
                this.inventory.Add(item, 1);
            }
        }

        public void AddItemToInventory(Item item, int number)
        {
            // If dictionary contains item, increase count by 1. Else add item to inventory.
            if (this.inventory.ContainsKey(item))
            {
                this.inventory[item] += number;
            }
            else
            {
                this.inventory.Add(item, number);
            }
        }

        public void RemoveItemFromInventory(Item item)
        {
            if (!this.inventory.ContainsKey(item))
            {
                return;
            }

            // If dictionary contains item, increase count by 1. Else add item to inventory.
            if (this.inventory[item] > 1)
            {
                this.inventory[item] -= 1;
            }
            else
            {
                this.inventory.Remove(item);
            }
        }

        public void RemoveItemFromInventory(Item item, int numRemoved)
        {
            if (!this.inventory.ContainsKey(item))
            {
                return;
            }

            // If dictionary contains  multiple of item, decrease count by numRemoved. Else remove item from inventory.
            if (this.inventory[item] > numRemoved)
            {
                this.inventory[item] -= numRemoved;
            }
            else
            {
                this.inventory.Remove(item);
            }
        }

        public bool Contains(Item inputItem)
        {
            foreach(Item item in this.inventory.Keys)
            {
                if (item.Equals(inputItem))
                {
                    return true;
                }
            }

            return false;
        }

        public void PrintInventory()
        {
            // If inv empty, tell player.
            if (this.inventory.Count() == 0)
            {
                Console.WriteLine("You have no items in your inventory.");
            }
            else    // Otherwise, list their items and item count
            {
                Console.WriteLine("Your inventory contains:");

                foreach (Item item in this.inventory.Keys)
                {
                    Console.WriteLine(item.name + "\tx" + this.inventory[item].ToString());
                }
            }
        }
    }
}
