using System;
using System.Collections.Generic;
using System.Linq;

public class GameController
{
    private const int MINE = -1;

    private int _cellsLeft;

    private int _flagsLeft;

    private bool _gameEnded = false;

    private readonly int[,] _fieldMap;

    private readonly FieldStatus[,] _fieldStatus;

    public event EventHandler<EventArgs> OnWin;

    public event EventHandler<PositionEventArgs> OnLoose;

    public event EventHandler<OpenCellEventArgs> OnOpenCell;

    public event EventHandler<PositionEventArgs> OnWrongFlag;

    public event EventHandler<PositionEventArgs> OnClosedMines;

    public event EventHandler<ToggleFlagEventArgs> OnToggleFlag;

    public GameController(int mines, int width, int height, int seed = 0)
    {
        _cellsLeft = width * height - mines;

        _flagsLeft = mines;

        _fieldStatus = new FieldStatus[width, height];
        _fieldMap = new int[width, height];

        var random = seed == 0 ? new Random() : new Random(seed);

        while (mines > 0)
        {
            var x = random.Next(width);
            var y = random.Next(height);

            if (_fieldMap[x, y] == MINE)
            {
                continue;
            }

            _fieldMap[x, y] = MINE;
            mines--;

            foreach (var pos in SurroundingCells(x, y))
            {
                if (_fieldMap[pos.X, pos.Y] == MINE)
                {
                    continue;
                }

                _fieldMap[pos.X, pos.Y]++;
            }
        }
    }

    public int Width
    {
        get
        {
            return _fieldStatus.GetLength(0);
        }
    }

    public int Height
    {
        get
        {
            return _fieldStatus.GetLength(1);
        }
    }

    private void CheckClosedMinesOnLoose()
    {
        foreach (var pos in AllFieldCells())
        {
            var x = pos.X;
            var y = pos.Y;

            if (_fieldStatus[x, y] == FieldStatus.Flag || _fieldMap[x, y] != MINE)
            {
                continue;
            }

            OnClosedMines?.Invoke(this, new PositionEventArgs(x, y));
        }
    }

    private void CheckFlagsOnLoose()
    {
        foreach (var pos in AllFieldCells())
        {
            var x = pos.X;
            var y = pos.Y;

            if (_fieldStatus[x, y] != FieldStatus.Flag || _fieldMap[x, y] == MINE)
            {
                continue;
            }

            OnWrongFlag?.Invoke(this, new PositionEventArgs(x, y));
        }
    }

    public void ToggleFlag(int x, int y)
    {
        if (_gameEnded)
        {
            return;
        }

        FlagStatus flagStatus;
        FieldStatus fieldStatus;
        switch (_fieldStatus[x, y])
        {
            case FieldStatus.Opened:
                return;
            case FieldStatus.Closed:
                if (_flagsLeft == 0)
                {
                    return;
                }
                flagStatus = FlagStatus.Flag;
                fieldStatus = FieldStatus.Flag;
                _flagsLeft--;
                break;
            case FieldStatus.Flag:
                flagStatus = FlagStatus.NoFlag;
                fieldStatus = FieldStatus.Closed;
                _flagsLeft++;
                break;
            default:
                throw new Exception("Unexpected field status");
        }

        _fieldStatus[x, y] = fieldStatus;
        OnToggleFlag?.Invoke(this, new ToggleFlagEventArgs(x, y, flagStatus, _flagsLeft));
    }

    public void OpenCell(int x, int y)
    {
        if (_gameEnded)
        {
            return;
        }

        if (_fieldStatus[x, y] == FieldStatus.Flag)
        {
            return;
        }

        if (_fieldStatus[x, y] != FieldStatus.Closed)
        {
            OpenCellsAround(x, y);
            return;
        }

        _fieldStatus[x, y] = FieldStatus.Opened;

        if (_fieldMap[x, y] == MINE)
        {
            _gameEnded = true;
            CheckFlagsOnLoose();
            CheckClosedMinesOnLoose();
            OnLoose?.Invoke(this, new PositionEventArgs(x, y));
            return;
        }

        var args = new OpenCellEventArgs(x, y, _fieldMap[x, y]);
        OnOpenCell?.Invoke(this, args);

        _cellsLeft--;
        if (_cellsLeft == 0)
        {
            OnWin?.Invoke(this, EventArgs.Empty);
            _gameEnded = true;
            return;
        }

        if (_fieldMap[x, y] != 0)
        {
            return;
        }

        foreach (var pos in SurroundingCells(x, y))
        {
            OpenCell(pos.X, pos.Y);
        }
    }

    private void OpenCellsAround(int x, int y)
    {
        var flagsAround = 0;

        foreach (var pos in SurroundingCells(x, y))
        {
            if (_fieldStatus[pos.X, pos.Y] == FieldStatus.Flag)
            {
                flagsAround++;
            }
        }

        if (flagsAround != _fieldMap[x, y])
        {
            return;
        }

        foreach (var pos in SurroundingCells(x, y))
        {
            if (_fieldStatus[pos.X, pos.Y] == FieldStatus.Closed)
            {
                OpenCell(pos.X, pos.Y);
            }
        }
    }

    private IEnumerable<Position> AllFieldCells()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                yield return new Position(x, y);
            }
        }
    }

    private IEnumerable<Position> SurroundingCells(int x, int y)
    {
        for (var col = x - 1; col <= x + 1; col++)
        {
            for (var row = y - 1; row <= y + 1; row++)
            {
                if (col == x && row == y)
                {
                    continue;
                }

                if (col >= _fieldMap.GetLength(0) || col < 0 || row >= _fieldMap.GetLength(1) || row < 0)
                {
                    continue;
                }

                yield return new Position(col, row);
            }
        }
    }

    public bool IsMine(int x, int y)
    {
        return _fieldMap[x, y] == MINE;
    }

    public int MinesAround(int x, int y)
    {
        return _fieldMap[x, y];
    }
}
