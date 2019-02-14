using System;
using System.Collections.Generic;
using System.Linq;
using digsite.Data;

namespace digsite.Models
{
    public class MonsterTable
    {
        public MonsterTable(List<MonsterTableMonster> monsters)
        {
            _monsters = monsters;
        }
        private List<MonsterTableMonster> _monsters { get; set; }
        public List<int> Roll() 
        {
            if (!_monsters.Any() || !RollForEncounter())
            {
                return new List<int>();
            }            

            return new List<int>
            {
                RollForMonster()
            };
        }

        private bool RollForEncounter()
        {
            return new Random().Next(1, 10) == 5;
        }

        private int RollForMonster()
        {
            var totalRarityValue = _monsters.Sum(i => i.Rarity);
            var roll = new Random().Next(1, totalRarityValue);

            foreach (var monster in _monsters)
            {
                roll -= monster.Rarity;
                if (roll <= 0)
                {
                    return monster.MonsterId;
                }
            }

            // This shouldn't happen, but if it fails,
            // just try again instead of stopping the game
            return RollForMonster(); 
        }
    }
}