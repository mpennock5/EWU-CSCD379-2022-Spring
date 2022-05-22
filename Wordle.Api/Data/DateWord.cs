using System.ComponentModel.DataAnnotations.Schema;

namespace Wordle.Api.Data;
public class DateWord
{
    public int DateWordId { get; set; }
    public DateTime Date { get; set; }
    public Word Word { get; set; } = null!;
    public int WordId { get; set; }
    public int TotalPlays { get; set; }
    public int AverageGuesses { get; set; }
    public int AverageSeconds { get; set; }

    public IList<Game> Games { get; set; } = null!;
    public IList<Player> Players { get; set; } = null!;
}