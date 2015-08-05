using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace gpc
{
    public class Program
    {
        static void UnhandledEx(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.Error.WriteLine("Compiler Fatal Error: " + e.Message);
            Console.Error.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            Environment.ExitCode = -1;
            Environment.Exit(-1);
        }

        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledEx);
            CommandLine cmd = new CommandLine(args);

            if (cmd.Length < 2)
            {
                Console.WriteLine("");
                Console.WriteLine("GamePower Compiler v1.0");
                Console.WriteLine("");
                Console.WriteLine("Usage: gpc.exe -i <input.prg> -o <output.js>");
                Console.WriteLine("");
                Console.WriteLine("Notes: If the -o argument is not passed, the output file will be created");
                Console.WriteLine("in the caller folder with the .js extension.");
                return -1;
            }

            try
            {
                String filename = cmd["i"];
                FileInfo fi = new FileInfo(filename);
                String inFile = filename;
                String outFile = cmd.ContainsArg("o") ? cmd["o"] : String.Empty;
                GPCompiler Compiler = new GPCompiler();

                String[] lines;
                lines = File.ReadAllLines(filename, cmd.ContainsArg("msdos") ? Encoding.GetEncoding(437) : Encoding.Default);

                List<String> JSOutput = Compiler.Convert(lines.ToList());
                List<String> PreOutput = new List<String>();
                List<String> Output = new List<String>();

                Output.AddRange(PreOutput);
                Output.Add("");
                Output.AddRange(JSOutput);
                Output.Add("");
                Output.Add(Compiler.GPEngineVarName + ".Init(" + Compiler.ProgramProcess + ");");

                String Compiled = String.Join(Environment.NewLine, Output.ToArray());

                String OutputFilename = cmd.ContainsArg("o") ? cmd["o"] : fi.Directory.FullName + fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".js";
                using (StreamWriter sw = new StreamWriter(OutputFilename)) { sw.WriteLine(Compiled); }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return -1;
            }

            return 0;

        }
    }
}
