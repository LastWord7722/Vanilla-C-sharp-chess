using System.Text.RegularExpressions;

namespace WinFormsApp1.ValueObjects;

// юзаем структуру по причине того что позиции имутабельная и простая 
public readonly record struct Position
{
    private readonly char _column; // A–H
    private readonly  int _row;  // 1–8

    public Position(char column, int row)
    {
        column = char.ToUpper(column);
        if (column < 'A' || column > 'H')
            throw new Exception("not valid position column");
        if (row < 1 || row > 8)
            throw new Exception("not valid position row");

        _column = column;
        _row = row;
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
    
    public bool IsColumn(char column)
    {
        return char.ToLower(_column) == char.ToLower(column);
    }
    
}