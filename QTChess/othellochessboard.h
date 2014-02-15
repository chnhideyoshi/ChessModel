#ifndef OTHELLOCHESSBOARD_H
#define OTHELLOCHESSBOARD_H
#include "chessboard.h"
#include <QList>

enum Direction
{
    North = 0, NorthEast = 1, East = 2, SouthEast = 3, South = 4, SouthWest = 5, West = 6, NorthWest = 7
};

struct VectorUnit
{
public:
    VectorUnit(int x,int y)
    {
        X=x;
        Y=y;
    }
    int X;
    int Y;
    static VectorUnit ToVectorUnit(Direction direction)
    {
        VectorUnit  vector(0,0);
        switch(direction)
        {
            case North: { return VectorUnit(0,1); }
            case NorthEast: { return VectorUnit(1,1); }
            case East: { return  VectorUnit(1,0); }
            case SouthEast: { return  VectorUnit(1,-1) ; }
            case South: { return  VectorUnit(0,-1);  }
            case SouthWest: { return  VectorUnit(-1,-1) ; }
            case West: { return  VectorUnit(-1,0); }
            case NorthWest: { return  VectorUnit(-1,1);  }
        }
        return vector;
    }
    static int ToInt(Direction direction)
    {
        switch(direction)
        {
            case North: { return 0; }
            case NorthEast: { return 1; }
            case East: { return  2; }
            case SouthEast: { return  3; }
            case South: { return  4;  }
            case SouthWest: { return  5 ; }
            case West: { return  6; }
            case NorthWest: { return  7;  }
        }
        return -1;
    }
    static Direction ToDirection(int directionIndex)
    {
        switch(directionIndex)
        {
            case 0:{ return North;}
            case 1:{ return NorthEast;}
            case 2:{ return East;}
            case 3:{ return SouthEast;}
            case 4:{ return South;}
            case 5:{ return SouthWest;}
            case 6:{ return West;}
            case 7:{ return NorthWest;}
        }
        return North;
    }
};

class OthelloChessBoard:public ChessBoard
{
public:
    OthelloChessBoard();
    bool SetUnit(UnitKind kind,int rowIndex,int columnIndex);
    void InitStartGameBoard();
    bool CanPutAUnit(UnitKind kind,int rowIndex,int columnIndex);
    int GetPossiblePositionsCount(UnitKind kind) ;
    bool** GetPossiblePositions(UnitKind kind);
    bool CanPutAnyUnit(UnitKind kind);
    bool HasNoneNeibeOrNeibeIsInSameColor(UnitKind kind, int rowIndex,int columnIndex);
    QList<ChessUnit*>** GetChangeList(UnitKind kind,int rowIndex,int columnIndex);
private:
    bool** possiblePositions;
    QList<ChessUnit*>* changeList[8];
    ChessUnit* GetNextNeibe(ChessUnit* baseUnit,Direction direction);
    QList<ChessUnit*>* GetMidList(UnitKind kind,int rowIndex,int columnIndex,Direction direction);

};

#endif // OTHELLOCHESSBOARD_H
