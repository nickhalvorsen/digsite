using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using digsite.Data;
using digsite.DataServices;

namespace digsite.GameServices
{
    public class DigTickService
    {
        private readonly GameStateDataService _gameStateDataService;
        private readonly DiggingService _diggingService;
        private readonly ItemService _itemService;
        private readonly MonsterCombatService _monsterCombatService;
        private readonly MonsterEncounterService _monsterEncounterService;

        public DigTickService()
        {
            _gameStateDataService = new GameStateDataService();
            _diggingService = new DiggingService();
            _itemService = new ItemService();
            _monsterCombatService = new MonsterCombatService();
            _monsterEncounterService = new MonsterEncounterService();
        }

        public async Task<List<string>> Tick(int playerId)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            var messageLists = new List<List<string>>()
            {
                 ActivateItems(gameState)
                , MonsterAttacks(gameState)
                , ProgressDigging(gameState)
                , FindItems(gameState)
                , MonsterApproach(gameState)
                , new List<string>() { "You dig a little deeper." }
            };
            return messageLists.SelectMany(m => m).Where(m => !string.IsNullOrEmpty(m)).ToList();
        }
        
        private List<string> ActivateItems(GameState gameState)
        {
            return _itemService.ActivateItems(gameState);
        }

        private List<string> MonsterAttacks(GameState gameState)
        {
            return _monsterCombatService.HandleMonsterAttacks(gameState);
        }

        private List<string> ProgressDigging(GameState gameState)
        {
            _diggingService.Progress(gameState);
            return new List<string>();
        }

        private List<string> FindItems(GameState gameState)
        {
            // todo 
            return new List<string>();
        }

        private List<string> MonsterApproach(GameState gameState)
        {
            return _monsterEncounterService.GenerateMonsterEncounters(gameState);
        }
    }
}