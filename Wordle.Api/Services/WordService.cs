using Wordle.Api.Data;

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
    public Pageination GetWordPage(int pageSize, int currentPage, string query)
    {
        //TODO move object from data folder to DTO and business logic to this method
        return new Pageination(_context, pageSize, currentPage, query);
    }
}