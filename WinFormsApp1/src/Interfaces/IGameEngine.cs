using WinFormsApp1.Engin;
using WinFormsApp1.Entities.Chessboard;

namespace WinFormsApp1.Interfaces;

public interface IGameEngine
{
    public Chessboard? Chessboard { set; }
    public UpdateGame HandleClick(Cell cell);
    public UpdateGame HandleBack();
}