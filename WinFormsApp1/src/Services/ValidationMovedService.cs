using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Services;

public class ValidationMovedService : IValidationMovedService
{
    private IMovedService _movedService;

    public ValidationMovedService(IMovedService movedService)
    {
        _movedService = movedService;
    }

    public List<Position> GetRealAvailableMoves(BaseFigure figure, Chessboard chessboard)
    {
        List<Position> availableMoves = new List<Position>();

        Chessboard chessboardClone = chessboard.DeppClone();
        BaseFigure figureClone = figure.Clone();
        Position basePosition = figure.Position;
        foreach (var pos in figureClone.GetAvailableMoves(chessboardClone))
        {
            Cell toCell = chessboardClone.GetCellByPosition(pos);
            Cell fromCell = chessboardClone.GetCellByPosition(basePosition);
            if (MoveAndDetectCheck(toCell, fromCell, chessboardClone, figureClone.Color))
            {
                availableMoves.Add(pos);
            }
        }

        if (figure.GetTypeFigure() == FigureType.King)
        {
            King king = (King)figure;
            bool[] isAvailableCastlingMove = CheckCastling(chessboard, figure.Color);
            Position?[] castlingMove = king.GetCasstlingMove(chessboard);

            if (isAvailableCastlingMove.Length != castlingMove.Length)
            {
                throw new InvalidCastException("Wrong number of castling moves");
            }

            King kingClone = (King)king.Clone();
            for (int i = 0; i < isAvailableCastlingMove.Length; i++)
            {
                if (isAvailableCastlingMove[i] && castlingMove[i].HasValue)
                {
                    Cell toCell = chessboardClone.GetCellByPosition(castlingMove[i]!.Value);
                    Cell fromCell = chessboardClone.GetCellByPosition(kingClone.Position);
                    if (MoveAndDetectCheck(toCell, fromCell, chessboardClone, kingClone.Color))
                    {
                        availableMoves.Add(castlingMove[i]!.Value);
                    }
                }
            }
        }

        return availableMoves;
    }

    public bool DetectNotCheckMate(Chessboard chessboard, FigureColor checkedColor)
    {
        bool isNotCheckMate = true;
        var filteredCells = chessboard.Cells
            .Where(kv => kv.Value.HasFigure() && kv.Value.Figure!.Color == checkedColor)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var cell in filteredCells)
        {
            isNotCheckMate = GetRealAvailableMoves(cell.Value.Figure!, chessboard).Count > 0;

            if (isNotCheckMate)
            {
                break;
            }
        }

        return isNotCheckMate;
    }

    public bool DetectCheck(Chessboard chessboard, FigureColor checkedColor)
    {
        bool flag = false;
        var enemyKing = chessboard.GetKingByColor(checkedColor);

        var filteredCells = chessboard.Cells
            .Where(kv => kv.Value.HasFigure() && kv.Value.Figure!.Color != checkedColor)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var cell in filteredCells)
        {
            BaseFigure figure = cell.Value.Figure!;

            foreach (var pos in figure!.GetAvailableMoves(chessboard))
            {
                if (pos == enemyKing.Position)
                {
                    flag = true;
                }
            }
        }

        return flag;
    }


    public bool[] CheckCastling(Chessboard chessboard, FigureColor color)
    {
        bool[] result = [true, true];
        List<Cell> cells = chessboard.Cells.Select(kv => kv.Value).ToList();

        King kingFigure = chessboard.GetKingByColor(color) as King;

        List<Rook> rooksFigure = cells.Where(cell =>
                cell.HasFigure() && cell!.Figure.Color == color && cell.Figure!.GetTypeFigure() == FigureType.Rook)
            .Select(cell => cell.Figure as Rook)
            .OrderBy(cell => cell!.Position.GetColumn())
            .ToList();

        if (kingFigure != null && !kingFigure.IsFigureNotMoved || DetectCheck(chessboard, color))
        {
            return [false, false];
        }

        result = kingFigure.GetCasstlingMove(chessboard)
            .Select(value => value.HasValue)
            .ToArray();

        if (rooksFigure.Count > 0 && !rooksFigure[0].IsFigureNotMoved)
        {
            result[0] = false;
        }

        if (rooksFigure.Count > 1 && !rooksFigure[1].IsFigureNotMoved)
        {
            result[1] = false;
        }

        List<BaseFigure> enymyFigure = cells.Where(cell => cell.HasFigure() && cell.Figure.Color != color)
            .Select(cell => cell.Figure)
            .ToList();

        List<Position>[] positions =
        [
            kingFigure.GetNextColumns(chessboard, true, kingFigure.Color == FigureColor.White ? 3 : 2),
            kingFigure.GetNextColumns(chessboard, false, kingFigure.Color == FigureColor.White ? 2 : 3)
        ];

        foreach (BaseFigure figure in enymyFigure)
        {
            foreach (Position move in figure.GetAvailableMoves(chessboard))
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (positions[i].Any(pos => pos == move))
                    {
                        result[i] = false;
                    }
                }
            }
        }

        return result;
    }
    
    private bool MoveAndDetectCheck(Cell toCell, Cell fromCell, Chessboard chessboardClone, FigureColor color)
    {
        BaseFigure? dieFigure = toCell.Clone().Figure;
            
        _movedService.MoveFigure(toCell, fromCell);

        bool check = !DetectCheck(chessboardClone, color);
            
        _movedService.MoveFigure(fromCell, toCell);
        
        if (dieFigure != null)
        {
            toCell.Figure = dieFigure;
        }

        return check;
    }
}