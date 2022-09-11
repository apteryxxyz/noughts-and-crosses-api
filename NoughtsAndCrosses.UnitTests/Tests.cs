using NUnit.Framework;
using NoughtsAndCrosses.Domain;
using NoughtsAndCrosses.Services;

namespace NoughtsAndCrosses.UnitTests;

public class Tests
{
    Game game = default!;

    [SetUp]
    public void Setup()
    {
        game = new Game { };
    }

    // Base test, checks that moves can be made
    [Test, Order(1)]
    public void MakeMoves()
    {
        GameLogic.MakeMove(game, 1, 4); // Middle middle
        GameLogic.MakeMove(game, 2, 7); // Bottom middle
        GameLogic.MakeMove(game, 1, 6); // Bottom left
        GameLogic.MakeMove(game, 2, 2); // Top right

        Assert.Multiple(delegate
        {
            Assert.That(game.Grid[4], Is.EqualTo('1'));
            Assert.That(game.NextPlayer, Is.EqualTo(1));
            Assert.That(game.FinishState, Is.EqualTo(0));
        });
    }
    
    // Test a player making a move when it is not their turn
    [Test, Order(2)]
    public void NotTurn()
    {
        MakeMoves();

        Assert.Multiple(delegate {
            Assert.Throws(Is.TypeOf<Exception>(),
                delegate { GameLogic.MakeMove(game, 999, 8); });
            Assert.Throws(Is.TypeOf<Exception>(),
                delegate { GameLogic.MakeMove(game, 2, 8); });
        });
    }

    // Test a taken position
    [Test, Order(2)]
    public void TakenPosition()
    {
        MakeMoves();

        Assert.Multiple(delegate {
            Assert.Throws(Is.TypeOf<Exception>(),
                delegate { GameLogic.MakeMove(game, 1, 999); });
            Assert.Throws(Is.TypeOf<Exception>(),
                delegate { GameLogic.MakeMove(game, 1, 4); });
        });
    }

    // Test for a winner
    [Test, Order(3)]
    public void FinishWinner()
    {
        MakeMoves();

        GameLogic.MakeMove(game, 1, 3); // Middle Left
        GameLogic.MakeMove(game, 2, 5); // Middle right
        GameLogic.MakeMove(game, 1, 0); // Top left

        Assert.Multiple(delegate
        {
            Assert.Throws(Is.TypeOf<Exception>(),
                delegate { GameLogic.MakeMove(game, 2, 8); });
            Assert.That(game.FinishState, Is.EqualTo(1));
        });
    }

    // Test for a draw
    [Test, Order(3)]
    public void FinishDraw()
    {
        MakeMoves();

        GameLogic.MakeMove(game, 1, 8); // Bottom right
        GameLogic.MakeMove(game, 2, 0); // Top left
        GameLogic.MakeMove(game, 1, 1); // Top middle
        GameLogic.MakeMove(game, 2, 3); // Middle left
        GameLogic.MakeMove(game, 1, 5); // Middle right

        Assert.Multiple(delegate
        {
            Assert.Throws(Is.TypeOf<Exception>(),
                delegate { GameLogic.MakeMove(game, 2, 0); });
            Assert.That(game.FinishState, Is.EqualTo(3));
        });
    }
}
