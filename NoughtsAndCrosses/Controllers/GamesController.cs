using Microsoft.AspNetCore.Mvc;
using NoughtsAndCrosses.Domain;
using NoughtsAndCrosses.Services;

namespace NoughtsAndCrosses.Controllers;


/// <summary>Handles the games API endpoints.</summary>
[Route("api/games")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;

    public GamesController(ILogger<GamesController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>Get a game instance by its ID.</summary>
    /// <param name="id">The ID.</param>
    /// <returns>The game object if it exists.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDTO>> GetGame(int id)
    {
        var game = await DatabaseLogic.GetGame(id);
        if (game is null) return NotFound();
        return new GameDTO(game, false);
    }

    /// <summary>Create a new game instance.</summary>
    /// <returns>The just created game object.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GameDTO>> PutGame()
    {
        var game = await DatabaseLogic.CreateGame();
        _logger.LogInformation($"CREATED game with ID {game.Id}");

        return CreatedAtAction(
            nameof(GetGame),
            new { id = game.Id },
            new GameDTO(game, true)
        );
    }

    /// <summary>Make a move within a game.</summary>
    /// <param name="id">The game ID.</param>
    /// <param name="move">The move object.</param>
    /// <param name="key">API Key</param>
    /// <returns>The new game object.</returns>
    [HttpPost("{id}/move")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDTO>> MakeMove(int id, GameMoveDTO move, [FromHeader(Name = "key")] string key)
    {
        var game = await DatabaseLogic.GetGame(id);
        if (game is null) return NotFound();

        try { GameLogic.MakeMove(game, move.Player, move.Position); }
        catch (Exception e) { return BadRequest(e.Message); };

        Console.WriteLine(game.ToString());
        await DatabaseLogic.SaveChanges();

        return CreatedAtAction(
             nameof(GetGame),
             new { id = game.Id },
             new GameDTO(game, true)
         );
    }

    /// <summary>Show a diagram of the current state of a game.</summary>
    /// <param name="id">Game ID.</param>
    /// <returns>String grid.</returns>
    [HttpGet("{id}/diagram")]
    public async Task<string> GridVisual(int id)
    {
        var game = await DatabaseLogic.GetGame(id);
        if (game is null) return "";
        Console.WriteLine(game.ToString());
        return game.ToString();
    }
}
