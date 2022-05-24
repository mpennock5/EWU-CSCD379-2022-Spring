namespace Wordle.Api.Dtos
{
    public class DailyWordStatDto
    {
        public DateTime Date { get; set; }
        public int TotalPlays { get; set; }
        public int AverageScore { get; set; }
        public int AverageTime { get; set; }
        public bool? HasPlayed { get; set; }

        public DailyWordStatDto(DateTime date, int totalPlays, int averageScore, int averageTime, bool? hasPlayed)
        {
            Date = date;
            TotalPlays = totalPlays;
            AverageScore = averageScore;
            AverageTime = averageTime;
            HasPlayed = hasPlayed;
        }

    }
}
