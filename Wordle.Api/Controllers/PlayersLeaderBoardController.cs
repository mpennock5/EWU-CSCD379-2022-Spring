using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class PlayersLeaderBoardController : ControllerBase
{
    private readonly Players _players;

    public PlayersLeaderBoardController(Players players)
    {
        _players = players;
    }

    /*
    [HttpGet]
    public string GetTop10PlayersJson()
    {
        string jsonString = JsonSerializer.Serialize(GetTop10Players());
        return jsonString;
    }
    */
    [HttpGet]
    public IEnumerable<Player> GetTop10Players()
    {
        var allPlayers = new List<Player>();
        allPlayers.AddRange(_players.GetPlayers());
        
        IEnumerable<Player> top10 = allPlayers.OrderByDescending(x => (7 - x.AverageAttempts) * x.GameCount).Take(10);
        var top10Players = new List<Player>();
        /*
        for (int i = 0; i < top10.Count() && i < 10; i++)
        {
            top10Players.Add(top10.);
        }
        */
        return top10;
    }

    [HttpPost]
    public IActionResult PostResult([FromBody] PostablePlayer playerResult)
    {
        _players.Update(playerResult.Name,playerResult.Attempts, playerResult.Seconds);
        return Ok();
    }

    public class PostablePlayer
    {
        public string Name { get; set; }
        public int Attempts { get; set; }
        public int Seconds { get; set; }
    }

}
