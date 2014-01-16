using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using ChessLogicModels;

namespace ChessModel
{
    public static class ProcessManager
    {
        private static string Start(string path, string args)
        {
            if (File.Exists(path))
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = path;
                p.EnableRaisingEvents = true;
                p.StartInfo.Arguments = args;
                p.Start();
                p.WaitForExit();
                // this.Dispatcher.BeginInvoke(new Action(() => { //textBox1.Text = }));
                string Result = p.StandardOutput.ReadToEnd();
                p.Close();
                return Result;
            }
            return "error";
        }
        public static Coordinate GetSolve(string path, string args)
        {
            try
            {
                string s = Start(path, args);
                if (s.Contains("error")) { return null; }
                string[] s2 = s.Split('(', ',', ')');
                int rowIndex = int.Parse(s2[1]);
                int columnIndex = int.Parse(s2[2]);
                return new Coordinate() { RowIndex=rowIndex,ColumnIndex=columnIndex};
            }
            catch
            {
                return null;
            }
        }
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
            string blackPositions = GetPositionsString(UnitKind.Black,currentBoard);
            builder.Append(blackPositions);
            builder.Append(" ");
            string whitePositions = GetPositionsString(UnitKind.White, currentBoard);
            builder.Append(whitePositions);
            builder.Append(" ");
            string currentPlayerKind = GetKindString(kind);
            builder.Append(currentPlayerKind);
            builder.Append(" ");
            string possiblePlaces = GetPossiblePosString(kind,currentBoard);
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
                builder.Append(coordinates[i].RowIndex);
                builder.Append(",");
                builder.Append(coordinates[i].ColumnIndex);
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
            List<Coordinate> list=new List<Coordinate>();
            for (int i = 0; i < ChessBoard.RowCount; i++)
            {
                for (int j = 0; j < ChessBoard.ColumnCount; j++)
                {
                    if (map[i, j])
                    {
                        Coordinate co = new Coordinate();
                        co.RowIndex=i;
                        co.ColumnIndex=j;
                        list.Add(co);
                    }
                }
            }
            return list;
        }
        private static string GetPositionsString(UnitKind unitKind,OthelloChessBoard board)
        {
            List<ChessUnit> coordinates = GetCoordinates(unitKind, board);
            StringBuilder builder=new StringBuilder();
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
