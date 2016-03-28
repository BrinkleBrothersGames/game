using Game.Content.World;
using SurvivalGame.Content.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Utils
{
    public class MapUtils
    {
        /// <summary>
        /// Returns an int representing the distance between two points. Diagonal moves not considered. 
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static int GetAbsoluteDistanceBetweenTwoPoints(int[] firstPoint, int[] secondPoint)
        {
            int distance = 0;

            distance += Math.Abs(firstPoint[0] - secondPoint[0]) + Math.Abs(firstPoint[1] - secondPoint[1]);

            return distance;
        }

        public static int[] ConvertPerceivableMapCoordsToMapCoords(int[] perceivableMapCoords, Tile[,] perceivableTiles, Map superMap, int[] centreOfPercMapSuperMapCoords)
        {
            int[] percMapCoordsRelativeToCentre = new int[] { perceivableMapCoords[0] - (perceivableTiles.GetLength(0) / 2), perceivableMapCoords[1] - (perceivableTiles.GetLength(1) / 2) };

            return new int[]{ centreOfPercMapSuperMapCoords[0] + percMapCoordsRelativeToCentre[0],  centreOfPercMapSuperMapCoords[1] + percMapCoordsRelativeToCentre[1]};
        }

        public static bool IsOutsideMap(int[] newCoords, Map map)
        {
            if ( newCoords[0] < 0 || newCoords[0] > (map.width - 1) || newCoords[1] < 0 || newCoords[1] > (map.height - 1))
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
