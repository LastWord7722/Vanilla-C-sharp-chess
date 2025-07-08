using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Knight : BaseFigure
{
    public Knight(FigureColor color, Position position) : base(color, position)
    {
    }

    public List<Position> ProcessTopBottom(Chessboard.Chessboard chessboard, bool isTop = true)
    {
        List<Position> moves = new List<Position>();
        char[] columnList = chessboard.GetListColumns();
        int currentRow = Position.GetRow();
        char currentColumn = Position.GetColumn();
        Func<int, int> method = isTop ? GetNextRow : GetPreviousRow;

        int twoMore = method(method(currentRow));
        char? leftColumn = GetLeftColumn(currentColumn, columnList);
        char? rightColumn = GetRightColumn(currentColumn, columnList);

        if (twoMore == -1)
        {
            return moves;
        }
        
        if (leftColumn.HasValue &&
            !chessboard.HasUnionFigureByPosition(Position.Make(leftColumn!.Value, twoMore), Color))
        {
            moves.Add(Position.Make(leftColumn!.Value, twoMore));
        }

        if (rightColumn.HasValue &&
            !chessboard.HasUnionFigureByPosition(Position.Make(rightColumn!.Value, twoMore), Color))
        {
            moves.Add(Position.Make(rightColumn!.Value, twoMore));
        }
        return moves;
    }

    protected List<Position> ProcessLeftRight(Chessboard.Chessboard chessboard, bool isLeft = true)
    {
        List<Position> moves = new List<Position>();
        char[] columnList = chessboard.GetListColumns();
        int currentRow = Position.GetRow();
        char currentColumn = Position.GetColumn();
        Func<char, char[], char?> method = isLeft ? GetLeftColumn : GetRightColumn;

        char? oneMoreColumn = method(currentColumn, columnList);
        if (!oneMoreColumn.HasValue)
        {
            return moves;
        }

        int oneNext = GetNextRow(currentRow);
        int onePrevious = GetPreviousRow(currentRow);
        char? twoMoreColumn = method(oneMoreColumn.Value, columnList);
        if (!twoMoreColumn.HasValue)
        {
            return moves;
        }
        
        if (oneNext != -1 && !chessboard.HasUnionFigureByPosition(Position.Make(twoMoreColumn!.Value, oneNext), Color))
        {
            moves.Add(Position.Make(twoMoreColumn!.Value, oneNext));
        }
        
        if (onePrevious != -1 && !chessboard.HasUnionFigureByPosition(Position.Make(twoMoreColumn!.Value, onePrevious), Color))
        {
            moves.Add(Position.Make(twoMoreColumn!.Value, onePrevious));
        }


        return moves;
    }

    public override List<Position> GetAvailableMoves(Chessboard.Chessboard chessboard)
    {
        List<Position> moves = new List<Position>();

        moves.AddRange(ProcessLeftRight(chessboard));
        moves.AddRange(ProcessLeftRight(chessboard, false));
        moves.AddRange(ProcessTopBottom(chessboard));
        moves.AddRange(ProcessTopBottom(chessboard, false));
        return moves;
    }
    
    public override BaseFigure Clone()
    {
        return new Knight(Color, Position);
    }
}