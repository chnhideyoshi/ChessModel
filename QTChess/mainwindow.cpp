#include "mainwindow.h"
#include "ui_mainwindow.h"
#include  <qmessagebox.h>
#include <QMouseEvent>
#include "thread.h"


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    ColumnCount(10),
    RowCount(10),
    rectMap(new QGraphicsRectItem*[10][10])
{
    ui->setupUi(this);
    thread=0;

    InitMembers();
    InitGraphicsView();
}

void MainWindow::InitGraphicsView()
{
    //**************************
    double height=ui->CA_Main->height();
    double width=ui->CA_Main->width();
    gs = new QGraphicsScene(0,0,width,height);
    ui->CA_Main->setScene(gs);
    int count=10;
    for(int i=0;i<count+1;i++)
    {
        gs->addLine(0,i*(height/count),width,i*(height/count),QPen(lineColor,2));
        gs->addLine(i*(width/count),height,i*(width/count),0,QPen(lineColor,2));
    }
    ui->CA_Main->setBackgroundBrush(QBrush(QColor(255,206,135)));
    ui->CA_Main->show();
    ui->CA_Main->installEventFilter(this);
    //***************************
    QGraphicsScene *sceneBlack=new QGraphicsScene(0,0,ui->CA_Black->width(),ui->CA_Black->height());
    ui->CA_Black->setScene(sceneBlack);
    ui->CA_Black->setBackgroundBrush(QBrush(blackColor));
    ui->CA_Black->show();
}
void MainWindow::InitMembers()
{
    blackColor=QColor(0,0,0,210);
    whiteColor=QColor(255,255,255);
    lineColor=QColor(0,0,0,255);
    for(int i=0;i<RowCount;i++)
    {
        for(int j=0;j<ColumnCount;j++)
        {
            rectMap[i][j]=0;
        }
    }
}

bool  MainWindow::eventFilter(QObject *target, QEvent *event)
{
    if(target==ui->CA_Main&&event->type()==QEvent::MouseButtonPress)
    {
        QMouseEvent *mouseEvent = (QMouseEvent *)event;
        int x=mouseEvent->x();
        int y=mouseEvent->y();
        int rowIndex=(int)(x/30);
        int columnIndex=(int)(y/30);
        if(rectMap[rowIndex][columnIndex]==0)
        {
            if(thread->isRunning()&&thread->game.isStarted)
            {
                InputData data(rowIndex, columnIndex);
                thread->SendInput(data);
            }
        }
        else
        {
            ShowMessage("Invalid Input");
        }
    }
    return QMainWindow::eventFilter(target,event);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::AddUnit(bool isBlack,int rowIndex,int columnIndex)
{
    if(isBlack)
    {
        QRectF rec(rowIndex*30,columnIndex*30,29,29);
        QGraphicsRectItem *prec;
         prec=gs->addRect(rec,QPen(),QBrush(blackColor));
         rectMap[rowIndex][columnIndex]=prec;
    }
    else
    {
           QRectF rec(rowIndex*30,columnIndex*30,29,29);
           QGraphicsRectItem *prec;
           prec=gs->addRect(rec,QPen(),QBrush(whiteColor));
           rectMap[rowIndex][columnIndex]=prec;
    }
}
void MainWindow::RemoveUnit(int rowIndex,int columnIndex)
{
    if(rectMap[rowIndex][columnIndex]!=0)
    {
        QGraphicsRectItem* prec=rectMap[rowIndex][columnIndex];
        gs->removeItem((QGraphicsItem*)prec);
        rectMap[rowIndex][columnIndex]=0;
    }
}
void MainWindow::UpdateAll(const ChessBoard &chessBoard)
{
    ui->LB_BlackCount->setText(QString::number(chessBoard.GetCount(Black)));
    ui->LB_WhiteCount->setText(QString::number(chessBoard.GetCount(White)));
    for(int i=0;i<RowCount;i++)
    {
        for(int j=0;j<ColumnCount;j++)
        {
            ChessUnit* unit= chessBoard.GetPositionState(i,j);
            if(unit!=0)
            {
                RemoveUnit(i,j);
                bool isblack=true;
                if(unit->Kind==White)
                {
                    isblack=false;
                }
                AddUnit(isblack,i,j);
            }
            else
            {
                if(rectMap[i][j]!=0)
                {
                    RemoveUnit(i,j);
                }
            }
        }
    }
}


void MainWindow::on_BTN_Restart_clicked()
{
    if(thread!=0)
    {
        thread->game.EndGame();
        thread->stop();
        thread->terminate();
    }
    thread=new Thread();
    connect(thread,SIGNAL(ShowMessageOnWorkingThread(QString)),this,SLOT(ShowMessage(QString)),Qt::BlockingQueuedConnection);
    connect(thread,SIGNAL(ShowMap(const ChessBoard&)),this,SLOT(UpdateAll(const ChessBoard&)),Qt::BlockingQueuedConnection);
    thread->InitGame();
    thread->start();
}

void MainWindow::on_BTN_Apply_clicked()
{

}

void MainWindow::ShowMessage(const QString &message)
{
    ui->TB_Message->setText(message);
}


