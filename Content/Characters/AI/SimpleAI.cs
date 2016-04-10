﻿using Game.Content.World;
using SurvivalGame.Content.Items;
using SurvivalGame.Content.World;
using SurvivalGame.Content.World.TerrainTypes;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters.AI
{    

    // TODO - For more advanced AI capable of better route-finding, should store a list of squares the creature has previously visited. This would be its 'memory', and could help avoid repeated movements. Length of memory should be based on creature's intelligence.
    // TODO - Useful methods would include: coord creator in MapUtils; perceivableTiles -> map.layout translator; IsValidMove(int[], map),  two functions (?) determine whether a square is in the bounds of the map and another one determines whether a square blockMovement
    // The Simple AI class represents a dumb animal. It will seek safety, then to satisfy its needs. No complex or interesting behaviour.
    public class SimpleAI
    {        
        // TODO - these contants should be replaced with values derived from stats, when stats are implemented.
        int creatureIntelligence = 20; // Used in pathfinding
        int curiosity = 2; // Used in determining liklihood of movement when idle (how likely the monster is to wander around when 'bored'). Should be between 0/10. May need to think more carefully about this in future.
        int perception = 5;
        bool aggressive = true;

        int[] targetCoords; // The coords of the AI's current target. Can be an enemy to flee from/attack or food/drink, etc.
        List<Coords> adjacentTiles;
        List<int[]> previouslyVisitedTiles = new List<int[]>(); // This should be much more powerful: maybe a weighting system that adds tiles the creature has visited often to its permenant memory?
        Tile[,] perceivableTiles;
        Dictionary<int[], List<ConsumableItem>> perceivableConsumableItems = new Dictionary<int[], List<ConsumableItem>>();
        Creature creature;
        // TODO - Replace wich 'character', which can be player or creature.
        Player player;
        Map map;
        Random rnd = new Random(Map.SEED);
        

        public SimpleAI(Creature creature)
        {
            this.creature = creature;
        }

        public void Update(Creature creature)
        {
            this.creature = creature;
        }

        public void Update(Map map, Player player)
        {
            this.map = map;
            this.player = player;
        }

        // Core logic for choosing an action
        public void ActionSelection()
        {
            // We need to know what the creature is aware of. Its actions will depend on this.
            perceivableTiles = GetTilesInPerceptionRange();
            adjacentTiles = MapUtils.GetAdjacentTiles(perceivableTiles, new Coords(perceivableTiles.GetLength(0) / 2, perceivableTiles.GetLength(1) / 2 ));
            perceivableConsumableItems = GetPerceivableConsumableItems();

            // First it prioritises its safety. If it's in serious danger, or is hurt and is in less danger, it should flee. If it can't flee, it should fight.


            if (aggressive)
            {
                // TODO - here, should check if it can see the player. If not, move down priority. Use 'IsThreatened'
                Fight();
            }
            else if (IsInDanger() || (IsInjured() && IsThreatened()))
            {
                if (CanFlee())
                {
                    Flee();
                }
                else
                {
                    Fight();
                }
            }
            else if (IsThirsty()) // If thirsty, seek a drink
            {
                SeekNeed("thirst");
            }
            else
            if (IsHungry()) // If hungry, seek food
            {
                SeekNeed("hunger");
            }
            else if (IsTired() && !IsThreatened()) // If tired and safe, sleep
            {
                Sleep();
            }
            else
            {
                Idle();
            }
        }

        // The following methods are use by the intelligence to determine actions. They represent its knowledge of the world.

        // Returns true if a 'threat' is within a creature's comfort range.
        public bool IsInDanger()
        {
            // TODO - replace magic string with something determined from a stat the creature has. Replace magic 'threat' with some kind of threat list generated by creature
            int comfortRange = 2;
            Terrain threat = new Terrain("player");

            for (int y = (comfortRange * 2); y > 0; y--)
            {
                for (int x = 0; x <= (comfortRange * 2); x++)
                {
                    if (perceivableTiles[(perceivableTiles.GetLength(0) / 2) + x - comfortRange, (perceivableTiles.GetLength(1) / 2) + y - comfortRange]!=null && perceivableTiles[(perceivableTiles.GetLength(0) / 2) + x - comfortRange, (perceivableTiles.GetLength(1) / 2) + y - comfortRange].contentsTerrain.Contains(threat))
                    {
                        // Converts x and y to values in the creature's perception range, then sets those as its 'targetCoords'
                        targetCoords = new int[] { (perceivableTiles.GetLength(0) / 2) + x - comfortRange, (perceivableTiles.GetLength(1) / 2) + y - comfortRange };
                        return true;
                    }
                }
            }
            
            return false;
        }

        // Returns true if a 'threat' is within the creature's perception range.
        public bool IsThreatened()
        {
            Terrain threat = new Terrain("player");
            int perceptionRange = perceivableTiles.GetLength(0)/2;

            for (int y = (perceptionRange * 2); y > 0; y--)
            {
                for (int x = 0; x <= (perceptionRange * 2); x++)
                {
                    if ((perceivableTiles[x, y] != null) && perceivableTiles[x, y].contentsTerrain.Contains(threat))
                    {
                        targetCoords = new int[] { x, y};
                        return true;
                    }
                }
            }
            return false;
        }

        // Checks if a movement to an adjacent square moves creature away from a threat. Returns true if a square exists that doesn't block movement and doesn't contain the threat
        public bool CanFlee()
        {
            Terrain threat = new Terrain("player");
            int[] creatureCoords = new int[] { (perceivableTiles.GetLength(0) / 2), (perceivableTiles.GetLength(0) / 2) };
            int currentThreatDistance = MapUtils.GetAbsoluteDistanceBetweenTwoPoints(creatureCoords, targetCoords);
            int newThreatDistance = currentThreatDistance;

            foreach(Coords tileCoords in adjacentTiles)
            {
                newThreatDistance = MapUtils.GetAbsoluteDistanceBetweenTwoPoints(tileCoords.GetCoordsArray(), targetCoords);
                
                if((perceivableTiles[tileCoords.x, tileCoords.y]!=null) && !(perceivableTiles[tileCoords.x, tileCoords.y].blocksMovement) && (newThreatDistance > currentThreatDistance))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool CanPerceiveConsumable(string need)
        {
            foreach(List<ConsumableItem> consItemList in perceivableConsumableItems.Values)
            {
                foreach (ConsumableItem consItem in consItemList)
                {
                    if (consItem.GetNeedValue(need) > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsInjured()
        {
            // TODO - REMOVE MAGIC STRINGS! COME ON!
            if(creature.needs.health < 4)
            {
                return true;
            }
            return false;
        }

        public bool IsThirsty()
        {
            // TODO - MAGIC STRINGS! BEGONE!
            if(creature.needs.thirstLevel < 5)
            {
                return true;
            }
            return false;
        }

        public bool IsHungry()
        {
            // TODO - Clear magic string
            if(creature.needs.hungerLevel < 5)
            {
                return true;
            }
            return false;
        }

        public bool IsTired()
        {
            // TODO - Get rid of magic string
            if(creature.needs.tirednessLevel < 4)
            {
                return true;
            }
            return false;
        }

        // The following methods are used to gather information about the world
        // TODO - perception range shouldn't passed in, it should be known/calculated from the creature itself
        public Tile[,] GetTilesInPerceptionRange()
        {
            int perceptionRange = perception;

            Tile[,] perceivableTiles = new Tile[(perceptionRange * 2) + 1, (perceptionRange * 2) + 1];

            // Loop over tiles within perception range and add them to a smaller grid, for use in decision making
            for (int y = (perceptionRange * 2); y > 0; y--)
            {
                for (int x = 0; x <= (perceptionRange * 2); x++)
                {
                    if (!(creature.position[0] + x - perceptionRange < 0 || creature.position[0] + x - perceptionRange >= map.layout.GetLength(0) || (creature.position[1] + y - perceptionRange) < 0 || (creature.position[1] + y - perceptionRange) >= map.layout.GetLength(1)))
                    {
                        perceivableTiles[x, y] = map.layout[(creature.position[0] + x - perceptionRange), (creature.position[1] + y - perceptionRange)];
                    }
                }
            }

            return perceivableTiles;
        }

        // TODO - decide whether this should be put together with 'getTilesInPerceptionRange'. Is it better to do this every time, or just when it's needed?
        public Dictionary<int[], List<ConsumableItem>> GetPerceivableConsumableItems()
        {
            Dictionary<int[], List<ConsumableItem>> perceivableItems = new Dictionary<int[], List<ConsumableItem>>();

            // Foreach nonempty tile the creature can perceive...
            for (int y = perceivableTiles.GetLength(1) - 1 ; y > 0; y--)
            {
                for (int x = 0; x <= perceivableTiles.GetLength(0) - 1; x++)
                {
                    if (perceivableTiles[x, y] != null)
                    {
                        List<ConsumableItem> perceivableItemsList = new List<ConsumableItem>();

                        foreach (KeyValuePair<Item, int> itemNumberPair in perceivableTiles[x, y].contentsItems.inventory)
                        {
                            // ... add the tile's contents and its location to our dictionary.
                            if (itemNumberPair.Key.GetType() == typeof(ConsumableItem))
                            {
                                perceivableItemsList.Add(itemNumberPair.Key as ConsumableItem);
                            }
                        }

                        if(perceivableItemsList.Count > 0)
                        {
                            perceivableItems.Add(new int[] { x, y }, perceivableItemsList);
                        }
                    }
                }
            }
            
            return perceivableItems;
        }

        public void UpdatePreviouslyVisitedTiles(int[] newlyVisited)
        {
            previouslyVisitedTiles.Add(newlyVisited);

            // If more tiles are in the list than the creature can remember, forget the oldest (ie first) tile
            if(previouslyVisitedTiles.Count > creatureIntelligence)
            {
                previouslyVisitedTiles.RemoveAt(0);
            }
        }
        
        // The following methods are the actions the AI will undertake.
        public void Flee()
        {
            Terrain threat = new Terrain("player");
            int[] creatureCoords = new int[] { (perceivableTiles.GetLength(0) / 2), (perceivableTiles.GetLength(1) / 2) };
            int currentThreatDistance = MapUtils.GetAbsoluteDistanceBetweenTwoPoints(creatureCoords, targetCoords);
            int newThreatDistance = currentThreatDistance;
            
            // TODO - This randomises the order of a list to ensure more random movement of creatures. This should be in a utils class
            int n = adjacentTiles.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Coords value = adjacentTiles[k];
                adjacentTiles[k] = adjacentTiles[n];
                adjacentTiles[n] = value;
            }

                foreach (Coords tileCoords in adjacentTiles)
            {
                newThreatDistance = MapUtils.GetAbsoluteDistanceBetweenTwoPoints(tileCoords.GetCoordsArray(), targetCoords);

                if ((perceivableTiles[tileCoords.x, tileCoords.y]!=null) && !(perceivableTiles[tileCoords.x, tileCoords.y].blocksMovement) && (newThreatDistance > currentThreatDistance))
                {
                    int[] newPosition = new int[] { creature.position[0] + (tileCoords.x - ((perceivableTiles.GetLength(0)) / 2)), creature.position[1] + (tileCoords.y - ((perceivableTiles.GetLength(1)) / 2)) };
                    creature.UpdatePosition(map, newPosition);
                    return;
                }
            }
        }
        
        /// <summary>
        /// When the creature chooses to fight, it will attack if possible, otherwise it will move towards its target (the player).
        /// </summary>
        /// <param name="targetCoords">Coords of the target to attack</param>
        public void Fight()
        {        
            Coords creatureCoords = new Coords(creature.position);
            Coords playerCoords = new Coords(player.GetPlayerCoords());

            bool playerIsAdj = false;

            List<Coords> adjacentTilesToCreature = MapUtils.GetAdjacentTiles(map.layout, creatureCoords);

            foreach (Coords coord in adjacentTilesToCreature)
            {
                if (coord.Equals(playerCoords))
                {
                    playerIsAdj = true;
                }
            }
            
            if (playerIsAdj)
            {
                Fight fight = new Fight();
                fight.CreatureAttackPlayer(creature, player);
            }
            else
            {
                // TODO - Create some method to do this. Too hacky
                if(Math.Abs(playerCoords.x - creatureCoords.x) >= 5 || Math.Abs(playerCoords.y - creatureCoords.y) >= 5)
                {
                    Wander();
                }
                else
                {
                    targetCoords = new Coords(Math.Abs(playerCoords.x - creatureCoords.x) + 5, Math.Abs(playerCoords.y - creatureCoords.y) + 5).GetCoordsArray();

                    MoveToTarget();
                }
            }
                
        }

        public void SeekNeed(string need)
        {            
            ConsumableItem needItem = null;
            int targetDistance = 1000; // initialise to some high number
            int[] nearestCoords = null;

            foreach (KeyValuePair<int[], List<ConsumableItem>> dictPair in perceivableConsumableItems)
            {
                foreach (ConsumableItem consumable in dictPair.Value)
                {
                    if (consumable.GetNeedValue(need) > 0)
                    {
                        int thisTargetDistance = MapUtils.GetAbsoluteDistanceBetweenTwoPoints(creature.position, MapUtils.ConvertPerceivableMapCoordsToMapCoords(dictPair.Key, perceivableTiles, map, creature.position));

                        // If we find a closer food item
                        if (thisTargetDistance < targetDistance)
                        {
                            targetDistance = thisTargetDistance;
                            nearestCoords = dictPair.Key;
                            needItem = consumable;
                        }
                    }
                }
            }

            // If creature is close enough, fulfil the need
            if(targetDistance <= 1)
            {
                map.GetItemFromMap(needItem.name, creature.inv, MapUtils.ConvertPerceivableMapCoordsToMapCoords(nearestCoords, perceivableTiles, map, creature.position));
                (creature.inv.GetItemByName(needItem.name) as ConsumableItem).OnConsumption(creature);
                return;
            }

            // No close item was found, just wander
            // TODO - replace this with behaviour that actively looks to explore new tiles
            if(targetDistance == 1000)
            {
                Wander();
                return;
            }

            targetCoords = nearestCoords;
            MoveToTarget();
        }

        public void Sleep()
        {
            // TODO - need to design a sensible way for this to happen
            creature.needs.tirednessLevel += 5;
        }

        public void Idle()
        {
            // TODO - for now this is random, but could be deterministic? Consider this
            int idleAction = rnd.Next(1, 11);

            // If random number greater than creature's curiosity, they don't wander. Do nothing.
            // TODO - Maybe this shouldn't be curiosity... would something like 'activeness' be more appropes?
            if (idleAction > curiosity)
            {
                // Do nothing
                return;
            }
            else
            {
                // Creature wanders around.
                Wander();
            }
        }

        public void Wander()
        {
            Dictionary<int[], int> weightDictionary = new Dictionary<int[], int>();

            // Weights how much the creature should prefer each adjacent tile
            foreach (Coords tileCoords in adjacentTiles)
            {
                // TODO - this needs testing to ensure it actually works. Rats don't seem to particularly stick to their curiosity.
                int weight = 0;

                if ((perceivableTiles[tileCoords.x, tileCoords.y] != null) && (!perceivableTiles[tileCoords.x, tileCoords.y].blocksMovement))
                {
                    // If current tile has been visited recently, weigh more strongly for less curious creatures.
                    if (previouslyVisitedTiles.Contains(MapUtils.ConvertPerceivableMapCoordsToMapCoords(tileCoords.GetCoordsArray(), perceivableTiles, map, creature.position)))
                    {
                        // TODO - the 10 should be set in a global variable as "MAX_CURIOSITY" or something. May want to do this differently.
                        weight += 2*(10 - curiosity);
                    }
                    else // If tile not visited before, weight more strongly if creature is very curious
                    {
                        weight += curiosity;
                    }

                    weightDictionary.Add(tileCoords.GetCoordsArray(), weight);
                }
            }

            var list = weightDictionary.Keys.ToList();
            // Get the total weighting given to all adjacent squares, then generate a number between 1 and this.
            int totalWeight = list.Sum(x => weightDictionary[x]);
            int directionChosen = rnd.Next(1, totalWeight + 1);

            foreach(int[] tile in list)
            {
                // Take the weight of the current square from our generated number
                directionChosen -= weightDictionary[tile];
                    
                if (directionChosen <= 0) // Then this is the direction we move in. Do so and return.
                {
                    int[] newCoords = MapUtils.ConvertPerceivableMapCoordsToMapCoords(tile, perceivableTiles, map, creature.position);

                    creature.UpdatePosition(map, newCoords);
                    UpdatePreviouslyVisitedTiles(newCoords);
                    return;
                }
            }
            
        }

        public void MoveToTarget()
        {
            int BIG_NUMBER = 1000;
            // Intialise to really big number. May want to do this more sentibly in future (TODO?)
            int currentDistanceToTarget = BIG_NUMBER;
            // TODO - remove magic strings!
            int[] moveCoords = new int[] { 5, 5 };
            Dictionary<Coords, int> dMap = MapUtils.GetDijkstraMap(perceivableTiles, targetCoords);
            
            foreach (Coords tileCoords in adjacentTiles)
            {
                if (dMap.ContainsKey(tileCoords) && dMap[tileCoords] < currentDistanceToTarget)
                {
                    // TODO - Replace this with the Coords logic
                    moveCoords = new int[] { tileCoords.x, tileCoords.y };
                    currentDistanceToTarget = dMap[tileCoords];
                }
            }

            if(currentDistanceToTarget != BIG_NUMBER)
            {
                creature.UpdatePosition(map, MapUtils.ConvertPerceivableMapCoordsToMapCoords(moveCoords, perceivableTiles, map, creature.position));
            }
            else
            {
                Wander();
            }
        }
    }
}
