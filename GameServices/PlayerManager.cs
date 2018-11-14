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
        private MonsterDataService _monsterDataService;
        private NearbyMonsterDataService _nearbyMonsterDataService;
        private PlayerItemDataService _playerItemDataService;
        private ItemDataService _itemDataService;
        public static Dictionary<int, Timer> _timers = new Dictionary<int, Timer>();


        public PlayerManager(IHubContext<DigHub> hubContext)
        {
            _hubContext = hubContext;
            _playerStateDataService = new PlayerStateDataService();
            _digStateDataService = new DigStateDataService();
            _monsterDataService = new MonsterDataService();
            _nearbyMonsterDataService = new NearbyMonsterDataService();
            _playerItemDataService = new PlayerItemDataService();
            _itemDataService = new ItemDataService();
        }

        public async Task SendPlayerState(int playerId)
        {
            var playerState = await _playerStateDataService.GetPlayerState(playerId);
            await _hubContext.Clients.All.SendAsync("ReceivePlayerState", playerState);
        }

        public async Task SendDigState(int playerId)
        {
            var digState = await _digStateDataService.Get(playerId);
            await SendDigState(digState);
        }

        private async Task SendDigState(DigState digState)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveDigState", digState);
        }

        public async Task StartDigging(int playerId)
        {
            await _digStateDataService.GetOrCreate(playerId); 
            var state = await _digStateDataService.SetPaused(false, playerId);
            await SendDigState(state);
            ContinueDigging(playerId);
        }

        public void ContinueDigging(int playerId)
        {
            _timers[playerId] = new Timer(2000);
            _timers[playerId].Elapsed += (sender, eventArgs) => TimerElapsed(sender, eventArgs, playerId);
            _timers[playerId].Enabled = true;
        }

        public async Task StopDigging(int playerId)
        {
            if (_timers.ContainsKey(playerId))
            {
                _timers[playerId].Dispose();
            }

            var state = await _digStateDataService.SetPaused(true, playerId);
            await SendDigState(state);
        }

        private async void TimerElapsed(object source, ElapsedEventArgs eventArgs, int playerId)
        {
            Console.WriteLine("timer elapsed");
            ((Timer)source).Dispose();
            await ProcessDigEvent(playerId);
            ContinueDigging(playerId);
        }

        private async Task ProcessDigEvent(int playerId)
        {
            await _playerStateDataService.AddMoney(playerId, 1);
            await _hubContext.Clients.All.SendAsync("Find", "table", 1);
            await _digStateDataService.Progress(playerId);
            if (new Random().Next(1, 10) == 4)
            {
                await MonsterApproach(playerId);
            }
            if (new Random().Next(90, 99) > 90)
            {
                await GiveRandomItem(playerId);
            }
            await SendDigState(playerId);
            await SendNearbyMonsterState(playerId);
            await SendItemState(playerId);
        }

        private async Task MonsterApproach(int playerId)
        {
            var randomMonsterId = new Random().Next(1, 3);
            var randomMonster = await _monsterDataService.Get(randomMonsterId);
            await _nearbyMonsterDataService.Add(playerId, randomMonster); 
        }

        private async Task GiveRandomItem(int playerId)
        {
            var itemId = new Random().Next(1, 7);
            await _playerItemDataService.Give(playerId, itemId);
            var item = _itemDataService.Get(itemId);
            await SendGameLogMessage(playerId, "You found a " + item.Name);
        }

        public async Task SendNearbyMonsterState(int playerId)
        {
            var nearbyMonsterState = await GetNearbyMonstersDto(playerId);
            await _hubContext.Clients.All.SendAsync("ReceiveNearbyMonsterState", nearbyMonsterState);
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

        public async Task SendItemState(int playerId)
        {
            var items = await GetItemStateDto(playerId);
            await _hubContext.Clients.All.SendAsync("ReceiveItemState", items);
        }

        private async Task<List<PlayerItemDto>> GetItemStateDto(int playerId)
        {
            var items = await _playerItemDataService.Get(playerId);
            return items.Select(ConvertToPlayerItemDto).ToList();
        }

        private PlayerItemDto ConvertToPlayerItemDto(PlayerItem playerItem)
        {
            return new PlayerItemDto
            {
                name = playerItem.Item.Name
                , itemCategoryId = playerItem.Item.ItemCategoryId
            };
        }

        private async Task SendGameLogMessage(int playerId, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveGameLogMessage", message);
        }
    }
}