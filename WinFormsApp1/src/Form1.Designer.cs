using WinFormsApp1.Entities.Chessboard;
using WinFormsApp1.FormLayout;
using WinFormsApp1.ValueObjects;

namespace WinFormsApp1;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        rendreChessboard();

        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Chess";

    }

    private void rendreChessboard()
    {
        int heightWidth = 50;
        int x = 0;
        int y = 0;
        bool isBlack = false;
        var sortedCells = Chessboard.Make()
            .GetCells()
            .OrderByDescending(kvp => kvp.Key.GetRow());
        
        foreach (var (_, cell) in sortedCells)
        {
            ButtonCell currentCell = ButtonCell.Make(cell, isBlack, x,y);
            Position position = cell.GetPosition(); 
            
            if (position.IsColumn('a'))
                currentCell.SetTopLeft(position.GetRow().ToString());
            if (y == heightWidth * 7)
                currentCell.SetBottomRight(position.GetColumn().ToString());
            
            this.Controls.Add(currentCell);
            isBlack = !isBlack;

            if (position.IsColumn('h'))
            {
                x = 0;
                y += heightWidth;
                isBlack = !isBlack;
            }
            else x += heightWidth;
        }

    }
    #endregion
}