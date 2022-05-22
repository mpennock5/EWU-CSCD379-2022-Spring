using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Wordle.Api.Data;
using Wordle.Api.Dtos;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DateWordController : Controller
{
    private readonly AppDbContext _context;
    private readonly GameService _gameService;
    private readonly static object _mutex = new();
    private static readonly ConcurrentDictionary<DateTime, string> _cache = new();

    public DateWordController(AppDbContext context, GameService gameService)
    {
        _context = context;
        _gameService = gameService;
    }

    //[HttpGet]
    //public IEnumerable<DailyWordStatDto> GetLast10Words()
    //{

    //}

    //[HttpGet]
    //public string? GetDailyWord(DateTime date)
    //{
    //    //Sanitize the date by dropping time data
    //    date = date.Date;
    //    if (date.ToUniversalTime() >= System.DateTime.Today.ToUniversalTime().AddDays(0.5))
    //    {
    //        return null;
    //    }
    //    //Check if the day has a word in the database
    //    if (_cache.TryGetValue(date, out var word))
    //    {
    //        return word;
    //    }
    //    DateWord? wordOfTheDay = _context.DateWords
    //        .Include(x => x.Word)
    //        .FirstOrDefault(dw => dw.Date == date);

    //    if (wordOfTheDay != null)
    //    //Yes: return the word
    //    {
    //        _cache.TryAdd(date, wordOfTheDay.Word.Value);
    //        return wordOfTheDay.Word.Value;
    //    }
    //    else
    //    {
    //        //Mutex magic
    //        lock (_mutex)
    //        {
    //            wordOfTheDay = _context.DateWords
    //                .Include(x => x.Word)
    //                .FirstOrDefault(dw => dw.Date == date);
    //            if (wordOfTheDay != null)
    //            //Yes: return the word
    //            {
    //                return wordOfTheDay.Word.Value;
    //            }
    //            else
    //            {
    //                //No: get a random word from our list
    //                var chosenWord = _gameService.GetWord();
    //                //Save the word to the database with the date
    //                _context.DateWords.Add(new DateWord { Date = date, Word = chosenWord });
    //                _context.SaveChanges();
    //                //Return the word
    //                _cache.TryAdd(date, chosenWord.Value);
    //                return chosenWord.Value;
    //            }
    //        }
    //    }
    //}
    [Route("[action]")]
    [HttpGet]
    public string? GetWordByDate(int year, int month, int day)
    {
        //Sanitize the date by dropping time data
        DateTime date = new DateTime(year, month, day);

        //Check if the day has a word in the database
        if (_cache.TryGetValue(date, out var word))
        {
            return word;
        }
        DateWord? wordOfTheDay = _context.DateWords
            .Include(x => x.Word)
            .FirstOrDefault(dw => dw.Date == date);

        if (wordOfTheDay != null)
        //Yes: return the word
        {
            _cache.TryAdd(date, wordOfTheDay.Word.Value);
            return wordOfTheDay.Word.Value;
        }
        else
        {
            //Mutex magic
            lock (_mutex)
            {
                wordOfTheDay = _context.DateWords
                    .Include(x => x.Word)
                    .FirstOrDefault(dw => dw.Date == date);
                if (wordOfTheDay != null)
                //Yes: return the word
                {
                    return wordOfTheDay.Word.Value;
                }
                else
                {
                    //No: get a random word from our list
                    var chosenWord = _gameService.GetWord();
                    //Save the word to the database with the date
                    _context.DateWords.Add(new DateWord { Date = date, Word = chosenWord });
                    _context.SaveChanges();
                    //Return the word
                    _cache.TryAdd(date, chosenWord.Value);
                    return chosenWord.Value;
                }
            }
        }
    }
}
