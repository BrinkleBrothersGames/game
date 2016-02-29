using Game.Content.World;
using SurvivalGame.Content.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    // TODO - Is this really the gameInit? Refactor?
    // TODO - Aim to do just rendering and i/o in this class
    // TODO - Split out player character - have their location tracked separately to map...
    //              ...to more easily center map in future builds
    class GameInit
    {
        private Map currentLevel;
        private Player player;
        bool run;

        /// <summary>
        /// Game Init constructor class.
        /// </summary>
        /// <param name="currentLevel">The currently loaded Map</param>
        /// <param name="player">The player character</param>
        GameInit(Map currentLevel, Player player)
        {
            this.currentLevel = currentLevel;
            this.player = player;
        }

        // TODO - Should probably go in a player control class or something
        // TODO - logic is terrible - should call updatePlayerPosition only once
        public void doPlayerAction(string action)
        {
            int[] playerCoords = player.getPlayerCoords();

            switch (action)
            {
                case ("w"):
                    playerCoords[0] -= 1;
                    player.updatePlayerPosition(currentLevel, player, playerCoords);
                    break;
                case ("s"):
                    playerCoords[0] += 1;
                    player.updatePlayerPosition(currentLevel, player, playerCoords);
                    break;
                case ("a"):
                    playerCoords[1] -= 1;
                    player.updatePlayerPosition(currentLevel, player, playerCoords);
                    break;
                case ("d"):
                    playerCoords[1] += 1;
                    player.updatePlayerPosition(currentLevel, player, playerCoords);
                    break;
                case ("exit"):
                    run = false;
                    break;
                default:
                    break;
            }
        }

        static void Main()
        {
            Map currentLevel = new Map();
            Player player = new Player();
            GameInit game = new GameInit(currentLevel, player);
            game.run = true;
            string input;

            player.updatePlayerPosition(currentLevel, player, new int[] { 5, 5 });

            while (game.run)
            {
                // Clear old map
                Console.Clear();

                // Render changes to map
                // TODO - shouldn't have to pass 'currentLevel' twice
                currentLevel.RenderMap(currentLevel);

                // Wait for player to take action
                input = Console.ReadLine();
                game.doPlayerAction(input);

                // Update map

            }
        }

// TODO - MUCH LATER - For collisions, consider adding 'blockMovement' bool to our place vector.
// TODO - DURING REFACTOR - Hide everything in get/set methods. 

    }
}