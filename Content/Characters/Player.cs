using Game.Content.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    class Player
    {
        int playerXCoord;
        int playerYCoord;

        public Player()
        {
        }

        public Player(int x, int y)
        {
            this.playerXCoord = x;
            this.playerYCoord = y;
        }

        public void setPlayerCoords(int x, int y)
        {
            this.playerXCoord = x;
            this.playerYCoord = y;
        }

        // TODO - should implement 0-1-infinity rule here too
        public void setPlayerCoords(int[] coords)
        {
            this.playerXCoord = coords[0];
            this.playerYCoord = coords[1];
        }

        public int[] getPlayerCoords()
        {
            int[] playerCoords = new int[2];
            playerCoords[0] = playerXCoord;
            playerCoords[1] = playerYCoord;

            return playerCoords;
        }

        public void moveUp()
        {
            this.playerYCoord += 1;
        }
        public void moveDown()
        {
            this.playerYCoord -= 1;
        }
        public void moveRight()
        {
            this.playerXCoord += 1;
        }
        public void moveLeft()
        {
            this.playerXCoord -= 1;
        }

        public void updatePlayerPosition(Map map, Player player, int[] newCoords)
        {
            // Remove the player from their old position on the map
            map.layout[player.playerXCoord, player.playerYCoord].contents.Remove("player");

            // Set the player's coords to their new position
            player.setPlayerCoords(newCoords);

            // Update map to reflect new position

            map.layout[player.playerXCoord, player.playerYCoord].contents.Add("player");
        }
    }
}
