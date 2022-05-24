using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Api.Controllers;
using Wordle.Api.Dtos;

namespace Wordle.Api.Tests;
[TestClass]
public class DailyWordTests : DatabaseBaseTests
{
    [TestMethod]
    public void GetDailyWord()
    {
        using var context = new TestAppDbContext(Options);
        context.Words.Add(new Data.Word() { Value = "tests" });
        context.Words.Add(new Data.Word() { Value = "zebra" });
        context.Words.Add(new Data.Word() { Value = "hello" });
        context.Words.Add(new Data.Word() { Value = "apple" });
        context.Players.Add(new Data.Player() { Name = "Gunther" });
        var player = context.Players.First();
        context.Games.Add(new Data.Game() 
        { 
            Player = player,
        });
        context.SaveChanges();

        DateWordController sut = new(context, new Services.GameService(context), new Services.DateWordService(context));

        string? word = sut.GetWordByDate(2020, 1 , 1);
        Assert.IsNotNull(word);
        Assert.AreEqual<int>(5, word.Length);
        string? word2 = sut.GetWordByDate(2020, 1, 1);
        Assert.AreEqual<string?>(word, word2);
    }

    [TestMethod]
    public void GetLastTen()
    {
        using var context = new TestAppDbContext(Options);
        DateWordController sut = new(context, new Services.GameService(context), new Services.DateWordService(context));
        IEnumerable<DailyWordStatDto> dailyWords = sut.GetLast10DailyWords("Gunther");
        Assert.IsNotNull(dailyWords);
    }

    [TestInitialize]
    public void Setup()
    {
        using var context = new TestAppDbContext(Options);
        context.Words.Add(new Data.Word() { Value = "tests" });
        context.Words.Add(new Data.Word() { Value = "zebra" });
        context.Words.Add(new Data.Word() { Value = "hello" });
        context.Words.Add(new Data.Word() { Value = "apple" });
        context.Players.Add(new Data.Player() 
        { 
            Name = "Gunther",
            GameCount = 1,
            AverageAttempts = 1,
            AverageSecondsPerGame = 1

        });
        context.SaveChanges();
        var player = context.Players.First(x => x.Name == "Gunther");
        var word = context.Words.First();
        context.Games.Add(new Data.Game()
        {
            GameId = 1,
            Player = player,
            Word = word,
            DateEnded = new DateTime(2020, 1, 1),
            ScoreStat =  new Data.ScoreStat()
            {
                Score = 5,
                Seconds = 60,
                GameId = 1
            }
        });
        context.SaveChanges();
        var game = context.Games.First();
        context.DateWords.Add(new Data.DateWord()
        {
            Date = new DateTime(2020,1, 1),
            Word = word,
            TotalPlays = 1,
            AverageGuesses = 1,
            AverageSeconds = 60

        });
        context.SaveChanges();
    }
}
