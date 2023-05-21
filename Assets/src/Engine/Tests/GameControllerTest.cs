using NUnit.Framework;
using System.Collections.Generic;

public class GameControllerTest
{
    private const int WIDTH = 5;

    private const int HEIGHT = 4;

    private const int MINES = 6;

    private GameController _gameController;

    private List<OpenCellEventArgs> _openCellEvents;

    private List<ToggleFlagEventArgs> _toggleFlagEvents;

    [SetUp]
    public void SetUp()
    {
        _gameController = new GameController(MINES, WIDTH, HEIGHT, 321);

        _openCellEvents = new List<OpenCellEventArgs>();
        _gameController.OnOpenCell += GameControllerOnOpenCell;

        _toggleFlagEvents = new List<ToggleFlagEventArgs>();
        _gameController.OnToggleFlag += GameControllerOnToggleFlag;
    }

    private void GameControllerOnOpenCell(object sender, OpenCellEventArgs args)
    {
        _openCellEvents.Add(args);
    }

    private void GameControllerOnToggleFlag(object sender, ToggleFlagEventArgs args)
    {
        _toggleFlagEvents.Add(args);
    }

    [Test]
    public void TestPlaceFlag()
    {
        _gameController.ToggleFlag(3, 2);

        var expectedArgs = new ToggleFlagEventArgs(3, 2, FlagStatus.Flag);

        Assert.AreEqual(1, _toggleFlagEvents.Count);
        Assert.AreEqual(expectedArgs, _toggleFlagEvents[0]);
    }

    [Test]
    public void TestRemoveFlag()
    {
        _gameController.ToggleFlag(3, 2);
        _gameController.ToggleFlag(3, 2);

        var expectedArgs = new ToggleFlagEventArgs(3, 2, FlagStatus.NoFlag);

        Assert.AreEqual(2, _toggleFlagEvents.Count);
        Assert.AreEqual(expectedArgs, _toggleFlagEvents[1]);
    }

    [Test]
    public void TestCanNotPlaceFlag()
    {
        _gameController.OpenCell(2, 1);
        _gameController.ToggleFlag(2, 1);

        Assert.AreEqual(0, _toggleFlagEvents.Count);
    }

    [Test]
    public void TestOpenMultipleCells()
    {
        _gameController.OpenCell(0, 3);

        var expectedArgs = new HashSet<OpenCellEventArgs>() {
            new OpenCellEventArgs(0, 2, 1),
            new OpenCellEventArgs(0, 3, 0),
            new OpenCellEventArgs(1, 2, 1),
            new OpenCellEventArgs(1, 3, 0),
            new OpenCellEventArgs(2, 2, 3),
            new OpenCellEventArgs(2, 3, 1),
        };

        Assert.AreEqual(expectedArgs.Count, _openCellEvents.Count);
        foreach (var args in _openCellEvents)
        {
            Assert.IsTrue(expectedArgs.Contains(args));
            expectedArgs.Remove(args);
        }
    }

    [Test]
    public void TestOpenOneCell()
    {
        _gameController.OpenCell(0, 2);

        var expectedArgs = new OpenCellEventArgs(0, 2, 1);

        Assert.AreEqual(1, _openCellEvents.Count);
        Assert.AreEqual(expectedArgs, _openCellEvents[0]);
    }

    [Test]
    public void TestOpenCellOnce()
    {
        _gameController.OpenCell(0, 2);
        _gameController.OpenCell(0, 2);

        Assert.AreEqual(1, _openCellEvents.Count);
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

