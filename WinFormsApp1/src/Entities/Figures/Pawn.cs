using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Pawn : BaseFigure
{
    public Pawn(FigureColor color, Position position) : base(color, position)
    {
    }
    //пешка первый ход 2 вперед, последущиие ходы по 1, бъет на диоганаль на 1, перехват - не реализуем

    public override List<Position> GetAvailableMoves(Chessboard.Chessboard chessboard)
    {
        char curColumn = Position.GetColumn();
        int curRow = Position.GetRow();
        List<Position> moves = new List<Position>();
        bool isDefaultPosition = (Color == FigureColor.White && curRow == 2) ||
                                 (Color == FigureColor.Black && curRow == 7);

        int rowOneMore = GetNextRow(curRow);
        Position oneMorePosition = Position.Make(curColumn, rowOneMore);
        
        
        //attack
        char[] columnList = chessboard.GetListColumns();
        char? leftAttackColumn = GetLeftColumn(curColumn, columnList);

        if (leftAttackColumn.HasValue &&
            chessboard.HasEnemyFigureByPosition(Position.Make(leftAttackColumn.Value, rowOneMore), GetEnemyColor()))
        {
            moves.Add(Position.Make(rowOneMore, leftAttackColumn.Value));
        }

        char? rightAttackColumn = GetRightColumn(curColumn, columnList);
        if (rightAttackColumn.HasValue &&
            chessboard.HasEnemyFigureByPosition(Position.Make(rightAttackColumn.Value, rowOneMore), GetEnemyColor()))
        {
            moves.Add(Position.Make(rowOneMore, rightAttackColumn.Value));
        }
        
        //moved
        if (chessboard.HasFigureByPosition(oneMorePosition))
        {
            return moves;
        }
        
        if (isDefaultPosition)
        {
            Position twoMorePosition = Position.Make(curColumn, GetNextRow(rowOneMore));
            
            foreach (Position position in (Position[]) [oneMorePosition, twoMorePosition])
            {
                if (!chessboard.HasFigureByPosition(position))
                    moves.Add(position);
            }
        }
        else
        {
            moves.Add(oneMorePosition);
        }
        return moves;
    }
    
    public override BaseFigure Clone()
    {
        return new Pawn(Color, Position);
    }
    
    public override FigureType GetTypeFigure()
    {
        return FigureType.Pawn;
    }
}