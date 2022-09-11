namespace NoughtsAndCrosses.Domain;

public class GameDTO
{
    public int Id { get; set; }
    public int NextPlayer { get; set; }
    public int FinishState { get; set; }
    public string Grid { get; set; }
    public string? Key { get; set; }

    public GameDTO(Game game, bool includeKey)
    {
        Id = game.Id;
        if (includeKey) Key = game.Key;

        NextPlayer = game.NextPlayer;
        FinishState = game.FinishState;
        Grid = game.Grid;
    }
}

public class GameMoveDTO
{
    public int Player { get; set; }
    public int Position { get; set; }
}