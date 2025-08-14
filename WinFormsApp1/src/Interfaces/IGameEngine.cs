using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.FormLayout;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Interfaces;

public interface IGameEngine
{
    public Dictionary<Position, ButtonCell> ButtonCells { set; }
    public Chessboard? Chessboard { set; }
    public void HandleClick(Cell cell);
    public void HandleBack();
}