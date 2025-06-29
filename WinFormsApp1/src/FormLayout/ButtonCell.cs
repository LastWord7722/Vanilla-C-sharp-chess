using WinFormsApp1.Entities.Chessboard;

namespace WinFormsApp1.FormLayout;

public class ButtonCell : Button
{
    private readonly Cell _cell;
    private Color _defaultColor;
    private string _topLeftText = "";
    private string _centerText = "";
    private string _bottomRightText = "";
    public ButtonCell(Cell cell, Color color)
    {
        _cell = cell;
        _defaultColor = color;
        SetBackGroundColor(_defaultColor);
    }
    
    public static ButtonCell Make(Cell cell, bool isBlack, int x, int y)
    {
        Color defaultColor = isBlack 
            ? Color.FromArgb(115, 149, 82) 
            : Color.FromArgb(240, 241, 214);
        
        ButtonCell btn = new ButtonCell(cell,defaultColor);
        btn.Size = new Size(50, 50);
        btn.Location = new Point(x, y);
        
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

    public void ResetColorToDefault()
    {
        SetBackGroundColor(_defaultColor);
    }
    public ButtonCell SetBackGroundColor(Color color)
    {
        BackColor = color;
        return this;
    }
    public ButtonCell SetClickEvent(EventHandler handler)
    {
        Click += handler;
        return this;
    }
    public ButtonCell SetTopLeft(string text)
    {
        _topLeftText = text;
        Invalidate();
        return this;
    }
    public ButtonCell SetBottomRight(string text)
    {
        _bottomRightText = text;
        Invalidate();
        return this;
    }
    public ButtonCell SetCenter(string text)
    {
        _centerText = text;
        Invalidate();
        return this;
    }
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent); //Рисует фон и границу кнопки

        var g = pevent.Graphics;
        using var brush = new SolidBrush(this.ForeColor);
        var font = this.Font;
        var fontCenter = new Font(Font.FontFamily, 20);

        //Левый верхний угол
        g.DrawString(_topLeftText, font, brush, new PointF(2, 2));

        //Центр
        var centerSize = g.MeasureString(_centerText, fontCenter);
        var centerX = (this.Width - centerSize.Width) / 2;
        var centerY = (this.Height - centerSize.Height) / 2;
        g.DrawString(_centerText, fontCenter, brush, new PointF(centerX, centerY));

        //Правый нижний угол
        var bottomRightSize = g.MeasureString(_bottomRightText, font);
        float rightX = this.Width - bottomRightSize.Width - 2;
        float bottomY = this.Height - bottomRightSize.Height - 2;
        g.DrawString(_bottomRightText, font, brush, new PointF(rightX, bottomY));
    }
    public Cell GetCell() => _cell;
    
    
    
}