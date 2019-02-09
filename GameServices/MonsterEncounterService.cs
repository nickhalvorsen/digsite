using System;
using System.Collections.Generic;
using digsite.Data;

namespace digsite.GameServices
{
    public class MonsterEncounterService
    {
        public List<string> GenerateMonsterEncounters(GameState gameState)
        {
            if (!ARandomEncounterOccurred())
            {
                return new List<string>();
            }

            var monster = GenerateRandomMonsterEncounter(gameState);
            gameState.Player.DigState.NearbyMonster.Add(monster);
            return new List<string> { $"A {monster.Monster.Name} approaches."};
        }

        private bool ARandomEncounterOccurred()
        {
            var rand = new Random().Next(1, 4);
            return rand == 1;
        }

        private NearbyMonster GenerateRandomMonsterEncounter(GameState gameState)
        {
            var monsterTemplate = GetBaseMonster();
            return new NearbyMonster()
            {
                CurrentAttackCooldown = monsterTemplate.AttackRate,
                CurrentHealth = monsterTemplate.Health,
                MonsterId = monsterTemplate.MonsterId,
                Monster = monsterTemplate
            };
        }

        private Monster GetBaseMonster()
        {
            return new Monster()
            {
                MonsterId = 1,
                Name = "cat",
                Attack = 1,
                AttackRate = 6,
                Health = 15,
            };
        }
    }
}