using Wordle.Api.Data;
using Wordle.Api.Dtos;

namespace Wordle.Api.Services;

public class WordService
{
    private readonly AppDbContext _context;
    private readonly static object _mutex = new();
    // lock (_mutex){}
    public WordService(AppDbContext context)
    {
        _context = context;
    }
    //TODO implement await async pattern
    public bool DeleteWordByValue(string target)
    {
        lock (_mutex)
        {
            var word = _context.Words.FirstOrDefault(x => x.Value == target);
            if (word != null)
            {
                _context.Words.Remove(word);
                _context.SaveChanges();
                return true;
            }
            return false;   
        }
    }
    //TODO implement await async pattern
    public bool AddWordByValue(string target)
    {
        lock( _mutex)
        {
            var tableCheck = _context.Words.FirstOrDefault(x => x.Value == target);
            if(tableCheck == null)
            {
                var word = new Word()
                {
                    Value = target,
                    Common = true

                };
                _context.Add(word);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
    public PageDto GetWordPage(int pageSize, int currentPage, string query)
    {

        var foundWords = _context.Words
                .Where(x => x.Value.StartsWith(query))
                //test to see if this is faster (not sorting in database)
                //ToList().OrderBy().Skip().Take()
                .Skip(pageSize * currentPage)
                .Take(pageSize)
                .OrderBy(x => x.Value)
                .Select(x => new Tuple<string, bool>(x.Value, x.Common)).ToList();
        int total = foundWords.Count();
        return new PageDto()
        {

            PageSize = pageSize,
            CurrentPage = currentPage,
            returnable = foundWords,
            TotalItems = total,
            MaxPages = (int)Math.Ceiling((double)total / pageSize)
        };

    }
    
}