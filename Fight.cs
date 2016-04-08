using Game.Content.World;
using SurvivalGame.Content.World;
using SurvivalGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurvivalGame.Content.Characters
{
    class Fight
    {
        Player player;
        Creature creature1;
        Creature creature2;
        List<Creature> adjacentCreatures;
        float playerDamageModifier = 0.5f;
        float creature1DamageModifier = 0.95f;
        float creature2DamageModifier = 0.9f;
        float playerHitModifier = 1.1f;
        float creature1HitModifier = 0.9f;
        float creature2HitModifier = 1.2f;
        Random r = new Random();
             
        public Fight()
        {

        }
        

        bool PlayerHitsCreature()
        {
            if (creature1.stats.agility * creature1HitModifier * 2 * r.NextDouble() > player.stats.agility * playerHitModifier)
            {
                return false;
            }
            return true;
        }

        bool CreatureHitsPlayer()
        {
            if (player.stats.agility * playerHitModifier * 2 * r.NextDouble() > creature1.stats.agility * creature1HitModifier)
            {
                return false;
            }
            return true;
        }

        bool Creature1HitsCreature2()
        {
            if (creature2.stats.agility * creature2HitModifier * 2 * r.NextDouble() > creature1.stats.agility * creature1HitModifier)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Player attacks creature.
        /// </summary>
        public void PlayerAttackCreature(Player player, Creature creature, Map map)
        {
            this.player = player;
            this.creature1 = creature;

            if (PlayerHitsCreature())
            {
                int damageDone = (Convert.ToInt32(player.stats.strength * playerDamageModifier));
                creature1.needs.UpdateCreatureHealth(-damageDone, map, creature);

                Console.WriteLine("You do " + damageDone + " damage to the " + creature.name);
            }
            else
            {
                Console.WriteLine("You miss the " + creature.name);
            }

        }

        public void CreatureAttackPlayer(Creature creature, Player player)
        {
            this.player = player;
            creature1 = creature;

            if (CreatureHitsPlayer())
            {
                int damageDone = (Convert.ToInt32(creature1.stats.strength * creature1DamageModifier));
                player.needs.UpdateHealth(Convert.ToInt32(creature1.stats.strength * creature1DamageModifier));

                Console.WriteLine(creature.name + " does " + damageDone + " damage to the you");
            }
            else
            {
                Console.WriteLine(creature.name + " attacks you but misses.");
            }
        }

        public void Creature1AttackCreature2(Creature creature1, Creature creature2)
        {
            this.creature1 = creature1;
            this.creature2 = creature2;

            if (Creature1HitsCreature2())
            {
                creature2.needs.UpdateHealth(Convert.ToInt32(creature1.stats.strength * creature1DamageModifier));
            }
        }

        /// <summary>
        /// If more than one enemy, asks player which it wants to attack, if one then automatically attacks, or if none then tells player an attack is impossible.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="map"></param>
        public void PlayerAttackScreen(Player player, Map map)
        {
            this.player = player;
            Coords playerCoords = new Coords(player.GetPlayerCoords());

            List<Creature> adjacentCreatures = MapUtils.GetAdjacentCreatures(map, playerCoords);

            if (adjacentCreatures.Count() > 1)
            {
                string inputCreatureNumber;
                int creatureNumber;
                
                Console.WriteLine("Which creature do you want to attack?");

                for (int i = 1; i <= adjacentCreatures.Count(); i++)
                {
                    Console.WriteLine(i + ". The " + adjacentCreatures[i] + " ...some other identifier.");
                }

                inputCreatureNumber = Console.ReadLine();
                bool invalidInputCreatureNumber = !(int.TryParse(inputCreatureNumber, out creatureNumber));

                while (invalidInputCreatureNumber)
                {
                    Console.WriteLine("Invalid input. Type creature number.");
                    inputCreatureNumber = Console.ReadLine();
                    invalidInputCreatureNumber = !(int.TryParse(inputCreatureNumber, out creatureNumber));
                }
                                
                PlayerAttackCreature(player, adjacentCreatures[creatureNumber], map);                
            }

            else if (adjacentCreatures.Count() == 1)
            {
                PlayerAttackCreature(player, adjacentCreatures[0], map);
            }

            else
            {
                Console.WriteLine("There are no creatures to attack");
            }
        }

    }

}
