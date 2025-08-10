using WinFormsApp1.Entities.Chessboard;

namespace WinFormsApp1.Interfaces;

public interface IMovedService
{
    public void MoveFigure(Cell toMovCell, Cell fromCell);
    public void MoveKingFigure(Cell toMovCell, Cell fromCell, Chessboard chessboard, IStateService stateService);
}