using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Api.Controllers;
using Wordle.Api.Data;
using Wordle.Api.Dtos;
using Wordle.Api.Services;
using static Wordle.Api.Controllers.DateWordController;

namespace Wordle.Api.Tests;
[TestClass]
public class DailyWordTests : DatabaseBaseTests
{
    [TestMethod]
    public void GetDailyWord()
    {
        using var context = new TestAppDbContext(Options);
        
        context.SaveChanges();

        DateWordController sut = new(context, new Services.GameService(context), new Services.DateWordService(context));

        string? word = sut.GetWordByDate(2020, 1 , 1);
        Assert.IsNotNull(word);
        Assert.AreEqual<int>(5, word.Length);
        string? word2 = sut.GetWordByDate(2020, 1, 1);
        Assert.AreEqual<string?>(word, word2);
    }

    [TestMethod]
    public void GettingDailyWordAddsDateWord()
    {
        using var context = new TestAppDbContext(Options);
        
        DateTime date = new(2020, 2, 1);
        context.SaveChanges();

        DateWordController sut = new(context, new Services.GameService(context), new Services.DateWordService(context));
        DateWord? dateWord = context.DateWords.FirstOrDefault(x => x.Date.Date == date);
        Assert.IsNull(dateWord);
        string? word = sut.GetWordByDate(2020, 2, 1);
        dateWord = context.DateWords.FirstOrDefault(x => x.Date.Date == date);
        Assert.IsNotNull(dateWord);
        Assert.AreEqual(date.Date, dateWord.Date.Date);

    }

    [TestMethod]
    public void GetLastTen()
    {
        using var context = new TestAppDbContext(Options);
        DateWordController sut = new(context, new Services.GameService(context), new Services.DateWordService(context));
        IEnumerable<DailyWordStatDto> dailyWords = sut.GetLast10DailyWords("Gunther");
        Assert.IsNotNull(dailyWords);
    }

    [TestMethod]
    public void SubmitGame()
    {
        using var context = new TestAppDbContext(Options);
        DateWordService sut = new(context);
        DateTime date = new DateTime(2022, 5, 26);
        (bool, string) response = sut.SubmitGame(date.Date, "Gunther", 5, 45);
        Assert.IsTrue(response.Item1);
    }

    [TestMethod]
    public void DateWordControllerPost()
    {
        using var context = new TestAppDbContext(Options);
        DateWordController sut = new(context, new Services.GameService(context), new Services.DateWordService(context));
        GameDetails gameDetails = new(){
            Year = 2022,
            Month = 05,
            Day = 26,
            Player = "Dakota",
            Score = 5,
            TimeSeconds = 45
        };
        IActionResult result = sut.Post(gameDetails);
        Assert.IsNotNull(result);
    }

    [TestInitialize]
    public void Setup()
    {
        using var context = new TestAppDbContext(Options);
        context.Words.Add(new Data.Word() { Value = "tests", Common = true });
        context.Words.Add(new Data.Word() { Value = "zebra", Common = true });
        context.Words.Add(new Data.Word() { Value = "hello", Common = true });
        context.Words.Add(new Data.Word() { Value = "apple", Common = true });
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
        context.SaveChanges();
        context.ScoreStats.Add(new Data.ScoreStat()
        {
            Score = 4,
            Seconds = 60,
            
        });
        context.SaveChanges();
        var score = context.ScoreStats.First();
        context.DateWords.Add(new Data.DateWord()
        {
            Date = new DateTime(2020,1, 1),
            WordId = 1,
            TotalPlays = 1,
            AverageGuesses = 4,
            AverageSeconds = 60

        });
        context.SaveChanges();
        var dateWord = context.DateWords.First();
        context.Games.Add(new Data.Game()
        {

            Player = player,
            WordId = 1,
            DateEnded = new DateTime(2020, 1, 1),
            ScoreStatId = score.ScoreStatId,
            DateWord = dateWord

        });
        context.SaveChanges();
        context.DateWords.Add(new Data.DateWord()
        {
            Date = new DateTime(2022, 5, 26),
            WordId = 3,
            TotalPlays = 1,
            AverageGuesses = 4,
            AverageSeconds = 60

        });
        var date = new DateTime(2022, 5, 26);
        context.SaveChanges();
        //var dateWord2 = context.DateWords
        //    .First(x => x.Date.Date == date.Date);
        //context.Games.Add(new Data.Game()
        //{

        //    Player = player,
        //    WordId = 3,
        //    DateWord = dateWord2

        //});
        //context.SaveChanges();
    }
}
