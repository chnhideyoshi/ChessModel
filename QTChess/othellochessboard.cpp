#include "othellochessboard.h"
#include <QList>
OthelloChessBoard::OthelloChessBoard()
{
    //changeList=new QList<ChessUnit*>[8];
    for(int i=0;i<8;i++)
    {
        QList<ChessUnit*> *list=new QList<ChessUnit*>();
        changeList[i]=list;
    }
    possiblePositions=new bool*[RowCount];
    for(int i=0;i<RowCount;i++)
    {
        possiblePositions[i]=new bool[ColumnCount];
        for(int j=0;j<ColumnCount;j++)
        {
            possiblePositions[i][j]=false;
        }
    }
}

bool OthelloChessBoard::SetUnit(UnitKind kind,int rowIndex,int columnIndex)
{
    if (!IsInBoardRange(rowIndex,columnIndex)) { return false; }
    if (HasUnit(rowIndex, columnIndex)) { return false; }
    if (HasNoneNeibeOrNeibeIsInSameColor(kind, rowIndex, columnIndex)) { return false; }
    bool canEat = false;
    QList<ChessUnit*>** tempchangeList = GetChangeList(kind,rowIndex,columnIndex);
    for (int i = 0; i < 8; i++)
    {
        if (tempchangeList[i]->count() != 0)
        {
            canEat = true;
            for(int j=0;j<tempchangeList[i]->count();j++)
            {
                (tempchangeList[i])->at(j)->Kind=kind;
            }
            tempchangeList[i]->clear();
        }
    }
    if (canEat)
    {
        ChessUnit* unit=new ChessUnit(kind,rowIndex,columnIndex);
        currentMap[rowIndex][columnIndex] = unit;
        return true;
    }
    else
    {
        return false;
    }
}

void OthelloChessBoard::InitStartGameBoard()
{
    SetUnitByForce(Black,RowCount / 2 - 1,ColumnCount / 2 - 1);
    SetUnitByForce(White,RowCount / 2, ColumnCount / 2 - 1);
    SetUnitByForce(Black,RowCount / 2,ColumnCount / 2);
    SetUnitByForce(White,RowCount / 2 - 1, ColumnCount / 2);
}

bool OthelloChessBoard::CanPutAUnit(UnitKind kind, int rowIndex, int columnIndex)
{
    if (!IsInBoardRange(rowIndex,columnIndex)) { return false; }
    if (HasUnit(rowIndex, columnIndex)) { return false; }
    if (HasNoneNeibeOrNeibeIsInSameColor(kind, rowIndex, columnIndex)) { return false; }
    bool canEat = false;
    QList<ChessUnit*>** tempchangeList = GetChangeList(kind,rowIndex,columnIndex);
    for (int i = 0; i < 8; i++)
    {
        if (tempchangeList[i]->count() != 0)
        {
            canEat = true;
            tempchangeList[i]->clear();
        }
    }
    return canEat;
}

int OthelloChessBoard::GetPossiblePositionsCount(UnitKind kind)
{
    int count = 0;
    bool** map = GetPossiblePositions(kind);
    for (int i = 0; i < RowCount; i++)
    {
        for (int j = 0; j < ColumnCount; j++)
        {
            if (map[i][j])
            {
                count++;
            }
        }
    }
    return count;
}

bool ** OthelloChessBoard::GetPossiblePositions(UnitKind kind)
{
    bool** map=possiblePositions;
    for (int i = 0; i < RowCount; i++)
    {
        for (int j = 0; j < ColumnCount; j++)
        {
            map[i][j] = CanPutAUnit(kind, i, j);
        }
    }
    return map;
}

bool OthelloChessBoard::CanPutAnyUnit(UnitKind kind)
{
    return GetPossiblePositionsCount(kind) > 0;
}

ChessUnit * OthelloChessBoard::GetNextNeibe(ChessUnit *baseUnit,Direction direction)
{
    VectorUnit deltaVector = VectorUnit::ToVectorUnit(direction);
    if (baseUnit->CurrentRow+ deltaVector.X < 0 || baseUnit->CurrentRow + deltaVector.X>RowCount-1)
        return 0;
    if (baseUnit->CurrentColumn + deltaVector.Y < 0 || baseUnit->CurrentColumn + deltaVector.Y > ColumnCount - 1)
        return 0;
    return currentMap[baseUnit->CurrentRow + deltaVector.X][baseUnit->CurrentColumn + deltaVector.Y];
}

QList<ChessUnit *>* OthelloChessBoard::GetMidList(UnitKind kind, int rowIndex, int columnIndex, Direction direction)
{
    QList<ChessUnit*> *list =changeList[direction];
    list->clear();
    ChessUnit baseUnit(kind,rowIndex,columnIndex);
    ChessUnit* unit = GetNextNeibe(&baseUnit, direction);
    while (unit!=0&&unit->Kind != kind)
    {
        list->append(unit);
        unit = GetNextNeibe(unit,direction);
    }
    if (unit == 0)
    {
        list->clear();
        return list;
    }
    else
    {
        return list;
    }
}

QList<ChessUnit *> ** OthelloChessBoard::GetChangeList(UnitKind kind, int rowIndex, int columnIndex)
{
    for (int i = 0; i < 8; i++)
    {
        changeList[i]=GetMidList(kind,rowIndex,columnIndex,VectorUnit::ToDirection(i));
    }
    return changeList;
}

bool OthelloChessBoard::HasNoneNeibeOrNeibeIsInSameColor(UnitKind kind, int rowIndex, int columnIndex)
{
    bool north = true, northeast = true, east = true, southeast = true, south = true, southwest = true, west = true, northwest = true;

    if (rowIndex != 0)
        north = currentMap[rowIndex - 1][ columnIndex] == 0 || currentMap[rowIndex - 1][ columnIndex]->Kind == kind;
    else
        north = true;

    if (rowIndex != 0&&columnIndex!=ColumnCount-1)
        northeast = currentMap[rowIndex - 1][columnIndex + 1] == 0 || currentMap[rowIndex - 1][columnIndex + 1]->Kind== kind;
    else
        northeast = true;

    if (columnIndex != ColumnCount-1)
        east = currentMap[rowIndex][columnIndex+1] == 0 || currentMap[rowIndex][columnIndex+1]->Kind == kind;
    else
        east = true;

    if (rowIndex !=RowCount-1&&columnIndex!=ColumnCount-1)
        southeast = currentMap[rowIndex + 1][columnIndex+1] == 0 || currentMap[rowIndex + 1][ columnIndex+1]->Kind == kind;
    else
        southeast = true;

    if (rowIndex != RowCount-1)
        south = currentMap[rowIndex + 1][ columnIndex] == 0 || currentMap[rowIndex + 1][columnIndex]->Kind == kind;
    else
        south = true;

    if (rowIndex != RowCount-1&&columnIndex!=0)
        southwest = currentMap[rowIndex + 1][ columnIndex-1] == 0 || currentMap[rowIndex + 1][ columnIndex-1]->Kind == kind;
    else
        southwest = true;

    if (columnIndex != 0)
        west = currentMap[rowIndex][columnIndex-1] == 0 || currentMap[rowIndex][ columnIndex-1]->Kind == kind;
    else
        west = true;

    if (rowIndex != 0&&columnIndex!=0)
        northwest = currentMap[rowIndex - 1][columnIndex-1] == 0 || currentMap[rowIndex - 1][ columnIndex-1]->Kind == kind;
    else
        northwest = true;

    return north&&northeast&&east&&southeast&&south&&southwest&&west&&northwest;
}
