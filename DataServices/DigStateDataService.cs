using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class DigStateDataService
    {
        public async Task<DigState> GetOrCreate(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var state = await Get(playerId);
                if (state != null)
                {
                    return state;
                }

                await context.DigState.AddAsync(new DigState
                {
                    PlayerId = playerId,
                    Depth = 0,
                    Fuel = 100,
                    IsPaused = (byte)1
                });

                await context.SaveChangesAsync();

                return await Get(playerId);
            }
        }

        // don't use this method in conjunction with updating the dig state
        public async Task<DigState> Get(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.DigState.FirstOrDefaultAsync(d => d.PlayerId == playerId);
            }
        }

        public async Task Progress(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var state = await context.DigState.FirstOrDefaultAsync(d => d.PlayerId == playerId);

                state.Depth++;
                state.Fuel--;
                context.SaveChanges();
            }
        }

        public async Task<DigState> SetPaused(bool isPaused, int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var state = await context.DigState.FirstOrDefaultAsync(d => d.PlayerId == playerId);
                state.IsPaused = isPaused ? (byte)1 : (byte)0;
                await context.SaveChangesAsync();
                return state;
            }
        }

        public async Task Clear(int playerId)
        {
            using (var context = new DigsiteContext())
            {
                var digState = await Get(playerId);
                context.DigState.Remove(digState);
                await context.SaveChangesAsync();
            }
        }
    }
}