using System.Text.RegularExpressions;

namespace WinFormsApp1.ValueObjects;

// юзаем структуру по причине того что позиции имутабельная и простая 
public readonly struct Position
{
    private readonly char _file; // A–H
    private readonly int _rank;  // 1–8

    public Position(char file, int rank)
    {
        const string patternFile = @"^[A-H]$"; // Столбец
        const string patternRank = @"^[1-8]$"; // Ряд

        if (!Regex.Match(file.ToString(), patternFile).Success || !Regex.Match(rank.ToString(), patternRank).Success)
            throw new Exception("not valid position");
        _file = file;
        _rank = rank;
    }

    public string GetPositionCode()
    {
        return _file.ToString() + _rank.ToString();
    }
    public char GetFile()
    {
        return _file;
    }  
    public int GetRank()
    {
        return _rank;
    }
    
}