using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Bishop : BaseFigure
{
    public Bishop(FigureColor color, Position position) : base(color, position)
    {
    }
    private List<Position> GetAvailableMovesProcess(Chessboard.Chessboard chessboard, bool isLeft = true, bool isTop = true)
    {
        List<Position> moves = new List<Position>();
        char[] columnList = chessboard.GetListColumns();
        int currentRow = Position.GetRow();
        char? currentColumn = Position.GetColumn();
        
        while (true)
        {
         
            if (currentRow == -1 || currentColumn == null)
            {
                break;
            }

            currentRow = isTop ? GetNextRow(currentRow) : GetPreviousRow(currentRow);
            currentColumn = isLeft 
                ? GetLeftColumn(currentColumn.Value, columnList) 
                : GetRightColumn(currentColumn.Value, columnList);
            
            
            if (currentRow == -1 || currentColumn == null)
            {
                break;
            }

            Position currentPosition = Position.Make(currentRow, currentColumn.Value);
            if (chessboard.HasUnionFigureByPosition(currentPosition, Color))
            {
                break;
            }

            if (chessboard.HasEnemyFigureByPosition(currentPosition, GetEnemyColor()))
            {
                moves.Add(currentPosition);
                break;
            }

            moves.Add(currentPosition);
        }

        return moves;
    }
    public override List<Position> GetAvailableMoves(Chessboard.Chessboard chessboard)
    {
        List<Position> moves = new List<Position>();
        moves.AddRange(GetAvailableMovesProcess(chessboard,true,true));
        moves.AddRange(GetAvailableMovesProcess(chessboard,false,false));
        moves.AddRange(GetAvailableMovesProcess(chessboard,true,false));
        moves.AddRange(GetAvailableMovesProcess(chessboard,false,true));

        return moves;
    }
}