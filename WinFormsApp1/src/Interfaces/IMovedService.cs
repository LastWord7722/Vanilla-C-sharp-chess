using WinFormsApp1.Entities.Chessboard;

namespace WinFormsApp1.Interfaces;

public interface IMovedService
{
    public void MoveFigure(Cell toMovCell, Cell currentCell);
    public void MoveKingFigure(Cell toMovCell, Cell currentCell, Chessboard chessboard);
}