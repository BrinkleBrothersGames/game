using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.World
{
    public class Coords
    {
        public int x;
        public int y;
        
        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coords(int[] coords)
        {
            this.x = coords[0];
            this.y = coords[1];
        }

        public int[] GetCoordsArray()
        {
            return new int[] { x, y };
        }
        
        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Point return false.
            Coords coords = obj as Coords;
            if ((System.Object)coords == null)
            {
                return false;
            }

            if ((this.x == coords.x) && (this.y == coords.y))
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
            return x.GetHashCode() + y.GetHashCode();
        }
    }
}
