#ifndef OTHELLOGAME_H
#define OTHELLOGAME_H
#include "othellochessboard.h"
class OthelloGame
{
public:
    OthelloGame();
    void StartGame();
    ChessPlayer* GetPlayerByColor(UnitKind kind);
    int GetSumByColor(UnitKind kind);
    void NextPlayer();
    bool IsEndOfGame();
    void EndGame();
    ChessPlayer* GetWinner();
    bool SetUnit(UnitKind kind,int rowIndex,int columnIndex);
    bool isStarted;
    ChessPlayer Player1;
    ChessPlayer Player2;
    ChessPlayer *CurrentPlayer;
    OthelloChessBoard CurrentChessBoard;

};

#endif // OTHELLOGAME_H
