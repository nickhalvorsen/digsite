

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using digsite.Data;

namespace digsite.GameServices
{
    public class DiggingService
    {

        public DiggingService()
        {
        }

        public void StartDigging(GameState gameState, Func<int, List<string>, Task> updateDataCallback)
        {
            EnsureDigStateExists(gameState);
            gameState.Player.DigState.IsPaused = (byte)0;
        }

        private void EnsureDigStateExists(GameState gameState)
        {
            if (gameState.Player.DigState != null)
            {
                return;
            }

            gameState.Player.DigState = new DigState
            {
                Depth = 0
                , Fuel = 100
                , IsPaused = (byte)1
            };
        }

        public void PauseDigging(GameState gameState)
        {
            gameState.Player.DigState.IsPaused = (byte)1;
        }

        public List<string> Progress(GameState gameState)
        {
            gameState.Player.DigState.Depth++;
            gameState.Player.DigState.Fuel--;
            return new List<string>();
        }

        public void ReturnToSurface(GameState gameState)
        {
            gameState.Player.DigState = null;
        }
    }
}