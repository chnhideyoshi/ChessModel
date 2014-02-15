#include <QtGui/QApplication>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
    qRegisterMetaType<ChessBoard>("ChessBoard");
    QApplication a(argc, argv);
    MainWindow w;
    w.setFixedSize(467,322);
    w.show();

    return a.exec();
}
