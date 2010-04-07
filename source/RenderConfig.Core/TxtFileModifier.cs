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

namespace RenderConfig.Core
{
    /// <summary>
    /// Provides a class with all the required functionality to handle modifying text files
    /// </summary>
    public class TxtFileModifier : IFileModifier
    {
        TxtTargetFile file;
        IRenderConfigLogger log;
        string targetFile;
        Boolean breakOnNoMatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtFileModifier"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="log">The log.</param>
        /// <param name="breakOnNoMatch">if set to <c>true</c> [break on no match].</param>
        public TxtFileModifier(TxtTargetFile file, string targetFile, IRenderConfigLogger log, Boolean breakOnNoMatch)
        {
            this.log = log;
            this.file = file;
            this.targetFile = targetFile;
            this.breakOnNoMatch = breakOnNoMatch;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
			int count = 0;
            foreach (IniReplace mod in file.Replace)
            {
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "REPLACE", 27, MessageImportance.High, log);
                LogUtilities.LogKeyValue("REGEX", mod.regex, 27, MessageImportance.Normal, log);
                LogUtilities.LogKeyValue("VALUE", mod.Value, 27, MessageImportance.Normal, log);
				count = RenderConfigEngine.ReplaceTokenInFile(mod.regex, mod.Value, targetFile);
                LogUtilities.LogCount(count,log);
            }
            //TODO
			if (breakOnNoMatch && count == 0)
			{
				return false;
			}
			else
			{
	            return true;				
			}
        }
    }
}
