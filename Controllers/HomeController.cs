using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using digsite.Models;
using digsite.Data;

namespace digsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            EnsureTestUserExists();

            return View();
        }

        private void EnsureTestUserExists()
        {

            var testUserId = 1001;
            var context = new DigsiteContext();
            if (context.Player.Count(p => p.PlayerId == testUserId) == 0)
            {
                context.Player.Add(new Player
                {
                    PlayerId = 1001,
                    GameState = new GameState() 
                    {
                        IsDigging = 0
            
                    },
                    PlayerState = new PlayerState()
                    {
                        Money = 0
                    }
                });
            }
            context.SaveChanges();
        }
    }
}
