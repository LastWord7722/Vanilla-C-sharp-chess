using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
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
}