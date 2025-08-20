using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Services;

public class MovedService : IMovedService
{
    public void MoveFigure(Cell toCell, Cell fromCell)
    {
        if (!fromCell.HasFigure())
        {
            throw new Exception("fromCell not a figure");
        }

        fromCell.Figure!.Position = toCell.Position;
        toCell.Figure = fromCell.Figure!;
        toCell.Figure.IsFigureNotMoved = false;
        fromCell.Figure = null;
    }

    public void MoveKingFigure(Cell toCell, Cell fromCell, Chessboard chessboard, IStateService stateService)
    {
        if (!fromCell.HasFigure() || fromCell.Figure.GetTypeFigure() != FigureType.King)
        {
            throw new Exception("Figure is null or figure type not king");
        }

        King king = fromCell!.Figure as King;
        Position?[] castingMove = king.GetCasstlingMove(chessboard);
        char kingStartColumn = king.Position.GetColumn();

        if (!castingMove.Contains(toCell.Position))
        {
            stateService.AddHistoryMove(fromCell, toCell);
            MoveFigure(toCell, fromCell);
        }
        else
        {
            MoveFigure(toCell, fromCell);
            bool isLeft = king.Color == FigureColor.White
                ? toCell.Position.GetColumn() < fromCell.Position.GetColumn()
                : toCell.Position.GetColumn() > fromCell.Position.GetColumn();
            
            char[] listColumn = chessboard.GetListColumns();
            Func<char, char[], char?> nextColumnFun = isLeft
                ? king.GetLeftColumn
                : king.GetRightColumn;

            Func<char, char[], char?> prevColumnFun = isLeft
                ? king.GetRightColumn
                : king.GetLeftColumn;

            var nextColumn = nextColumnFun(kingStartColumn, listColumn);
            var currentPos = Position.Make(nextColumn!.Value, king.Position.GetRow());
            var fromCellRook = chessboard.GetCellByPosition(currentPos);

            while (!(fromCellRook.HasFigure() && fromCellRook.Figure!.GetTypeFigure() == FigureType.Rook))
            {
                nextColumn = nextColumnFun(nextColumn.Value, listColumn);
                currentPos = Position.Make(nextColumn!.Value, king.Position.GetRow());
                fromCellRook = chessboard.GetCellByPosition(currentPos);
            }

            if (!fromCellRook.HasFigure() || fromCellRook.Figure!.GetTypeFigure() != FigureType.Rook)
            {
                throw new Exception("Figure is null or figure type not rook");
            }

            Cell toCellRook = chessboard.GetCellByPosition(
                Position.Make(prevColumnFun(king.Position.GetColumn(), listColumn)!.Value, king.Position.GetRow())
            );
            stateService.AddCastlingHistoryMove(fromCell, toCell, fromCellRook, toCellRook);
            MoveFigure(toCellRook, fromCellRook);
        }
    }
}