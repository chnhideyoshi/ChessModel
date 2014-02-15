#include "thread.h"
#include "othellogamestrategy.h"

Thread::Thread()
{
    stopped=false;
    hasInput=false;
}

void Thread::stop()
{
    stopped=true;
}

void Thread::run()
{
    game.StartGame();
    emit ShowMap(game.CurrentChessBoard);
    while (!game.IsEndOfGame())
    {
        if (game.CurrentChessBoard.CanPutAnyUnit(Black))
        {
            PutUnit(game, Black);
            emit ShowMessageOnWorkingThread("Next To White");
        }
        else
        {
            emit  ShowMessageOnWorkingThread("Next To White NoInput");
        }
        game.NextPlayer();
        if (game.CurrentChessBoard.CanPutAnyUnit(White))
        {
            AutoPlay(White);
            PutUnit(game, White);
            emit ShowMessageOnWorkingThread("Next To Black");
        }
        else
        {
            emit ShowMessageOnWorkingThread("Next To Black NoInput");
        }
        game.NextPlayer();
    }
    game.EndGame();
    ShowMessageOnWorkingThread("Get End Winner:");
}

void Thread::GetInputData(const InputData data)
{
    inputdata=data;
    mutex.lock();
    hasInput=true;
    mutex.unlock();
}

void Thread::PutUnit(OthelloGame &game, UnitKind kind)
{
    while (true)
    {
        InputData data = ReceveiInputData();
        if (game.SetUnit(game.CurrentPlayer->Kind, data.X, data.Y))
        {
            emit ShowMap(game.CurrentChessBoard);
            break;
        }
        else
        {
            emit ShowMessageOnWorkingThread("InvalidInput");
        }
    }

}

InputData Thread::ReceveiInputData()
{
    while (!hasInput)
    {
        //QThread::sleep(1);
    }
    try
    {
        mutex.lock();
        hasInput = false;
        mutex.unlock();
    }
    catch(QString exception)
    {
        return inputdata;
    }
    return inputdata;
}

bool Thread::SendInput(InputData data)
{
    if (!hasInput)
    {
        inputdata=data;
        mutex.lock();  
        hasInput = true;
        mutex.unlock();
        return true;
    }
    else
    {
        return false;
    }
}

void Thread::InitGame()
{
    //game=*(new OthelloGame());

}
void Thread::AutoPlay(UnitKind kind)
{
    if (this->game.isStarted)
    {
        OthelloGameStrategy strategy(&(this->game.CurrentChessBoard),kind);
        Coodinate co=strategy.GetBestSolveByGreedyAlgorithm();
        InputData data(co.X, co.Y);
        this->SendInput(data);
        QThread::msleep(1000);
    }

}
