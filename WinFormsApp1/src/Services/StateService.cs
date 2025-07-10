using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Services;

public class StateService : IStateService
{
    private FigureColor _figureColor = FigureColor.White;

    public void ToogleColor()
    {
        _figureColor = GetOtherColor();
    }

    public FigureColor GetCurrentColor()
    {
        return _figureColor;
    }
    
    public FigureColor GetOtherColor()
    {
        return _figureColor == FigureColor.White ? FigureColor.Black : FigureColor.White;
    }

    public void CheckAndPromotePawns(List<Cell> cells, FigureColor color)
    {
        int needPosition = color == FigureColor.Black ? 1 : 8;
        foreach (var cell in cells)
        {
            if (cell.HasFigure() && cell.Figure!.GetTypeFigure() == FigureType.Pawn && cell.Position.GetRow() == needPosition)
            {
                cell.Figure = new Queen(color, cell.Position);
            }
        }
    }
}