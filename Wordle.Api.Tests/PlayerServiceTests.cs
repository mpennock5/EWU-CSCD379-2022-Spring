using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Tests;

[TestClass]
public class PlayerServiceTests : DatabaseBaseTests
{
    [TestMethod]
    public void GetPlayers_MatchesPlayerCount_Success()
    {
        using var context = new TestAppDbContext(Options);
        PlayersService sut = new(context);
        int playerCount = sut.GetPlayers().Count();
        Assert.AreEqual(playerCount, sut.GetPlayers().Count());
    }

    

}
