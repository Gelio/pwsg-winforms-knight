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
    public partial class Form1 : Form
    {
        private int gameRows = 8;
        private int gameColumns = 8;
        private Settings settings;

        public Form1()
        {
            InitializeComponent();
            settings = new Settings();
            ResetGameMap();
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
    }
}
