using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Api.Controllers;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Tests
{
    [TestClass]
    public class PlayerTests
    {
        private AppDbContext _context;

        public PlayerTests()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Wordle.Api.Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new AppDbContext(contextOptions.Options);
            _context.Database.Migrate();
            Players.Seed(_context);

        }
        [TestMethod]
        public void getTop10Players()
        {
            Players players = new Players(_context);
            PlayersLeaderBoardController sut = new PlayersLeaderBoardController(players);

            var top10 = sut.GetTop10Players();

            Assert.AreEqual(10, top10.Count());
        }

        [TestMethod]
        public void update()
        {
            Players players = new Players(_context);
            var before = players.GetPlayers().ToList();

            players.Update("Inigo", 2, 3);

            Assert.AreEqual(before.Count() + 1, players.GetPlayers().Count());

            Player inigo = _context.Players.First(x => x.Name == "Inigo");
            _context.Players.Remove(inigo);
            _context.SaveChanges();
        }

    }
}
