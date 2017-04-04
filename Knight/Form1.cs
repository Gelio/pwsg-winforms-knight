using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Knight
{
    enum KnightDirection
    {
        Left,
        Right
    }

    public partial class Form1 : Form
    {
        private int gameRows = 8;
        private int gameColumns = 8;
        private Settings settings;
        private int playerX = 0;
        private int playerY = 0;
        private KnightDirection knightDirection = KnightDirection.Right;
        private PictureBox knight;

        private Bitmap knightBitmapLeft = Properties.Resources.knight2;
        private Bitmap knightBitmapRight = Properties.Resources.knight;

        public Form1()
        {
            InitializeComponent();
            settings = new Settings();
            knight = new PictureBox();
            knightBitmapLeft.MakeTransparent(Color.White);
            knightBitmapRight.MakeTransparent(Color.White);

            knight.SizeMode = PictureBoxSizeMode.StretchImage;
            knight.Dock = DockStyle.Fill;

            ResetGameMap();
        }

        private void PlaceKnight()
        {
            knight.Image = knightDirection == KnightDirection.Left ? knightBitmapLeft : knightBitmapRight;
            Panel panel = gameMap.GetControlFromPosition(playerX, playerY) as Panel;
            panel.Controls.Add(knight);
        }

        private void SetInitialKnightPosition()
        {
            for (int x = 0; x < gameColumns; x++)
            {
                for (int y=0; y < gameRows; y++)
                {
                    if (IsPositionValid(x, y))
                    {
                        playerX = x;
                        playerY = y;
                        break;
                    }
                }
            }
        }

        private bool IsPositionValid(int x, int y)
        {
            if (x < 0 || x >= gameColumns)
                return false;
            if (y < 0 || y >= gameRows)
                return false;

            return gameMap.GetControlFromPosition(x, y).BackColor == Color.ForestGreen;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.None) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void ResetGameMap()
        {
            gameMap.CellPaint -= GameMap_CellPaint;

            gameMap.Controls.Clear();

            float rowHeightPercentage = 100 / gameRows;
            gameMap.RowCount = gameRows;
            gameMap.RowStyles.Clear();
            for (int i=0; i < gameRows; i++)
            {
                gameMap.RowStyles.Add(new RowStyle(SizeType.Percent, rowHeightPercentage));
            }

            float columnWidthPercentage = 100 / gameColumns;
            gameMap.ColumnCount = gameColumns;
            gameMap.ColumnStyles.Clear();
            for (int i = 0; i < gameColumns; i++)
            {
                gameMap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidthPercentage));
            }

            Random r = new Random();
            for (int y = 0; y < gameRows; y++)
            {
                for (int x = 0; x < gameColumns; x++)
                {
                    Panel panel = new Panel();
                    panel.BackColor = r.Next(2) == 0 ? Color.Maroon : Color.ForestGreen;
                    panel.Dock = DockStyle.Fill;
                    gameMap.Controls.Add(panel, x, y);
                }
            }

            SetInitialKnightPosition();
            PlaceKnight();

            gameMap.CellPaint += GameMap_CellPaint;
        }

        private void GameMap_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Control c = gameMap.GetControlFromPosition(e.Column, e.Row);
            if (c == null)
                return;
            using (var b = new SolidBrush(c.BackColor))
            {
                e.Graphics.FillRectangle(b, e.CellBounds);
            }
        }
        
        private void ShowSettings()
        {
            if (settings.ShowDialog() == DialogResult.Cancel)
                return;

            BoardSize boardSize = settings.BoardSize;
            gameRows = boardSize.rows;
            gameColumns = boardSize.columns;
            ResetGameMap();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {
                ResetGameMap();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.M))
            {
                ShowSettings();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetGameMap();
        }

        private bool[] isArrowKeyDown = new bool[4];

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            bool knightMoved = false;
            if (e.KeyCode == Keys.Right && !isArrowKeyDown[0])
            {
                isArrowKeyDown[0] = true;
                knightDirection = KnightDirection.Right;
                knightMoved = true;
                if (IsPositionValid(playerX + 1, playerY))
                    playerX++;
                
            }
            else if (e.KeyCode == Keys.Left && !isArrowKeyDown[1])
            {
                isArrowKeyDown[1] = true;
                knightDirection = KnightDirection.Left;
                knightMoved = true;
                if (IsPositionValid(playerX - 1, playerY))
                    playerX--;
            }
            else if (e.KeyCode == Keys.Down && !isArrowKeyDown[2])
            {
                isArrowKeyDown[2] = true;
                if (IsPositionValid(playerX, playerY + 1))
                {
                    playerY++;
                    knightMoved = true;
                }
            }
            else if (e.KeyCode == Keys.Up && !isArrowKeyDown[3])
            {
                isArrowKeyDown[3] = true;
                if (IsPositionValid(playerX, playerY - 1))
                {
                    playerY--;
                    knightMoved = true;
                }
            }

            if (knightMoved)
                PlaceKnight();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                isArrowKeyDown[0] = false;
            else if (e.KeyCode == Keys.Left)
                isArrowKeyDown[1] = false;
            else if (e.KeyCode == Keys.Down)
                isArrowKeyDown[2] = false;
            else if (e.KeyCode == Keys.Up)
                isArrowKeyDown[3] = false;
        }
    }
}
