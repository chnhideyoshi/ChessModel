#ifndef CHESSUNIT_H
#define CHESSUNIT_H

class ChessUnit
{
public:
    ChessUnit();
};
class Coordinate
{
public:
    Coordinate(int x=0,int y=0)
    {
        X=x;
        Y=y;
    }
    int X;
    int Y;
};

#endif // CHESSUNIT_H
