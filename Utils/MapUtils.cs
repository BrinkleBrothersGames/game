using Game.Content.World;
using SurvivalGame.Content.World;
using System;
using System.Collections;
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

        // Returns a dictionary of coordinates and the number moves required to get from there to a target point, taking terrain into account
        public static Dictionary<Coords, int> GetDijkstraMap(Tile[,] map, int[] targetCoords)
        {
            Dictionary<Coords, int> distanceByCoordinates = new Dictionary<Coords, int>();
            List<Coords> mappedTiles = new List<Coords>();
            List<Coords> tilesToBeMapped = new List<Coords>();
            List<Coords> tilesToBeMappedThisLoop = new List<Coords >();
            Coords currentTile = new Coords(targetCoords);
            int distance = 0;
            
            tilesToBeMapped.Add(new Coords(targetCoords));

            // Add current tiles with distance incremented by one. Get adjacent tiles for next iteration.
            while(tilesToBeMapped.Count > 0)
            {
                distance++;
                tilesToBeMappedThisLoop = tilesToBeMapped.ToList();
                tilesToBeMapped.Clear();

                // If each coord set is not null and doesn't block movement, add it to our dictionary and add its neighbours to the tilesToBeMapped
                foreach (Coords coords in tilesToBeMappedThisLoop)
                {
                    if(map[coords.x, coords.y] != null && !map[coords.x, coords.y].blocksMovement)
                    {
                        distanceByCoordinates.Add(coords, distance);
                        mappedTiles.Add(coords);

                        foreach(Coords adjacentCoords in GetAdjacentTiles(map, coords))
                        {
                            if(!tilesToBeMapped.Contains(adjacentCoords) && !mappedTiles.Contains(adjacentCoords))
                            {
                                tilesToBeMapped.Add(adjacentCoords);
                            }
                        }
                    }
                }
            }

            return distanceByCoordinates;
        }

        public static List<Coords> GetAdjacentTiles(Tile[,] map, Coords location)
        {
            List<Coords> adjacentTiles = new List<Coords>();
            List<Coords> adjacentTilesReturn = new List<Coords>();

            adjacentTiles.Add(new Coords (location.x + 1, location.y));
            adjacentTiles.Add(new Coords (location.x - 1, location.y));
            adjacentTiles.Add(new Coords (location.x, location.y + 1 ));
            adjacentTiles.Add(new Coords (location.x, location.y - 1 ));

            foreach(Coords coords in adjacentTiles)
            {
                // If int[] doesn't lie on map, or no tile exists at that point...
                if(!(coords.x < 0 || coords.x > map.GetLength(0)-1 || coords.y < 0 || coords.y > map.GetLength(1)-1 || map[coords.x, coords.y] == null))
                {
                    adjacentTilesReturn.Add(coords);
                }
            }

            return adjacentTilesReturn;
        }
    }
}