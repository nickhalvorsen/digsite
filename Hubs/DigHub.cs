using digsite.GameServices.PlayerManager;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace digsite.Hubs
{
    public class DigHub : Hub
    {
        private PlayerManager _playerManager;

        public DigHub(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public async Task RequestPlayerState(int userId)
        {
           await _playerManager.SendPlayerState(userId); 
        }

        public async Task RequestDigState(int userId)
        {
            await _playerManager.SendDigState(userId);
        }

        public async Task StartDigging(int userId)
        {
            await _playerManager.StartDigging(userId);
        }

        public async Task StopDigging(int userId)
        {
            await _playerManager.StopDigging(userId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // todo this will error if dig state doesnt exist
            await _playerManager.StopDigging(1001);
            await base.OnDisconnectedAsync(exception);
        }
    }
}