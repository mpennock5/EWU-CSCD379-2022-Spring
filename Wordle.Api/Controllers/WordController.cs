﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Wordle.Api.Data;
using Wordle.Api.Services;
using Wordle.Api.Dtos;

namespace Wordle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController : Controller
{
    private readonly WordService _wordService;
    private readonly AppDbContext _context;
    public WordController(WordService wordService, AppDbContext context)
    {
        _context = context;
        _wordService = wordService;
    }

    //can also use this method for the search
    [HttpGet("GetWordsPerPage")]
    public PageDto GetWordPage(int pageSize, int currentPage, string? query)
    {
        // verify query
        if (query is not null)
        {
            query = query.ToLower();
            Regex rgx = new Regex("[a-z]{1,5}");
            if (!rgx.IsMatch(query))
            {
                //return "regex fail";
                throw new ArgumentException("regex fail");
            }
        }
        
        else query = "";

        // verify page size
        if (pageSize >= 10 && pageSize <= 100)
        {
            PageDto page = _wordService.GetWordPage(pageSize, currentPage, query);
            return page;
        }

        //return "controller fail";
        throw new Exception("controller fail");
    }
    [HttpPost("SetCommonWord")]
    [Authorize]
    public IActionResult SetCommonWord(string target, bool common)
    {
        var word = _context.Words.FirstOrDefault(x=>x.Value == target);
        if(word == null)
        {
            return StatusCode(500, "An error has occured. Contact your database administrator");
        }

        word.Common = common;
        return Ok();
    }
    [HttpPost("DeleteWord")]
    [Authorize]
    public IActionResult DeleteWord(string target)
    {
       
        if (_wordService.DeleteWordByValue(target))
        {
            return Ok();
            // call the getlist method to display updated wordlist?
        }
        return BadRequest();
        
    }

    [HttpPost("AddWord")]
    [Authorize]
    public IActionResult AddWord(string target)
    {
        if (_wordService.AddWordByValue(target))
        {
            return Ok();
        }
        return BadRequest();
    }

}
