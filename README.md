ChessModel
==========

B-W Chess (Othello Chess)
黑白棋对战平台

任意一方可设置为手动或者AI。

AI为独立进程，命令行参数传递棋手和棋局信息，即时算出下一步走位。

AI.exe输入输出格式如下：

输入：

输入命令行参数用空格隔开，依次为：

<RowCount> <ColumnCount> <BlackPositions> <WhitePositions> <Kind> <PossibleList>

解释：

<棋盘行数> <棋盘列数> <当前棋盘黑棋位置> <当前棋盘白棋位置> <当前棋手> <可能放置位置列表>

例子：

10 10 (0,1)&(1,1)&(2,1) (2,2)&(1,2)&(2,3) White (1,3)&(2,3)

输出为下子位置：

例子：

(2,4)
