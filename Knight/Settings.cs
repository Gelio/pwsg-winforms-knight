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
    public partial class Settings : Form
    {
        private List<BoardSize> availableBoardSizes = new List<BoardSize>();
        private int selectedBoardSize = 0;
        public BoardSize BoardSize => availableBoardSizes[selectedBoardSize];

        public Settings()
        {
            InitializeComponent();
            availableBoardSizes.Add(new BoardSize(8, 8));
            availableBoardSizes.Add(new BoardSize(10, 10));
            availableBoardSizes.Add(new BoardSize(12, 12));

            comboBoxBoardSize.Items.Clear();
            foreach (BoardSize boardSize in availableBoardSizes)
                comboBoxBoardSize.Items.Add(boardSize.ToString());
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            comboBoxBoardSize.SelectedIndex = selectedBoardSize;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            selectedBoardSize = comboBoxBoardSize.SelectedIndex;
            DialogResult = DialogResult.OK;
        }
    }

    public struct BoardSize
    {
        public int rows;
        public int columns;

        public BoardSize(int _rows, int _columns)
        {
            rows = _rows;
            columns = _columns;
        }

        public override string ToString()
        {
            return $"{columns}x{rows}";
        }
    }
}
