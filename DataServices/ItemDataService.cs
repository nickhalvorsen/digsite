using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class ItemDataService
    {
        public async Task<Item> Get(int itemId)
        {
            using (var context = new DigsiteContext())
            {
                return await context.Item.FindAsync(itemId);
            }
        }
    }
}