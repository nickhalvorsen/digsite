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
    // This class directly recieves requests from the front end.
    public class DigHub : Hub
    {
        private PlayerManager _playerManager;

        public DigHub(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public async Task GameLoadData(int playerId)
        {
            await _playerManager.GameUpdateData(playerId);
        }

        public async Task StartDigging(int playerId)
        {
            await _playerManager.StartDigging(playerId);
        }

        public async Task StopDigging(int playerId)
        {
            await _playerManager.StopDigging(playerId);
        }

        public async Task EquipItem(int playerId, int? equippedPlayerItemId, int? unEquippedPlayerItemId)
        {
            if (unEquippedPlayerItemId != null && unEquippedPlayerItemId > 0)
            {
                await _playerManager.UnequipItem(playerId, unEquippedPlayerItemId.Value);
            }

            if (equippedPlayerItemId != null && equippedPlayerItemId > 0)
            {
                await _playerManager.EquipItem(playerId, equippedPlayerItemId.Value);
            }

            await _playerManager.GameUpdateData(playerId);
        }

        public async Task UpgradeItem(int playerId, int playerItemId1, int playerItemId2)
        {
            await _playerManager.UpgradeItem(playerId, playerItemId1, playerItemId2);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var s = Context.User.Identity;
            // todo this will error if dig state doesnt exist
            await _playerManager.StopDigging(1001);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ReturnToSurface(int playerId) 
        {
            await _playerManager.ReturnToSurface(playerId);
        }
    }
}