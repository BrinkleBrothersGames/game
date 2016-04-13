using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Content.Items;
using SurvivalGame.Content.World;
using SurvivalGame.Engine;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    // TODO - Is this really the gameInit? Refactor?
    // TODO - Aim to do just rendering and i/o in this class
    public class GameInit
    {
        private Map currentLevel;
        private Player player;
        private Clock clock;
        bool run;

        /// <summary>
        /// Game Init constructor class.
        /// </summary>
        /// <param name="currentLevel">The currently loaded Map</param>
        /// <param name="player">The player character</param>
        public GameInit()
        {
            this.currentLevel = new Map();
            Inventory inv = new Inventory();
            this.player = new Player(15, 15, inv);
            this.clock = new Clock(player, currentLevel);
            this.run = true;

            PlayerSetupClass setupPlayer = new PlayerSetupClass();
            setupPlayer.PlayerSetup(player);

            player.UpdatePlayerPosition(currentLevel, player, new Coords(15, 15));
        }

        // TODO - Should probably go in a player control class or something
        // TODO - logic is terrible - should call updatePlayerPosition only once
        public void doPlayerAction(string actionLong)
        {
            // Initialised and assigned here to account for player not taking specific action
            int timeTaken = 0;

            string[] splitAction = actionLong.Split(' ');
            string action = splitAction[0];
            // TODO - change this to 'actionPassed' or something, use for special case for failure of all actions
            bool hasItem;
            bool playerMoves = false;
            Coords newCoords;

            switch (action)
            {
                case ("w"):
                    newCoords = new Coords(player.coords.x, player.coords.y + 1);
                    playerMoves = player.UpdatePlayerPosition(currentLevel, player, newCoords);
                    break;
                case ("s"):
                    newCoords = new Coords(player.coords.x, player.coords.y - 1);
                    playerMoves = player.UpdatePlayerPosition(currentLevel, player, newCoords);
                    break;
                case ("a"):
                    newCoords = new Coords(player.coords.x - 1, player.coords.y);
                    playerMoves = player.UpdatePlayerPosition(currentLevel, player, newCoords);
                    break;
                case ("d"):
                    newCoords = new Coords(player.coords.x + 1, player.coords.y);
                    playerMoves = player.UpdatePlayerPosition(currentLevel, player, newCoords);
                    break;
                // TODO - these 'movement' commands shouldn't work this way. Should call method themselves with the new value
                case ("status"):
                    player.GetStatus();
                    break;
                case ("look"):
                    if (currentLevel.layout[player.coords.x, player.coords.y].contentsItems.inventory.Count > 0)
                    {
                        Console.Write("Here, you can see");
                        foreach (Item mapItem in currentLevel.layout[player.coords.x, player.coords.y].contentsItems.inventory.Keys)
                        {
                            Console.Write(" a " + mapItem.name);
                        }
                        Console.WriteLine(".");
                    }
                    else
                    {
                        Console.WriteLine("You don't see anything nearby.");
                    }
                    break;
                case ("drop"):
                    hasItem = true;

                    foreach (Item invItem in player.inv.inventory.Keys)
                    {
                        if (invItem.name == splitAction[1])
                        {
                            player.inv.RemoveItemFromInventory(invItem);
                            Console.WriteLine("You drop a " + splitAction[1]);
                            currentLevel.layout[player.coords.x, player.coords.y].contentsItems.AddItemToInventory(invItem);
                            hasItem = false;
                            break;
                        }
                    }
                    if (hasItem)
                    {
                        Console.WriteLine("You don't have a " + splitAction[1] + " in your inventory. You don't drop anything.");
                    }
                    break;
                case ("get"):
                    hasItem = true;

                    foreach (Item mapItem in currentLevel.layout[player.coords.x, player.coords.y].contentsItems.inventory.Keys)
                    {
                        if (mapItem.name == splitAction[1])
                        {
                            currentLevel.layout[player.coords.x, player.coords.y].contentsItems.RemoveItemFromInventory(mapItem);
                            Console.WriteLine("You take a " + splitAction[1] + " from the floor.");
                            player.inv.AddItemToInventory(mapItem);
                            hasItem = false;
                            break;
                        }
                    }
                    if (hasItem)
                    {
                        Console.WriteLine("You don't have a " + splitAction[1] + " in your inventory. You don't drop anything.");
                    }
                    break;
                case ("use"):
                case ("eat"):
                case ("drink"):
                    hasItem = true;

                    foreach (ConsumableItem consmItem in player.inv.inventory.Keys)
                    {
                        if (consmItem.name == splitAction[1])
                        {
                            consmItem.OnConsumption(player);
                            hasItem = false;
                            Console.WriteLine("You " + splitAction[0] + " the " + splitAction[1] + ".");
                            break;
                        }
                    }
                    if (hasItem)
                    {
                        Console.WriteLine("You don't have a " + splitAction[1] + " in your inventory.");
                    }
                    break;
                case ("sleep"):
                    // TODO - This needs to be split out into a method
                    // TODO - Get rid of magic numbers here. Should have global constants
                    // TODO - If tiredness above (say) 8, shouldn't be able to sleep.
                    int sleepTime = 10 - player.needs.tirednessLevel;
                    player.needs.UpdateNeeds(10, "tiredness");
                    clock.AddTime(sleepTime * 20);
                    Console.WriteLine("You sleep for " + (sleepTime * 20).ToString() + " time units.");
                    break;
                case ("inv"):
                    player.inv.PrintInventory();
                    break;
                case ("exit"):
                    run = false;
                    break;
                case ("attack"):
                    Fight fight = new Fight();
                    fight.PlayerAttackScreen(player, currentLevel);
                    break;
                default:
                    // TryDebugAction for testing only. Should be removed in release versions.
                    DebugCommands.TryDebugAction(actionLong, clock, player, currentLevel);

                    // Do nothing
                    return;
            }

            // TODO - Ouch! Move the update player position to only where it's relevant!

            if (playerMoves)
            {
                timeTaken = 2;
            }

            clock.AddTime(timeTaken);

        }
        
        static void Main()
        {
            GameInit game = new GameInit();
            string input;

            game.currentLevel.WriteToFile();

            // TODO - Add enumerator for inventory class.

            while (game.run)
            { 
                // TODO - move this to where it makes sense
                // TODO - this shouldn't take player
                // Creatures present on the map make their move
                game.currentLevel.DoCreatureActions(game.player);
                
                // Render changes to map
                // TODO - shouldn't have to pass 'currentLevel' twice
                game.currentLevel.RenderMap(game.currentLevel, game.player);

                // Wait for player to take action
                input = Console.ReadLine();

                // TODO - Need to refactor this and map.render to be somewhere sensible.
                // Clear the console to output results of new action
                Console.Clear();

                game.doPlayerAction(input);

                // If player is dead, end the game
                if (!game.player.IsPlayerAlive())
                {
                    Console.WriteLine("You have died. Well done.");
                    game.run = false;
                    input = Console.ReadLine();
                }
            }
        }

        // TODO - MUCH LATER - For collisions, consider adding 'blockMovement' bool to our place vector.
        // TODO - DURING REFACTOR - Hide everything in get/set methods. 

    }
}