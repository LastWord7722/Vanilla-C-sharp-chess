using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Enums;

namespace WinFormsApp1.Interfaces;

public interface IStateService : IColorService
{
    public void CheckAndPromotePawns(List<Cell> cells, FigureColor color);
}