using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.World.TerrainTypes
{
    public class Terrain
    {
        public string name;
        public bool blocksMovement = false;

        public Terrain(string name)
        {
            this.name = name;
        }



        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Point return false.
            Terrain terrain = obj as Terrain;
            if ((System.Object)terrain == null)
            {
                return false;
            }

            if (this.name == terrain.name)
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
    }
}
