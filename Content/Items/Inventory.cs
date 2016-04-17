using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Items
{
    public class Inventory
    {
        // Contains item and the number of it the player has in their inventory

        public Dictionary<Item, int> inventory = new Dictionary<Item, int>() { };

        
        //TODO - Default inventory capacity for, say tile, should be unlimited? Better way of doing this?
        public int capacity = 99999;

        public Inventory()
        {
        }

        //TODO - Better with or without setting capacity?
        public Inventory(int capacity)
        {
            this.capacity = capacity;
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

        /// <summary>
        /// Add a single item to the inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItemToInventory(Item item)
        {
            if (CountOfAllItemsInInventory() >= capacity)
            {
                Console.WriteLine("Your inventory is full.");
                return false;
            }

            else
            {
                // If dictionary contains item, increase count, else add item to inventory.
                if (this.inventory.ContainsKey(item))
                {
                    this.inventory[item] += 1;
                }
                else
                {
                    this.inventory.Add(item, 1);
                }
            }
            return true;
        }

        /// <summary>
        /// Add a dictionary of items to an inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="numberOfItems"></param>
        /// <returns></returns>
        public bool AddItemsToInventory(Dictionary<Item, int> inventoryToAdd)
        {
            Inventory inv = new Inventory(inventoryToAdd);

            if (inv.CountOfAllItemsInInventory() + this.CountOfAllItemsInInventory() > capacity)
            {
                Console.WriteLine("The Item(s) won't fit in the inventory.");
                return false;
            }

            else
            { 
                foreach (KeyValuePair<Item, int> entry in inventory)
                {
                    for (int i = 1; i <= entry.Value; i++)
                    {
                        AddItemToInventory(entry.Key);
                    }
                }
                return true;
            }
        }


        public void RemoveItemFromInventory(Item item)
        {
            if (!this.inventory.ContainsKey(item))
            {
                return;
            }

            // If dictionary contains item, decrease count by 1. Else remove item from inventory.
            if (this.inventory[item] > 1)
            {
                this.inventory[item] -= 1;
            }
            else
            {
                this.inventory.Remove(item);
            }
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

        public int CountOfAllItemsInInventory()
        {
            int count = 0;

            foreach (int itemCount in inventory.Values)
            {
                count += itemCount;
            }

            return count;
        }

        /// <summary>
        /// Returns a list of all the containers within an inventory and in the containers within those containers etc.
        /// </summary>
        /// <returns></returns>
        public List<Container> FindContainersInInventory()
        {
            List<Container> containers = new List<Container>();
            List<Container> containersUncheckedForNest = new List<Container>();

            foreach (KeyValuePair<Item, int> item in inventory)
            {
                for (int i = 1; i <= item.Value; i++)
                {
                    if (item.Key.GetType() == typeof(Container))
                    {
                        containers.Add(item.Key as Container);
                        containersUncheckedForNest.Add(item.Key as Container);
                    }
                }
            }

            while (containersUncheckedForNest.Count > 0)
            {
                containersUncheckedForNest.Clear();

                foreach (Container container in containersUncheckedForNest)
                {
                    foreach (KeyValuePair<Item, int> item in container.inv.inventory)
                    {
                        for (int i = 1; i <= item.Value; i++)
                        {
                            if (item.Key.GetType() == typeof(Container))
                            {
                                containers.Add(item.Key as Container);
                                containersUncheckedForNest.Add(item.Key as Container);
                            }
                        }
                    }
                }
            }

            return containers;
        }

    }
}
