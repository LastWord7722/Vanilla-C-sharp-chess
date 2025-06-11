namespace WinFormsApp1.ValueObjects;

// юзаем структуру по причине того что позиции имутабельная и простая 
public readonly record struct Position
{
    private readonly char _column; // A–H
    private readonly int _row; // 1–8

    public Position(char column, int row)
    {
        Validatate(row, column);
        _column = char.ToUpper(column);
        _row = row;
    }

    public static Position Make(char column, int row) =>
        new Position(column, row);

    public static Position Make(int row, char column) =>
        new Position(column, row);

    private static void Validatate(int row, char column)
    {
        column = char.ToUpper(column);
        if (column < 'A' || column > 'H')
            throw new Exception("not valid position column");
        if (row < 1 || row > 8)
            throw new Exception("not valid position row");
    }

    public string GetPositionCode()
    {
        return _column.ToString() + _row.ToString();
    }

    public char GetColumn()
    {
        return _column;
    }

    public int GetRow()
    {
        return _row;
    }

    public bool IsColumn(char column) =>
        char.ToLower(_column) == char.ToLower(column);
}