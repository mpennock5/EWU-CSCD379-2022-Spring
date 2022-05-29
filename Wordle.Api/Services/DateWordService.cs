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
            int maxReturedWords = 10;
            int wordCount = _context.DateWords.Count();
            if (wordCount > 9)
            {
                words = _context.DateWords.OrderByDescending(x => x.Date).Take(10);
            }
            else
            {
                words = _context.DateWords.OrderBy(x => x.Date).Take(maxReturedWords);
            }

            var player = _context.Players.Include(x => x.Games).FirstOrDefault(x => x.Name == name);

            foreach (var word in words)
            {
                DailyWordStatDto dto = new(word.Date, GetTotalPlays(word), GetAverageScore(word), GetAverageTime(word), GetHasPlayed(name, word));
                lastTen.Add(dto);
            }

            return lastTen;

        }

        public bool GetHasPlayed(string? name, DateWord word)
        {
            if (name == null)
            {
                return false;
            }
            var player = _context.Players.Include(x => x.Games).FirstOrDefault(x => x.Name == name);
            if (player is null) return false;
            foreach (var g in player.Games)
            {
                if (g.DateWordId == word.DateWordId)
                {
                    return true;
                }
            }

            return false;

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
                if (s.ScoreStat is not null)
                {
                    listOfScores.Add(s.ScoreStat!.Score);
                }
            }
            if (listOfScores.Count() > 0)
            {
                return (int)listOfScores.Average();
            }
            else
            {
                return 0;
            }
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
                if (s.ScoreStat is not null)
                {
                    listOfScores.Add(s.ScoreStat!.Seconds);
                }
            }

            if (listOfScores.Count() > 0)
            {
                return (int)listOfScores.Average();
            }
            else
            {
                return 0;
            }
        }

        public (bool, string) SubmitGame(DateTime date, string username, int score, int timeSeconds)
        {
            string msg = "";
            try
            {
                // find the dateword
                DateWord? currentDateWord = _context.DateWords
                    .Include(x => x.Games)
                    .Include(x => x.Players)
                    .Include(x => x.Word)
                    .FirstOrDefault(x => x.Date == date);
                // if currentDateWord is null, the current days game has not been created 
                // previously and this game submission is wrong/malicious
                if (currentDateWord == null)
                {
                    // end here and do nothing with the game submission
                    msg = "DateWord does not exist";
                    return (false, msg);
                }
                // find player by username
                Player? currentPlayer = _context.Players
                    .Include(x => x.Games)
                    .FirstOrDefault(x => x.Name == username);
                // if player not already in database, create a new player using submitted game stats 
                if (currentPlayer == null)
                {
                    _context.Players.Add(new Player()
                    {
                        Name = username,
                        GameCount = 1,
                        AverageAttempts = score,
                        AverageSecondsPerGame = timeSeconds
                    });
                    _context.SaveChanges();
                    // then reset currentPlayer reference to the newly created player
                    currentPlayer = _context.Players
                        .Include(x => x.Games)
                        .First(x => x.Name == username);
                }
                else
                {
                    //update that players stats
                    currentPlayer.AverageSecondsPerGame =
                        (currentPlayer.GameCount * currentPlayer.AverageSecondsPerGame + timeSeconds)
                        / (currentPlayer.GameCount + 1);

                    currentPlayer.AverageAttempts =
                        (currentPlayer.GameCount * currentPlayer.AverageAttempts + score)
                        / (currentPlayer.GameCount + 1);

                    currentPlayer.GameCount++;
                }
                var scoreStat = new ScoreStat()
                {
                    Score = score,
                    Seconds = timeSeconds
                };
                _context.ScoreStats.Add(scoreStat);
                _context.SaveChanges();
                // create a game object
                Game g = new Game()
                {
                    Player = currentPlayer,
                    Word = currentDateWord.Word,
                    DateWord = currentDateWord,
                    ScoreStat = scoreStat,
                    // There will be a difference between the aggregated DateStarted and DateEnded times 
                    // vs an individual Players AverageSecondsPerGame. By having the explicit field for
                    // AverageSecondsPerGame on the Player object we can reduce the DB calls that 
                    // would otherwise calculate the 'Actual' average from the list of Game objects
                    DateStarted = DateTime.Now.AddSeconds(-timeSeconds),// adding negative seconds
                    DateEnded = DateTime.Now,
                };



                //currentPlayer.Games.Add(g);
                //currentDateWord.Games.Add(g);
                _context.Games.Add(g);
                _context.SaveChanges();


                // attach game to DateWord and Player

                //_context.Games.Add(submittedGame);

                // make sure save goes through (NOT async) before returning result

                return (true, msg);
            }
            catch (Exception e)
            {
                msg += e;
                return (false, msg);
            }
        }

    }
}
