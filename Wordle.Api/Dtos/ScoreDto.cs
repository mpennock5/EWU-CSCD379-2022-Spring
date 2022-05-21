namespace Wordle.Api.Dtos
{
    public class ScoreDto
    {
        public string Name { get; set; }
        public int NumberGames { get; set; }
        public double AverageGuesses { get; set; }

        public ScoreDto(string name, int numberGames, double averageGuesses)
        {
            Name = name;
            NumberGames = numberGames;
            AverageGuesses = averageGuesses;
        }
    }
}
