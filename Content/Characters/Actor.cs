using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Content.Items;
using SurvivalGame.Content.World;
using SurvivalGame.Content.World.TerrainTypes;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Actor
{
    public class Actor
    {
        public string name;
        public Inventory inv;
        public Coords coords;
        public Needs needs;
        public Stats stats;

        public Actor(string name, int xCoord, int yCoord, Inventory inv)
        {
            this.name = name;
            this.coords = new Coords(xCoord, yCoord);
            this.needs = new Needs();
            this.stats = new Stats();
            this.inv = inv;
            inv.capacity = stats.strength;

        }

        public Actor(string name, int xCoord, int yCoord, int agility, int endurance, int intelligence, int perception, int strength)
        {
            this.name = name;
            this.coords = new Coords(xCoord, yCoord);
            this.needs = new Needs();
            this.stats = new Stats(strength, endurance, agility, perception, intelligence);
            this.inv = new Inventory(strength);
        }

        public bool UpdatePosition(Map map, Coords newPosition)
        {
            if (MapUtils.IsOutsideMap(newPosition, map) || map.layout[newPosition.x, newPosition.y].blocksMovement)
            {
                return false;
            }

            Terrain characterTerrain = new Terrain(name);

            // Remove the player from their old position on the map
            map.layout[this.coords.x, this.coords.y].contentsTerrain.Remove(characterTerrain);

            // Set the player's coords to their new position
            this.coords = new Coords(newPosition.x, newPosition.y);

            // Update map to reflect new position
            map.layout[newPosition.x, newPosition.y].contentsTerrain.Add(characterTerrain);

            return true;
        }

        public void SetCharacterCoords(int x, int y)
        {
            this.coords.x = x;
            this.coords.y = y;            
        }

    }
}
