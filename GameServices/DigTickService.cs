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
        private readonly PlayerItemService _playerItemService;
        private readonly MonsterCombatService _monsterCombatService;
        private readonly MonsterEncounterService _monsterEncounterService;
        private readonly ItemDiscoveryService _itemDiscoveryService;
        private readonly PlayerDeathService _playerDeathService;
        private readonly BuffService _buffService;

        public DigTickService()
        {
            _gameStateDataService = new GameStateDataService();
            _diggingService = new DiggingService();
            _playerItemService = new PlayerItemService();
            _monsterCombatService = new MonsterCombatService();
            _monsterEncounterService = new MonsterEncounterService();
            _itemDiscoveryService = new ItemDiscoveryService();
            _playerDeathService = new PlayerDeathService();
            _buffService = new BuffService();
        }

        public async Task<List<string>> Tick(int playerId)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            if (gameState.Player.DigState == null)
            {
                _diggingService.PauseDigging(gameState);
                return new List<string>();
            }
            var messageLists = new List<List<string>>()
            {
                HandleBuffs(gameState)
                , HandlePlayerDeath(gameState)
                , HandleMonsterDeaths(gameState)
                , ActivateItems(gameState)
                , HandlePlayerDeath(gameState)
                , HandleMonsterDeaths(gameState)
                , MonsterAttacks(gameState)
                , HandlePlayerDeath(gameState)
                , ProgressDigging(gameState)
                , FindItems(gameState)
                , MonsterApproach(gameState)
            };
            await _gameStateDataService.SaveGameState(gameState);
            return messageLists.SelectMany(m => m).Where(m => !string.IsNullOrEmpty(m)).ToList();
        }

        private List<string> HandleBuffs(GameState gameState)
        {
            var messages1 = _buffService.HandlePlayerBuffs(gameState);
            var messages2 = _buffService.HandleMonsterBuffs(gameState);
            return messages1.Concat(messages2).ToList();
        }

        private List<string> ActivateItems(GameState gameState)
        {
            var messages = _playerItemService.ActivateItems(gameState);
            _playerItemService.CooldownTick(gameState);
            return messages;
        }

        private List<string> HandleMonsterDeaths(GameState gameState)
        {
            return _monsterCombatService.HandleMonsterDeaths(gameState).ToList();
        }

        private List<string> MonsterAttacks(GameState gameState)
        {
            return _monsterCombatService.HandleMonsterAttacks(gameState);
        }

        private List<string> HandlePlayerDeath(GameState gameState)
        {
            return _playerDeathService.HandlePlayerDeath(gameState);
        }

        private List<string> ProgressDigging(GameState gameState)
        {
            _diggingService.Progress(gameState);
            return new List<string>();
        }

        private List<string> FindItems(GameState gameState)
        {
            return _itemDiscoveryService.FindItems(gameState);
        }

        private List<string> MonsterApproach(GameState gameState)
        {
            return _monsterEncounterService.GenerateMonsterEncounters(gameState);
        }
    }
}