using System;
using System.Collections.Generic;
using System.Linq;
using digsite.Data;
using digsite.DataServices;
using digsite.Models;

namespace digsite.GameServices
{
    public class MonsterEncounterService
    {
        public List<string> GenerateMonsterEncounters(GameState gameState)
        {
            var monsterTable = GetMonsterTable(gameState);
            var encounteredMonsterIds = monsterTable.Roll();
            var encounteredMonsters = GetMonsters(encounteredMonsterIds).ToList();
            AddEncounteredMonsters(gameState, encounteredMonsters);
            return encounteredMonsters.Select(em => $"A {em.Name} approaches.").ToList();
        }

        private MonsterTable GetMonsterTable(GameState gameState)
        {
            var monsters = new List<MonsterTableMonster>
            {
                new MonsterTableMonster((int)MonsterId.Monster1, 40)
                , new MonsterTableMonster((int)MonsterId.Monster2, 30)
            };
            return new MonsterTable(monsters);
        }

        private IEnumerable<Monster> GetMonsters(List<int> monsterIds)
        {
            var monsterDataService = new MonsterDataService();

            foreach (var monsterId in monsterIds)
            {
                var monster = monsterDataService.Get(monsterId);
                if (monster == null)
                {
                    continue;
                }

                yield return monster;
            }
        }

        private void AddEncounteredMonsters(GameState gameState, List<Monster> monsters)
        {
            monsters.ForEach(m => AddEncounteredMonster(gameState, m));
        }

        private void AddEncounteredMonster(GameState gameState, Monster monster)
        {
            var nearbyMonster = GenerateMonsterFromTemplate(monster);
            gameState.Player.DigState.NearbyMonster.Add(nearbyMonster);
        }

        private NearbyMonster GenerateMonsterFromTemplate(Monster monster)
        {
            return new NearbyMonster()
            {
                CurrentAttackCooldown = monster.AttackRate,
                CurrentHealth = monster.Health,
                MonsterId = monster.MonsterId,
                Monster = monster
            };
        }
    }
}