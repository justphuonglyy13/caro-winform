using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaroNet
{
    public class ChessBoardManager
    {
        //Attributes & Properties
        private Panel chessBoard;
        public Panel ChessBoard {
            get => chessBoard;
            set => chessBoard = value;
        }

        private Boolean turn = false;

        private List<Player> player;
        public List<Player> Player {
            get => player;
            set => player = value;
        }

        private int currentPlayer;
        public int CurrentPlayer {
            get => currentPlayer;
            set => currentPlayer = value;
        }

        private TextBox playerName;
        public TextBox PlayerName {
            get => playerName;
            set => playerName = value;
        }

        private PictureBox playerMark;
        public PictureBox PlayerMark {
            get => playerMark;
            set => playerMark = value;
        }

        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }    

        //Event Handler
        private event EventHandler playerMarked;
        public event EventHandler PlayerMarked
        {
            add { playerMarked += value; }
            remove { playerMarked -= value; }
        }

        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add { endedGame += value; }
            remove { endedGame -= value; }
        }

        private Stack<PlayInfo> playLog;
        public Stack<PlayInfo> PlayLog { 
            get => playLog; 
            set => playLog = value; 
        }


        //Constructors
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark)
        {
            this.chessBoard = chessBoard;
            this.playerName = playerName;
            this.playerMark = mark;
            this.player = new List<Player>()
            {
                new Player("Player_1", Properties.Resources.xBtn),
                new Player("Player_2", Properties.Resources.oBtn)
            };
        }

        //Methods
        public void drawChessBoard()
        {
            //Enable ChessBorad
            ChessBoard.Enabled = true;
            ChessBoard.Controls.Clear();
            this.PlayLog= new Stack<PlayInfo>();

            this.CurrentPlayer = 0;

            this.changePlayer();

            Matrix = new List<List<Button>>();

            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Constants.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());

                for (int j = 0; j < Constants.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Constants.CHESS_WIDTH,
                        Height = Constants.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };

                    btn.Click += btn_Click;

                    ChessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);

                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Constants.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.BackgroundImage != null) return;

            this.mark(btn);
            this.PlayLog.Push(new PlayInfo(this.GetChessPoint(btn), this.CurrentPlayer));

            //TODO: Cần tạo thêm methods mới để tách riêng việc chuyển người chơi 
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            this.changePlayer();

            if (playerMarked != null)
            {
                playerMarked(this, new EventArgs());
            }

            if (this.isEndGame(btn))
            {
                EndGame();       
            }
        }

        private void EndGame()
        {
            //this.CurrentPlayer = this.CurrentPlayer == 0 ? 1 : 0;
            //MessageBox.Show("GAME OVER!!!\nThe winner is " + Player[currentPlayer].Name);

            if (endedGame != null)
            {
                endedGame(this, new EventArgs());
            }
            //Application.Exit();
        }

        public bool Undo()
        {
            if (this.PlayLog.Count <= 0) { 
                return false; 
            }

            PlayInfo prevMove = PlayLog.Pop();
            Button btn = Matrix[prevMove.Point.Y][prevMove.Point.X];

            btn.BackgroundImage = null;
            if (PlayLog.Count <= 0)
            {
                CurrentPlayer = 0;
            }
            else
            {
                prevMove = PlayLog.Peek();
                CurrentPlayer = prevMove.CurrentPlayer == 1 ? 0 : 1;
            }

            this.changePlayer();

            return false;
        }

        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }

        private Point GetChessPoint(Button btn)
        {
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);

            Point point = new Point(horizontal, vertical);

            return point;
        }

        private bool isEndHorizontal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;
            for (int i = point.X + 1; i < Constants.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }

            return countLeft + countRight == 5;
        }
        private bool isEndVertical(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = point.Y + 1; i < Constants.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }
        private bool isEndPrimary(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Constants.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Constants.CHESS_BOARD_HEIGHT || point.X + i >= Constants.CHESS_BOARD_WIDTH)
                    break;

                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }
        private bool isEndSub(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Constants.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Constants.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Constants.CHESS_BOARD_HEIGHT || point.X - i < 0)
                    break;

                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        private void mark(Button btn)
        {
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
        }

        private void changePlayer()
        {
            this.PlayerName.Text = Player[CurrentPlayer].Name;
            this.PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
    }
}
