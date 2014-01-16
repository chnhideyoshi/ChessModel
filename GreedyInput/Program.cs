using System;
using System.Collections.Generic;
using System.Text;
using ChessLogicModels;

namespace GreedyInput
{
    class Program
    {
        public static int ColumnCount = 10;
        public static int RowCount = 10;
        public static OthelloChessBoard Board = null;
        public static UnitKind Kind;
        public static List<Coordinate> PossibleList;
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 6)
                {
                    InitArgs(args);
                    OthelloGameStrategy strategy = new OthelloGameStrategy(Board, Kind);
                    Coordinate co = strategy.GetBestSolveWithoutWeight();
                    Console.WriteLine(co.ToString());
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
            catch
            {
                Console.WriteLine("error");
            }
        }

        private static void InitArgs(string[] args)
        {
            RowCount = Convert.ToInt32(args[0]);
            ColumnCount = Convert.ToInt32(args[1]);
            Board = CreateBoard(args[2], args[3]);
            Kind = GetKind(args[4]);
            PossibleList = GetPossibleList(args[5]);
        }

        private static List<Coordinate> GetPossibleList(string p)
        {
            List<Coordinate> list = new List<Coordinate>();
            if (!p.Contains("&"))
            {
                string[] s2 = p.Split('(', ',', ')');
                int rowIndex = int.Parse(s2[1]);
                int columnIndex = int.Parse(s2[2]);
                Coordinate co = new Coordinate(rowIndex, columnIndex);
                return list;
            }
            else
            {
                string[] s = p.Split('&');
                for (int i = 0; i < s.Length; i++)
                {
                    string[] s2 = s[i].Split('(', ',', ')');
                    int rowIndex = int.Parse(s2[1]);
                    int columnIndex = int.Parse(s2[2]);
                    Coordinate co = new Coordinate(rowIndex, columnIndex);
                    list.Add(co);
                }
                return list;
            }
        }
        private static UnitKind GetKind(string s)
        {
            if (s == "Black")
            {
                return UnitKind.Black;
            }
            if (s == "White")
            {
                return UnitKind.White;
            }
            throw new Exception("");
        }
        private static OthelloChessBoard CreateBoard(string blackString, string whiteString)
        {
            OthelloChessBoard board = new OthelloChessBoard();
            string[] s = blackString.Split('&');
            InitUnits(board, s, UnitKind.Black);
            string[] s2 = whiteString.Split('&');
            InitUnits(board, s2, UnitKind.White);
            return board;
        }

        private static void InitUnits(OthelloChessBoard board, string[] s, UnitKind unitKind)
        {
            for (int i = 0; i < s.Length; i++)
            {
                string[] s2 = s[i].Split('(', ',', ')');
                int rowIndex = int.Parse(s2[1]);
                int columnIndex = int.Parse(s2[2]);
                OthelloChessUnit unit = new OthelloChessUnit(unitKind);
                unit.CurrentRow = rowIndex;
                unit.CurrentColumn = columnIndex;
                board.SetUnitByForce(unit);
            }
        }
    }
}
