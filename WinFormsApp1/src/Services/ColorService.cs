using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Services;

public class ColorService : IColorService
{
    private FigureColor _figureColor = FigureColor.White;

    public void ToogleColor()
    {
        _figureColor = GetOtherColor();
    }

    public FigureColor GetCurrentColor()
    {
        return _figureColor;
    }
    
    public FigureColor GetOtherColor()
    {
        return _figureColor == FigureColor.White ? FigureColor.Black : FigureColor.White;
    }
}