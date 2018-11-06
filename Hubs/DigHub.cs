using digsite.GameServices.PlayerManager;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
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

        public void StartDigging(int userId)
        {
            _playerManager.StartDigging(userId);
        }

        public void StopDigging(int userId)
        {
            _playerManager.StopDigging(userId);
        }
    }
}