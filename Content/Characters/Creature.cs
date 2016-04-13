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

        
        public Creature(string name, Coords coords)
        {
            this.name = name;
            this.coords = coords;
            ai = new SimpleAI(this);           
        }

        public void DecideAction()
        {
            ai.Update(this);
            ai.ActionSelection();
        }

        public bool UpdatePosition(Map map, Coords newCoords)
        {
            if (MapUtils.IsOutsideMap(newCoords, map) || map.layout[newCoords.x, newCoords.y].blocksMovement)
            {
                return false;
            }

            Terrain creatureTerrain = new Terrain(name);

            // Remove the player from their old position on the map
            map.layout[this.coords.x, this.coords.y].contentsTerrain.Remove(creatureTerrain);

            // Set the player's coords to their new position
            this.coords = newCoords;

            // Update map to reflect new position
            map.layout[newCoords.x, newCoords.y].contentsTerrain.Add(creatureTerrain);

            return true;
        }
        
        public void UpdateCreatureNeeds(Dictionary<string, int> needsDictionary)
        {
            this.needs.UpdateNeeds(needsDictionary);
        }
        
    }
}
