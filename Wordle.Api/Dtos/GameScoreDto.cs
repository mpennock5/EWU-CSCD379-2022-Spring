namespace Wordle.Api.Dtos
{
    public class GameScoreDto
    {
        public int Score { get; set; }
        public string Name { get; set; }

        public GameScoreDto(int score, string name)
        {
            Score = score;
            Name = name;
        }
    }
}
