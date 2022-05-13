using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Tests
{
    [TestClass]
    public class ScoreStatsServiceTests
    {
        private AppDbContext _context;
        
        public ScoreStatsServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Wordle.Api.Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new AppDbContext(contextOptions.Options);
            _context.Database.Migrate();
            ScoreStatsService.Seed(_context);
            Players.Seed(_context);
            
        }
        [TestMethod]
        public void GetScoreStats()
        {
            ScoreStatsService sut = new ScoreStatsService(_context);

            Assert.AreEqual(6, sut.GetScoreStats().Count());
        }
    }
}
