using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLogicModels
{
    public class ChessBoard
    {
        public static readonly int RowCount = 10;
        public static readonly int ColumnCount = 10;
        public ChessBoard()
        {
            currentMap = new ChessUnit[RowCount, ColumnCount];
        }
        protected List<ChessUnit> currentChessUnitCollection = new List<ChessUnit>();
        protected ChessUnit[,] currentMap;
        public ChessUnit[,] CurrentMap
        {
            get { return currentMap; }
        }
        public List<ChessUnit> CurrentChessUnitCollection
        {
            get { return currentChessUnitCollection; }
            set { currentChessUnitCollection = value; }
        }
        public static bool IsInBoardRange(ChessUnit unit)
        {
            if (unit.CurrentColumn.HasValue && unit.CurrentRow.HasValue)
            {
                if (unit.CurrentColumn.Value >= 0
                    && unit.CurrentColumn.Value <= ColumnCount - 1
                    && unit.CurrentRow.Value >= 0
                    && unit.CurrentRow.Value <= RowCount - 1)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsInBoardRange(int rowIndex, int columnIndex)
        {
            if (columnIndex >= 0
                && columnIndex <= ColumnCount - 1
                && rowIndex >= 0
                && rowIndex <= RowCount - 1)
            {
                return true;
            }
            return false;
        }
        public bool HasUnit(int rowIndex, int columnIndex)
        {
            if (currentMap[rowIndex, columnIndex] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HasCertainKindUnit(int rowIndex,int columnIndex,UnitKind kind)
        {
            if (!HasUnit(rowIndex, columnIndex))
            {
                return false;
            }
            else
            {
                ChessUnit unit = currentMap[rowIndex, columnIndex];
                if (unit.Kind == kind)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public virtual bool SetUnit(ChessUnit unit)
        {
            if (IsInBoardRange(unit))
            {
                currentChessUnitCollection.Add(unit);
                currentMap[unit.CurrentRow.Value, unit.CurrentColumn.Value] = unit;
                return true;
            }
            else
            {
                return false;
            }

        }
        public virtual void SetUnitByForce(ChessUnit unit)
        {
            if (IsInBoardRange(unit))
            {
                currentChessUnitCollection.Add(unit);
                currentMap[unit.CurrentRow.Value, unit.CurrentColumn.Value] = unit;
            }
            else
            {
                throw new Exception();
            }
        }
        public int GetSum(UnitKind kind)
        {
            int count = currentChessUnitCollection.FindAll(new Predicate<ChessUnit>((obj) => { return obj.Kind == kind; })).Count;
            return count;
        }
    }
    public interface ChessUnit
    {
        int? CurrentRow { get; set; }
        int? CurrentColumn { get; set; }
        UnitKind Kind { get; set; }
    }
    public enum UnitKind
    {
        Black=1,White=2
    }

}
