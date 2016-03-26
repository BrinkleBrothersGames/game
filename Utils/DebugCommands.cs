using Game.Content.World;
using SurvivalGame.Content.Characters;
using SurvivalGame.Content.Items;
using SurvivalGame.Engine;
using System;
using System.Collections.Generic;

namespace SurvivalGame.Utils
{
    public class DebugCommands
    {
        // TODO - When calling debug, should start sting with special character. Makes for more efficient checking when we have lots of debug commands

        public static void TryDebugAction(string action, Clock clock, Player player, Map currentLevel)
        {
            string[] splitAction = action.Split(' ');

            if (splitAction[0].Contains("gettime"))
            {
                Console.WriteLine(clock.GetTime().ToString());
            }
            else if (splitAction[0].Contains("addtime"))
            {
                clock.AddTime(int.Parse(splitAction[1]));
                Console.WriteLine("Time updated. New time = " + clock.GetTime().ToString());
            }
            else if ((splitAction[0].Contains("settime")))
            {
                clock.SetTime(int.Parse(splitAction[1]));
                Console.WriteLine("Time set to " + clock.GetTime().ToString());
            }
            else if ((splitAction[0].Contains("sethunger")))
            {
                player.needs.hungerLevel = int.Parse(splitAction[1]);
                Console.WriteLine("Player hunger set to " + player.needs.hungerLevel.ToString());
            }
            else if ((splitAction[0].Contains("setthirst")))
            {
                player.needs.thirstLevel = int.Parse(splitAction[1]);
                Console.WriteLine("Player thirst set to " + player.needs.thirstLevel.ToString());
            }
            else if ((splitAction[0].Contains("settiredness")))
            {
                player.needs.tirednessLevel = int.Parse(splitAction[1]);
                Console.WriteLine("Player tiredness set to " + player.needs.tirednessLevel.ToString());
            }
            else if ((splitAction[0].Contains("sethealth")))
            {
                player.needs.health = int.Parse(splitAction[1]);
                Console.WriteLine("Player health set to " + player.needs.health.ToString());
            }
            else if ((splitAction[0].Contains("runinvtest1")))
            {
                runInvTest1(player);
            }
            else if ((splitAction[0].Contains("runinvtest2")))
            {
                runInvTest2(player);
            }
            else if ((splitAction[0].Contains("givefooditems")))
            {
                GiveFoodItems(player);
            }
        }

        public static void runInvTest1(Player player)
        {
            Item testItem1 = new Item("TestItem1");
            Item testItem2 = new Item("TestItem2");
            Item testItem3 = new Item("TestItem1");

            player.inv.AddItemToInventory(testItem1);
            player.inv.AddItemToInventory(testItem2);
            player.inv.AddItemToInventory(testItem1);
            player.inv.AddItemToInventory(testItem2);
            player.inv.AddItemToInventory(testItem3);
        }

        public static void runInvTest2(Player player)
        {
            Item testItem1 = new Item("TestItem1");
            Item testItem2 = new Item("TestItem2");
            Item testItem3 = new Item("TestItem1");

            player.inv.RemoveItemFromInventory(testItem1);
            player.inv.RemoveItemFromInventory(testItem2);

        }

        public static void GiveFoodItems(Player player)
        {
            Dictionary<string, int> needsChangeBurger = new Dictionary<string, int>()
            {
                {"hunger", 5}
            };
            Dictionary<string, int> needsChangeBeer = new Dictionary<string, int>()
            {
                {"tiredness", -1},
                {"thirst", 5}
            };
            Dictionary<string, int> needsChangeCoffee = new Dictionary<string, int>()
            {
                {"tiredness", 2},
                {"thirst", 2}
            };
            Dictionary<string, int> needsChangeMedicine = new Dictionary<string, int>()
            {
                {"health", 5}
            };

            ConsumableItem burger = new ConsumableItem("burger", needsChangeBurger);
            ConsumableItem beer = new ConsumableItem("beer", needsChangeBeer);
            ConsumableItem coffee = new ConsumableItem("coffee", needsChangeCoffee);
            ConsumableItem medicine = new ConsumableItem("medicine", needsChangeMedicine);

            player.inv.AddItemToInventory(burger);
            player.inv.AddItemToInventory(beer);
            player.inv.AddItemToInventory(coffee);
            player.inv.AddItemToInventory(medicine);
            player.inv.AddItemToInventory(burger);
            player.inv.AddItemToInventory(beer);
            player.inv.AddItemToInventory(coffee);
            player.inv.AddItemToInventory(medicine);
            player.inv.AddItemToInventory(burger);
            player.inv.AddItemToInventory(beer);
            player.inv.AddItemToInventory(coffee);
            player.inv.AddItemToInventory(medicine);
            player.inv.AddItemToInventory(burger);
            player.inv.AddItemToInventory(beer);
            player.inv.AddItemToInventory(coffee);
            player.inv.AddItemToInventory(medicine);
        }
    }
}

