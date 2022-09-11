using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoughtsAndCrosses.Domain;

public class Game
{
    // The games unique identifier
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // The player with the next turn
    public int NextPlayer { get; set; } = 1;

    // Represents the end state
    // 0 = not finished, 1 = player 1 won, 2 = player 2 won, 3 = draw
    public int FinishState { get; set; } = 0;
    
    // The game grid, 9 chars representing player numbers
    public string Grid { get; set; } = "000000000";

    // API key needed to interactive with this game
    public string Key { get; set; } = default!;

    /// <summary>Makes a nice little visual of what this games grid would look like.</summary>
    /// <returns>Noughts and crosses grid.</returns>
    public override string ToString()
    {
        char PosToChar(char pos)
        {
            if (pos == '1') return 'x';
            else if (pos == '2') return 'o';
            return ' ';
        }

        return PosToChar(Grid[0]) + "|" + PosToChar(Grid[1]) + "|" + PosToChar(Grid[2]) +
               "\n-----\n" +
               PosToChar(Grid[3]) + "|" + PosToChar(Grid[4]) + "|" + PosToChar(Grid[5]) +
               "\n-----\n" +
               PosToChar(Grid[6]) + "|" + PosToChar(Grid[7]) + "|" + PosToChar(Grid[8]);
    }
}
