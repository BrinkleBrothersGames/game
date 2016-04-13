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
        
        public Player(int x, int y, Inventory inv)
        {
            this.coords = new Coords(x, y);
            this.inv = inv;
            this.needs = new Needs();
            this.stats = new Stats();
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
    }
}
