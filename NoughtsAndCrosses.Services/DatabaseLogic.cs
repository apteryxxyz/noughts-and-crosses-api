using NoughtsAndCrosses.Domain;
using Microsoft.EntityFrameworkCore;
using NoughtsAndCrosses.Repository;

namespace NoughtsAndCrosses.Services;

public class DatabaseLogic
{
    public static DatabaseContext _context = new DatabaseContext(new DbContextOptions<DatabaseContext> { });

    /// <summary>Get a game by its ID.</summary>
    /// <param name="id">Game ID int.</param>
    /// <returns>The game if it exists.</returns>
    public static async Task<Game?> GetGame(int id)
    {
        var result = await _context.Game.FindAsync(id);
        return result;
    }

    /// <summary>Creates a brand new game.</summary>
    /// <returns>The game instance.</returns>
    public static async Task<Game> CreateGame()
    {
        var key = Guid.NewGuid().ToString("N");
        var game = new Game { Key = key };
        _context.Game.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    /// <summary>Save the game changes.</summary>
    public static async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }


}
