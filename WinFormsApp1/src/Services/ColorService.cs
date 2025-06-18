using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Services;

public class ColorService : IColorService
{
    private FigureColor _figureColor = FigureColor.White;

    public void ToogleColor()
    {
        _figureColor = _figureColor == FigureColor.White ? FigureColor.Black : FigureColor.White;
    }

    public FigureColor GetCurrentColor()
    {
        return _figureColor;
    }
}