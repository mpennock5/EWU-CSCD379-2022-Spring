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
    public Pageination GetWordPage(int pageSize, int currentPage, string query)
    {
        return new Pageination(_context, pageSize, currentPage, query);
    }
}