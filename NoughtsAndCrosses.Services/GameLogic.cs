using NoughtsAndCrosses.Domain;

namespace NoughtsAndCrosses.Services;

public class GameLogic
{
    /// <summary>Confirm whether an input is a valid player number.</summary>
    /// <param name="player">Player number.</param>
    /// <returns>True or false.</returns>
    public static bool IsValidPlayer(int player)
    {
        if (player == 1 || player == 2) return true;
        return false;
    }

    /// <summary>Confirm whether an input is within the grid index range.</summary>
    /// <param name="position">Position number.</param>
    /// <returns>True or false.</returns>
    public static bool IsValidPosition(int position)
    {
        if (position < 0 || position > 9) return false;
        return true;
    }

    /// <summary>Confirm whether it is a players turn.</summary>
    /// <param name="game">Game to check on.</param>
    /// <param name="player">Player number.</param>
    /// <returns>True or false.</returns>
    public static bool IsPlayersTurn(Game game, int player)
    {
        if (game.NextPlayer != player) return false;
        return true;
    }

    /// <summary>Confirm whether a index is available in a game grid.</summary>
    /// <param name="game">Game to check grid of.</param>
    /// <param name="position">Position index.</param>
    /// <returns>True or false.</returns>
    public static bool IsPositionAvailable(Game game, int position)
    {
        if (game.Grid[position] != '0') return false;
        return true;
    }

    /// <summary>Check whether a game has finished.</summary>
    /// <param name="game">Game to check state of.</param>
    /// <returns>True or false.</returns>
    public static bool HasFinished(Game game)
    {
        if (game.FinishState == 0) return false;
        return true;
    }

    /// <summary>Make a move in a game.</summary>
    /// <param name="game">The game to make the move on.</param>
    /// <param name="player">The player number that is making this move.</param>
    /// <param name="position">The grid position to make the move in.</param>
    /// <returns>The finish state of the game</returns>
    /// <exception cref="Exception">Thrown when this move cannot be made.</exception>
    public static int MakeMove(Game game, int player, int position)
    {
        // Check that this move can be made
        if (HasFinished(game)) throw new Exception("Game has ended");
        if (!IsValidPlayer(player)) throw new Exception("That is not a valid player");
        if (!IsPlayersTurn(game, player)) throw new Exception("It is not that players turn");
        if (!IsValidPosition(position)) throw new Exception("That is not a valid grid position");
        if (!IsPositionAvailable(game, position)) throw new Exception("Grid position is taken");

        // Update the game object to include the move
        game.NextPlayer = player == 1 ? 2 : 1;
        game.Grid = game.Grid.Remove(position, 1).Insert(position, player.ToString());
        return CheckForWinner(game);
    }

    /// <summary>Check the game grid for a winner.</summary>
    /// <param name="game">Game to check grid of.</param>
    /// <returns>The games [new] finish state.</returns>
    public static int CheckForWinner(Game game)
    {
        // Check the game hasnt already finished
        if (game.FinishState != 0) return game.FinishState;

        int[] top = Array.ConvertAll(new char[] { game.Grid[0], game.Grid[1], game.Grid[2] }, v => v - '0');
        int[] middle = Array.ConvertAll(new char[] { game.Grid[3], game.Grid[4], game.Grid[5] }, v => v - '0');
        int[] bottom = Array.ConvertAll(new char[] { game.Grid[6], game.Grid[7], game.Grid[8] }, v => v - '0');

        int state = 0;

        // Check rows
        if (top[0] != 0 && top[0] == top[1] && top[1] == top[2]) state = top[0];
        if (middle[0] != 0 && middle[0] == middle[1] && middle[1] == middle[2]) state = middle[0];
        if (bottom[0] != 0 && bottom[0] == bottom[1] && bottom[1] == bottom[2]) state = bottom[0];

        // Check columns
        if (top[0] != 0 && top[0] == middle[0] && middle[0] == bottom[0]) state = top[0];
        if (top[1] != 0 && top[1] == middle[1] && middle[1] == bottom[1]) state = top[1];
        if (top[2] != 0 && top[2] == middle[2] && middle[2] == bottom[2]) state = top[2];

        // Check diagonal
        if (top[0] != 0 && top[0] == middle[1] && middle[1] == bottom[2]) state = top[0];
        if (top[2] != 0 && top[2] == middle[1] && middle[1] == bottom[0]) state = top[2];

        // If grid is full then return a draw
        if (state == 0 && !game.Grid.Contains('0')) state = 3;

        // Update game if has ended
        if (state != 0) game.FinishState = state;
        return game.FinishState;
    }
}
