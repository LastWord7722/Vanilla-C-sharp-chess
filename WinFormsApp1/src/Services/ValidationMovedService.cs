using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
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
        BaseFigure king = chessboardClone.GetKingByColor(figure.GetColor());

        foreach (var pos in figure.GetAvailableMoves(chessboardClone))
        {
            //
        }

        return availableMoves;
    }  
}