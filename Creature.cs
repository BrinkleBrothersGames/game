using Game.Content.World;
using SurvivalGame.Content.Characters.AI;
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
    public class Creature
    {
        public Inventory inv = new Inventory();
        public string name;
        public SimpleAI ai;
        public Needs needs = new Needs();
        public Stats stats = new Stats();
        public Coords coords;
        public int[] position;

        
        public Creature(string name, int[] position)
        {
            this.name = name;
            this.position = position;
            ai = new SimpleAI(this);
            this.coords = new Coords(position);            
        }

        public void DecideAction()
        {
            ai.Update(this);
            ai.ActionSelection();
        }

        public bool UpdatePosition(Map map, int[] newPosition)
        {
            if (MapUtils.IsOutsideMap(newPosition, map) || map.layout[newPosition[0], newPosition[1]].blocksMovement)
            {
                return false;
            }

            Terrain creatureTerrain = new Terrain(name);

            // Remove the player from their old position on the map
            map.layout[this.position[0], this.position[1]].contentsTerrain.Remove(creatureTerrain);

            // Set the player's coords to their new position
            this.position = newPosition;

            // Update map to reflect new position
            map.layout[newPosition[0], newPosition[1]].contentsTerrain.Add(creatureTerrain);

            return true;
        }
        
        public void UpdateCreatureNeeds(Dictionary<string, int> needsDictionary)
        {
            this.needs.UpdateNeeds(needsDictionary);
        }
        
    }
}
