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

        //return alphabetical list of wordsw by value
        List<Word> words = _context.Words
            .OrderBy(x => x.Value)
            .ToList();

        //dont return extra items in word class
        List<(string, bool)> queryWord = new();

        if (query is null) query = "*";
        foreach (Word word in words)
        {
            if (word.Value.Contains(query))
            {
                queryWord.Add((word.Value, word.Common));
            }
        }

        totalItems = queryWord.Count;
        maxPages = totalItems / _pageSize;
        int currentIndex = _currentPage * _pageSize;
        //add pages items to returnable list
        for (int i = 0; i < _pageSize; i++)
        {
            if (currentIndex + i < totalItems)
                returnable.Add(queryWord[currentIndex + i]);
        }
    }


}
