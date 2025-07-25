using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Interfaces;

public interface IFigureLayout
{
    Dictionary<Position, BaseFigure> GetLayout();
}
