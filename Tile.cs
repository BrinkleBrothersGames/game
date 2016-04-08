using SurvivalGame.Content.Items;
using SurvivalGame.Content.World.TerrainTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

// TODO - Go around and clean up these unused usings for all classes
namespace SurvivalGame.Content.World
{
    public class Tile
    {
        // Probably want to change this to 'thing' class, which is implemented by Object and Terrain classes.
        public Inventory contentsItems = new Inventory();
        public bool blocksMovement = false;
        public List<Terrain> contentsTerrain = new List<Terrain>{ new Terrain("floor") };

        Tile()
        {
        }

        Tile(List<Terrain> contentsTerrain, Inventory contentsItems)
        { 
            this.contentsTerrain = contentsTerrain;
            this.contentsItems = contentsItems;
        }

        // TODO - We need to think carefully about this method and CreateFloorTile. Do we definitely want them, or should something else replace them?
        /// <summary>
        /// Creates a wall tile. 
        /// </summary>
        /// <returns></returns>
        public static Tile CreateWallTile()
        {
            Terrain wall = new Terrain("wall");
            Tile wallTile = new Tile();
            wallTile.contentsTerrain.Add(wall);
            wallTile.BlockMovement();
            return wallTile;
        }

        // TODO - Do we really want this to exist?
        /// <summary>
        /// Creates an empty floor tile.
        /// </summary>
        /// <returns></returns>
        public static Tile CreateFloorTile()
        {
            Tile floorTile = new Tile();
            return floorTile;
        }

        /// <summary>
        /// Adds content to the contents of a Tile
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Tile AddContentToTile(Tile tile, Terrain contentTerrain, Item contentItem)
        {
            tile.contentsTerrain.Add(contentTerrain);
            tile.contentsItems.AddItemToInventory(contentItem);
            return tile;       
        }

        public Tile RemoveContentFromTile(Tile tile, Terrain contentTerrain, Item contentItem)
        {
            tile.contentsTerrain.Remove(contentTerrain);
            tile.contentsItems.RemoveItemFromInventory(contentItem);
            return tile;
        }

        public void BlockMovement()
        {
            this.blocksMovement = true;
        }
    }
}