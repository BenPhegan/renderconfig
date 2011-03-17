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
using System.IO;

namespace RenderConfig.Core
{
    /// <summary>
    /// Provides logging utilities to simplify log output.
    /// </summary>
    public class LogUtilities
    {
        /// <summary>
        /// Logs the RenderConfigConfig settings provided to the IRenderConfigLogger instance passed in.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="log">The log.</param>
        public static void LogSettings(RenderConfigConfig config, IRenderConfigLogger log)
        {
            log.LogMessage("--------------------------------------------------------");
            log.LogMessage(string.Concat("Config File = ".PadLeft(30) + config.ConfigFile));
            log.LogMessage(string.Concat("Configuration = ".PadLeft(30) + config.Configuration));
            log.LogMessage(string.Concat("Input Directory = ".PadLeft(30) + config.InputDirectory));
            log.LogMessage(string.Concat("Output Directory = ".PadLeft(30) + config.OutputDirectory));
            log.LogMessage(string.Concat("Delete Output Directory = ".PadLeft(30) + config.DeleteOutputDirectory));
            log.LogMessage(string.Concat("Break On No Match = ".PadLeft(30) + config.BreakOnNoMatch));
            log.LogMessage(string.Concat("Clean XML Output = ".PadLeft(30) + config.CleanOutput));
            log.LogMessage(string.Concat("Preserve Source Structure = ".PadLeft(30) + config.PreserveSourceStructure));
            log.LogMessage(string.Concat("Subdirectory per Config = ".PadLeft(30) + config.SubDirectoryEachConfiguration));
            log.LogMessage("--------------------------------------------------------");
        }
        
        /// <summary>
        /// Logs the target file header information via an IRenderConfigLogger.
        /// </summary>
        /// <param name="targetFile">The target file.</param>
        /// <param name="Log">The log.</param>
        public static void LogTargetFileHeader(string source, string destination, string sourceDir, string destinationDir, IRenderConfigLogger Log)
        {
            FileInfo destinationFile;
            FileInfo sourceFile = new FileInfo(source);
            if (string.IsNullOrEmpty(sourceDir))
            {
                sourceDir = string.Empty;
            }
            if (string.IsNullOrEmpty(destinationDir))
            {
                destinationDir = string.Empty;
            }

            if (String.IsNullOrEmpty(destination))
            {
                destinationFile = new FileInfo(source);
            }
            else
            {
                destinationFile = new FileInfo(destination);
            }
          
            Log.LogMessage(MessageImportance.High, "SOURCE = ".PadLeft(15) + Path.Combine(sourceDir,sourceFile.Name));
            Log.LogMessage("TARGET = ".PadLeft(15) + Path.Combine(destinationDir, destinationFile.Name));
        }

        /// <summary>
        /// Logs an int as a count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="log">The log.</param>
        public static void LogCount(int count, IRenderConfigLogger log)
        {
            if (count == 0)
            {
                log.LogMessage(MessageImportance.High,"COUNT = ".PadLeft(27) + count);
            }
            else
            {
                log.LogMessage("COUNT = ".PadLeft(27) + count);
            }
            
            log.LogMessage("");
        }

        /// <summary>
        /// Logs a key value pair as "Key = Value".
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="padding">The padding.</param>
        /// <param name="importance">The importance.</param>
        /// <param name="log">The log.</param>
        public static void LogKeyValue(string key, string value, int padding, MessageImportance importance, IRenderConfigLogger log)
        {
            log.LogMessage(importance, string.Concat(string.Concat(key, " = ").PadLeft(padding), value));
        }
    }
}
