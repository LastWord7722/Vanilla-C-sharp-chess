using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Services;

public class StateService : IStateService
{
    private FigureColor _figureColor = FigureColor.White;
    public List<HistoryMoveItem> HistoryMoves { get; } = new();

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
            if (cell.HasFigure() && cell.Figure!.GetTypeFigure() == FigureType.Pawn &&
                cell.Position.GetRow() == needPosition)
            {
                // todo: не очень нравится что state service делает превращение хоть и маленькое 
                cell.Figure = new Queen(color, cell.Position);
            }
        }
    }

    public void SetHistoryMoves(BaseFigure figure, Position from, Position to)
    {
        HistoryMoves.Add(new HistoryMoveItem(figure, from, to));
    }
    
}