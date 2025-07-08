using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Interfaces;

public interface IValidationMovedService
{
    public bool DetectNotCheckMate(Chessboard chessboard, FigureColor checkedColor);
    public bool DetectCheck(Chessboard chessboard, FigureColor checkedColor);
    public List<Position> GetRealAvailableMoves(BaseFigure figure, Chessboard chessboard);
}
