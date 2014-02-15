#include "othellogame.h"
#include"othellochessboard.h"
OthelloGame::OthelloGame()
    :Player1(Black),Player2(White)
{
}

ChessPlayer * OthelloGame::GetPlayerByColor(UnitKind kind)
{
    if(Player1.Kind==kind)
    {
        return &Player1;
    }
    return &Player2;
}

void OthelloGame::StartGame()
{
    CurrentChessBoard.InitStartGameBoard();
    CurrentPlayer=&Player1;
    isStarted = true;
}

int OthelloGame::GetSumByColor(UnitKind kind)
{
    return CurrentChessBoard.GetCount(kind);
}

void OthelloGame::NextPlayer()
{
    if (CurrentPlayer == &Player1)
    {
        CurrentPlayer = &Player2;
    }
    else
    {
        CurrentPlayer = &Player1;
    }
}

bool OthelloGame::IsEndOfGame()
{
    if (GetSumByColor(Black) == OthelloChessBoard::RowCount * OthelloChessBoard::ColumnCount)
    {
        return true;
    }
    if (GetSumByColor(White)== OthelloChessBoard::RowCount * OthelloChessBoard::ColumnCount)
    {
        return true;
    }
    if (GetSumByColor(Black) + GetSumByColor(White) == OthelloChessBoard::RowCount * OthelloChessBoard::ColumnCount)
    {
        return true;
    }
    if (GetSumByColor(Black) == 0&&GetSumByColor(White)!=0) { return true; }
    if (GetSumByColor(White) == 0 && GetSumByColor(Black) != 0) { return true; }
    return false;
}

void OthelloGame::EndGame()
{
    isStarted = false;
    CurrentPlayer = 0;
}

ChessPlayer * OthelloGame::GetWinner()
{
    if (IsEndOfGame())
    {
        if (GetSumByColor(White) >= GetSumByColor(Black))
        {
            return GetPlayerByColor(White);
        }
        else
        {
            return GetPlayerByColor(Black);
        }
    }
    else
    {
        return 0;
    }
}

bool OthelloGame::SetUnit(UnitKind kind, int rowIndex, int columnIndex)
{
     return CurrentChessBoard.SetUnit(kind,rowIndex,columnIndex);
}

