using SurvivalGame.Content.Items;
using SurvivalGame.Content.World;
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
        const int SEED = 19951995;
        Random rnd = new Random(SEED);

        /// <summary>
        /// Default constructor creates empty room - test purposes only
        /// </summary>
        public Map()
        {
            layout = GenerateSuburbsMap(rnd.Next(101, 201), rnd.Next(51, 81));
        }

        // TODO this should be moved to some kind of graphics class - in Engine or Utils. Fine here for now.
        /// <summary>
        /// Takes a map and displays it in its entirety, row by row
        /// </summary>
        /// <param name="map"></param>
        public void RenderMap(Map map)
        {
            for (int x = 0; x < map.layout.GetLength(0); x++)
            {
                string rowString = "";

                // And for each column of that row...
                for (int y = 0; y < map.layout.GetLength(1); y++)
                {
                    // Add the graphic representation of that tile to a string
                    rowString += GetTileImage(map.layout[x, y]);
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
            if (tile.contents.Contains("player"))
            {
                return "X";
            }
            else if ( tile.contents.Contains("wall"))
            {
                return "#";
            }
            else if ( tile.contentsItems.inventory.Count > 0)
            {
                return "S";
            }
            else if ( tile.contents.Contains("floor"))
            {
                return ".";
            }
            else
            {
                return " ";
            }
        }

        public Tile[,] GenerateSuburbsMap(int mapWidth, int mapHeight)
        {
            Tile[,] map = new Tile[mapWidth, mapHeight];

            Random rnd = new Random(SEED);

            // Generates a massive open space. Will then drop houses in to taste.
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    // Creates an empty floor tile
                    map[x, y] = Tile.CreateFloorTile();
                }
            }

            bool needMoreHouses = true;
            // TODO - Find a more sensible way of doing this - render houses until no more spaces are available?
            int failedAttempts = 0;
            int[,] invalidPositions = new int[mapWidth, mapHeight];
            bool isValidPosition;

            while (needMoreHouses)
            {
                int houseXPos = rnd.Next(0, mapWidth - 1);
                int houseYPos = rnd.Next(0, mapHeight - 1);
                int houseXLength = rnd.Next(5, 10);
                int houseYLength = rnd.Next(5, 10);
                isValidPosition = true;

                for (int x = 0; x < houseXLength + 2; x++)
                {
                    for (int y = 0; y < houseYLength + 2; y++)
                    {
                        // If house position would be outside map, scrap it and try again
                        if ((houseXPos + x) < 0 || houseXPos + x >= mapWidth || houseYPos + y < 0 || houseYPos + y >= mapHeight)
                        {
                            isValidPosition = false;
                            break;
                        }
                        else if (invalidPositions[houseXPos + x, houseYPos + y] != 0)
                        {
                            isValidPosition = false;
                            break;
                        }

                        invalidPositions[houseXPos + x, houseYPos + y] = 1;
                    }
                }

                if (isValidPosition)
                {
                    Tile[,] house = GenerateHouse(houseXLength, houseYLength);
                
                    for (int x = 0 + 1; x < houseXLength + 1; x++)
                    {
                        for (int y = 0 + 1; y < houseYLength + 1; y++)
                        {
                            map[houseXPos + x, houseYPos + y] = house[x - 1 , y - 1];
                        }
                    }
                }
                else
                {
                    failedAttempts += 1;
                }

                
                // TODO - NO MAGIC STRINGS!!!
                if(failedAttempts == 100)
                {
                    needMoreHouses = false;
                }
            }

                return map;
        }

        public Tile[,] GenerateHouse(int houseWidth, int houseHeight)
        {
            Tile[,] house = new Tile[houseWidth, houseHeight];

            // Creates a set of closed walls with floor inside
            for (int i = 0; i < houseWidth; i++)
            {
                for (int j = 0; j < houseHeight; j++)
                {
                    if ((i == 0) || (j == 0) || (i == houseWidth - 1) || (j == houseHeight - 1))
                    {
                        house[i, j] = Tile.CreateWallTile();
                    }
                    else
                    {
                        house[i, j] = Tile.CreateFloorTile();
                    }
                }
            }

            // Generates the number of exits for the house and adds them to it. Creates up to two exits
            int exitNo = rnd.Next(1, 11);

            while(exitNo > 0)
            {
                int xDoorPosition;
                int yDoorPosition; 
                int doorSide = rnd.Next(0, 2);
                
                if(exitNo%2 == 0)
                {
                    xDoorPosition = rnd.Next(1, houseWidth - 2);
                    yDoorPosition = doorSide * (houseHeight - 1);
                }
                else
                {
                    yDoorPosition = rnd.Next(1, houseHeight - 2); ;
                    xDoorPosition = doorSide * (houseWidth - 1);
                }

                house[xDoorPosition, yDoorPosition] = Tile.CreateFloorTile();

                exitNo -= 8;
            }

            // Adds random 'loot' items to the house
            while (rnd.Next(1, 11) > 4)
            {
                int lootXPos = rnd.Next(1, houseWidth-1);
                int lootYPos = rnd.Next(1, houseHeight-1);

                // TODO - Random loot items should be picked from some table somewhere, not chosen like this.
                Item lootItem = GetRandomItem();

                house[lootXPos, lootYPos].contentsItems.AddItemToInventory(lootItem);
            }

            return house;
        }

        // TODO - move this elsewhere. Only here temporarily
        public Item GetRandomItem()
        {
            int lootType = rnd.Next(1, 52);

            if (lootType < 10)
            {
                Dictionary<string, int> needsChangeburger = new Dictionary<string, int>()
                {
                    {"hunger", 5}
                };
                return new ConsumableItem("burger", needsChangeburger);
            }
            else if (lootType < 20)
            {
                Dictionary<string, int> needsChangeCoffee = new Dictionary<string, int>()
                {
                    {"tiredness", -1},
                    {"thirst", 2}
                };
                return new ConsumableItem("beer", needsChangeCoffee);
            }
            else if (lootType < 30)
            {
                Dictionary<string, int> needsChangeCrisps = new Dictionary<string, int>()
                {
                    {"hunger", 2},
                    {"thirst", 2}
                };
                return new ConsumableItem("bag of crisps");
            }
            else if (lootType < 40)
            {
                Dictionary<string, int> needsChangeWater = new Dictionary<string, int>()
                    {
                        {"thirst", 5}
                    };
                return new ConsumableItem("water", needsChangeWater);
            }
            else if (lootType < 45)
            {
                Dictionary<string, int> needsChangeMedicine = new Dictionary<string, int>()
                    {
                        {"health", 5}
                    };
                return new ConsumableItem("medicine", needsChangeMedicine);
            } 
            else if (lootType < 50)
            {
                return new Item("a bundle of cash");
            }
            else
            {
                Dictionary<string, int> needsChangeSock = new Dictionary<string, int>()
                    {
                        {"health", -10}
                    };
                return new ConsumableItem("old sock", needsChangeSock);
            }


        }
    }
}
