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

            int wordCount = _context.Words.Count();
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
            return _context.DateWords
                .Where(x => x == word)
                .Include(x => x.Games)
                .Count();
        }

        //TODO:Test
        public int GetAverageScore(DateWord word)
        {
            var scores = _context.DateWords
                .Where(x => x == word)
                    .Include(x => x.Games)
                    .ThenInclude(x => x.ScoreStat)
                    .ThenInclude(x => x!.Score);

            var listOfGames = scores.Select(x => x.Games).ToList();
            List<int> listOfScores = new();

            foreach(ScoreStat s in listOfGames)
            {
                listOfScores.Add(s.Score);
            }

            
            
            return (int)listOfScores.Average();
        }
        //TODO:Test
        public int GetAverageTime(DateWord word)
        {
            var scores = _context.DateWords
                .Where(x => x == word)
                    .Include(x => x.Games)
                    .ThenInclude(x => x.ScoreStat)
                    .ThenInclude(x => x!.Seconds);

            var listOfGames = scores.Select(x => x.Games).ToList();
            List<int> listOfScores = new();

            foreach (ScoreStat s in listOfGames)
            {
                listOfScores.Add(s.Seconds);
            }



            return (int)listOfScores.Average();
        }
    }
}
