/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created: Tue Dec 27 17:09:35 2011
**      by: Qt User Interface Compiler version 4.7.4
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtGui/QAction>
#include <QtGui/QApplication>
#include <QtGui/QButtonGroup>
#include <QtGui/QComboBox>
#include <QtGui/QFrame>
#include <QtGui/QGraphicsView>
#include <QtGui/QGroupBox>
#include <QtGui/QHeaderView>
#include <QtGui/QLabel>
#include <QtGui/QMainWindow>
#include <QtGui/QPushButton>
#include <QtGui/QRadioButton>
#include <QtGui/QTextEdit>
#include <QtGui/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralWidget;
    QGroupBox *groupBox;
    QLabel *label;
    QLabel *label_2;
    QComboBox *CB_BlackAI;
    QRadioButton *RB_BlackManual;
    QRadioButton *RB_BlackAI;
    QComboBox *CB_WhiteAI;
    QRadioButton *RB_WhiteManual;
    QRadioButton *RB_WhiteAI;
    QLabel *label_3;
    QLabel *label_4;
    QFrame *line;
    QPushButton *BTN_Apply;
    QPushButton *BTN_Restart;
    QTextEdit *TB_Message;
    QLabel *LB_BlackCount;
    QLabel *LB_WhiteCount;
    QGraphicsView *CA_White;
    QGraphicsView *CA_Black;
    QGraphicsView *CA_Main;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QString::fromUtf8("MainWindow"));
        MainWindow->resize(467, 322);
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QString::fromUtf8("centralWidget"));
        groupBox = new QGroupBox(centralWidget);
        groupBox->setObjectName(QString::fromUtf8("groupBox"));
        groupBox->setGeometry(QRect(470, 10, 181, 261));
        label = new QLabel(groupBox);
        label->setObjectName(QString::fromUtf8("label"));
        label->setGeometry(QRect(10, 20, 41, 21));
        label_2 = new QLabel(groupBox);
        label_2->setObjectName(QString::fromUtf8("label_2"));
        label_2->setGeometry(QRect(10, 140, 41, 21));
        CB_BlackAI = new QComboBox(groupBox);
        CB_BlackAI->setObjectName(QString::fromUtf8("CB_BlackAI"));
        CB_BlackAI->setGeometry(QRect(10, 90, 121, 22));
        RB_BlackManual = new QRadioButton(groupBox);
        RB_BlackManual->setObjectName(QString::fromUtf8("RB_BlackManual"));
        RB_BlackManual->setGeometry(QRect(10, 50, 61, 16));
        RB_BlackAI = new QRadioButton(groupBox);
        RB_BlackAI->setObjectName(QString::fromUtf8("RB_BlackAI"));
        RB_BlackAI->setGeometry(QRect(90, 50, 41, 16));
        CB_WhiteAI = new QComboBox(groupBox);
        CB_WhiteAI->setObjectName(QString::fromUtf8("CB_WhiteAI"));
        CB_WhiteAI->setGeometry(QRect(10, 210, 121, 22));
        RB_WhiteManual = new QRadioButton(groupBox);
        RB_WhiteManual->setObjectName(QString::fromUtf8("RB_WhiteManual"));
        RB_WhiteManual->setGeometry(QRect(10, 170, 61, 16));
        RB_WhiteAI = new QRadioButton(groupBox);
        RB_WhiteAI->setObjectName(QString::fromUtf8("RB_WhiteAI"));
        RB_WhiteAI->setGeometry(QRect(80, 170, 41, 16));
        label_3 = new QLabel(groupBox);
        label_3->setObjectName(QString::fromUtf8("label_3"));
        label_3->setGeometry(QRect(10, 70, 61, 21));
        label_4 = new QLabel(groupBox);
        label_4->setObjectName(QString::fromUtf8("label_4"));
        label_4->setGeometry(QRect(10, 190, 61, 21));
        line = new QFrame(groupBox);
        line->setObjectName(QString::fromUtf8("line"));
        line->setGeometry(QRect(10, 120, 151, 16));
        line->setFrameShape(QFrame::HLine);
        line->setFrameShadow(QFrame::Sunken);
        BTN_Apply = new QPushButton(centralWidget);
        BTN_Apply->setObjectName(QString::fromUtf8("BTN_Apply"));
        BTN_Apply->setGeometry(QRect(530, 280, 61, 31));
        BTN_Restart = new QPushButton(centralWidget);
        BTN_Restart->setObjectName(QString::fromUtf8("BTN_Restart"));
        BTN_Restart->setGeometry(QRect(360, 280, 61, 31));
        TB_Message = new QTextEdit(centralWidget);
        TB_Message->setObjectName(QString::fromUtf8("TB_Message"));
        TB_Message->setGeometry(QRect(320, 110, 141, 161));
        TB_Message->setReadOnly(true);
        LB_BlackCount = new QLabel(centralWidget);
        LB_BlackCount->setObjectName(QString::fromUtf8("LB_BlackCount"));
        LB_BlackCount->setGeometry(QRect(390, 30, 31, 21));
        LB_WhiteCount = new QLabel(centralWidget);
        LB_WhiteCount->setObjectName(QString::fromUtf8("LB_WhiteCount"));
        LB_WhiteCount->setGeometry(QRect(390, 80, 31, 21));
        CA_White = new QGraphicsView(centralWidget);
        CA_White->setObjectName(QString::fromUtf8("CA_White"));
        CA_White->setGeometry(QRect(350, 70, 31, 31));
        QBrush brush(QColor(255, 234, 251, 255));
        brush.setStyle(Qt::SolidPattern);
        CA_White->setBackgroundBrush(brush);
        CA_Black = new QGraphicsView(centralWidget);
        CA_Black->setObjectName(QString::fromUtf8("CA_Black"));
        CA_Black->setGeometry(QRect(350, 20, 31, 31));
        CA_Black->setFrameShape(QFrame::NoFrame);
        QBrush brush1(QColor(0, 0, 0, 255));
        brush1.setStyle(Qt::SolidPattern);
        CA_Black->setBackgroundBrush(brush1);
        CA_Main = new QGraphicsView(centralWidget);
        CA_Main->setObjectName(QString::fromUtf8("CA_Main"));
        CA_Main->setGeometry(QRect(10, 10, 300, 300));
        CA_Main->setFrameShape(QFrame::NoFrame);
        MainWindow->setCentralWidget(centralWidget);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "MainWindow", 0, QApplication::UnicodeUTF8));
        groupBox->setTitle(QApplication::translate("MainWindow", "PlayerSetting", 0, QApplication::UnicodeUTF8));
        label->setText(QApplication::translate("MainWindow", "Black:", 0, QApplication::UnicodeUTF8));
        label_2->setText(QApplication::translate("MainWindow", "White:", 0, QApplication::UnicodeUTF8));
        RB_BlackManual->setText(QApplication::translate("MainWindow", "Manual", 0, QApplication::UnicodeUTF8));
        RB_BlackAI->setText(QApplication::translate("MainWindow", "AI", 0, QApplication::UnicodeUTF8));
        RB_WhiteManual->setText(QApplication::translate("MainWindow", "Manual", 0, QApplication::UnicodeUTF8));
        RB_WhiteAI->setText(QApplication::translate("MainWindow", "AI", 0, QApplication::UnicodeUTF8));
        label_3->setText(QApplication::translate("MainWindow", "ChooseAI:", 0, QApplication::UnicodeUTF8));
        label_4->setText(QApplication::translate("MainWindow", "ChooseAI:", 0, QApplication::UnicodeUTF8));
        BTN_Apply->setText(QApplication::translate("MainWindow", "Apply", 0, QApplication::UnicodeUTF8));
        BTN_Restart->setText(QApplication::translate("MainWindow", "Restart", 0, QApplication::UnicodeUTF8));
        LB_BlackCount->setText(QApplication::translate("MainWindow", "0", 0, QApplication::UnicodeUTF8));
        LB_WhiteCount->setText(QApplication::translate("MainWindow", "0", 0, QApplication::UnicodeUTF8));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
