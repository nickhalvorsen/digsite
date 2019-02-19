using System.Linq;
using digsite.Data;
using digsite.DataServices;

namespace digsite.GameServices
{
    public class AllPlayersManager
    {
        public void PauseAllDigging()
        {
            var context = new DigsiteContext();
            context.DigState.ToList().ForEach(d => d.IsPaused = (byte)1);
            context.SaveChanges();
        }
    }
}