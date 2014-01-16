using System;
using System.Collections.Generic;
using System.Text;
using ChessLogicModels;
using System.ComponentModel;
namespace ChessLogicModels
{
    public class GameProcessor
    {
        public GameProcessor()
        {
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            game = new OthelloGame();
            game.CurrentChessBoard = new OthelloChessBoard();
            game.CurrentChessBoard.UnitChanged += (unit, reason) => { UpdateUI(); };
            game.Player1 = new ChessPlayer(UnitKind.Black);
            game.Player2 = new ChessPlayer(UnitKind.White);
            game.CurrentPlayer = game.Player1;
            game.StartGame();
            while (!game.IsEndOfGame())
            {
                if (game.CurrentChessBoard.CanPutAnyUnit(UnitKind.Black))
                {
                    if (BlackAutoPlay)
                        AutoPlay(UnitKind.Black);
                    PutUnit(game, UnitKind.Black);
                    ShowMessage("Next To White");
                }
                else
                {
                    ShowMessage("Next To White NoInput");
                }
                game.NextPlayer();
                if (game.CurrentChessBoard.CanPutAnyUnit(UnitKind.White))
                {
                    if (WhiteAutoPlay)
                        AutoPlay(UnitKind.White);
                    PutUnit(game, UnitKind.White);
                    ShowMessage("Next To Black");
                }
                else
                {
                    ShowMessage("Next To Black NoInput");
                }
                game.NextPlayer();
            }
            game.EndGame();
            ShowMessage("End Winner:" + game.GetWinner().Color.ToString());
        }
        private BackgroundWorker worker;
        private OthelloGame game;
        protected void ShowMessage(string message)
        {
            if (ProcessorEventHappened != null)
                ProcessorEventHappened("Message", message);
        }
        protected void UpdateUI()
        {
            if (ProcessorEventHappened != null)
                ProcessorEventHappened("UpdateUI", MessageEncoder.ConvertToString(game.CurrentChessBoard));
        }
        protected void AlertInputError(string message)
        {
            if (ProcessorEventHappened != null)
                ProcessorEventHappened("InputError", message);
        }
        private void AutoPlay(UnitKind kind)
        {
            if (game != null && game.IsStarted)
            {
                string path = kind == UnitKind.Black ? BlackFilePath : WhiteFilePath;
                Coordinate co = ProcessManager.GetSolve(path.Trim(), ParameterStringBuilder.GetParameterString(game.CurrentChessBoard, kind));
                if (co == null)
                {
                    ShowMessage("Error! Check File Name!");
                    return;
                }
                InputData data = new InputData(co.X, co.Y);
                SendInputData(data);
                System.Threading.Thread.Sleep(1000);
            }
        }
        private void PutUnit(OthelloGame game, UnitKind kind)
        {
            while (true)
            {
                InputData data = (InputData)InputBufferManager.Read(); //thread.ReceiveInput() as InputData;
                if (game.SetUnit(kind, data.X, data.Y))
                {
                    break;
                }
                else
                {
                    ShowMessage("InvalidInput");
                }
            }
        }

        public bool IsRunning
        {
            get { if (worker == null) { return false; } return worker.IsBusy; }
        }
        public bool BlackAutoPlay { get; set; }
        public bool WhiteAutoPlay { get; set; }
        public string BlackFilePath { get; set; }
        public string WhiteFilePath { get; set; }

