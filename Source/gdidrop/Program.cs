using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CueSharp;

namespace gdidrop
{
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [STAThread]
        static void Main(string[] args)
        {
            Form1 form1 = new Form1();
            string[] commandLineArgs = Environment.GetCommandLineArgs()?.Skip(1).ToArray();
            if (!commandLineArgs.Any())
            {
                Console.WriteLine("Usage: gdidrop <path_to_cue_file>");
                FreeConsole();
                form1.ShowDialog();
            }
            else
            {
                string cueFilePath = commandLineArgs[0];

                if (!File.Exists(cueFilePath) || Path.GetExtension(cueFilePath).ToLowerInvariant() != ".cue")
                {
                    Console.WriteLine("Invalid cue file path.");
                    Console.WriteLine("Usage: gdidrop <path_to_cue_file>");
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("Starting conversion...");

                    Form1.ThreadDto threadData = new Form1.ThreadDto { cueFileInfo = new FileInfo(cueFilePath) };
                    try {
                        form1.DoConversion(threadData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Conversion failed.");
                        Console.WriteLine(ex.Message);
                        
                        Environment.Exit(2);
                    }

                    Console.WriteLine("Conversion completed.");
                    Environment.Exit(0);
                }

            }
        }
    }
}
