using System.Collections.Generic;
using System.Linq;
using digsite.Data;
using digsite.Enum;

namespace digsite.GameServices
{
    public class BuffService
    {
        public List<string> HandlePlayerBuffs(GameState gameState)
        {
            var messages = gameState.Player.PlayerBuff.ToList().SelectMany(b => HandlePlayerBuff(gameState, b)).ToList();
            gameState.Player.PlayerBuff.ToList().ForEach(b => b.RemainingDuration--);
            RemoveExpiredPlayerBuffs(gameState);

            return messages;
        }

        private List<string> HandlePlayerBuff(GameState gameState, PlayerBuff buff)
        {
            switch (buff.BuffId)
            {
                case (int)BuffId.Healing: 
                    return HealPlayer(gameState);
                case (int)BuffId.OnFire:
                    return PlayerOnFire(gameState);
                default:
                    return new List<string>();
            }
        }

        private List<string> HealPlayer(GameState gameState)
        {
            gameState.Player.DigState.Fuel++;
            return new List<string> { "The player gets healed for 1." };
        }
        
        private List<string> PlayerOnFire(GameState gameState)
        {
            gameState.Player.DigState.Fuel--;
            return new List<string> { "The player takes fire damage." };
        }

        private void RemoveExpiredPlayerBuffs(GameState gameState)
        {
            var buffsToRemove = gameState.Player.PlayerBuff.Where(pb => pb.RemainingDuration <= 0);
            foreach (var buff in buffsToRemove)
            {
                gameState.Player.PlayerBuff.Remove(buff);
            }
        }

        public List<string> HandleMonsterBuffs(GameState gameState)
        {
            return gameState.Player.DigState.NearbyMonster.ToList().SelectMany(nm => HandleMonsterBuffs(gameState, nm)).ToList();
        }

        private List<string> HandleMonsterBuffs(GameState gameState, NearbyMonster monster)
        {
            return monster.NearbyMonsterBuff.ToList().SelectMany(b => HandleMonsterBuffs(gameState, monster, b)).ToList();
        }

        private List<string> HandleMonsterBuffs(GameState gameState, NearbyMonster monster, NearbyMonsterBuff buff)
        {
            switch (buff.BuffId)
            {
                case (int)BuffId.Healing: 
                    return HealMonster(monster);
                case (int)BuffId.OnFire:
                    return MonsterOnFire(monster);
                default:
                    return new List<string>();
            }
        }

        private List<string> HealMonster(NearbyMonster monster)
        {
            monster.CurrentHealth++;
            if (monster.CurrentHealth > monster.Monster.Health)
            {
                monster.CurrentHealth = monster.Monster.Health;
            }

            return new List<string> { $"The {monster.Monster.Name} gets healed for 1." };
        }

        private List<string> MonsterOnFire(NearbyMonster monster)
        {
            monster.CurrentHealth--;
            return new List<string> { $"The {monster.Monster.Name} takes 1 damage from fire." };
        }

        private void RemoveExpiredMonsterBuffs(GameState gameState)
        {
            gameState.Player.DigState.NearbyMonster.ToList().ForEach(RemoveExpiredMonsterBuffs);
        }

        private void RemoveExpiredMonsterBuffs(NearbyMonster monster)
        {
            var buffsToDelete = monster.NearbyMonsterBuff.Where(b => b.RemainingDuration <= 0);
            foreach (var buff in buffsToDelete)
            {
                monster.NearbyMonsterBuff.Remove(buff);
            }
        }
    }
}