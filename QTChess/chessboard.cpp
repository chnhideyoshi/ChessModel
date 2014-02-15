#include "chessboard.h"

ChessBoard::ChessBoard()
{
    currentMap=new ChessUnit**[RowCount];
    for(int i=0;i<RowCount;i++)
    {
        currentMap[i]= new ChessUnit*[ColumnCount];
        for(int j=0;j<ColumnCount;j++)
        {
            currentMap[i][j]=0;
        }
    }
}

bool ChessBoard::HasCertainKindUnit(int rowIndex, int columnIndex, UnitKind kind)
{
    if (!HasUnit(rowIndex, columnIndex))
    {
        return false;
    }
    else
    {
        ChessUnit* unit = currentMap[rowIndex][columnIndex];
        if (unit->Kind==kind)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

bool ChessBoard::IsInBoardRange(int rowIndex, int columnIndex)
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

bool ChessBoard::HasUnit(int rowIndex, int columnIndex)
{
    if(!IsInBoardRange(rowIndex,columnIndex))
    {
        return false;
    }
    if(currentMap[rowIndex][columnIndex]!=0)
    {
        return true;
    }
    else
    {
        return false;
    }
}

void ChessBoard::SetUnitByForce(UnitKind kind,int rowIndex,int columnIndex)
{
    if (IsInBoardRange(rowIndex,columnIndex))
    {
        if(currentMap[rowIndex][columnIndex]!=0)
        {
            delete currentMap[rowIndex][columnIndex];
        }
        ChessUnit* unit=new ChessUnit(kind,rowIndex,columnIndex);
        currentMap[unit->CurrentRow][unit->CurrentColumn] = unit;
    }
}

int ChessBoard::GetCount(UnitKind kind) const
{
    int count=0;
    for(int i=0;i<RowCount;i++)
    {
        for(int j=0;j<ColumnCount;j++)
        {
            ChessUnit *ps=currentMap[i][j];
            if(ps!=0&&ps->Kind==kind)
            {
                count++;
            }
        }
    }
    return count;
}

void ChessBoard::ClearAll()
{
    for(int i=0;i<RowCount;i++)
    {
        for(int j=0;j<ColumnCount;j++)
        {
            ChessUnit *ps=currentMap[i][j];
            if(ps!=0)
            {
                delete ps;
            }
        }
    }
}

ChessUnit * ChessBoard::GetPositionState(int rowIndex, int columnIndex) const
{
    return currentMap[rowIndex][columnIndex];
}

