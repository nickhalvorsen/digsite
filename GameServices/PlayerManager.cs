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
        private PlayerStateDataService _playerStateDataService;
        private DigStateDataService _digStateDataService;
        private NearbyMonsterDataService _nearbyMonsterDataService;
        private PlayerItemDataService _playerItemDataService;
        private DigTimerService _digTimerService;
        private PlayerItemService _playerItemService;

        public PlayerManager(IHubContext<DigHub> hubContext)
        {
            _hubContext = hubContext;
            _playerStateDataService = new PlayerStateDataService();
            _digStateDataService = new DigStateDataService();
            _nearbyMonsterDataService = new NearbyMonsterDataService();
            _playerItemDataService = new PlayerItemDataService();
            _digTimerService = new DigTimerService();
            _playerItemService = new PlayerItemService();
        }

        public async Task GameUpdateData(int playerId)
        {
            await GameUpdateData(playerId, new List<string>());
        }
        public async Task GameUpdateData(int playerId, List<string> messages)
        {
            var playerState = await _playerStateDataService.GetPlayerState(playerId);
            var digState = await _digStateDataService.Get(playerId);
            var itemState = await _playerItemDataService.GetPlayer(playerId);

            var playerStateDto = playerState;
            var digStateDto = await GetDigStateDto(digState);
            var playerItemsDto = itemState.Select(ConvertToPlayerItemDto).ToList();

            var gameData = MakeGameUpdatePayload(playerStateDto, digStateDto, playerItemsDto);

            await _hubContext.Clients.All.SendAsync("ReceiveGameUpdate", gameData);

            foreach (var message in messages)
            {
                await SendGameLogMessage(playerId, message);
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

        private async Task<List<NearbyMonsterDto>> GetNearbyMonstersDto(int playerId)
        {
            return (await _nearbyMonsterDataService.Get(playerId))
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

        private async Task<DigStateDto> GetDigStateDto(DigState digState)
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
                , nearbyMonsters = await GetNearbyMonstersDto(digState.PlayerId)
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
            };
        }

        public async Task StartDigging(int playerId)
        {
            await _digStateDataService.GetOrCreate(playerId); 
            var state = await _digStateDataService.SetPaused(false, playerId);
            await _digTimerService.Start(playerId, GameUpdateData);
            await GameUpdateData(playerId);
        }

        public async Task StopDigging(int playerId)
        {
            var state = await _digStateDataService.SetPaused(true, playerId);
            _digTimerService.Stop(playerId);
            await _digStateDataService.SetPaused(true, playerId);
            await GameUpdateData(playerId);
        }

        private async Task SendGameLogMessage(int playerId, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveGameLogMessage", message);
        }

        public async Task UnequipItem(int playerId, int playerItemId)
        {
            await _playerItemService.Unequip(playerId, playerItemId);
        }

        public async Task EquipItem(int playerId, int playerItemId)
        {
            await _playerItemService.Equip(playerId, playerItemId);
        }
    }
}