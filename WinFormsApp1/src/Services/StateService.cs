using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Services;

public class StateService : IStateService
{
    private FigureColor _figureColor = FigureColor.White;
    public MoveList HistoryMoves { get; protected set; } = new();

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
                HistoryMoves.SetIsPromoteByIndex(HistoryMoves.Count() - 1);
            }
        }
    }

    public void AddHistoryMove(Cell cellFrom, Cell cellTo)
    {
        if (cellFrom.Figure == null)
        {
            throw new Exception($"Can't add history move, figure is null {cellFrom.Position.GetPositionCode()}");
        }

        HistoryMoves.Add(cellFrom.Figure!, cellFrom.Position, cellTo.Position, cellTo.Figure);
    }

    public void AddCastlingHistoryMove(Cell kingCellFrom, Cell kingCellTo, Cell rookCellFrom, Cell rookCellTo)
    {
        HistoryMoves.AddCastling(
            kingCellTo.Figure as King,
            kingCellFrom.Position,
            kingCellTo.Position,
            rookCellFrom.Figure as Rook,
            rookCellFrom.Position,
            rookCellTo.Position
        );
    }
}