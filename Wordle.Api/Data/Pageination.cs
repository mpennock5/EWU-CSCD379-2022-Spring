using System.Linq;

namespace Wordle.Api.Data;
public class Pageination
{
    private AppDbContext _context;
    private int totalItems;
    private int _currentPage;
    private int maxPages;
    private int _pageSize;
    public List<(string, bool)> returnable = new();


    public Pageination(AppDbContext context, int pageSize, int currentPage, string query)
    {
        _context = context;
        _pageSize = pageSize;
        _currentPage = currentPage;

        // pageSized alphabetical list of words
        var foundwords = _context.Words
            .Where(x => x.Value.StartsWith(query))
            //test to see if this is faster (not sorting in database)
            //ToList().OrderBy().Skip().Take()
            .Skip(pageSize * currentPage)
            .Take(pageSize)
            .OrderBy(x => x.Value);

        foreach (var word in foundwords)
        {
            returnable.Add((word.Value, word.Common));
        }

        // it is possible to have an empty list
        totalItems = returnable.Count;
        maxPages = (int)Math.Ceiling((double)totalItems / _pageSize);
    }


}
