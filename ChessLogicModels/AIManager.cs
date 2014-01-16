using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using ChessLogicModels;

namespace ChessLogicModels
{
    public static class ProcessManager
    {
        private static string Start(string path, string args)
        {
            if (File.Exists(path))
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = path;
                p.EnableRaisingEvents = true;
                p.StartInfo.Arguments = args;
                p.Start();
                p.WaitForExit();
                // this.Dispatcher.BeginInvoke(new Action(() => { //textBox1.Text = }));
                string Result = p.StandardOutput.ReadToEnd();
                p.Close();
                return Result;
            }
            return "error";
        }
        public static Coordinate GetSolve(string path, string args)
        {
            try
            {
                string s = Start(path, args);
                if (s.Contains("error")) { return null; }
                string[] s2 = s.Split('(', ',', ')');
                int rowIndex = int.Parse(s2[1]);
                int columnIndex = int.Parse(s2[2]);
                return new Coordinate(rowIndex,columnIndex);
            }
            catch
            {
                return null;
            }
        }
    }

}
