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
            _digStateDataService = new DigStateDataService();
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
            await SendDigState(playerId);
        }
    }
}