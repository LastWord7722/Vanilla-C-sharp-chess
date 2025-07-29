using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Services;

public class MovedService : IMovedService
{
    public void MoveFigure(Cell toMovCell, Cell currentCell)
    {
        if (!currentCell.HasFigure())
        {
            throw new Exception("currentCell not a figure");
        }
        
        currentCell.Figure!.Position = toMovCell.Position;
        toMovCell.Figure = currentCell.Figure!;
        toMovCell.Figure.SetFigureMove();
        currentCell.Figure = null;
    }

    public void MoveKingFigure(Cell toMovCell, Cell currentCell, Chessboard chessboard)
    {
        if (!currentCell.HasFigure() || currentCell.Figure.GetTypeFigure() != FigureType.King)
        {
            throw new Exception("Figure is null or figure type not king");
        }
        
        King king = currentCell!.Figure as King;
        
        Position?[] castingMove = king.GetCasstlingMove(chessboard);
        char kingStartColumn = king.Position.GetColumn();
        MoveFigure(toMovCell, currentCell);
        if (castingMove.Contains(toMovCell.Position))
        {
            char[] listColumn = chessboard.GetListColumns();
            Func<char, char[], char?> nextColumnFun = castingMove[0].HasValue 
                ? king.GetLeftColumn
                : king.GetRightColumn;
            
            var nextColumn = nextColumnFun(kingStartColumn, listColumn);
            var currentPos = Position.Make(nextColumn!.Value, king.Position.GetRow());
            var rookCell = chessboard.GetCellByPosition(currentPos);
            
            Func<char, char[], char?> prevColumnFun = castingMove[0].HasValue 
                ? king.GetRightColumn
                : king.GetLeftColumn;
            
            while (!(rookCell.HasFigure() && rookCell.Figure!.GetTypeFigure() == FigureType.Rook))
            {
                nextColumn = nextColumnFun(nextColumn.Value, listColumn);
                currentPos = Position.Make(nextColumn!.Value, king.Position.GetRow());
                rookCell = chessboard.GetCellByPosition(currentPos);
            }

            if (!rookCell.HasFigure() || rookCell.Figure!.GetTypeFigure() != FigureType.Rook)
            {
                throw new Exception("Figure is null or figure type not rook");
            }
            
            MoveFigure(chessboard.GetCellByPosition(
                Position.Make(prevColumnFun(king.Position.GetColumn(), listColumn)!.Value, king.Position.GetRow())),
                rookCell
            );
        }
    }
}