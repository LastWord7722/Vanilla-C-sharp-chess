using WinFormsApp1.Entities.Figures;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1.Engin;

public class UpdateGame
{
    public List<UpdateGameItem> UpdatedList { get; } = new();
    public List<Position> ClearList { get; } = new();
    public List<Position> MoveList { get; } = new();

    public void AddUpdated(Position? to, BaseFigure figure, Position? form = null)
        => UpdatedList.Add(new UpdateGameItem(to, figure, form));

    public void AddRangeUpdated(List<UpdateGameItem> updatedList)
        => UpdatedList.AddRange(updatedList);

    public void AddCleared(Position pos)
        => ClearList.Add(pos);

    public void AddRangeCleared(List<Position> clearedList)
        => ClearList.AddRange(clearedList);

    public void AddMoved(Position pos)
        => MoveList.Add(pos);

    public void AddRangeMoved(List<Position> clearedList)
        => MoveList.AddRange(clearedList);

    public void AddRangeAll(UpdateGame updateGame)
    {
        AddRangeUpdated(updateGame.UpdatedList);
        AddRangeCleared(updateGame.ClearList);
        AddRangeMoved(updateGame.MoveList);
    }

    public void Clear()
    {
        UpdatedList.Clear();
        ClearList.Clear();
        MoveList.Clear();
    }
}