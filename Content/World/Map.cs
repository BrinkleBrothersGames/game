using SurvivalGame.Content.Characters;
using SurvivalGame.Content.Items;
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
        public const int SEED = 68800;
        Random rnd = new Random(SEED);
        public List<Creature> presentCreatures = new List<Creature>();

        /// <summary>
        /// Default constructor creates empty room - test purposes only
        /// </summary>
        public Map()
        {
            //height = rnd.Next(101, 151);
            //width = rnd.Next(101, 151);
            height = 20;
            width = 20;
            layout = GenerateSuburbsMap(width, height);
            Creature rat = new Creature("rat", new int[] { 5, 5 });
            presentCreatures.Add(rat);
            rat.UpdatePosition(this, new int[] { 9, 7 });
            rat.needs.hungerLevel = 2;
           // for(int i = 0; i < 50; i++)
           // {
           //     Creature ratBaby = new Creature("rat", new int[] { 0, 0});
           //     presentCreatures.Add(ratBaby);
           // }
           //
            foreach(Creature creature in presentCreatures)
            {
                creature.UpdatePosition(this, new int[] { rnd.Next(10, width), rnd.Next(10, height) });
            }
        }

        public void DoCreatureActions(Player player)
        {
            foreach(Creature creature in presentCreatures)
            {
                // TODO - this shouldn't take player. AI should find player if player nearby. 
                creature.ai.Update(this, player);
                creature.DecideAction();
            }
        }
        
        /// <summary>
        /// Removes dead creatures from map, and adds a corpse along with any items in the creatures inventory to the inventory of the tile.
        /// </summary>
        /// <param name="map"></param>
        public static void RemoveDeadCreature(Map map, Creature creature)
        {
            
            if (!creature.needs.isAlive)
            {
                Item creatureCorpse = new Item(creature.name + " corpse");

                Tile creatureTile = map.layout[creature.position[0], creature.position[1]];
         
                creatureTile.contentsItems.AddItemToInventory(creatureCorpse);
           
                foreach (KeyValuePair<Item, int> entry in creature.inv.inventory)
                {
                    //TODO This for loop should be replaced with a statement to be written in inventory, which can add multiple items.
                    for (int i = 1; i <= entry.Value; i++)
                    {
                        creatureTile.contentsItems.AddItemToInventory(entry.Key);
                    }
                
                }

                map.presentCreatures.Remove(creature);

                Terrain rat = new Terrain("rat");

                creatureTile.contentsTerrain.Remove(rat);


            }
        }
        

        /// <summary>
        /// Removes an item from a map's tile, and places it in an inventory
        /// </summary>
        /// <param name="item"></param>
        /// <param name="inv"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool GetItemFromMap(string item, Inventory inv, int[] coords)
        {
            foreach (Item mapItem in layout[coords[0], coords[1]].contentsItems.inventory.Keys)
            {
                if (mapItem.name == item)
                {
                    layout[coords[0], coords[1]].contentsItems.RemoveItemFromInventory(mapItem);
                    inv.AddItemToInventory(mapItem);
                    return true;
                }
            }

            return false;
        }

        // TODO this should be moved to some kind of graphics class - in Engine or Utils. Fine here for now.
        /// <summary>
        /// Takes a map and displays it in its entirety, row by row
        /// </summary>
        /// <param name="map"></param>
        public void RenderMap(Map map, Player player)
        {
            int[] playerCoords = player.GetPlayerCoords();

            for (int y = playerCoords[1] + 15; y >= playerCoords[1] - 15; y--)
            {
                string rowString = "";

                // And for each column of that row...
                for (int x = playerCoords[0] - 15; x <= playerCoords[0] + 15; x++)
                {
                    // Add the graphic representation of that tile to a string
                    if (x < 0 || x >= map.layout.GetLength(0) || y < 0 || y >= map.layout.GetLength(1))
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
            // TODO - each piece of terrain/creature/item should have an int indicating its importance and a character to be displayed.
            Terrain wall = new Terrain("wall");
            Terrain floor = new Terrain("floor");
            Terrain playerTerrain = new Terrain("player");            
            Terrain rat = new Terrain("rat");

            if (tile.contentsTerrain.Contains(playerTerrain))
            {
                return "X";
            }
            else if (tile.contentsTerrain.Contains(wall))
            {
                return "#";
            }
            else if (tile.contentsTerrain.Contains(rat))
            {
                return "r";
            }
            else if (tile.contentsItems.inventory.Count > 0)
            {
                return "S";
            }
            else if (tile.contentsTerrain.Contains(floor))
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
                if(failedAttempts == 1000)
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
            while (rnd.Next(1, 11) > 3)
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

        public void WriteToFile()
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\GameLogs\SuburbanMap.txt"))


            for (int y = height; y >= 0; y--)
            {
                string rowString = "";

                // And for each column of that row...
                for (int x = 0; x <= width; x++)
                {
                    // Add the graphic representation of that tile to a string
                    if (x < 0 || x >= this.layout.GetLength(0) || y < 0 || y >= this.layout.GetLength(1))
                    {
                        rowString += " ";
                    }
                    else
                    {
                        rowString += GetTileImage(this.layout[x, y]);
                    }

                }

                // When the row is complete, print it
                file.WriteLine(rowString);
            }
            
        }
    }
}