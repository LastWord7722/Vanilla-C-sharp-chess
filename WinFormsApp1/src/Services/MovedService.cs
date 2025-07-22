using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Services;

public class MovedService : IMovedService
{
    public void MoveFigure(Cell toMovCell, Cell currentCell)
    {
        currentCell.Figure!.Position = toMovCell.Position;
        toMovCell.Figure = currentCell.Figure!;
        toMovCell.Figure.SetFigureMove();
        currentCell.Figure = null;
    }
}