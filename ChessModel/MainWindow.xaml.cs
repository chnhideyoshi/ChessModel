using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using ChessLogicModels;

namespace ChessModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
            InitEventHandler();
        }

        private void Init()
        {
            BTN_Restart.IsEnabled = false;
            string path = System.IO.Directory.GetCurrentDirectory();
            if (System.IO.Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path);
                for (int i = 0; i < files.Length; i++)
                {
                    if (System.IO.Path.GetExtension(files[i]).ToUpper() == ".EXE" &&! System.IO.Path.GetFileNameWithoutExtension(files[i]).Contains("ChessModel"))
                    {
                        TB_BlackFilePath.Items.Add(System.IO.Path.GetFileName(files[i]));
                        TB_WhiteFilePath.Items.Add(System.IO.Path.GetFileName(files[i]));
                    }
                }
            }
        }
        #region MyRegion
        private void InitEventHandler()
        {
            #region UIcontrol
            CA_TouchPad.MouseDown += (sender, e) =>
            {
                if (game != null && game.IsStarted)
                {
                    Point p = e.GetPosition(sender as UIElement);
                    int columnIndex = GetIndex((sender as Canvas).ActualHeight, OthelloChessBoard.ColumnCount, p.X);
                    int rowIndex = GetIndex((sender as Canvas).ActualWidth, OthelloChessBoard.RowCount, p.Y);
                    if (OthelloChessBoard.IsInBoardRange(rowIndex, columnIndex))
                    {
                        InputData data = new InputData(game.CurrentPlayer, rowIndex, columnIndex);
                        thread.SendInput(data);
                    }
                }
            };
            #region UI
            CA_TouchPad.MouseMove += (sender, e) =>
            {
                Point p = e.GetPosition(sender as UIElement);
                int columnIndex = GetIndex((sender as Canvas).ActualHeight, OthelloChessBoard.ColumnCount, p.X);
                int rowIndex = GetIndex((sender as Canvas).ActualWidth, OthelloChessBoard.RowCount, p.Y);
                if (OthelloChessBoard.IsInBoardRange(rowIndex, columnIndex))
                {
                    if (game != null && game.IsStarted)
                    {
                        ShowHover(game.CurrentPlayer.Color, rowIndex, columnIndex);
                    }
                }
            };
            #endregion
            BTN_Restart.Click += (sender, e) =>
            {
                BlackAutoPlay = RB_BlackAi.IsChecked == true;
                WhiteAutoPlay = RB_WhiteAi.IsChecked == true;
                InitThread();
                InitUI();
            };
            BTN_Apply.Click += (sender, e) => 
            {
                if ((sender as Button).Content.ToString() == "Alter")
                {
                    (sender as Button).Content = "Apply";
                    GB_Setting.IsEnabled = true;
                    BTN_Restart.IsEnabled = false;
                }
                else
                {
                    if ((RB_BlackAi.IsChecked == true && TB_BlackFilePath.SelectedItem == null) || (RB_WhiteAi.IsChecked == true && TB_WhiteFilePath.SelectedItem == null))
                    {
                        MessageBox.Show("Please Choose Ai");
                        return;
                    }
                    (sender as Button).Content = "Alter";
                    GB_Setting.IsEnabled = false;
                    BTN_Restart.IsEnabled = true;
                }
            };
            RB_BlackManual.Checked += (sender, e) => { TB_BlackFilePath.IsEnabled = false; };
            RB_BlackAi.Checked += (sender, e) => { TB_BlackFilePath.IsEnabled = true; };
            RB_WhiteManual.Checked += (sender, e) => { TB_WhiteFilePath.IsEnabled = false; };
            RB_WhiteAi.Checked += (sender, e) => { TB_WhiteFilePath.IsEnabled = true; };
            TB_BlackFilePath.SelectionChanged += (sender, e) => { BlackFilePath = e.AddedItems[0].ToString(); };
            TB_WhiteFilePath.SelectionChanged += (sender, e) => { WhiteFilePath = e.AddedItems[0].ToString(); };
            #endregion
        }
        private void InitUI()
        {
            for (int i = 0; i < GD_MainContainner.Children.Count; i++)
            {
                GD_MainContainner.Children.Clear();
            }
            RecMap = new Rectangle[OthelloChessBoard.RowCount, OthelloChessBoard.ColumnCount];
        }
        private void ShowHover(UnitKind unitKind, int rowIndex, int columnIndex)
        {
            if (unitKind == UnitKind.White)
            {
                BD_HOVERBlack.Visibility = Visibility.Hidden;
                BD_HOVERWhite.Visibility = Visibility.Visible;
                Grid.SetRow(BD_HOVERWhite, rowIndex);
                Grid.SetColumn(BD_HOVERWhite, columnIndex);
            }
            else
            {
                BD_HOVERWhite.Visibility = Visibility.Hidden;
                BD_HOVERBlack.Visibility = Visibility.Visible;
                Grid.SetRow(BD_HOVERBlack, rowIndex);
                Grid.SetColumn(BD_HOVERBlack, columnIndex);
            }
        }
        //[WorkingThread]
        private void PutUnit(OthelloGame game, UnitKind kind)
        {
            while (true)
            {
                InputData data = thread.ReceiveInput() as InputData;
                if (game.SetUnit(kind, data.RowIndex, data.ColumnIndex))
                {
                    break;
                }
                else
                {
                    ShowMessageOnWorkingThread("InvalidInput");
                }
            }
        }
        //[WorkingThread]
        public void AutoPlay(UnitKind kind)
        {
            if (game != null && game.IsStarted)
            {
                string path = kind == UnitKind.Black ? BlackFilePath : WhiteFilePath;
                Coordinate co = ProcessManager.GetSolve(path.Trim(),ParameterStringBuilder.GetParameterString(game.CurrentChessBoard,kind));
                if (co == null)
                {
                    ShowMessageOnWorkingThread("Error! Check File Name!");
                    return;
                }
                InputData data = new InputData(game.CurrentPlayer, co.RowIndex, co.ColumnIndex);
                thread.SendInput(data);
                System.Threading.Thread.Sleep(1000);
            }
        }
        private void ShowMessageOnWorkingThread(string message)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                TBK_Message.Text = message;
                BlackCount = game.SumOfBlackUnit.ToString();
                WhiteCount = game.SumOfWhiteUnit.ToString();
            }));
        }
        //[WorkingThread]
        private void MainWork()
        {
            InitGame();
            game.StartGame();
            while (!game.IsEndOfGame())
            {
                if (game.CurrentChessBoard.CanPutAnyUnit(UnitKind.Black))
                {
                    if (BlackAutoPlay)
                        AutoPlay(UnitKind.Black);
                    PutUnit(game, UnitKind.Black);
                    ShowMessageOnWorkingThread("Next To White");
                }
                else
                {
                    ShowMessageOnWorkingThread("Next To White NoInput");
                }
                game.NextPlayer();
                if (game.CurrentChessBoard.CanPutAnyUnit(UnitKind.White))
                {
                    if (WhiteAutoPlay)
                        AutoPlay(UnitKind.White);
                    PutUnit(game, UnitKind.White);
                    ShowMessageOnWorkingThread("Next To Black");
                }
                else
                {
                    ShowMessageOnWorkingThread("Next To Black NoInput");
                }
                game.NextPlayer();
            }
            game.EndGame();
            ShowMessageOnWorkingThread("End Winner:" + game.GetWinner().Color.ToString());
        }
        private void InitThread()
        {
            thread = new WorkingThread() { CheckInputInterval = 100 };
            thread.Work = MainWork;
            thread.StartThreadCircle();
        }
        private void InitGame()
        {
            game = new OthelloGame();
            game.CurrentChessBoard = new OthelloChessBoard();
            game.CurrentChessBoard.UnitChanged += (unit, reason) =>
            {
                UpdateAll();
            };
            game.Player1 = new ChessPlayer(UnitKind.Black);
            game.Player2 = new ChessPlayer(UnitKind.White);
            game.CurrentPlayer = game.Player1;
        } 
        #endregion
        #region Privatevar
        private OthelloGame game;
        WorkingThread thread;
        #endregion
        #region UIMethod
        private void SetRectange(UnitKind unitKind, int rowIndex, int columnIndex)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Rectangle rec = new Rectangle();
                if (unitKind == UnitKind.White)
                {
                    rec.Style = this.FindResource("WhiteUnitStyle") as Style;
                }
                else
                {
                    rec.Style = this.FindResource("BlackUnitStyle") as Style;
                }
                GD_MainContainner.Children.Add(rec);
                RecMap[rowIndex, columnIndex] = rec;
                rec.Tag = new Coordinate() { RowIndex = rowIndex, ColumnIndex = columnIndex };
                Grid.SetRow(rec, rowIndex);
                Grid.SetColumn(rec, columnIndex);
            }), null);
        }
        private void UpdateRectange(UnitKind unitKind, int rowIndex, int columnIndex)
        {
           Dispatcher.BeginInvoke(new Action(() =>
           {
               Rectangle rec = RecMap[rowIndex, columnIndex];
               if (rec == null) { return; }
               if (unitKind == UnitKind.White)
               {
                   rec.Style = this.FindResource("WhiteUnitStyle") as Style;
               }
               else
               {
                   rec.Style = this.FindResource("BlackUnitStyle") as Style;
               }
           }), null);
        }
        private void UpdateAll()
        {
            try
            {
                if (game == null) { return; }
                for (int i = 0; i < OthelloChessBoard.RowCount; i++)
                {
                    for (int j = 0; j < OthelloChessBoard.ColumnCount; j++)
                    {
                        if (game.CurrentChessBoard.CurrentMap[i, j] != null)
                        {
                            if (RecMap[i, j] == null)
                            {
                                SetRectange(game.CurrentChessBoard.CurrentMap[i, j].Kind, i, j);
                            }
                            else
                            {
                                UpdateRectange(game.CurrentChessBoard.CurrentMap[i, j].Kind, i, j);
                            }
                        }
                    }
                }
            }
            catch { return; }
        }
        #endregion
        #region
        
        private int GetIndex(double sumLength, int count, double position)
        {
            double h = sumLength / count;
            return (int)(position / h);
        }
        #endregion
        #region Properties
        public Rectangle[,] RecMap;
        public string BlackCount
        {
            set { TBK_BlackCount.Text = value; }
            get { return TBK_BlackCount.Text; }
        }
        public string WhiteCount
        {
            set { TBK_WhiteCount.Text = value; }
            get { return TBK_WhiteCount.Text; }
        }
        public bool WhiteAutoPlay = false;
        public bool BlackAutoPlay = false;
        public string BlackFilePath = null;
        public string WhiteFilePath = null;
        #endregion
    }
    public class Coordinate
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public static Coordinate GetCoordinate(string a)
        {
            string[] s = a.Split(' ');
            if (s.Length == 2)
            {
                int x = Convert.ToInt32(s[0]);
                int y = Convert.ToInt32(s[1]);
                return new Coordinate() { RowIndex = x, ColumnIndex = y };
            }
            else
            {
                return null;
            }
        }
    }
    public class InputData:IInputData
    {
        public InputData(ChessPlayer player, int rowIndex, int columnIndex)
        {
            Player = player;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }
        public ChessPlayer Player { get; set; }
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
    }
}