        public void Run(object args)
        {
            worker.RunWorkerAsync(args);
        }
        public bool SendInputData(InputData data)
        {
            InputBufferManager.Write(data);
            return false;
        }
        public event ProcessorEventHander ProcessorEventHappened;
    }
    public delegate void ProcessorEventHander(string kind, string errorMessage);
    public struct InputData
    {
        public InputData(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }
    public static class MessageEncoder
    {
        public static string ConvertToString(OthelloChessBoard currentBoard)
        {
            return null;
        }
    }
    static class InputBufferManager
    {
        static InputBufferManager()
        {
            mutex = new System.Threading.Mutex(false, "M1", out createNew);
        }
        public static int CheckInputInterval = 1000;
        static bool createNew;
        static bool IsWaitingInput = true;
        static ValueType _data;
        public static object Read()
        {
            while (IsWaitingInput)
            {
                System.Threading.Thread.Sleep(CheckInputInterval);
            }
            mutex.WaitOne();
            IsWaitingInput = true;
            mutex.ReleaseMutex();
            return _data;
        }
        public static bool Write(ValueType data)
        {
            if (IsWaitingInput)
            {
                mutex.WaitOne();
                _data = data;
                IsWaitingInput = false;
                mutex.ReleaseMutex();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static System.Threading.Mutex mutex;
    }
    public static class ParameterStringBuilder
    {
        public static string GetParameterString(OthelloChessBoard currentBoard, UnitKind kind)
        {
            StringBuilder builder = new StringBuilder();
            string rowCount = OthelloChessBoard.RowCount.ToString();
            builder.Append(rowCount);
            builder.Append(" ");
            string columnCount = OthelloChessBoard.ColumnCount.ToString();
            builder.Append(columnCount);
            builder.Append(" ");
            string blackPositions = GetPositionsString(UnitKind.Black, currentBoard);
            builder.Append(blackPositions);
            builder.Append(" ");
            string whitePositions = GetPositionsString(UnitKind.White, currentBoard);
            builder.Append(whitePositions);
            builder.Append(" ");
            string currentPlayerKind = GetKindString(kind);
            builder.Append(currentPlayerKind);
            builder.Append(" ");
            string possiblePlaces = GetPossiblePosString(kind, currentBoard);
            builder.Append(possiblePlaces);
            return builder.ToString();
        }
        private static string GetPossiblePosString(UnitKind kind, OthelloChessBoard currentBoard)
        {
            List<Coordinate> coordinates = GetPossiblePos(kind, currentBoard);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < coordinates.Count; i++)
            {
                builder.Append("(");
                builder.Append(coordinates[i].X);
                builder.Append(",");
                builder.Append(coordinates[i].Y);
                builder.Append(")");
                if (i != coordinates.Count - 1)
                {
                    builder.Append("&");
                }
            }
            return builder.ToString();
        }
        private static List<Coordinate> GetPossiblePos(UnitKind kind, OthelloChessBoard currentBoard)
        {
            bool[,] map = currentBoard.GetPossiblePositions(kind);
            List<Coordinate> list = new List<Coordinate>();
            for (int i = 0; i < ChessBoard.RowCount; i++)
            {
                for (int j = 0; j < ChessBoard.ColumnCount; j++)
                {
                    if (map[i, j])
                    {
                        Coordinate co = new Coordinate(i, j);
                        list.Add(co);
                    }
                }
            }
            return list;
        }
        private static string GetPositionsString(UnitKind unitKind, OthelloChessBoard board)
        {
            List<ChessUnit> coordinates = GetCoordinates(unitKind, board);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < coordinates.Count; i++)
            {
                builder.Append("(");
                builder.Append(coordinates[i].CurrentRow);
                builder.Append(",");
                builder.Append(coordinates[i].CurrentColumn);
                builder.Append(")");
                if (i != coordinates.Count - 1)
                {
                    builder.Append("&");
                }
            }
            return builder.ToString();
        }
        private static List<ChessUnit> GetCoordinates(UnitKind unitKind, OthelloChessBoard board)
        {
            List<ChessUnit> list = board.CurrentChessUnitCollection.FindAll(new Predicate<ChessUnit>((unit) => { return unit.Kind == unitKind; }));
            return list;
        }
        private static string GetKindString(UnitKind kind)
        {
            if (kind == UnitKind.White)
            {
                return "White";
            }
            else
            {
                return "Black";
            }
        }

    }
}
