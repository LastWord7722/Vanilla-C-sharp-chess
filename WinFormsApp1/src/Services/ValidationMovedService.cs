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
        //тут будет клон
        Chessboard chessboardClone = chessboard; 

        foreach (var pos in figure.GetAvailableMoves(chessboardClone))
        {
            Position oldPosFigure = figure.GetPosition();
            _movedService.MoveFigure(
                chessboardClone.GetCellByPosition(pos),
                chessboardClone.GetCellByPosition(oldPosFigure)
            );
            
            bool haveCheck = DetectCheck(chessboardClone, figure.GetColor());
            
            _movedService.MoveFigure(
                chessboardClone.GetCellByPosition(oldPosFigure),
                chessboardClone.GetCellByPosition(pos)
            );

            if (!haveCheck)
            {
                availableMoves.Add(pos);
            }
        }
        return availableMoves;
    }

    public bool DetectNotCheckMate(Chessboard chessboard, FigureColor checkedColor)
    {
        bool isNotCheckMate = true;
        var filteredCells = chessboard.GetCells()
            .Where(kv => kv.Value.HasFigure() && kv.Value.GetFigure()!.GetColor() != checkedColor)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var cell in filteredCells)
        {
            isNotCheckMate = GetRealAvailableMoves(cell.Value.GetFigure()!,chessboard).Count > 0;
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
        
        var filteredCells = chessboard.GetCells()
            .Where(kv => kv.Value.HasFigure() && kv.Value.GetFigure()!.GetColor() != checkedColor)
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var cell in filteredCells)
        {
            BaseFigure figure = cell.Value.GetFigure()!;

            foreach (var pos in figure!.GetAvailableMoves(chessboard))
            {
                if (pos == enemyKing.GetPosition())
                {
                    flag = true;
                }
            }
        }

        return flag;
    }
}