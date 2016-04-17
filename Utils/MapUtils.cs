using Game.Content.World;
using SurvivalGame.Content.Characters;
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
        public static int GetAbsoluteDistanceBetweenTwoPoints(Coords firstPoint, Coords secondPoint)
        {
            int distance = 0;

            distance += Math.Abs(firstPoint.x - secondPoint.x) + Math.Abs(firstPoint.y - secondPoint.y);

            return distance;
        }

        public static Coords ConvertPerceivableMapCoordsToMapCoords(Coords perceivableMapCoords, Tile[,] perceivableTiles, Map superMap, Coords centreOfPercMapSuperMapCoords)
        {
            Coords percMapCoordsRelativeToCentre = new Coords(perceivableMapCoords.x - (perceivableTiles.GetLength(0) / 2), perceivableMapCoords.y - (perceivableTiles.GetLength(1) / 2) );

            return new Coords( centreOfPercMapSuperMapCoords.x + percMapCoordsRelativeToCentre.x, centreOfPercMapSuperMapCoords.y + percMapCoordsRelativeToCentre.y );
        }

        public static bool IsOutsideMap(Coords newCoords, Map map)
        {
            if (newCoords.x < 0 || newCoords.x > (map.width - 1) || newCoords.y < 0 || newCoords.y > (map.height - 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Returns a dictionary of coordinates and the number moves required to get from there to a target point, taking terrain into account
        public static Dictionary<Coords, int> GetDijkstraMap(Tile[,] map, Coords targetCoords)
        {
            Dictionary<Coords, int> distanceByCoordinates = new Dictionary<Coords, int>();
            List<Coords> mappedTiles = new List<Coords>();
            List<Coords> tilesToBeMapped = new List<Coords>();
            List<Coords> tilesToBeMappedThisLoop = new List<Coords>();
            Coords currentTile = new Coords(targetCoords);
            int distance = 0;

            tilesToBeMapped.Add(new Coords(targetCoords));

            // Add current tiles with distance incremented by one. Get adjacent tiles for next iteration.
            while (tilesToBeMapped.Count > 0)
            {
                distance++;
                tilesToBeMappedThisLoop = tilesToBeMapped.ToList();
                tilesToBeMapped.Clear();

                // If each coord set is not null and doesn't block movement, add it to our dictionary and add its neighbours to the tilesToBeMapped
                foreach (Coords coords in tilesToBeMappedThisLoop)
                {
                    if (map[coords.x, coords.y] != null && !map[coords.x, coords.y].blocksMovement)
                    {
                        distanceByCoordinates.Add(coords, distance);
                        mappedTiles.Add(coords);

                        foreach (Coords adjacentCoords in GetAdjacentTiles(map, coords))
                        {
                            if (!tilesToBeMapped.Contains(adjacentCoords) && !mappedTiles.Contains(adjacentCoords))
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

            adjacentTiles.Add(new Coords(location.x + 1, location.y));
            adjacentTiles.Add(new Coords(location.x - 1, location.y));
            adjacentTiles.Add(new Coords(location.x, location.y + 1));
            adjacentTiles.Add(new Coords(location.x, location.y - 1));


            foreach (Coords coords in adjacentTiles)
            {
                // If int[] doesn't lie on map, or no tile exists at that point...
                if (!(coords.x < 0 || coords.x > map.GetLength(0) - 1 || coords.y < 0 || coords.y > map.GetLength(1) - 1 || map[coords.x, coords.y] == null))
                {
                    adjacentTilesReturn.Add(coords);
                }
            }

            return adjacentTilesReturn;
        }

        // TODO - Perhaps move this somewhere else.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="location"></param>
        /// <param name="allCreatures"></param>
        /// <returns></returns>
        public static List<Creature> GetAdjacentCreatures(Map map, Coords location)
        {
            List<Coords> adjacentCoords = GetAdjacentTiles(map.layout, location);
            
            List<Creature> adjacentCreatures = new List<Creature>();            

            foreach (Creature creature in map.presentCreatures)
            {
                Coords creaturesssCoords = new Coords(creature.coords);

                foreach (Coords coords in adjacentCoords)
                {
                    if (coords.Equals(creaturesssCoords))
                    {
                        adjacentCreatures.Add(creature);
                    }
                }
            }

            return adjacentCreatures;
        }
    }
}