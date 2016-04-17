using Game.Content.World;
using SurvivalGame.Content.Items;
using SurvivalGame.Content.World;
using SurvivalGame.Content.World.TerrainTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class Player
    {
        public Inventory inv;
        public Needs needs;
        public Stats stats;
        public TemporaryStats tempStats;
        public Coords coords;
        public List<BodyPart> bodyParts = new List<BodyPart>()
        {
            new BodyPart("hand", 1),
            new BodyPart("back", 1),
            new BodyPart("torso", 2)
        };

        //Was it right to remove inventory? I couldn't see why it was being set directly.
        public Player(int x, int y)
        {
            this.coords = new Coords(x, y);
            this.stats = new Stats();
            this.inv = new Inventory(stats.strength);
            this.needs = new Needs();
            this.tempStats = new TemporaryStats(stats);
        }

        public void SetPlayerCoords(int x, int y)
        {
            this.coords.x = x;
            this.coords.y = y;
        }

        public void SetPlayerCoords(Coords coords)
        {
            this.coords = coords;
        }

        
        public bool UpdatePlayerPosition(Map map, Player player, Coords newCoords)
        {
            if(map.layout[newCoords.x, newCoords.y].blocksMovement)
            {
                return false;
            }

            Terrain playerTerrain = new Terrain("player");

            // Remove the player from their old position on the map
            map.layout[player.coords.x, player.coords.y].contentsTerrain.Remove(playerTerrain);

            // Set the player's coords to their new position
            player.SetPlayerCoords(newCoords);

            // Update map to reflect new position.

            map.layout[player.coords.x, player.coords.y].contentsTerrain.Add(playerTerrain);

            return true;
        }
        
        /// <summary>
        /// Updates the player needs from a dictionary object
        /// </summary>
        /// <param name="needsDictionary">Dictionary<string, int> of the need and the amount to increase by</string></param>
        public void UpdatePlayerNeeds(Dictionary<string, int> needsDictionary)
        {
            this.needs.UpdateNeeds(needsDictionary);
        }
        
        /// <summary>
        /// Changes the inputted stat by the inputted amount
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="amount"></param>
        public void ChangePlayerStats(string stat, int amount)
        {
            this.stats.ChangeStats(stat, amount);
        }

        // TODO - Should this be here, or in the 'needs' class
        /// <summary>
        /// Prints the player's hunger, thirst, tiredness and health to the console.
        /// </summary>
        public void GetStatus()
        {
            Console.WriteLine("Hunger: " + this.needs.hungerLevel.ToString() + "/" + this.needs.MAX_HUNGER.ToString());
        
            Console.WriteLine("Thirst: " + this.needs.thirstLevel.ToString() + "/" + this.needs.MAX_THIRST.ToString());
           
            Console.WriteLine("Tiredness: " + this.needs.tirednessLevel.ToString() + "/" + this.needs.MAX_TIREDNESS.ToString());
            
            Console.WriteLine("Health: " + this.needs.health.ToString() + "/" + this.needs.MAX_HEALTH.ToString());
        }
        
        /// <summary>
        /// Returns true if player health is greater than zero, and false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerAlive()
        {
            if (this.needs.isAlive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a player's extended inventory.
        /// </summary>
        /// <returns></returns>
        public Inventory ExtendedPlayerInventory()
        {
            Inventory totalInv = inv;

            totalInv.capacity = GetExtendedInvCapacity();

            foreach (BodyPart bodyPart in bodyParts)
            {
                totalInv.AddItemsToInventory(bodyPart.inv.inventory);
            }

            foreach (Container container in GetPlayerContainers())
            {
                totalInv.AddItemsToInventory(container.inv.inventory);
            }

            return totalInv;
        }

        public int GetExtendedInvCapacity()
        {
            int totalCapacity = inv.capacity;

            foreach (BodyPart bodyPart in bodyParts)
            {
                totalCapacity += bodyPart.inv.capacity;
            }

            foreach (Container container in GetPlayerContainers())
            {
                totalCapacity += container.inv.capacity;
            }

            return totalCapacity;
        }

        /// <summary>
        /// Returns a list of all containers possesed by a player.
        /// </summary>
        /// <returns></returns>
        public List<Container> GetPlayerContainers()
        {
            List<Container> storageItems = new List<Container>();
            List<Container>  storageItemsUncheckedForNest = new List<Container>();

            storageItems.AddRange(inv.FindContainersInInventory());

            foreach (BodyPart bodyPart in bodyParts)
            {
                storageItems.AddRange(bodyPart.inv.FindContainersInInventory());
            }
            return storageItems;
        }






    }
}
