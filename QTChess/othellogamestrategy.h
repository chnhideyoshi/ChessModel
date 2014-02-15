#ifndef OTHELLOGAMESTRATEGY_H
#define OTHELLOGAMESTRATEGY_H
#include "othellochessboard.h"
class OthelloGameStrategy
{
public:
    OthelloGameStrategy(OthelloChessBoard* currentBoard,UnitKind kind)
    {
        Kind=kind;
        CurrentBoard=currentBoard;
    }
    OthelloChessBoard* CurrentBoard;
    UnitKind Kind;
    Coodinate GetBestSolveByGreedyAlgorithm();
    int GetUnitWeight(int rowIndex, int columnIndex);
    int GetPositionWeight(int x, int y);
};

#endif // OTHELLOGAMESTRATEGY_H
