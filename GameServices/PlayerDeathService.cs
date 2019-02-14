using System.Collections.Generic;
using digsite.Data;

namespace digsite.GameServices
{
    public class PlayerDeathService
    {
        public List<string> HandlePlayerDeath(GameState gameState)
        {
            if (gameState.Player.DigState == null || gameState.Player.DigState.Fuel > 0)
            {
                return new List<string>();
            }

            new DiggingService().ReturnToSurface(gameState);

            return new List<string>() { "Oh no, you got KOed!" };
        }
    }
}