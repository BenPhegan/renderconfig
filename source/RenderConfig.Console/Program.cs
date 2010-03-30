//   Copyright (c) 2010 Ben Phegan

//   Permission is hereby granted, free of charge, to any person
//   obtaining a copy of this software and associated documentation
//   files (the "Software"), to deal in the Software without
//   restriction, including without limitation the rights to use,
//   copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the
//   Software is furnished to do so, subject to the following
//   conditions:

//   The above copyright notice and this permission notice shall be
//   included in all copies or substantial portions of the Software.

//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//   EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//   OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//   NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//   HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//   WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//   FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.


using System;
using NDesk.Options;
using RenderConfig.Core;

namespace RenderConfig.Console
{
    /// <summary>
    /// Provides the console application entry point.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main application entry point.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            Boolean showHelp = false;
            //Boolean verbose = false;
            RenderConfigConfig config = new RenderConfigConfig();
            config.DeleteOutputDirectory = false;
            config.CleanOutput = false;
            config.BreakOnNoMatch = true;
            config.PreserveSourceStructure = false;

            OptionSet options = null;

            try
            {
                options = new OptionSet()
                    //.Add("v|verbose", "Not yet implemented", delegate(string v) { if (v != null) verbose = true; })
                .Add("?|h|help", "Show this message and exit", delegate(string v) { if (v != null) showHelp = true; })
                .Add("f=|file=", "File containing configuration", delegate(string v) { if (v != null) { config.ConfigFile = v; } else { throw new OptionException("Missing Configuration File", "f|file"); } })
                .Add("c=|configuration=", "Target configuration", delegate(string v) { if (v != null) config.Configuration = v; else throw new OptionException("Missing Configuration", "c|configuration"); })
                .Add("o=|output=", "Target output directory", delegate(string v) { if (v != null) { config.OutputDirectory = v; } else { throw new OptionException("Missing Output Directory", "o|output"); } })
                .Add("i=|input=", "The input directory to use as the base for all relative source file paths", delegate(string v) { if (v != null) config.InputDirectory = v; })
                .Add("d|deleteoutput", "Delete target output directory", delegate(string v) { if (v != null) config.DeleteOutputDirectory = true; })
                .Add("l|clean", "Clean XML output files", delegate(string v) { if (v != null) config.CleanOutput = true; })
                .Add("b|break", "Break on no match", delegate(string v) { if (v != null) config.BreakOnNoMatch = true; })
                .Add("p|preserve", "Preserve source directory structure when outputting", delegate(string v) { if (v != null) config.PreserveSourceStructure = true; });

                options.Parse(args);
            }
            catch (OptionException e)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine();
                options.WriteOptionDescriptions(System.Console.Out);
                System.Console.ResetColor();
                System.Environment.Exit(1);
            }
            if (showHelp || args.Length == 0)
            {
                System.Console.WriteLine("RenderConfig.exe ");
                System.Console.WriteLine("Renders stuff");
                options.WriteOptionDescriptions(System.Console.Out);
                System.Console.ResetColor();
                System.Environment.Exit(1);
            }

            //HACK This is not right, shouldnt NDesk.Options be hanlding this shit????
            if (config.OutputDirectory == null)
            {
                OutputArgumentError(options, "Please provide an output directory");
            }

            if (config.Configuration == null)
            {
                OutputArgumentError(options, "Please provide a target configuration");
            }

            if (config.ConfigFile == null)
            {
                OutputArgumentError(options, "Please provide a config file to parse");
            }
            try
            {
                IRenderConfigLogger log = new ConsoleLogger();
                RenderConfigEngine engine = new RenderConfigEngine(config, log);
                engine.Render();
            }
            catch (Exception i)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Encountered error: " + i.Message);
                System.Console.WriteLine();
            }

            System.Console.ResetColor();

        }

        /// <summary>
        /// Outputs any argument error.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="message">The message.</param>
        private static void OutputArgumentError(OptionSet options, string message)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(message);
            System.Console.WriteLine();
            options.WriteOptionDescriptions(System.Console.Out);
            System.Console.ResetColor();
            System.Environment.Exit(1);
        }
    }
}
