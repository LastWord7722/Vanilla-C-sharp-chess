using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;

namespace WinFormsApp1.Factories;

public class IconFigureFactory
{
    private static readonly Dictionary<FigureColor, Dictionary<FigureType, string>> MAP = new()
    {
        {
            FigureColor.Black,
            new Dictionary<FigureType, string>()
            {
                { FigureType.King, "\u265A" },
                { FigureType.Queen, "\u265B" },
                { FigureType.Rook, "\u265C" },
                { FigureType.Bishop, "\u265D" },
                { FigureType.Knight, "\u265E" },
                { FigureType.Pawn, "\u265F" }
            }
        },
        {
            FigureColor.White,
            new Dictionary<FigureType, string>()
            {
                { FigureType.King, "\u2654" },
                { FigureType.Queen, "\u2655" },
                { FigureType.Rook, "\u2656" },
                { FigureType.Bishop, "\u2657" },
                { FigureType.Knight, "\u2658" },
                { FigureType.Pawn, "\u2659" }
            }
        }
    };


    public static string Create(BaseFigure figure)
    {
        //todo: add handler exception? 
        return MAP[figure.Color][figure.GetTypeFigure()];
    }
}