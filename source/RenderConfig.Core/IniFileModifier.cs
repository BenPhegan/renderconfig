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
using Nini.Config;

namespace RenderConfig.Core
{
    /// <summary>
    /// Runs changes into an ini-based configuration file.
    /// </summary>
    public class IniFileModifier : IFileModifier
    {
        Boolean returnCode;
        IniTargetFile file;
        string targetFile;
        IRenderConfigLogger log;
        Boolean breakOnNoMatch;


        /// <summary>
        /// Initializes a new instance of the <see cref="IniFileModifier"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="log">The log.</param>
        /// <param name="breakOnNoMatch">if set to <c>true</c> [break on no match].</param>
        public IniFileModifier(IniTargetFile file, string targetFile, IRenderConfigLogger log, Boolean breakOnNoMatch)
        {
            this.file = file;
            this.log = log;
            this.breakOnNoMatch = breakOnNoMatch;
            this.targetFile = targetFile;
        }

        /// <summary>
        /// Runs this modification.
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {

            //Add them in a specific order
            foreach (IniAdd mod in file.Add)
            {
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "ADD", 27, MessageImportance.High, log);
                LogModificationDetails(mod.section, mod.key, mod.Value, string.Empty, log);

                Add(mod);
            }

            foreach (IniUpdate mod in file.Update)
            {
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "Update", 27, MessageImportance.High, log);
                LogModificationDetails(mod.section, mod.key, mod.Value, string.Empty, log);
                Update(mod);
            }

            foreach (IniReplace mod in file.Replace)
            {
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "ADD", 27, MessageImportance.High, log);
                LogModificationDetails(string.Empty, string.Empty, mod.Value, mod.regex, log);
                LogUtilities.LogCount(RenderConfigEngine.ReplaceTokenInFile(mod.regex, mod.Value, targetFile), log);
            }

            foreach (IniDelete mod in file.Delete)
            {
                LogUtilities.LogKeyValue("TYPE", "DELETE", 27, MessageImportance.High, log);
                LogModificationDetails(mod.section, mod.key, string.Empty, string.Empty, log);
                Delete(mod);
            }

            //HACK BAd
            returnCode = true;
            return returnCode;
        }

        /// <summary>
        /// Runs any Add modifications.
        /// </summary>
        /// <param name="mod">The mod.</param>
        private void Add(IniAdd mod)
        {
            AddorUpdate(mod.section, mod.key, mod.Value, "Add");
        }

        /// <summary>
        /// Runs and Update modifications.
        /// </summary>
        /// <param name="mod">The mod.</param>
        private void Update(IniUpdate mod)
        {
            AddorUpdate(mod.section, mod.key, mod.Value, "Update");
        }

        /// <summary>
        /// Either Add or Update a particular section/key/value.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        private void AddorUpdate(string section, string key, string value, string type)
        {
            try
            {
                
                IConfigSource target = new IniConfigSource(targetFile);
                if (target.Configs[section] == null)
                {
                    target.AddConfig(section);
                }

                //If we want to break on no match, check
                if (breakOnNoMatch && type == "Update" && !target.Configs[section].Contains(key))
                {
                    throw new Exception("Could not match section");
                }
                target.Configs[section].Set(key, value);
                target.Save();

                LogCount(log);
            }
            catch (Exception inner)
            {
                throw new Exception("Failed to process " + type + " modification to Section/Key = " + section + "/" + key, inner);
            }
        }

        /// <summary>
        /// Logs the count of applied modifications.
        /// </summary>
        /// <param name="log">The log.</param>
        private void LogCount(IRenderConfigLogger log)
        {
            LogUtilities.LogCount(1, log);
        }

        /// <summary>
        /// Applies any Delete modifications
        /// </summary>
        /// <param name="mod">The mod.</param>
        private void Delete(IniDelete mod)
        {
            try
            {
                IConfigSource target = new IniConfigSource(targetFile);

                if (breakOnNoMatch && !target.Configs[mod.section].Contains(mod.key))
                {
                    throw new Exception("Could not match section");
                }

                target.Configs[mod.section].Remove(mod.key);
                target.Save();
                LogCount(log);
            }
            catch (Exception inner)
            {
                throw new Exception("Failed to process Delete modification to Section/Key = " + mod.section + "/" + mod.key, inner);
            }

        }

        /// <summary>
        /// Logs the modification details.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="log">The log.</param>
        private static void LogModificationDetails(string section, string key, string value, string regex, IRenderConfigLogger log)
        {
            if (String.IsNullOrEmpty(section))
            {
                LogUtilities.LogKeyValue("SECTION", section, 27, MessageImportance.Normal, log);
            }

            if (String.IsNullOrEmpty(key))
            {
                LogUtilities.LogKeyValue("KEY", key, 27, MessageImportance.Normal, log);
            }

            //Dont output if we are deleting a key, it just doesnt make sense
            if (!String.IsNullOrEmpty(value))
            {
                LogUtilities.LogKeyValue("VALUE", value.Trim(), 27, MessageImportance.Normal, log);
            }
        }
    }
}
