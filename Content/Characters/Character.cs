using Game.Content.World;
using SurvivalGame.Content.Items;
using SurvivalGame.Content.World;
using SurvivalGame.Content.World.TerrainTypes;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    public class Character
    {
        public string name;
        public Inventory inv;
        public Coords coords;
        public Needs needs;
        public Stats stats;

        public Character(string name, int xCoord, int yCoord)
        {
            this.name = name;
            this.coords = new Coords(xCoord, yCoord);
            this.needs = new Needs();
            this.stats = new Stats();
            this.inv = new Inventory();
        }

        public Character(string name, int xCoord, int yCoord, int agility, int endurance, int intelligence, int perception, int strength)
        {
            this.name = name;
            this.coords = new Coords(xCoord, yCoord);
            this.needs = new Needs();
            this.stats = new Stats(strength, endurance, agility, perception, intelligence);
            this.inv = new Inventory();
        }

        public bool UpdatePosition(Map map, int[] newPosition)
        {
            if (MapUtils.IsOutsideMap(newPosition, map) || map.layout[newPosition[0], newPosition[1]].blocksMovement)
            {
                return false;
            }

            Terrain characterTerrain = new Terrain(name);

            // Remove the player from their old position on the map
            map.layout[this.coords.x, this.coords.y].contentsTerrain.Remove(characterTerrain);

            // Set the player's coords to their new position
            this.coords = new Coords(newPosition[0], newPosition[1]);

            // Update map to reflect new position
            map.layout[newPosition[0], newPosition[1]].contentsTerrain.Add(characterTerrain);

            return true;
        }

        public void SetCharacterCoords(int x, int y)
        {
            this.coords.x = x;
            this.coords.y = y;            
        }

    }
}
