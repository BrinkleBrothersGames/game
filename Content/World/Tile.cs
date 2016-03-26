using SurvivalGame.Content.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO - Go around and clean up these unused usings for all classes
namespace SurvivalGame.Content.World
{
    public class Tile
    {
        // Probably want to change this to 'thing' class, which is implemented by Object and Terrain classes.
        public List<string> contents = new List<string>();
        public Inventory contentsItems = new Inventory();
        public bool blocksMovement = false;

        Tile(List<string> content)
        {
            this.contents = content;
        }

        // TODO - We need to think carefully about this method and CreateFloorTile. Do we definitely want them, or should something else replace them?
        /// <summary>
        /// Creates a wall tile. 
        /// </summary>
        /// <returns></returns>
        public static Tile CreateWallTile()
        { 
            List<string> wallTileContents= new List<string>(new string[] { "wall" });
            Tile wallTile = new Tile(wallTileContents);
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
            List<string> floorTileContents = new List<string>(new string[] { "floor" });
            Tile floorTile = new Tile(floorTileContents);
            return floorTile;
        }

        /// <summary>
        /// Adds content to the contents of a Tile
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Tile AddContentToTile(Tile tile, string content)
        {
            tile.contents.Add(content);
            return tile;       
        }

        public void BlockMovement()
        {
            this.blocksMovement = true;
        }
    }
}
