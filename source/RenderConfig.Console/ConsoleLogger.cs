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
using RenderConfig.Core;

namespace RenderConfig.Console
{
    /// <summary>
    /// Provides a console logger that can be used as an IRenderConfigLogger
    /// </summary>
    public class ConsoleLogger : IRenderConfigLogger
    {
        #region IRenderConfigLogger Members

        public void LogMessage(string message)
        {
            System.Console.ResetColor();
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine(message); 
        }

        public void LogMessage(MessageImportance importance, string message)
        {
            System.Console.ResetColor();
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            if (importance == MessageImportance.High)
            {
                System.Console.ForegroundColor = ConsoleColor.Gray;
            }
            System.Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            System.Console.ResetColor();
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
        }

        public void LogError(MessageImportance importance, string message)
        {
            System.Console.ResetColor();
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
        }

        #endregion
    }
}
