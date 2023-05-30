using NUnit.Framework;
using System;
using System.Collections.Generic;

public class GameControllerTest
{
    private const int WIDTH = 5;

    private const int HEIGHT = 4;

    private const int MINES = 6;

    private GameController _gameController;

    private List<EventArgs> _winEvents;

    private List<PositionEventArgs> _looseEvents;

    private List<OpenCellEventArgs> _openCellEvents;

    private List<PositionEventArgs> _wrongFlagEvents;

    private List<PositionEventArgs> _closedMinesEvents;

    private List<ToggleFlagEventArgs> _toggleFlagEvents;

    [SetUp]
    public void SetUp()
    {
        _gameController = new GameController(MINES, WIDTH, HEIGHT, 321);

        _winEvents = new List<EventArgs>();
        _gameController.OnWin += GameControllerOnWin;

        _looseEvents = new List<PositionEventArgs>();
        _gameController.OnLoose += GameControllerOnLoose;

        _wrongFlagEvents = new List<PositionEventArgs>();
        _gameController.OnWrongFlag += GameControllerOnWrongFlag;

        _openCellEvents = new List<OpenCellEventArgs>();
        _gameController.OnOpenCell += GameControllerOnOpenCell;

        _toggleFlagEvents = new List<ToggleFlagEventArgs>();
        _gameController.OnToggleFlag += GameControllerOnToggleFlag;

        _closedMinesEvents = new List<PositionEventArgs>();
        _gameController.OnClosedMines += GameControllerOnClosedMines;
    }

    private void GameControllerOnWin(object sender, EventArgs args)
    {
        _winEvents.Add(args);
    }

    private void GameControllerOnLoose(object sender, PositionEventArgs args)
    {
        _looseEvents.Add(args);
    }

    private void GameControllerOnOpenCell(object sender, OpenCellEventArgs args)
    {
        _openCellEvents.Add(args);
    }

    private void GameControllerOnToggleFlag(object sender, ToggleFlagEventArgs args)
    {
        _toggleFlagEvents.Add(args);
    }

    private void GameControllerOnWrongFlag(object sender, PositionEventArgs args)
    {
        _wrongFlagEvents.Add(args);
    }

    private void GameControllerOnClosedMines(object sender, PositionEventArgs args)
    {
        _closedMinesEvents.Add(args);
    }

    [Test]
    public void TestWin()
    {
        _gameController.ToggleFlag(0, 0);
        _gameController.ToggleFlag(1, 1);
        _gameController.ToggleFlag(2, 0);
        _gameController.ToggleFlag(3, 0);
        _gameController.ToggleFlag(3, 1);
        _gameController.ToggleFlag(3, 2);

        for (var x = 0; x < WIDTH; x++)
        {
            for (var y = 0; y < HEIGHT; y++)
            {
                _gameController.OpenCell(x, y);
            }
        }

        Assert.AreEqual(1, _winEvents.Count);
    }

    [Test]
    public void TestLooseClosedMines()
    {
        _gameController.ToggleFlag(1, 1);
        _gameController.ToggleFlag(2, 1);
        _gameController.ToggleFlag(3, 1);
        _gameController.ToggleFlag(2, 2);
        _gameController.ToggleFlag(2, 0);
        _gameController.ToggleFlag(3, 0);

        _gameController.OpenCell(3, 2);

        Assert.AreEqual(2, _closedMinesEvents.Count);
        Assert.AreEqual(new PositionEventArgs(0, 0), _closedMinesEvents[0]);
        Assert.AreEqual(new PositionEventArgs(3, 2), _closedMinesEvents[1]);
    }

    [Test]
    public void TestLooseWrongFlags()
    {
        _gameController.ToggleFlag(1, 1);
        _gameController.ToggleFlag(2, 1);
        _gameController.ToggleFlag(3, 1);
        _gameController.ToggleFlag(2, 2);

        _gameController.OpenCell(3, 2);

        Assert.AreEqual(2, _wrongFlagEvents.Count);
        Assert.AreEqual(new PositionEventArgs(2, 1), _wrongFlagEvents[0]);
        Assert.AreEqual(new PositionEventArgs(2, 2), _wrongFlagEvents[1]);
    }

    [Test]
    public void TestLoose()
    {
        _gameController.OpenCell(3, 2);

        Assert.AreEqual(1, _looseEvents.Count);
        Assert.AreEqual(new Position(3, 2), _looseEvents[0].Position);
        Assert.AreEqual(0, _openCellEvents.Count);
    }

    [Test]
    public void TestExceedFlagLimit()
    {
        _gameController.ToggleFlag(0, 0);
        _gameController.ToggleFlag(1, 0);
        _gameController.ToggleFlag(2, 0);
        _gameController.ToggleFlag(3, 0);
        _gameController.ToggleFlag(4, 0);
        _gameController.ToggleFlag(0, 1);
        _gameController.ToggleFlag(1, 1);

        Assert.AreEqual(6, _toggleFlagEvents.Count);
    }

    [Test]
    public void TestPlaceFlag()
    {
        _gameController.ToggleFlag(3, 2);

        var expectedArgs = new ToggleFlagEventArgs(3, 2, FlagStatus.Flag, 5);

        Assert.AreEqual(1, _toggleFlagEvents.Count);
        Assert.AreEqual(expectedArgs, _toggleFlagEvents[0]);
    }

    [Test]
    public void TestRemoveFlag()
    {
        _gameController.ToggleFlag(3, 2);
        _gameController.ToggleFlag(3, 2);

        var expectedArgs = new ToggleFlagEventArgs(3, 2, FlagStatus.NoFlag, 6);

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

