using System;
using System.Collections.Generic;

public class GameController
{
    private const int MINE = -1;

    private readonly int[,] _fieldMap;

    private readonly FieldStatus[,] _fieldStatus;

    public event EventHandler<OpenCellEventArgs> OnOpenCell;

    public GameController(int mines, int width, int height, int seed = 0)
    {
        _fieldStatus = new FieldStatus[width, height];
        _fieldMap = new int[width, height];

        var random = new Random(seed);

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

    public void OpenCell(int x, int y)
    {
        if (_fieldStatus[x, y] != FieldStatus.Closed)
        {
            return;
        }

        _fieldStatus[x, y] = FieldStatus.Opened;

        var args = new OpenCellEventArgs(x, y, _fieldMap[x, y]);
        OnOpenCell?.Invoke(this, args);

        if (_fieldMap[x, y] != 0)
        {
            return;
        }

        foreach (var pos in SurroundingCells(x, y))
        {
            OpenCell(pos.X, pos.Y);
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
