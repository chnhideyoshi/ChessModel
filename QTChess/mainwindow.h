#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QLabel>
#include <QComboBox>
#include <QGraphicsView>
#include <QTextEdit>
#include <QRadioButton>
#include "thread.h"

namespace Ui {
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();
    void InitGraphicsView();
    void InitMembers();
    void AddUnit(bool isBlack,int rowIndex,int columnIndex);
    void RemoveUnit(int rowIndex,int columnIndex);
protected:
    bool eventFilter(QObject *target, QEvent *event);
private slots:
    void on_BTN_Restart_clicked();
    void on_BTN_Apply_clicked();
    void UpdateAll(const ChessBoard& chessboard);
    void ShowMessage(const QString &message);
private:
    Ui::MainWindow *ui;
    QGraphicsScene *gs;
    const int ColumnCount;
    const int RowCount;
    QColor blackColor;
    QColor lineColor;
    QColor whiteColor;
    QGraphicsRectItem* (*rectMap)[10] ;

    Thread *thread;
};

#endif // MAINWINDOW_H
