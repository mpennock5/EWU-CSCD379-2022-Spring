using Wordle.Api.Data;
using System.Linq;
using Wordle.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Wordle.Api.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }
        
        public Game CreateGame(Guid guid)  
        {
            var player = _context.Players
                .FirstOrDefault(x => x.Guid == guid);
            if (player is null)
            {
                player = new Player { Guid = guid };
                _context.Players.Add(player);
                _context.SaveChanges();
            }

            var game = new Game()
            {
                Word = GetWord(),
                Player = player,
                DateStarted = DateTime.UtcNow
            };
            
            _context.SaveChanges();

            return game;
            
        }
        public Word GetWord()
        {
            int wordCount = _context.Words.Count();
            int randomIndex = new Random().Next(0, wordCount);
            Word chosenWord = _context.Words
                .OrderBy(w => w.WordId)
                .Skip(randomIndex)
                .Take(1)
                .First();
            return chosenWord;
        }

        public static void Seed(AppDbContext context)
        {
            if(!context.Games
                .Include(x => x.Player)
                .Any(x => x.Player.Name == "Knights who say Ni"))
            {
                var player = context.Players.First(x => x.Name == "Knights who say Ni");
                var dateWord = context.DateWords.First(x => x.WordId == 92);
                var score = context.ScoreStats.First(x => x.Seconds == 42);
                context.Games.Add(new Game()
                {

                    Player = player,
                    WordId = 92,
                    DateEnded = new DateTime(2022, 05, 18),
                    ScoreStatId = score.ScoreStatId,
                    DateWord = dateWord

                });
                context.SaveChanges();
                dateWord = context.DateWords.First(x => x.WordId == 270);
                score = context.ScoreStats.First(x => x.Seconds == 45);
                context.Games.Add(new Game()
                {

                    Player = player,
                    WordId = 270,
                    DateEnded = new DateTime(2022, 05, 19),
                    ScoreStatId = score.ScoreStatId,
                    DateWord = dateWord

                });
                context.SaveChanges();
                dateWord = context.DateWords.First(x => x.WordId == 104);
                score = context.ScoreStats.First(x => x.Seconds == 60);
                context.Games.Add(new Game()
                {

                    Player = player,
                    WordId = 104,
                    DateEnded = new DateTime(2022, 05, 20),
                    ScoreStatId = score.ScoreStatId,
                    DateWord = dateWord

                });
                context.SaveChanges();
                dateWord = context.DateWords.First(x => x.WordId == 944);
                score = context.ScoreStats.First(x => x.Seconds == 80);
                context.Games.Add(new Game()
                {

                    Player = player,
                    WordId = 944,
                    DateEnded = new DateTime(2022, 05, 21),
                    ScoreStatId = score.ScoreStatId,
                    DateWord = dateWord

                });
                context.SaveChanges();
                dateWord = context.DateWords.First(x => x.WordId == 1006);
                score = context.ScoreStats.First(x => x.Seconds == 12);
                context.Games.Add(new Game()
                {

                    Player = player,
                    WordId = 1006,
                    DateEnded = new DateTime(2022, 05, 22),
                    ScoreStatId = score.ScoreStatId,
                    DateWord = dateWord

                });
                context.SaveChanges();
            }
        }
    }
}
