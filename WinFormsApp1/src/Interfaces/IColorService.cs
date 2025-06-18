using WinFormsApp1.Enums;

namespace WinFormsApp1.Interfaces;

public interface IColorService
{
    public void ToogleColor();
    public FigureColor GetCurrentColor();
}