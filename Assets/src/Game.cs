using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private GameController _controller;

    private CellController[,] _cellControllers;

    public GameObject CellPrefab;

    public Canvas GameCanvas;

    private void Start()
    {
        var settings = FindObjectOfType<Settings>();
        _controller = new GameController(settings.Mines, settings.Width, settings.Height);

        _cellControllers = new CellController[settings.Width, settings.Height];

        GenerateField();

        _controller.OnWin += controllerOnWin;
        _controller.OnClosedMines += controllerOnClosedMines;
        _controller.OnLoose += controllerOnLoose;
        _controller.OnOpenCell += controllerOnOpenCell;
        _controller.OnToggleFlag += controllerOnToggleFlag;
        _controller.OnWrongFlag += controllerOnWrongFlag;
    }

    private void controllerOnWrongFlag(object sender, PositionEventArgs e)
    {
        _cellControllers[e.Position.X, e.Position.Y].ShowWrongFlag();
    }

    private void controllerOnToggleFlag(object sender, ToggleFlagEventArgs e)
    {
        var cell = _cellControllers[e.Position.X, e.Position.Y];

        if (e.FlagStatus == FlagStatus.NoFlag)
        {
            cell.RemoveFlag();
        }
        else
        {
            cell.PlaceFlag();
        }
    }

    private void controllerOnOpenCell(object sender, OpenCellEventArgs e)
    {
        _cellControllers[e.Position.X, e.Position.Y].OpenCell(e.Value);
    }

    private void controllerOnClosedMines(object sender, PositionEventArgs e)
    {
        _cellControllers[e.Position.X, e.Position.Y].OpenMine();
    }

    private void controllerOnLoose(object sender, PositionEventArgs e)
    {
        _cellControllers[e.Position.X, e.Position.Y].ShowExplodedMine();
    }

    private void controllerOnWin(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void GenerateField()
    {
        var cellSize = CellPrefab.GetComponent<RectTransform>().sizeDelta.x;
        var xPos = -_controller.Width * cellSize / 2 + cellSize / 2;
        var yPos = -_controller.Height * cellSize / 2 + cellSize / 2;

        for (var x = 0; x < _controller.Width; x++)
        {
            for (var y = 0; y < _controller.Height; y++)
            {
                var cell = Instantiate(CellPrefab, GameCanvas.transform);
                var pos = new Vector2(xPos + x * cellSize, yPos + y * cellSize);
                cell.GetComponent<RectTransform>().anchoredPosition = pos;

                var cellController = cell.GetComponent<CellController>();
                _cellControllers[x, y] = cellController;
                var xc = x;
                var yc = y;
                cellController.OnToggleFlagClick += (sender, args) => _controller.ToggleFlag(xc, yc);

                cellController.OnOpenCellClick += (sender, args) => _controller.OpenCell(xc, yc);
            }
        }
    }
}
