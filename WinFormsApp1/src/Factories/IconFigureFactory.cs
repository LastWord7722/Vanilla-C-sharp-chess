using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Interfaces;

namespace WinFormsApp1.Factories;

public class IconFigureFactory
{
    private static readonly Dictionary<FigureColor, Dictionary<Type, string>> MAP = new()
    {
        {
            FigureColor.Black,
            new Dictionary<Type, string>()
            {
                { typeof(King), "\u265A" },
                { typeof(Queen), "\u265B" },
                { typeof(Rook), "\u265C" },
                { typeof(Bishop), "\u265D" },
                { typeof(Knight), "\u265E" },
                { typeof(Pawn), "\u265F" }
            }
        },
        {
            FigureColor.White,
            new Dictionary<Type, string>()
            {
                { typeof(King), "\u2654" },
                { typeof(Queen), "\u2655" },
                { typeof(Rook), "\u2656" },
                { typeof(Bishop), "\u2657" },
                { typeof(Knight), "\u2658" },
                { typeof(Pawn), "\u2659" }
            }
        }
    };


    public static string Create(BaseFigure figure)
    {
        //todo: add handler exception? 
        return MAP[figure.Color][figure.GetType()];
    }
}