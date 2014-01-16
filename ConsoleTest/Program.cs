using System;
using System.Collections.Generic;
using System.Text;
namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessLogicModels.GameProcessor gp = new ChessLogicModels.GameProcessor();
            gp.BlackAutoPlay = true;
            gp.WhiteAutoPlay = true;
            gp.WhiteFilePath = "";
            gp.BlackFilePath = "";
            gp.ProcessorEventHappened += (kind, message) => 
            {
                switch (kind)
                {
                    case "UpdateUI": 
                        {
                            UpdateUI(message);
                        } break;
                    case "Message":
                        {
                            ShowMessage(message);
                        } break;
                    case "InputError":
                        {
                            AlertInputError(message);
                        } break;
                    default:break;
                }
            };
            gp.Run(null);
        }
        static void UpdateUI(string info)
        {
            Console.Clear();
            Console.Write("@");
            int rowCount=0;
            int columnCount=0;
            char[,] map = ResovleInfo(info,ref rowCount,ref columnCount);
            for (int j = 0; j < columnCount; j++)
            {
                Console.Write(j.ToString());
            }
            Console.Write("\n");
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = -1; j < columnCount; j++)
                {
                    if (j == -1)
                    {
                        Console.Write(i.ToString());
                        continue;
                    }
                    //ChessUnit chess = game.CurrentChessBoard.CurrentMap[i, j];
                    //if (chess != null)
                    //{
                    //    if (chess.Kind == UnitKind.Black)
                    //    {
                    //        Console.Write("B");
                    //    }
                    //    else
                    //    {
                    //        Console.Write("W");
                    //    }
                    //}
                    //else
                    //{
                    //    Console.Write("*");
                    //}
                    Console.Write(map[i,j]);
                }
                Console.Write("\n");
            }
        }

        static char[,] map;
        private static char[,] ResovleInfo(string info,ref int rowCount,ref int columnCount)
        {
            string[] args = info.Split(' ');
            rowCount = Convert.ToInt32(args[0]);
            columnCount = Convert.ToInt32(args[1]);
            if (map == null)
            {
                map = new char[rowCount, columnCount];
            }
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    map[i, j] = '*';
                }
            }
            string[] s = args[2].Split('&');
            InitMap(map, s, 'B');
            string[] s2 = args[3].Split('&');
            InitMap(map, s2, 'W');
            return map;
        }

        private static void InitMap(char[,] map, string[] s, char p)
        {
            for (int i = 0; i < s.Length; i++)
            {
                string[] s2 = s[i].Split('(', ',', ')');
                int rowIndex = int.Parse(s2[1]);
                int columnIndex = int.Parse(s2[2]);
                map[rowIndex, columnIndex] = p;
            }
        }

        static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        static void AlertInputError(string message)
        {
            Console.WriteLine(message);
         }
    }
}
