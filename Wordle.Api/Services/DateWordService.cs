using Microsoft.EntityFrameworkCore;
using Wordle.Api.Data;
using Wordle.Api.Dtos;

namespace Wordle.Api.Services
{
    public class DateWordService
    {
        private readonly AppDbContext _context;

        public DateWordService(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<DailyWordStatDto> GetLast10Words(string? name)
        {
            IEnumerable<DateWord> words;
            List<DailyWordStatDto> lastTen = new();

            int wordCount = _context.DateWords.Count();
            if (wordCount > 9)
            {
                words = _context.DateWords.OrderBy(x => x.Date).Take(10);
            }
            else
            {
                words = _context.DateWords.OrderBy(x => x.Date).Take(wordCount);
            }

            var player = _context.Players.Include(x => x.Games).First(x => x.Name == name);
            var games = player.Games.Select(x => x.DateStarted).ToList();

            foreach (var word in words)
            {
                DailyWordStatDto dto = new(word.Date, GetTotalPlays(word), GetAverageScore(word), GetAverageTime(word), GetHasPlayed(name, word));
                lastTen.Add(dto);
            }

            return lastTen;

        }

        public bool? GetHasPlayed(string? name, DateWord word)
        {
            if(name == null)
            {
                return null;
            }
            var player = _context.Players.Include(x => x.Games).First(x => x.Name == name);
            var games = player.Games.Select(x => x.DateStarted).ToList();
            
            if (games.Contains(word.Date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetTotalPlays(DateWord word)
        {
            int plays = _context.DateWords
                .Where(x => x == word)
                .Include(x => x.Games)
                .Count();

            return plays;
        }

        //TODO:Test
        public int GetAverageScore(DateWord word)
        {
            var scores = _context.DateWords
                    .Include(x => x.Games)
                    .ThenInclude(x => x.ScoreStat)
                    .Where(x => x.DateWordId == word.DateWordId)
                    .SelectMany(x => x.Games).ToList();
                    
             
            var count = scores.ToList();
            List<int> listOfScores = new();

            foreach (var s in scores)
            {
                    listOfScores.Add(s.ScoreStat!.Score);
            }
            int score = listOfScores.First();
            return (int)listOfScores.Average();
        }
        //TODO:Test
        public int GetAverageTime(DateWord word)
        {
            var scores = _context.DateWords
                    .Include(x => x.Games)
                    .ThenInclude(x => x.ScoreStat)
                    .Where(x => x.DateWordId == word.DateWordId)
                    .SelectMany(x => x.Games).ToList();

            
            List<int> listOfScores = new();

            foreach (var s in scores)
            {
                listOfScores.Add(s.ScoreStat!.Seconds);
            }



            return (int)listOfScores.Average();
        }
    }
}
