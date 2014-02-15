#ifndef CHESSBOARD_H
#define CHESSBOARD_H
#include <QObject>
#include <QList>
enum UnitKind{White,Black};
struct Coodinate
{
public:
    Coodinate(int x=0,int y=0)
    {
        this->X=x;
        this->Y=y;
    }
    int X;
    int Y;
};

class ChessUnit
{
 public:
    ChessUnit(UnitKind kind,int rowIndex=-1,int columnIndex=-1)
    {
        Kind=kind;
        CurrentRow=rowIndex;
        CurrentColumn=columnIndex;
    }
    UnitKind Kind;
    int CurrentRow;
    int CurrentColumn;
    static Coodinate* GetCoordinate(ChessUnit* unit)
    {
        Coodinate* co=new Coodinate(unit->CurrentRow,unit->CurrentColumn);
        return co;
    }
};

class ChessPlayer
{
public :
    ChessPlayer(UnitKind kind)
    {
        this->Kind=kind;
    }
    UnitKind Kind;
};

class ChessBoard
{
public:
    const static int RowCount=10;
    const static int ColumnCount=10;
    explicit ChessBoard();
    static bool IsInBoardRange(int rowIndex, int columnIndex);
    bool HasUnit(int rowIndex, int columnIndex);
    bool HasCertainKindUnit(int rowIndex,int columnIndex,UnitKind kind);
    virtual void SetUnitByForce(UnitKind kind,int rowIndex,int columnIndex);
    ChessUnit* GetPositionState(int rowIndex,int columnIndex) const;
    int GetCount(UnitKind kind) const;
    void ClearAll();
protected:
    ChessUnit*** currentMap;
};












#endif // CHESSBOARD_H
