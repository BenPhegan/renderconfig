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
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace RenderConfig.MSBuild
{
    /// <summary>
    /// Provides an MSBuildLogger that can be passed as an IRenderConfigLogger
    /// </summary>
    class MSBuildLogger : Core.IRenderConfigLogger
    {
        TaskLoggingHelper logger;
        #region IRenderConfigLogger Members

        public MSBuildLogger(TaskLoggingHelper log)
        {
            logger = log;
        }

        public void LogMessage(string message)
        {
            logger.LogMessage(message);
            
        }

        public void LogMessage(Core.MessageImportance importance, string message)
        {
            Microsoft.Build.Framework.MessageImportance imp = (Microsoft.Build.Framework.MessageImportance)Enum.ToObject(typeof(MessageImportance), (int)importance);
            logger.LogMessage(imp, message);
        }

        public void LogError(string message)
        {
            logger.LogError(message);
        }

        public void LogError(Core.MessageImportance importance, string message)
        {
            logger.LogError(message);
        }

        #endregion
    }
}
