using System.Drawing.Drawing2D;
using WinFormsApp1.Entities.Chessboard;

namespace WinFormsApp1.FormLayout;

public class ButtonCell : Button
{
    private int _maxSize;
    private int _maxSizePossible;
    private readonly Cell _cell;

    private string? _figureImagePath = null;
    private string? _possibleImagePath = null;

    public ButtonCell(Cell cell)
    {
        _cell = cell;
    }

    public ButtonCell SetClickEvent(EventHandler handler)
    {
        Click += handler;
        return this;
    }

    public ButtonCell SetCenter(string img, int size = 45)
    {
        _maxSize = size;
        _figureImagePath = img;
        Invalidate();
        return this;
    }
    public ButtonCell SetPossibleMove(string img, int size = 25)
    {
        _maxSizePossible = size;
        _possibleImagePath = img;
        Invalidate();
        return this;
    }
    public Cell GetCell() => _cell;

    public static ButtonCell Make(Cell cell, bool isBlack, int x, int y)
    {
        ButtonCell btn = new ButtonCell(cell);
        btn.BackColor = isBlack
            ? Color.FromArgb(115, 84, 46)
            : Color.FromArgb(240, 235, 210);
        btn.Size = new Size(50, 50);

        btn.Location = new Point(x, y);

        btn.ForeColor = Color.FromArgb(30, 30, 30);

        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;

        btn.TabStop = false;
        btn.Margin = new Padding(0);
        btn.Padding = new Padding(0);

        return btn;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        var g = pevent.Graphics;
        //дублируется код, вынести в метод / перебрать цыклом 2 варинта 
        if (!string.IsNullOrEmpty(_possibleImagePath))
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "Storage", "icons", "figures",
                _possibleImagePath);
            using var image = Image.FromFile(fullPath);

            int width, height;
            if (image.Width > image.Height)
            {
                width = _maxSizePossible;
                height = image.Height * _maxSizePossible / image.Width;
            }
            else
            {
                height = _maxSizePossible;
                width = image.Width * _maxSizePossible / image.Height;
            }

            int x = (Width - width) / 2;
            int y = (Height - height) / 2;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, new Rectangle(x, y, width, height));
        }
        if (!string.IsNullOrEmpty(_figureImagePath))
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "Storage", "icons", "figures",
                _figureImagePath);
            using var image = Image.FromFile(fullPath);

            int width, height;
            if (image.Width > image.Height)
            {
                width = _maxSize;
                height = image.Height * _maxSize / image.Width;
            }
            else
            {
                height = _maxSize;
                width = image.Width * _maxSize / image.Height;
            }

            int x = (Width - width) / 2;
            int y = (Height - height) / 2;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, new Rectangle(x, y, width, height));
        }

    }
}