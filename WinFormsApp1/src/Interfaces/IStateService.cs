using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Enums;

namespace WinFormsApp1.Interfaces;

public interface IStateService : IColorService
{
    public MoveList HistoryMoves { get; }
    public void CheckAndPromotePawn(Cell cell);
    public void AddHistoryMove(Cell cellFrom, Cell cellTo);
    public void AddCastlingHistoryMove(Cell kingCellFrom, Cell kingCellTo, Cell rookCellFrom, Cell rookCellTo);
}