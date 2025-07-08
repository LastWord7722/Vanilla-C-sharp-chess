using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Enums;
using WinFormsApp1.FormLayout;

namespace WinFormsApp1.Interfaces;

public interface IMovedService
{
    public void MoveFigure(Cell toMovCell, Cell currentCell);
}