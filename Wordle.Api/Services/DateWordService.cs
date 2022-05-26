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

        public string getDTObyDate(DateTime date, string username)
        {
            var theDateWord = _context.DateWords.Include(x => x.Games).Include(x => x.Players).Where(x => x.Date == date);
            var theList = theDateWord.ToList();
            string s = "";
            foreach(DateWord d in theList)
            {
                Console.WriteLine(d);
                s+=d.ToString();
                Console.WriteLine("\nnum games: " + d.Games.Count);
                s += "\nnum games: " + d.Games.Count.ToString();
                Console.WriteLine("num players: " + d.Players.Count);
                s += "\nnum players: " + d.Players.Count.ToString();
            }
            return s;
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
            var score = _context.DateWords
                .Where(x => x.WordId == word.WordId)
                    .SelectMany(dateWords => dateWords.Games);
            var listOfGames = score.Select(x => x.ScoreStat).ToList();
            var count = score.ToList();
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
