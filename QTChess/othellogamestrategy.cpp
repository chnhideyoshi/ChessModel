#include "othellogamestrategy.h"


Coodinate OthelloGameStrategy::GetBestSolveByGreedyAlgorithm()
{
    int max = -1;
    Coodinate co(-1,-1);
    int map[10][10];
    for (int i = 0; i < ChessBoard::RowCount; i++)
    {
        for (int j = 0; j < ChessBoard::ColumnCount; j++)
        {
            map[i][j] = GetUnitWeight(i,j);
            if (max < map[i][j])
            {
                max=map[i][j];
                co.X=i;
                co.Y=j;
            }
        }
    }
    if (max <= -1||(co.X==-1&&co.Y==-1))
    {
        return 0;
    }
    else
    {
        return co;
    }
}

int OthelloGameStrategy::GetUnitWeight(int rowIndex, int columnIndex)
{
    if (!CurrentBoard->CanPutAUnit(Kind,rowIndex, columnIndex))
    {
        return -1;
    }
    int weight = GetPositionWeight(rowIndex,columnIndex);
    QList<ChessUnit*>** changeList = CurrentBoard->GetChangeList(Kind,rowIndex,columnIndex);
    for (int i = 0; i < 8; i++)
    {
        if (changeList[i]->count() != 0)
        {
            for(int j=0;j<changeList[i]->count();j++)
            {
                ChessUnit* unitObj=changeList[i]->at(j);
                weight += GetPositionWeight(unitObj->CurrentRow,unitObj->CurrentColumn);
            }

        }
    }
    return weight;
}

int OthelloGameStrategy::GetPositionWeight(int x, int y)
{
    if ((x == 0 && y == 0) || (x == ChessBoard::RowCount - 1 && y == 0) || (x == 0 && y == ChessBoard::ColumnCount - 1) || (x == ChessBoard::RowCount - 1 && y == ChessBoard::ColumnCount - 1))
    {
        return 10000;
    }
    if (x == 0) { return 100; }
    if (y == 0) { return 100; }
    if (x ==ChessBoard::RowCount-1) { return 100; }
    if (x == ChessBoard::ColumnCount - 1) { return 100; }
    return 1;
}
