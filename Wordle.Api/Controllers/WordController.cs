using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController : Controller
{
    private readonly WordService _wordService;
    public WordController(WordService wordService)
    {
        _wordService = wordService;
    }


    [HttpGet("GetWordsPerPage")]
    public object GetWordPage(int pageSize, int currentPage, string? query)
    {
        // verify query
        if (query is not null)
        {
            query = query.ToLower();
            Regex rgx = new Regex("[a-z]{0,5}");
            if (!rgx.IsMatch(query))
            {
                return "regex fail";
            }
        }
        else query = "*";

        // verify page size
        if (pageSize >= 10 && pageSize <= 100)
        {
            Pageination page = _wordService.GetWordPage(pageSize, currentPage, query);
            return page;
        }

        return "controller fail";
    }
}
