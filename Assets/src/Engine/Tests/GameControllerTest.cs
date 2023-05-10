using NUnit.Framework;
using System.Collections.Generic;

public class GameControllerTest
{
    private const int WIDTH = 5;

    private const int HEIGHT = 4;

    private const int MINES = 6;

    private GameController _gameController;

    [SetUp]
    public void SetUp()
    {
        _gameController = new GameController(MINES, WIDTH, HEIGHT, 321);
    }

    [Test]
    public void TestMinesCount()
    {
        var mineCount = 0;

        foreach (var c in each())
        {
            if (_gameController.IsMine(c.x, c.y))
            {
                mineCount++;
            }
        }

        Assert.AreEqual(MINES, mineCount);
    }

    [Test]
    public void TestMapGeneration()
    {
        var field = new int[,]
        {
            {-1, 2, 1, 0},
            {3, -1, 1, 0},
            {-1, 5, 3, 1},
            {-1, -1, -1, 1},
            {2, 3, 2, 1}
        };

        foreach (var pos in each())
        {
            Assert.AreEqual(field[pos.x, pos.y], _gameController.MinesAround(pos.x, pos.y));
        }
    }

    private IEnumerable<(int x, int y)> each()
    {
        for (var x = 0; x < WIDTH; x++)
        {
            for (var y = 0; y < HEIGHT; y++)
            {
                yield return (x, y);
            }
        }
    }
}

