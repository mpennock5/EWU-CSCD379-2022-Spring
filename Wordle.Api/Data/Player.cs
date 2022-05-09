using System.ComponentModel.DataAnnotations;

namespace Wordle.Api.Data;

public class Player
{
    [Key]
    public int PlayerId { get; set; }
    public string? Name { get; set; }
    public int GameCount { get; set; }
    public double AverageAttempts { get; set; }
    public int AverageSecondsPerGame { get; set; }

    public Player()
    {
        PlayerId = PlayerId;
        Name = Name;
        GameCount = GameCount;
        AverageAttempts = AverageAttempts;
    }
    public Player(int id, string name, int attempts, int seconds)
    {
        PlayerId = id;
        Name = name;
        GameCount = 1;
        AverageAttempts = attempts;
        AverageSecondsPerGame = seconds;
    }
}

