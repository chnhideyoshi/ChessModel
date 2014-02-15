#ifndef THREAD_H
#define THREAD_H
#include "othellogame.h"
#include <QThread>
#include <QMutex>

struct InputData
{
public:
    InputData(int x=-1,int y=-1)
    {
        X=x;
        Y=y;
    }
    int X;
    int Y;
};
class Thread : public QThread
{
    Q_OBJECT
public:
    explicit Thread();
    void stop();
    void InitGame();
    bool SendInput(InputData data);
    void AutoPlay(UnitKind kind);
    OthelloGame game;
protected:
    void run();
private:
    void PutUnit(OthelloGame &game,UnitKind kind);
    InputData ReceveiInputData();
    volatile bool stopped;
    QMutex mutex;
    bool hasInput;
    InputData inputdata;
signals:
    void ShowMap(const ChessBoard &currentChessBoard);
    void ShowMessageOnWorkingThread(const QString& message);
private  slots:
    void GetInputData(const InputData data);

};

#endif // THREAD_H
