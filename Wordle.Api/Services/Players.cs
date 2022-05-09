using Wordle.Api.Data;

namespace Wordle.Api.Services
{
    public class Players
    {
        private AppDbContext _context;

        public Players(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Player> GetPlayers()
        {
            var result = _context.Players.OrderBy(x => x.Name);
            return result;
        }
        public bool PlayerExists(string name) => _context.Players.Any(x => x.Name == name);

        public Player GetPlayer(string name) => _context.Players.FirstOrDefault(x => x.Name == name);

        public void Update(string name, int attempts, int seconds)
        {
            if (attempts < 1 || attempts > 6)
            {
                throw new ArgumentException("Score must be between 1 and 6");
            }
            if (seconds < 1)
            {
                throw new ArgumentException("Seconds must be greater than 0");
            }
            if (PlayerExists(name))
            {
                var player = GetPlayer(name);
                player.AverageAttempts = (player.AverageAttempts * player.GameCount + attempts) / (player.GameCount + 1);
                player.AverageSecondsPerGame = (player.AverageSecondsPerGame * player.GameCount + seconds) / (player.GameCount + 1);
                player.GameCount++;
            }
            else
            {
                AddPlayer(name, attempts, seconds);
            }
            _context.SaveChanges();
        }

        public void AddPlayer(string name, int attempts, int seconds)
        {
            //_context.Players.Add(new Player(_context.Players.Count() + 1, name, attempts, seconds));
            _context.Players.Add(new Player()
            {
                Name = name,
                AverageAttempts = attempts,
                AverageSecondsPerGame = seconds,
                GameCount = 1
            });
        }

        public static void Seed(AppDbContext context)
        {
            if (!context.Players.Any())
            {
                for (int i = 1; i <= 6; i++)
                {
                    //context.Players.Add(new Player(context.Players.Count() + 1, "player" + i, 1, 600));
                    context.Players.Add(new Player()
                    {
                        Name = "player_" + i,
                        AverageAttempts = 3,
                        AverageSecondsPerGame = 600,
                        GameCount = 1
                    });
                }
                context.SaveChanges();
            }

        }
    }

    //public void Update(int score, int seconds)
    //{
    //    if (score < 1 || score > 6)
    //    {
    //        throw new ArgumentException("Score must be between 1 and 6, and the admin password is 'password123''");
    //    }
    //    if (seconds < 1)
    //    {
    //        throw new ArgumentException("Seconds must be greater than 0");
    //    }
    //    var scoreStat = _context.ScoreStats.First(x => x.Score == score);
    //    scoreStat.AverageSeconds = (scoreStat.AverageSeconds * scoreStat.TotalGames + seconds) / ++scoreStat.TotalGames;
    //    _context.SaveChanges();
    //}
}