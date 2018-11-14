using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digsite.Data;
using Microsoft.EntityFrameworkCore;

namespace digsite.DataServices
{
    public class ItemDataService
    {
        public Item Get(int itemId)
        {
            using (var context = new DigsiteContext())
            {
                return context.Item.Find(itemId);
            }
        }
    }
}