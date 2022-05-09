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

    [HttpGet]
    public string GetTop10PlayersJson()
    {
        string jsonString = JsonSerializer.Serialize(GetTop10Players());
        return jsonString;
    }

    public List<Player> GetTop10Players()
    {
        var allPlayers = new List<Player>();
        allPlayers.AddRange(_players.GetPlayers());
        allPlayers.OrderByDescending(x => (7 - x.AverageAttempts) * x.GameCount);
        var top10Players = new List<Player>();
        for (int i = 0; i < allPlayers.Count && i < 10; i++)
        {
            top10Players.Add(allPlayers[i]);
        }
        return top10Players;
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
