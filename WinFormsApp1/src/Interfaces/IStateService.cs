using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Interfaces;

public interface IStateService : IColorService
{
    public List<HistoryMoveItem> HistoryMoves { get; }
    public void CheckAndPromotePawns(List<Cell> cells, FigureColor color);
    public void SetHistoryMoves(BaseFigure figure, Position from, Position to);
}