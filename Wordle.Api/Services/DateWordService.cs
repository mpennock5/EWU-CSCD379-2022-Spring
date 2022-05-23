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


        public void getLast10Words()
        {
            IEnumerable<DateWord> words;
            List<DailyWordStatDto> last10 = new();

            int wordCount = _context.Words.Count();
            if (wordCount > 9)
            {
                words = _context.DateWords.OrderBy(x => x.Date).Take(10);
            }
            else
            {
                words = _context.DateWords.OrderBy(x => x.Date).Take(wordCount);
            }

            foreach (var word in words)
            {
                DailyWordStatDto dto = new(word.Date, getTotalPlays(word), getTotalPlays(word), getTotalPlays(word));
                last10.Add(dto);
            }

        }

        public int getTotalPlays(DateWord word)
        {
            return _context.DateWords
                .Where(x => x == word)
                .Include(x => x.Games)
                .Count();
        }

        //TODO
        public int getAverageScore(DateWord word)
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
        //TODO
        public int getAverageTime(DateWord word)
        {
            return _context.DateWords
                .Where(x => x == word)
                .Include(x => x.Games)
                .Count();
        }
    }
}
