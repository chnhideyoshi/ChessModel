using System;
using System.Collections.Generic;
using System.Text;

namespace RandomInput
{
    class Coordinate
    {
        public Coordinate(int x,int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 6)
            {
                Random r = new Random();
                List<Coordinate> list = GetPossibleList(args[5]);
                int choice = r.Next(0, list.Count - 1);
                Console.WriteLine(list[choice].ToString());
            }
            else
            {
                Console.WriteLine("error");
            }
        }
        private static List<Coordinate> GetPossibleList(string p)
        {
            List<Coordinate> list = new List<Coordinate>();
            if (!p.Contains("&"))
            {
                string[] s2 = p.Split('(', ',', ')');
                int rowIndex = int.Parse(s2[1]);
                int columnIndex = int.Parse(s2[2]);
                Coordinate co = new Coordinate(rowIndex, columnIndex);
                return list;
            }
            else
            {
                string[] s = p.Split('&');
                for (int i = 0; i < s.Length; i++)
                {
                    string[] s2 = s[i].Split('(', ',', ')');
                    int rowIndex = int.Parse(s2[1]);
                    int columnIndex = int.Parse(s2[2]);
                    Coordinate co = new Coordinate(rowIndex, columnIndex);
                    list.Add(co);
                }
                return list;
            }
        }
    }
}
