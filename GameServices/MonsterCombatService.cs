using System;
using System.Collections.Generic;
using System.Linq;
using digsite.Data;

namespace digsite.GameServices
{
    public class MonsterCombatService
    {
        public List<string> HandleMonsterDeaths(GameState gameState)
        {
            var dead = gameState.Player.DigState.NearbyMonster.Where(nm => nm.CurrentHealth <= 0).ToList();

            foreach (var d in dead)
            {
                gameState.Player.DigState.NearbyMonster.Remove(d);
            }

            return dead.Select(d => $"{d.Monster.Name} was defeated.").ToList();
        }

        public List<string> HandleMonsterAttacks(GameState gameState)
        {
            var attackers = GetMonstersThatWillAttack(gameState);
            var messages = attackers.Select(a => PerformAttack(gameState, a)).ToList();
            MonstersCooldownTick(gameState);
            return messages;
        }

        private List<NearbyMonster> GetMonstersThatWillAttack(GameState gameState)
        {
            return gameState.Player.DigState.NearbyMonster.Where(m => m.CurrentAttackCooldown == 0).ToList();
        }

        private string PerformAttack(GameState gameState, NearbyMonster monster)
        {
            var damage = RollForDamage(monster);
            gameState.Player.DigState.Fuel -= damage;
            monster.CurrentAttackCooldown = monster.Monster.AttackRate;
            return $"{monster.Monster.Name} hits for {damage}.";
        }

        private int RollForDamage(NearbyMonster monster)
        {
            return new Random().Next(1, monster.Monster.Attack);
        }

        private void MonstersCooldownTick(GameState gameState)
        {
            gameState.Player.DigState.NearbyMonster.ToList().ForEach(m => m.CurrentAttackCooldown--);
        }
    }
}