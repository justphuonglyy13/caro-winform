using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaroNet
{
    public partial class Form1 : Form
    {
        ChessBoardManager chessBoard;

        public Form1()
        {
            InitializeComponent();

            chessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName, ptcbMark);

            chessBoard.drawChessBoard();
        }

        #region Unused
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pctbAvatar_Click(object sender, EventArgs e)
        {

        }
        #endregion Unused
    }
}
