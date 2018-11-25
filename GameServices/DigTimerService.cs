using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using digsite.Models;

namespace digsite.GameServices
{
    public class DigTimerService
    {
        private const int TickMilliseconds = 2000;
        private static Dictionary<int, DigTimer> _timers = new Dictionary<int, DigTimer>();
        private readonly DigTickService _digTickService;

        public DigTimerService() 
        {
            _digTickService = new DigTickService();
        }

        public async Task Start(int playerId, Func<int, List<string>, Task> callback)
        {
            _timers[playerId] = new DigTimer
            {
                PlayerId = playerId
                , Callback = callback     
            };
            
            await ContinueDigging(playerId, new List<string>());
        }

        private async Task ContinueDigging(int playerId, List<string> messages)
        {
            if (!_timers.ContainsKey(playerId))
            {
                return;
            }

            await _timers[playerId].Callback(playerId, messages);
            _timers[playerId].Timer = new Timer(TickMilliseconds);
            _timers[playerId].Timer.Elapsed += (sender, eventArgs) => TimerElapsed(sender, eventArgs, playerId);
            _timers[playerId].Timer.Enabled = true;
        }

        private async void TimerElapsed(object source, ElapsedEventArgs eventArgs, int playerId)
        {
            Console.WriteLine("timer elapsed");
            ((Timer)source).Dispose();
            var messages = await _digTickService.Tick(playerId);
            await ContinueDigging(playerId, messages);
        }

        public void Stop(int playerId)
        {
            if (_timers.ContainsKey(playerId))
            {
                _timers[playerId].Timer.Enabled = false;
                _timers[playerId].Timer.Dispose();
            }
        }
    }
}