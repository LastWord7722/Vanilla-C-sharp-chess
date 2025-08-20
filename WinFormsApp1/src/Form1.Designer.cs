using System.Drawing.Drawing2D;
using WinFormsApp1.Engin;
using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.Entities.Figures;
using WinFormsApp1.Enums;
using WinFormsApp1.Factories;
using WinFormsApp1.FormLayout;
using WinFormsApp1.Interfaces;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1;

partial class Form1
{
    IGameEngine? _gameEngine = null;
    private System.ComponentModel.IContainer components = null;
    public Dictionary<Position, ButtonCell> ButtonCells { private get; set; } = new();
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    void rerenderButtonCells(UpdateGame updateGame)
    {
        //update
        foreach (UpdateGameItem item in updateGame.UpdatedList)
        {
            if (item.To.HasValue)
            {
                ButtonCells[item.To.Value]
                    .SetCenter(IconFigureFactory.Create(item.Figure!));
            }
            
            if (item.From.HasValue)
            {
                ButtonCells[item.From.Value]
                    .SetCenter("");
            }
        }
        //clear
        foreach (var move in updateGame.ClearList)
        {
            ButtonCells[move].SetPossibleMove("");
        }
        //move
        foreach (var move in updateGame.MoveList)
        {
            //при большой количестве ходов видна задержка отрисовки, именно отрисовки сам просчёт быстрый
            if (ButtonCells[move].GetCell().HasFigure()) //возможность атткаовать, нужно добавить особую иконку
            {
                ButtonCells[move].SetPossibleMove("possible_move.png", 40);
            }
            else
            {
                ButtonCells[move].SetPossibleMove("possible_move.png");
            }
        }
    }
    void BtnBack_Click(Object sender, EventArgs e)
    {
        UpdateGame updateGame = _gameEngine.HandleBack();
        rerenderButtonCells(updateGame);
    }
    void BtnCell_Click(Object sender, EventArgs e)
    {
        ButtonCell btnCell = sender as ButtonCell; 
        if (btnCell == null) return;
        
        UpdateGame updateGame = _gameEngine.HandleClick(btnCell.GetCell());
        rerenderButtonCells(updateGame);
    }

    private void InitializeComponent()
    {
        // === create style and form ===
        components = new System.ComponentModel.Container();
        this.Text = "Chess";
        this.ClientSize = new Size(940, 520);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.BackColor = Color.FromArgb(34, 34, 38); 
        
        Label title = new Label();
        title.Text = "♟ Chess Game ♟";
        title.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        title.ForeColor = Color.FromArgb(240, 240, 240);
        title.AutoSize = true;
        title.Location = new Point((this.ClientSize.Width - 260) / 2, 10);
        this.Controls.Add(title);
        
        Panel innerBoard = CreateBoardContainer(new Point(60, 60));
        this.Controls.Add(innerBoard);
        
        Panel controlPanel = new Panel();
        controlPanel.Size = new Size(150, 260);
        controlPanel.Location = new Point(740, 100);
        controlPanel.BackColor = Color.FromArgb(28, 28, 28);
        controlPanel.BorderStyle = BorderStyle.None;
        this.Controls.Add(controlPanel);

        Button back = CreateStyledButton("⟲ Undo Move", 15, BtnBack_Click);
        controlPanel.Controls.Add(back);

        Button restart = CreateStyledButton("⟳ Restart", 75, (s, e) =>
        {
            this.Controls.Clear();
            InitializeComponent();
        });
        controlPanel.Controls.Add(restart);

        Button exit = CreateStyledButton("✖ Exit", 135, (s, e) => Application.Exit());
        controlPanel.Controls.Add(exit);
        
        Label hint = new Label();
        hint.Text = "• Click a piece to move";
        hint.Font = new Font("Segoe UI", 8, FontStyle.Regular);
        hint.ForeColor = Color.FromArgb(180, 180, 180);
        hint.AutoSize = true;
        hint.Location = new Point(740, 370);
        this.Controls.Add(hint);
        // === create logic ===
        Chessboard chessboard = new ArrangerFigure(Chessboard.Make()).ClassicArrangement();
        ButtonCells = rendreChessboard(chessboard, innerBoard, 50);
        _gameEngine = GameEngineFactory.Get(chessboard);
    }
    private Panel CreateBoardContainer(Point topLeft)
    {
        Panel inner = new Panel();
        inner.Size = new Size(400, 400);
        inner.Location = new Point(topLeft.X, topLeft.Y);
        inner.BackColor = Color.FromArgb(48, 48, 50);
        inner.Margin = new Padding(0);
        inner.Padding = new Padding(0);

        return inner;
    }

