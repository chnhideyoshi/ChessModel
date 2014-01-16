using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLogicModels
{
    public class OthelloGameStrategy
    {
        public OthelloGameStrategy(OthelloChessBoard currentBoard,UnitKind kind)
        {
            CurrentBoard = currentBoard;
            if (currentBoard == null)
            {
                throw new Exception("!");
            }
            Kind = kind;
        }
        public UnitKind Kind { get; set; }
        public OthelloChessBoard CurrentBoard { get; set; }
        public Coordinate GetBestSolveByGreedyAlgorithm()
        {
            int max = -1;
            Coordinate co = new Coordinate(-1,-1);
            int[,] map = new int[ChessBoard.RowCount, ChessBoard.ColumnCount];
            for (int i = 0; i < ChessBoard.RowCount; i++)
            {
                for (int j = 0; j < ChessBoard.ColumnCount; j++)
                {
                    map[i, j] = GetUnitWeight(i,j);
                    if (max < map[i, j])
                    {
                        max=map[i,j];
                        co.X=i;
                        co.Y=j;
                    }
                }
            }
            if (max <= -1||(co.X==-1&&co.Y==-1))
            {
                return null;
            }
            else
            {
                return co;
            }
        }
        public Coordinate GetBestSolveWithoutWeight()
        {
            int max = -1;
            Coordinate co = new Coordinate(-1, -1);
            int[,] map = new int[ChessBoard.RowCount, ChessBoard.ColumnCount];
            for (int i = 0; i < ChessBoard.RowCount; i++)
            {
                for (int j = 0; j < ChessBoard.ColumnCount; j++)
                {
                    map[i, j] = EvenGetUnitWeight(i, j);
                    if (max < map[i, j])
                    {
                        max = map[i, j];
                        co.X = i;
                        co.Y = j;
                    }
                }
            }
            if (max <= -1 || (co.X == -1 && co.Y == -1))
            {
                return null;
            }
            else
            {
                return co;
            }
        }
        public int GetUnitWeight(int rowIndex, int columnIndex)
        {
            if (!CurrentBoard.CanPutAUnit(Kind,rowIndex, columnIndex))
            {
                return -1;
            }
            OthelloChessUnit unit = new OthelloChessUnit(Kind);
            unit.CurrentRow = rowIndex;
            unit.CurrentColumn = columnIndex;
            int weight = GetPositionWeight(rowIndex,columnIndex);
            List<OthelloChessUnit>[] changeList = CurrentBoard.GetChangeList(unit);
            for (int i = 0; i < 8; i++)
            {
                if (changeList[i].Count != 0)
                {
                    changeList[i].ForEach((unitObj) =>
                    {
                        weight += GetPositionWeight(unitObj.CurrentRow.Value,unitObj.CurrentColumn.Value);
                    });
                }
            }
            return weight;
        }
        public int EvenGetUnitWeight(int rowIndex, int columnIndex)
        {
            if (!CurrentBoard.CanPutAUnit(Kind, rowIndex, columnIndex))
            {
                return -1;
            }
            OthelloChessUnit unit = new OthelloChessUnit(Kind);
            unit.CurrentRow = rowIndex;
            unit.CurrentColumn = columnIndex;
            int weight = EvenGetPositionWeight(rowIndex, columnIndex);
            List<OthelloChessUnit>[] changeList = CurrentBoard.GetChangeList(unit);
            for (int i = 0; i < 8; i++)
            {
                if (changeList[i].Count != 0)
                {
                    changeList[i].ForEach((unitObj) =>
                    {
                        weight += EvenGetPositionWeight(unitObj.CurrentRow.Value, unitObj.CurrentColumn.Value);
                    });
                }
            }
            return weight;
        }
        public int GetPositionWeight(int x, int y)
        {
            if ((x == 0 && y == 0) || (x == OthelloChessBoard.RowCount - 1 && y == 0) || (x == 0 && y == ChessBoard.ColumnCount - 1) || (x == ChessBoard.RowCount - 1 && y == ChessBoard.ColumnCount - 1))
            {
                return 10000;
            }
            if (x == 0) { return 100; }
            if (y == 0) { return 100; }
            if (x == ChessBoard.RowCount-1) { return 100; }
            if (x == ChessBoard.ColumnCount - 1) { return 100; }
            return 1;
        }
        public int EvenGetPositionWeight(int x, int y)
        {
            return 1;
        }
    }
}
