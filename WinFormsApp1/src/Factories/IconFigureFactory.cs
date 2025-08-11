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
                { FigureType.King, "bk.png" },
                { FigureType.Queen, "bq.png" },
                { FigureType.Rook, "br.png" },
                { FigureType.Bishop, "bb.png" },
                { FigureType.Knight, "bn.png" },
                { FigureType.Pawn, "bp.png" }
            }
        },
        {
            FigureColor.White,
            new Dictionary<FigureType, string>()
            {
                { FigureType.King, "wk.png" },
                { FigureType.Queen, "wq.png" },
                { FigureType.Rook, "wr.png" },
                { FigureType.Bishop, "wb.png" },
                { FigureType.Knight, "wn.png" },
                { FigureType.Pawn, "wp.png" }
            }
        }
    };

    public static string Create(BaseFigure figure)
    {
        //todo: add handler exception? 
        return MAP[figure.Color][figure.GetTypeFigure()];
    }
}