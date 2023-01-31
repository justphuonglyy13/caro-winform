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
        public ChessBoardManager ChessBoard {  
            get => chessBoard; 
            set => chessBoard = value; 
        }

        public Form1()
        {
            InitializeComponent();

            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName, ptcbMark);
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            prcbCoolDown.Step = Constants.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Constants.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Constants.COOL_DOWN_INTERVAL;

            this.NewGame();

            //tmCoolDown.Start();
        }

        private void EndGame()
        {
            tmCoolDown.Stop();

            //while (prcbCoolDown.Value != prcbCoolDown.Maximum)
            //{
            //    continue;
            //}

            pnlChessBoard.Enabled = false;
            this.undoToolStripMenuItem.Enabled = false;
            MessageBox.Show("END GAME!!!!\nNgười chiến thắng là " + this.ChessBoard.Player[this.ChessBoard.CurrentPlayer == 1 ? 0 : 1].Name);
        }

        private void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            this.undoToolStripMenuItem.Enabled = true;
            ChessBoard.drawChessBoard();
        }

        private void Quit()
        {
            Application.Exit();
        }
        private void Undo()
        {
            this.ChessBoard.Undo();
        }
        private void ChessBoard_PlayerMarked(object sender, EventArgs e)
        {
            tmCoolDown.Start();
            prcbCoolDown.Value = 0;
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
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

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if (prcbCoolDown.Value >= prcbCoolDown.Maximum) {
                this.EndGame();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel= true;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
