using System;
using System.Collections.Generic;
using System.Text;

namespace ChessLogicModels
{
    public class OthelloChessBoard : ChessBoard
    {
        public event UnitChangedEventHandler UnitChanged;
        public OthelloChessBoard()
        {
           
        }
        public override bool SetUnit(ChessUnit unit)
        {
            if (!IsInBoardRange(unit)) { return false; }
            if (HasUnit(unit.CurrentRow.Value, unit.CurrentColumn.Value)) { return false; }
            bool canEat = false;
            List<OthelloChessUnit>[] changeList = GetChangeList((OthelloChessUnit)unit);
            for (int i = 0; i < 8; i++)
            {
                if (changeList[i].Count != 0)
                {
                    canEat = true;
                    changeList[i].ForEach((unitObj) => 
                    {
                        unitObj.Kind = unit.Kind;
                        if (UnitChanged != null)
                        {
                            UnitChanged(unit,ChangeReason.Update);
                        }
                    });
                }
            }
            if (canEat)
            {
                currentChessUnitCollection.Add(unit);
                CurrentMap[unit.CurrentRow.Value, unit.CurrentColumn.Value] = unit;
                if (UnitChanged != null)
                {
                    UnitChanged(unit, ChangeReason.Add);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void SetUnitByForce(ChessUnit unit)
        {
            if (IsInBoardRange(unit))
            {
                currentChessUnitCollection.Add(unit);
                currentMap[unit.CurrentRow.Value, unit.CurrentColumn.Value] = unit;
                if (UnitChanged != null)
                {
                    UnitChanged(unit, ChangeReason.Force);
                }
            }
        }
        private List<OthelloChessUnit> GetMidList(OthelloChessUnit baseUnit, Direction direction)
        {
            List<OthelloChessUnit> list = new List<OthelloChessUnit>();
            OthelloChessUnit unit = GetNextNeibe(baseUnit, direction);
            while (unit!=null&&unit.Kind != baseUnit.Kind)
            {
                list.Add(unit);
                unit = GetNextNeibe(unit,direction);
            }
            if (unit == null)
            {
                list.Clear();
                return list;
            }
            else
            {
                return list;
            }
           
        }
        private OthelloChessUnit GetNextNeibe(OthelloChessUnit baseUnit, Direction direction)
        {
            try
            {
                VectorUnit deltaVector = VectorUnit.ToVectorUnit(direction);
                if (baseUnit.CurrentRow.Value + deltaVector.X < 0 || baseUnit.CurrentRow.Value + deltaVector.X>OthelloChessBoard.RowCount-1)
                    return null;
                if (baseUnit.CurrentColumn.Value + deltaVector.Y < 0 || baseUnit.CurrentColumn.Value + deltaVector.Y > OthelloChessBoard.ColumnCount - 1)
                    return null;
                return (OthelloChessUnit)CurrentMap[baseUnit.CurrentRow.Value + deltaVector.X, baseUnit.CurrentColumn.Value + deltaVector.Y];
            }
            catch { return null; }
        }
        public List<OthelloChessUnit>[] GetChangeList(OthelloChessUnit unit)
        {
            List<OthelloChessUnit>[] list = new List<OthelloChessUnit>[8];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = GetMidList(unit,VectorUnit.ToDirection(i));
            }
            return list;
        }
        public bool[,] GetPossiblePositions(UnitKind kind)
        {
            bool[,] map = new bool[OthelloChessBoard.RowCount, OthelloChessBoard.ColumnCount];
            for (int i = 0; i < OthelloChessBoard.RowCount; i++)
            {
                for (int j = 0; j < OthelloChessBoard.ColumnCount; j++)
                {
                    map[i, j] = CanPutAUnit(kind, i, j);
                }
            }
            return map;
        }
        public bool CanPutAUnit(UnitKind kind, int rowIndex, int columnIndex)
        {
            if (!IsInBoardRange(rowIndex,columnIndex)) { return false; }
            if (HasUnit(rowIndex, columnIndex)) { return false; }
            if (HasNoneNeibeOrNeibeIsInSameColor(kind, rowIndex, columnIndex)) { return false; }
            bool canEat = false;
            OthelloChessUnit unit = new OthelloChessUnit(kind);
            unit.CurrentRow = rowIndex;
            unit.CurrentColumn = columnIndex;
            List<OthelloChessUnit>[] changeList = GetChangeList(unit);
            for (int i = 0; i < 8; i++)
            {
                if (changeList[i].Count != 0)
                {
                    canEat = true;
                }
            }
            return canEat;
        }
        public bool HasNoneNeibeOrNeibeIsInSameColor(UnitKind kind, int rowIndex, int columnIndex)
        {
            bool north = true, northeast = true, east = true, southeast = true, south = true, southwest = true, west = true, northwest = true;
            
            if (rowIndex != 0)
                north = currentMap[rowIndex - 1, columnIndex] == null || currentMap[rowIndex - 1, columnIndex].Kind == kind;
            else
                north = true;

            if (rowIndex != 0&&columnIndex!=ChessBoard.ColumnCount-1)
                northeast = currentMap[rowIndex - 1, columnIndex + 1] == null || currentMap[rowIndex - 1, columnIndex + 1].Kind == kind;
            else
                northeast = true;

            if (columnIndex != ChessBoard.ColumnCount-1)
                east = currentMap[rowIndex, columnIndex+1] == null || currentMap[rowIndex, columnIndex+1].Kind == kind;
            else
                east = true;

            if (rowIndex != ChessBoard.RowCount-1&&columnIndex!=ChessBoard.ColumnCount-1)
                southeast = currentMap[rowIndex + 1, columnIndex+1] == null || currentMap[rowIndex + 1, columnIndex+1].Kind == kind;
            else
                southeast = true;

            if (rowIndex != ChessBoard.RowCount-1)
                south = currentMap[rowIndex + 1, columnIndex] == null || currentMap[rowIndex + 1, columnIndex].Kind == kind;
            else
                south = true;

            if (rowIndex != ChessBoard.RowCount-1&&columnIndex!=0)
                southwest = currentMap[rowIndex + 1, columnIndex-1] == null || currentMap[rowIndex + 1, columnIndex-1].Kind == kind;
            else
                southwest = true;

            if (columnIndex != 0)
                west = currentMap[rowIndex, columnIndex-1] == null || currentMap[rowIndex, columnIndex-1].Kind == kind;
            else
                west = true;

            if (rowIndex != 0&&columnIndex!=0)
                northwest = currentMap[rowIndex - 1, columnIndex-1] == null || currentMap[rowIndex - 1, columnIndex-1].Kind == kind;
            else
                northwest = true;

            return north&&northeast&&east&&southeast&&south&&southwest&&west&&northwest;
        }
        public int GetPossiblePositionsCount(UnitKind kind)
        {
            int count = 0;
            bool[,] map = GetPossiblePositions(kind);
            for (int i = 0; i < OthelloChessBoard.RowCount; i++)
            {
                for (int j = 0; j < OthelloChessBoard.ColumnCount; j++)
                {
                    if (map[i, j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public bool CanPutAnyUnit(UnitKind kind)
        {
            return GetPossiblePositionsCount(kind) > 0;
        }
    }
    public class Coordinate
    {
        public Coordinate(int x,int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
        public Coordinate Add(Direction direction)
        {
            VectorUnit vector = VectorUnit.ToVectorUnit(direction);
            Coordinate co  =new Coordinate(this.X+vector.X,this.Y+vector.Y);
            return co;
        }
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
    public struct VectorUnit
    {
        public int X { get; set; }
        public int Y { get; set; }
        public static VectorUnit ToVectorUnit(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: { return new VectorUnit() { X = 0, Y = 1 }; }
                case Direction.NorthEast: { return new VectorUnit() { X = 1, Y = 1 }; }
                case Direction.East: { return new VectorUnit() { X = 1, Y = 0 }; }
                case Direction.SouthEast: { return new VectorUnit() { X = 1, Y = -1 }; }
                case Direction.South: { return new VectorUnit() { X = 0, Y = -1 }; }
                case Direction.SouthWest: { return new VectorUnit() { X = -1, Y = -1 }; }
                case Direction.West: { return new VectorUnit() { X = -1, Y = 0 }; }
                case Direction.NorthWest: { return new VectorUnit() { X = -1, Y = 1 }; }
                default: throw new Exception("direction error");
            }
            throw new Exception("direction error");
        }
        public static Direction ToDirection(int directionIndex)
        {
            switch (directionIndex)
            {
                case 0: { return Direction.North; }
                case 1: { return Direction.NorthEast; }
                case 2: { return Direction.East; }
                case 3: { return Direction.SouthEast; }
                case 4: { return Direction.South; }
                case 5: { return Direction.SouthWest; }
                case 6: { return Direction.West; }
                case 7: { return Direction.NorthWest; }
            }
            throw new Exception("direction error");
        }
    }
    public enum Direction
    {
        North = 0, NorthEast = 1, East = 2, SouthEast = 3, South = 4, SouthWest = 5, West = 6, NorthWest = 7
    }
    public enum ChangeReason
    {
        Add=1,Update=2,Force=3
    }
    public class OthelloChessUnit : ChessUnit
    {
        public OthelloChessUnit(UnitKind kind)
        {
            this.kind = kind;
        }
        public OthelloChessUnit(int rowIndex,int columnIndex,UnitKind kind)
        {
            this.kind = kind;
            CurrentRow = rowIndex;
            CurrentColumn = columnIndex;
        }
        public  int? CurrentRow { get; set; }
        public  int? CurrentColumn { get; set; }
        private UnitKind kind=default(UnitKind);
        public UnitKind Kind
        {
            get { return kind; }
            set
            {
                if (value == UnitKind.Black || value == UnitKind.White)
                {
                    if (kind != value)
                    {
                        kind = value;
                    }
                }
            }
        }
        
    }
    public delegate void UnitChangedEventHandler(ChessUnit unit,ChangeReason reason);

    public class OthelloGame
    {
        public OthelloGame()
        {
           
        }
        private void InitBoard()
        {
            if (currentChessBoard != null)
            {
                currentChessBoard.SetUnitByForce(new OthelloChessUnit(ChessBoard.RowCount / 2 - 1, ChessBoard.ColumnCount / 2 - 1, UnitKind.Black));
                currentChessBoard.SetUnitByForce(new OthelloChessUnit(ChessBoard.RowCount / 2, ChessBoard.ColumnCount / 2 - 1, UnitKind.White));
                currentChessBoard.SetUnitByForce(new OthelloChessUnit(ChessBoard.RowCount / 2, ChessBoard.ColumnCount / 2, UnitKind.Black));
                currentChessBoard.SetUnitByForce(new OthelloChessUnit(ChessBoard.RowCount / 2 - 1, ChessBoard.ColumnCount / 2, UnitKind.White));
            }
        }
        public ChessPlayer GetPlayerByColor(UnitKind kind)
        {
            if(Player1.Color==kind)
            {
                return Player1;
            }
            return Player2;
        }
        private bool isStarted = false;
        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }
        public void StartGame()
        {
            InitBoard();
            IsStarted = true;
        }

        private OthelloChessBoard currentChessBoard = null;
        public OthelloChessBoard CurrentChessBoard
        {
            get { return currentChessBoard; }
            set
            {
                currentChessBoard = value;
            }
        }

        public int SumOfBlackUnit 
        {
            get { return CurrentChessBoard.GetSum(UnitKind.Black); }
        }
        public int SumOfWhiteUnit
        {
            get { return CurrentChessBoard.GetSum(UnitKind.White); }
        }

        public ChessPlayer Player1{get;set;}
        public ChessPlayer Player2{get;set;}

        public ChessPlayer CurrentPlayer{get;set;}
        public void NextPlayer()
        {
            if (CurrentPlayer == Player1)
            {
                CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
            }
        }
        public void EndGame()
        {
            isStarted = false;
            CurrentPlayer = null;
        }
        public bool IsEndOfGame()
        {
            if (SumOfBlackUnit == OthelloChessBoard.RowCount * OthelloChessBoard.ColumnCount)
            {
                return true;
            }
            if (SumOfWhiteUnit == OthelloChessBoard.RowCount * OthelloChessBoard.ColumnCount)
            {
                return true;
            }
            if (SumOfBlackUnit + SumOfWhiteUnit == OthelloChessBoard.RowCount * OthelloChessBoard.ColumnCount)
            {
                return true;
            }
            if (SumOfBlackUnit == 0&&SumOfWhiteUnit!=0) { return true; }
            if (SumOfWhiteUnit == 0 && SumOfBlackUnit != 0) { return true; }
            return false;
        }
        public ChessPlayer GetWinner()
        {
            if (this.IsEndOfGame())
            {
                if (SumOfWhiteUnit >= SumOfBlackUnit)
                {
                    return GetPlayerByColor(UnitKind.White);
                }
                else
                {
                    return GetPlayerByColor(UnitKind.Black);
                }
            }
            else
            {
                return null;
            }
        }
        public OthelloChessUnit GetUnit(UnitKind kind)
        {
            return new OthelloChessUnit(kind);
        }

        public bool SetUnit(UnitKind kind,int rowIndex,int columnIndex)
        {
            OthelloChessUnit unit = GetUnit(kind);
            unit.CurrentRow = rowIndex;
            unit.CurrentColumn = columnIndex;
            return CurrentChessBoard.SetUnit(unit);
        }
        
    }
    public class ChessPlayer
    {
        public ChessPlayer(UnitKind kind)
        {
            Color = kind;
        }
        public UnitKind Color { get; set; }
    }
}