    private Dictionary<Position, ButtonCell> rendreChessboard(Chessboard chessboard, Panel parent, int cellSize = 50)
    {
        Dictionary<Position, ButtonCell> buttonCells = new Dictionary<Position, ButtonCell>();
        int x = 0;
        int y = 0;
        bool isBlack = false;
        
        var sortedCells = chessboard.Cells.OrderByDescending(kvp => kvp.Key.GetRow());

        foreach (var (_, cell) in sortedCells)
        {
            ButtonCell currentCell = ButtonCell.Make(cell, isBlack, x, y).SetClickEvent(BtnCell_Click);
            Position position = cell.Position;
            buttonCells.Add(cell.Position, currentCell);
            
            if (currentCell.GetCell().HasFigure())
            {
                currentCell.SetCenter(IconFigureFactory.Create(currentCell.GetCell().Figure));

                currentCell.ForeColor = currentCell.GetCell().Figure.Color == FigureColor.White
                    ? Color.White
                    : Color.FromArgb(20, 20, 20);
            }
            else
            {
                currentCell.ForeColor = Color.FromArgb(20, 20, 20);
            }
            
            parent.Controls.Add(currentCell);
            
            isBlack = !isBlack;
            if (position.IsColumn('h'))
            {
                x = 0;
                y += cellSize;
                isBlack = !isBlack;
            }
            else x += cellSize;
        }
        
        for (int i = 0; i < 8; i++)
        {
            char col = (char)('a' + i);
            Label lbl = new Label();
            lbl.Text = col.ToString();
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(200, 200, 200);
            lbl.AutoSize = true;
            int lblX = parent.Left + i * cellSize + (cellSize / 2) - 6;
            int lblY = parent.Bottom + 4;
            lbl.Location = new Point(lblX, lblY);
            this.Controls.Add(lbl);
        }
        
        for (int r = 0; r < 8; r++)
        {
            int rowNumber = 8 - r;
            Label lbl = new Label();
            lbl.Text = rowNumber.ToString();
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(200, 200, 200);
            lbl.AutoSize = true;
            int lblX = parent.Left - 18;
            int lblY = parent.Top + r * cellSize + (cellSize / 2) - 8;
            lbl.Location = new Point(lblX, lblY);
            this.Controls.Add(lbl);
        }

        return buttonCells;
    }

    private Button CreateStyledButton(string text, int top, EventHandler clickHandler)
    {
        var btn = new Button();
        btn.Text = text;
        btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        btn.Size = new Size(120, 40);
        btn.Location = new Point(15, top);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.ForeColor = Color.White;
        btn.BackColor = Color.FromArgb(63, 63, 70);
        btn.Click += clickHandler;
        
        var path = new GraphicsPath();
        int radius = 8;
        path.AddArc(new Rectangle(0, 0, radius * 2, radius * 2), 180, 90);
        path.AddArc(new Rectangle(btn.Width - radius * 2, 0, radius * 2, radius * 2), -90, 90);
        path.AddArc(new Rectangle(btn.Width - radius * 2, btn.Height - radius * 2, radius * 2, radius * 2), 0, 90);
        path.AddArc(new Rectangle(0, btn.Height - radius * 2, radius * 2, radius * 2), 90, 90);
        path.CloseAllFigures();
        btn.Region = new Region(path);

        return btn;
    }

    #endregion
}