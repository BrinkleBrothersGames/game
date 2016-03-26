using SurvivalGame.Content.Characters;
using SurvivalGame.Content.World;
using SurvivalGame.Content.World.TerrainTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Content.World
{
    public class Map
    {
        // TODO - We probable want to create getter/setter methods for these. Make everything private that should be private
        public int height;
        public int width;
        public Tile[,] layout;

        /// <summary>
        /// Default constructor creates empty room - test purposes only
        /// </summary>
        public Map()
        {
            this.height = 10;
            this.width = 10;
            Tile[,] emptyLayout = new Tile[width, height];

            this.layout = emptyLayout;

            // Makes a height by width grid, with the outside being walls.
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if( (i == 0) || (j == 0) || (i == width-1) || (j == height-1))
                    {
                        this.layout[i, j] = Tile.CreateWallTile();
                    }
                    else
                    {
                        this.layout[i, j] = Tile.CreateFloorTile();
                    }
                }
            }
        }

        // TODO this should be moved to some kind of graphics class - in Engine or Utils. Fine here for now.
        /// <summary>
        /// Takes a map and displays it in its entirety, row by row
        /// </summary>
        /// <param name="map"></param>
        public void RenderMap(Map map, Player player)
        {
            int[] playerCoords = player.GetPlayerCoords();

            for (int y = playerCoords[1] + 5; y >= playerCoords[1] - 5; y--)
            {
                string rowString = "";

                // And for each column of that row...
                for (int x = playerCoords[0] - 5; x <= playerCoords[0] + 5; x++)
                {
                    // Add the graphic representation of that tile to a string
                    if(x < 0 || x >= map.layout.GetLength(0) || y < 0 || y >= map.layout.GetLength(1))
                    {
                        rowString += " ";
                    }
                    else
                    {
                        rowString += GetTileImage(map.layout[x, y]);
                    }

                }

                // When the row is complete, print it
                Console.WriteLine(rowString);
            }
        }

        // TODO - should use an enum or something here - dict<int, string> might also work
        /// <summary>
        /// Displays what a tile contains. Things higher up the list are displayed first. If tile.contents empty, displays a space.
        /// </summary>
        /// <param name="tile">The tile to be displayed</param>
        /// <returns></returns>
        public string GetTileImage(Tile tile)
        {
            Terrain wall = new Terrain("wall");
            Terrain floor = new Terrain("floor");
            Terrain playerTerrain = new Terrain("player");

            if (tile.contentsTerrain.Contains(playerTerrain))
            {
                return "X";
            }
            else if ( tile.contentsTerrain.Contains(wall))
            {
                return "#";
            }
            else if ( tile.contentsItems.inventory.Count > 0)
            {
                return "S";
            }
            else if ( tile.contentsTerrain.Contains(floor))
            {
                return ".";
            }
            else
            {
                return " ";
            }
        }
        
    }
}
