using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Services;

public class MovedService : IMovedService
{
    public void MoveFigure(Cell toMovCell, ButtonCell selectedBtn)
    {
        BaseFigure currentFigure = selectedBtn.GetCell().GetFigure()!;
        currentFigure.SetPosition(toMovCell.GetPosition());
        toMovCell.SetFigure(currentFigure);
        selectedBtn.GetCell().SetFigure(null);
    }

    //checkmate
    //check
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