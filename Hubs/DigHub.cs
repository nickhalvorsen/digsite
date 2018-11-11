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

        public async Task RequestPlayerState(int playerId)
        {
           await _playerManager.SendPlayerState(playerId); 
        }

        public async Task RequestDigState(int playerId)
        {
            await _playerManager.SendDigState(playerId);
        }

        public async Task RequestNearbyMonsterState(int playerId)
        {
            await _playerManager.SendNearbyMonsterState(playerId);
        }

        public async Task StartDigging(int playerId)
        {
            await _playerManager.StartDigging(playerId);
        }

        public async Task StopDigging(int playerId)
        {
            await _playerManager.StopDigging(playerId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // todo this will error if dig state doesnt exist
            await _playerManager.StopDigging(1001);
            await base.OnDisconnectedAsync(exception);
        }
    }
}