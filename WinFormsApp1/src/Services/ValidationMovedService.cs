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
        //todo: есть баг если chessboardClone юзать до цыкла то есть ошибочные ходы, нужно перепроверить возврат на исходные позиции
        foreach (var pos in figure.GetAvailableMoves(chessboard))
        {
            Chessboard chessboardClone = chessboard.DeppClone();
            Position oldPosFigure = figure.Position;
            _movedService.MoveFigure(
                chessboardClone.GetCellByPosition(pos),
                chessboardClone.GetCellByPosition(oldPosFigure)
            );
            
            bool haveCheck = DetectCheck(chessboardClone, figure.Color);
            // Console.WriteLine($"{figure.Color}, {figure.GetType().Name}, {pos.GetPositionCode()}, {haveCheck}");

            // _movedService.MoveFigure(
            //     chessboardClone.GetCellByPosition(oldPosFigure),
            //     chessboardClone.GetCellByPosition(pos)
            // );
            // chessboardClone.GetCellByPosition(oldPosFigure).SetFigure(figure);

            if (!haveCheck)
            {
                availableMoves.Add(pos);
            }
        }

        if (figure.GetTypeFigure() == FigureType.King)
        {
            King king = figure as King;
            bool[] res = CheckCastling(chessboard, figure.Color);
            Position?[] casstlingMove = king.GetCasstlingMove(chessboard); 
            //todo: use loop
            if (res[0] && casstlingMove[0].HasValue)
            {
                Chessboard chessboardClone = chessboard.DeppClone();
                _movedService.MoveFigure(
                    chessboardClone.GetCellByPosition(casstlingMove[0].Value),
                    chessboardClone.GetCellByPosition(king.Position)
                );
                bool haveCheck = DetectCheck(chessboardClone, figure.Color);
                if (!haveCheck)
                {
                    availableMoves.Add(casstlingMove[0].Value);
                }
            }
            if (res[1] && casstlingMove[1].HasValue)
            {
                Chessboard chessboardClone = chessboard.DeppClone();
                _movedService.MoveFigure(
                    chessboardClone.GetCellByPosition(casstlingMove[1].Value),
                    chessboardClone.GetCellByPosition(king.Position)
                );
                bool haveCheck = DetectCheck(chessboardClone, figure.Color);
                if (!haveCheck)
                {
                    availableMoves.Add(casstlingMove[1].Value);
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
                var figure = cell.Value.Figure!;
                Console.WriteLine($"{figure.Color}, {figure.GetType().Name}, {cell.Value.Position.GetPositionCode()}");

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
        
        if (!rooksFigure[0].IsFigureNotMoved)
        {
            result[0] = false;
        }
        
        if (!rooksFigure[1].IsFigureNotMoved)
        {
            result[1] = false;
        }
        
        List<BaseFigure> enymyFigure = cells.Where(cell => cell.HasFigure() && cell.Figure.Color != color)
            .Select(cell => cell.Figure)
            .ToList();

        List<Position>[] positions =
        [
            kingFigure.GetNextColumns(chessboard, true, 3),
            kingFigure.GetNextColumns(chessboard, false, 2)
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
}