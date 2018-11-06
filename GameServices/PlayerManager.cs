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
        public static Dictionary<int, Timer> _timers = new Dictionary<int, Timer>();


        public PlayerManager(IHubContext<DigHub> hubContext)
        {
            _hubContext = hubContext;
            _playerStateDataService = new PlayerStateDataService();
        }

        // when they load the game
        public async Task SendPlayerState(int playerId)
        {
            var playerState = new DigsiteContext().PlayerState.Where(ps => ps.PlayerId == playerId).First();
            var payload = JsonConvert.SerializeObject(playerState);
            await _hubContext.Clients.All.SendAsync("ReceivePlayerState", payload);
        }

        public async Task SendDigState(int playerId)
        {
            var digState = _digStateDataService.Get(playerId);
            var payload = JsonConvert.SerializeObject(digState);
            await _hubContext.Clients.All.SendAsync("ReceiveDigState", payload);
        }

        public void StartDigging(int userId)
        {
            _timers[userId] = new Timer(2000);
            _timers[userId].Elapsed += (sender, eventArgs) => TimerElapsed(sender, eventArgs, userId);
            _timers[userId].Enabled = true;
        }

        public void StopDigging(int playerId)
        {
            _timers[playerId].Dispose();
        }

        private async void TimerElapsed(object source, ElapsedEventArgs eventArgs, int playerId)
        {
            Console.WriteLine("timer elapsed");
            await _playerStateDataService.AddMoney(playerId, 1);
            await _hubContext.Clients.All.SendAsync("Find", "table", 1);
            ((Timer)source).Dispose();
            StartDigging(playerId);
        }
    }
}