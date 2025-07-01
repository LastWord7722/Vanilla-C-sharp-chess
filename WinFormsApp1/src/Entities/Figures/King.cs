using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class King : BaseFigure
{
    public King(FigureColor color, Position position) : base(color, position)
    {
    }
    // в целом выглядит не очень тем более постояно инициализирую List<Position> нужен будет рефактор
    private List<Position> ProcessVertical(Chessboard.Chessboard chessboard, bool isTop = true)
    {
        char currentColumn = Position.GetColumn();
        int currentRow= Position.GetRow();
        
        List<Position> moves = new List<Position>();

        int targetRow = isTop ? GetNextRow(currentRow) : GetPreviousRow(currentRow);

        if (targetRow != -1 && !chessboard.HasUnionFigureByPosition(Position.Make(targetRow, currentColumn), GetColor()))
        {
            moves.Add(Position.Make(targetRow, currentColumn));
        }
        
        return moves;
    }

    private List<Position> ProcessHorizontal(Chessboard.Chessboard chessboard, bool isLeft = true)
    {
        char currentColumn = Position.GetColumn();
        int currentRow= Position.GetRow();
        
        char[] listColumns = chessboard.GetListColumns();
        List<Position> moves = new List<Position>();

        char? targetColumn = isLeft ? GetLeftColumn(currentColumn,listColumns) : GetRightColumn(currentColumn,listColumns);

        if (targetColumn.HasValue && !chessboard.HasUnionFigureByPosition(Position.Make(currentRow, targetColumn!.Value), GetColor()))
        {
            moves.Add(Position.Make(currentRow, targetColumn!.Value));
        }
        
        return moves;
    }

    private List<Position> ProcessDiagonal(Chessboard.Chessboard chessboard, bool isLeft = true, bool isTop = true)
    {
        List<Position> moves = new List<Position>();
        
        char currentColumn = Position.GetColumn();
        int currentRow= Position.GetRow();
        char[] listColumns = chessboard.GetListColumns();
        
        int targetRow = isTop 
            ? GetNextRow(currentRow) 
            : GetPreviousRow(currentRow);
        
        char? targetColumn = isLeft 
            ? GetLeftColumn(currentColumn,listColumns) 
            : GetRightColumn(currentColumn,listColumns);
        
        if (targetRow != -1 && targetColumn.HasValue && 
            !chessboard.HasUnionFigureByPosition(Position.Make(targetRow, targetColumn!.Value), GetColor()))
        {
            moves.Add(Position.Make(targetRow, targetColumn.Value));
            Console.WriteLine(1111);
        }
        
        return moves;
    }

    public override List<Position> GetAvailableMoves(Chessboard.Chessboard chessboard)
    {
   
        List<Position> moves = new List<Position>();
        bool[,] matrix = new bool[,]
        {
            { true, false },
            { false, true },
            { false, false },
            { true, true }
        };
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            moves.AddRange(
                ProcessDiagonal(chessboard, matrix[i,0], matrix[i,1])
            );
        }

        moves.AddRange(ProcessHorizontal(chessboard));
        moves.AddRange(ProcessHorizontal(chessboard, false));
        moves.AddRange(ProcessVertical(chessboard));
        moves.AddRange(ProcessVertical(chessboard, false));
        return moves;
    }
}