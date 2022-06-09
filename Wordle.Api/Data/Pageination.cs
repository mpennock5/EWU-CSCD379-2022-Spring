using System.Linq;

namespace Wordle.Api.Data;
public class Pageination
{
    private AppDbContext _context;
    private int totalItems;
    private int _currentPage;
    private int maxPages;
    private int _pageSize;
    List<(string, bool)> returnable = new();


    public Pageination(AppDbContext context, int pageSize, int currentPage, string query)
    {
        _context = context;
        _pageSize = pageSize;
        _currentPage = currentPage;

        // pageSized alphabetical list of words
        List<Word>? foundwords = _context.Words
            .Where(x => x.Value.Contains(query))
            .OrderBy(x => x.Value)
            .Skip(pageSize * currentPage)
            .Take(pageSize)
            .ToList();

        foreach (var word in foundwords)
        {
            returnable.Add((word.Value, word.Common));
        }

        // it is possible to have an empty list
        totalItems = returnable.Count;
        maxPages = (int)Math.Ceiling((double)totalItems / _pageSize);
    }


}
