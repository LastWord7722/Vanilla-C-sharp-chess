using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Pawn : BaseFigure
{
    public Pawn(FigureColor color, Position position) : base( color, position)
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

        if (isDefaultPosition)
        {
            moves.Add(new Position(curColumn,rowOneMore));
            moves.Add(new Position(curColumn, GetNextRow(rowOneMore)));
        }
        else
        {
            moves.Add(new Position(curColumn, rowOneMore));
        }
        
        return moves;
    }
    
}