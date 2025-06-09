using WinFormsApp1.Entities.Chessboard;

namespace WinFormsApp1.FormLayout;

public class ButtonCell : Button
{
    private Cell _cell;
    private string _topLeftText = "";
    private string _centerText = "";
    private string _bottomRightText = "";
    public ButtonCell(Cell cell)
    {
        _cell = cell;
    }
    
    public static ButtonCell Make(Cell cell, bool isBlack, int x, int y)
    {
        Color defaultColor = isBlack 
            ? Color.FromArgb(115, 149, 82) 
            : Color.FromArgb(240, 241, 214);
        
        ButtonCell btn = new ButtonCell(cell);
        btn.Size = new Size(50, 50);
        btn.Location = new Point(x, y);
        btn.BackColor = defaultColor;
        btn.ForeColor = Color.Black;
        
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 255, 51);
        btn.FlatAppearance.MouseDownBackColor = defaultColor;


        btn.TabStop = false;
        btn.Margin = new Padding(0);
        btn.Padding = new Padding(0);
        
        return btn;
    }

    public void SetTopLeft(string text)
    {
        _topLeftText = text;
        Invalidate();
    }
    public void SetBottomRight(string text)
    {
        _bottomRightText = text;
        Invalidate();
    }
    public void SetCenter(string text)
    {
        _centerText = text;
        Invalidate();
    }
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent); // Рисует фон и границу кнопки

        var g = pevent.Graphics;
        using var brush = new SolidBrush(this.ForeColor);
        var font = this.Font;

        // Левый верхний угол
        g.DrawString(_topLeftText, font, brush, new PointF(2, 2));

        // Центр
        var centerSize = g.MeasureString(_centerText, font);
        var centerX = (this.Width - centerSize.Width) / 2;
        var centerY = (this.Height - centerSize.Height) / 2;
        g.DrawString(_centerText, font, brush, new PointF(centerX, centerY));

        // Правый нижний угол
        var bottomRightSize = g.MeasureString(_bottomRightText, font);
        float rightX = this.Width - bottomRightSize.Width - 2;
        float bottomY = this.Height - bottomRightSize.Height - 2;
        g.DrawString(_bottomRightText, font, brush, new PointF(rightX, bottomY));
    }
    public Cell GetCell() => _cell;
    
}