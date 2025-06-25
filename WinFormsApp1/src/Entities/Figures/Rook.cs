using WinFormsApp1.Enums;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Entities.Figures;

public class Rook : BaseFigure
{
    public Rook(FigureColor color, Position position) : base(color, position)
    {
    }

    private List<Position> GetAvailableMovesVertical(Chessboard.Chessboard chessboard, bool isLeft = true)
    {
        char[] columnList = chessboard.GetListColumns();
        List<Position> moves = new List<Position>();
        char? currentColumn = Position.GetColumn();
        while (true)
        {
            if (currentColumn == null)
            {
                break;
            }

            currentColumn = isLeft
                ? GetLeftColumn(currentColumn.Value, columnList)
                : GetRightColumn(currentColumn.Value, columnList);

            if (currentColumn == null)
            {
                break;
            }

            Position currentPosition = Position.Make(currentColumn.Value, Position.GetRow());
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

    private List<Position> GetAvailableMovesHorizontal(Chessboard.Chessboard chessboard, bool isTop = true)
    {
        List<Position> moves = new List<Position>();
        int currentRow = Position.GetRow();

        while (true)
        {
            if (currentRow == -1)
            {
                break;
            }

            currentRow = isTop
                ? GetNextRow(currentRow)
                : GetPreviousRow(currentRow);

            if (currentRow == -1)
            {
                break;
            }

            Position currentPosition = Position.Make(currentRow, Position.GetColumn());
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
        moves.AddRange(GetAvailableMovesVertical(chessboard));
        moves.AddRange(GetAvailableMovesVertical(chessboard, false));
        moves.AddRange(GetAvailableMovesHorizontal(chessboard));
        moves.AddRange(GetAvailableMovesHorizontal(chessboard, false));

        return moves;
    }
}