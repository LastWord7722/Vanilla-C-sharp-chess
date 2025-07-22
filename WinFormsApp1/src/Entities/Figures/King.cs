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
        int currentRow = Position.GetRow();

        List<Position> moves = new List<Position>();

        int targetRow = isTop ? GetNextRow(currentRow) : GetPreviousRow(currentRow);

        if (targetRow != -1 && !chessboard.HasUnionFigureByPosition(Position.Make(targetRow, currentColumn), Color))
        {
            moves.Add(Position.Make(targetRow, currentColumn));
        }

        return moves;
    }

    private List<Position> ProcessHorizontal(Chessboard.Chessboard chessboard, bool isLeft = true)
    {
        char currentColumn = Position.GetColumn();
        int currentRow = Position.GetRow();

        char[] listColumns = chessboard.GetListColumns();
        List<Position> moves = new List<Position>();

        char? targetColumn =
            isLeft ? GetLeftColumn(currentColumn, listColumns) : GetRightColumn(currentColumn, listColumns);

        if (targetColumn.HasValue &&
            !chessboard.HasUnionFigureByPosition(Position.Make(currentRow, targetColumn!.Value), Color))
        {
            moves.Add(Position.Make(currentRow, targetColumn!.Value));
        }

        return moves;
    }

    private List<Position> ProcessDiagonal(Chessboard.Chessboard chessboard, bool isLeft = true, bool isTop = true)
    {
        List<Position> moves = new List<Position>();

        char currentColumn = Position.GetColumn();
        int currentRow = Position.GetRow();
        char[] listColumns = chessboard.GetListColumns();

        int targetRow = isTop
            ? GetNextRow(currentRow)
            : GetPreviousRow(currentRow);

        char? targetColumn = isLeft
            ? GetLeftColumn(currentColumn, listColumns)
            : GetRightColumn(currentColumn, listColumns);

        if (targetRow != -1 && targetColumn.HasValue &&
            !chessboard.HasUnionFigureByPosition(Position.Make(targetRow, targetColumn!.Value), Color))
        {
            moves.Add(Position.Make(targetRow, targetColumn.Value));
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
                ProcessDiagonal(chessboard, matrix[i, 0], matrix[i, 1])
            );
        }

        moves.AddRange(ProcessHorizontal(chessboard));
        moves.AddRange(ProcessHorizontal(chessboard, false));
        moves.AddRange(ProcessVertical(chessboard));
        moves.AddRange(ProcessVertical(chessboard, false));
        return moves;
    }

    public List<Position> GetNextColumns(Chessboard.Chessboard chessboard,bool isLeft, int count = 1)
    {
        if (count <= 0)
        {
            throw new ArgumentException("count must be > 0");
        }
        
        List<Position> positions = new List<Position>();
        char? next = Position.GetColumn();

        for (int i = 0; i < count; i++)
        {
            next = isLeft 
                ? GetLeftColumn(next.Value, chessboard.GetListColumns()) 
                : GetRightColumn(next.Value, chessboard.GetListColumns());
            
            if (!next.HasValue)
            {
                throw new ArgumentException("next column must have a value");
            }
            positions.Add(Position.Make(next.Value, Position.GetRow()));
        }
        
        return positions;
    }
    public Position?[] GetCasstlingMove(Chessboard.Chessboard chessboard)
    {
        Position?[] result = [null, null];
        //todo bug in GetNextColumns
        var left = GetNextColumns(chessboard, false, 3);
        var right = GetNextColumns(chessboard, true, 2);

        if (left.All(pos => !chessboard.HasFigureByPosition(pos)))
        {
            result[0] = left.Last();
        }

        if (right.All(pos => !chessboard.HasFigureByPosition(pos)))
        {
            result[1] = right.Last();
        }
        
        return result;
    }
    public override BaseFigure Clone()
    {
        return new King(Color, Position);
    }

    public override FigureType GetTypeFigure()
    {
        return FigureType.King;
    }
}