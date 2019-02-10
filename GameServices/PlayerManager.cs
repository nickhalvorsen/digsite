using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using digsite.Data;
using digsite.DataServices;
using digsite.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace digsite.GameServices.PlayerManager
{
    public class PlayerManager
    {
        private IHubContext<DigHub> _hubContext;
        private GameStateDataService _gameStateDataService;
        private DiggingService _diggingService;
        private PlayerItemService _playerItemService;
        private DigTimerService _digTimerService;

        public PlayerManager(IHubContext<DigHub> hubContext)
        {
            _hubContext = hubContext;
            _gameStateDataService = new GameStateDataService();
            _diggingService = new DiggingService();
            _playerItemService = new PlayerItemService();
            _digTimerService = new DigTimerService();
        }

        public async Task GameUpdateData(int playerId)
        {
            await GameUpdateData(playerId, new List<string>());
        }

        public async Task GameUpdateData(int playerId, List<string> messages)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            await GameUpdateData(gameState, messages);
        }

        public async Task GameUpdateData(GameState gameState)
        {
            await GameUpdateData(gameState, new List<string>());
        }

        public async Task GameUpdateData(GameState gameState, List<string> messages)
        {
            var playerState = gameState.Player.PlayerState;
            playerState.Player = null;
            var digState = gameState.Player.DigState;
            var itemState = gameState.Player.PlayerItem;

            var playerStateDto = playerState;
            var digStateDto = GetDigStateDto(digState);
            var playerItemsDto = itemState.Select(ConvertToPlayerItemDto).ToList();

            var gameData = MakeGameUpdatePayload(playerStateDto, digStateDto, playerItemsDto);

            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", gameData);

            foreach (var message in messages)
            {
                await SendGameLogMessage(gameState.PlayerId, message);
            }
        }

        private GameUpdateDto MakeGameUpdatePayload(PlayerState playerState, DigStateDto digStateDto, List<PlayerItemDto> playerItemsDto)
        {
            return new GameUpdateDto
            {
                playerState = playerState,
                digState = digStateDto,
                itemState = playerItemsDto
            };
        }

        private List<NearbyMonsterDto> GetNearbyMonstersDto(List<NearbyMonster> nearbyMonsters)
        {
            return nearbyMonsters
                .Select(ConvertToNearbyMonsterDto)
                .ToList();
        }

        private NearbyMonsterDto ConvertToNearbyMonsterDto(NearbyMonster nearbyMonster)
        {
            return new NearbyMonsterDto
            {
                name = nearbyMonster.Monster.Name
                , maxHealth = nearbyMonster.Monster.Health
                , currentHealth = nearbyMonster.CurrentHealth
            };
        }

        private DigStateDto GetDigStateDto(DigState digState)
        {
            if (digState == null)
            {
                return new DigStateDto
                {
                    hasDigState = false
                };
            }

            return new DigStateDto
            {
                hasDigState = true
                , depth = digState.Depth
                , fuel = digState.Fuel
                , isPaused = digState.IsPaused > 0 ? true : false
                , nearbyMonsters = GetNearbyMonstersDto(digState.NearbyMonster.ToList())
            };
        }

        private PlayerItemDto ConvertToPlayerItemDto(PlayerItem playerItem)
        {
            return new PlayerItemDto
            {
                playerItemId = playerItem.PlayerItemId
                , name = playerItem.Item.Name
                , itemCategoryId = playerItem.Item.ItemCategoryId
                , isEquipped = playerItem.IsEquipped != 0
                , itemSlotId = playerItem.Item.ItemSlotId
                , slotName = playerItem.Item.ItemSlot.Description
                , currentCooldown = playerItem.CurrentCooldown
            };
        }

        public async Task StartDigging(int playerId)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            _diggingService.StartDigging(gameState, GameUpdateData);
            await _digTimerService.Start(gameState.PlayerId, GameUpdateData);
            await _gameStateDataService.SaveGameState(gameState);
            await GameUpdateData(gameState);
        }

        public async Task StopDigging(int playerId)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            _diggingService.PauseDigging(gameState);
            _digTimerService.Stop(gameState.PlayerId);
            await GameUpdateData(gameState);
        }

        private async Task SendGameLogMessage(int playerId, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveGameLogMessage", message);
        }

        public async Task UnequipItem(int playerId, int playerItemId)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            _playerItemService.Unequip(gameState, playerItemId);
            await _gameStateDataService.SaveGameState(gameState);
            await GameUpdateData(gameState);
        }

        public async Task EquipItem(int playerId, int playerItemId)
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            _playerItemService.Equip(gameState, playerItemId);
            await _gameStateDataService.SaveGameState(gameState);
            await GameUpdateData(gameState);
        }

        public async Task ReturnToSurface(int playerId) 
        {
            var gameState = await _gameStateDataService.GetGameState(playerId);
            _diggingService.ReturnToSurface(gameState);
            _digTimerService.Stop(gameState.PlayerId);
            var message = new List<string> { "you return to the surface." };
            await _gameStateDataService.SaveGameState(gameState);
            await GameUpdateData(gameState, message);
        }
    }
}