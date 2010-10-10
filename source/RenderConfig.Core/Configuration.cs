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
using RenderConfig.Core.Interfaces;

namespace RenderConfig.Core
{
    /// <summary>
    /// Provides additional functionality to the Configuration class, the primary functional class used to apply a set of modifications.
    /// </summary>
    public partial class Configuration
    {
        private IRenderConfigLogger log;

        /// <summary>
        /// Applies the specified config by passing through each set of modifications (in order, deterministic).
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="inputLog">The input log.</param>
        /// <returns></returns>
        public Boolean Apply(RenderConfigConfig config, IRenderConfigLogger inputLog)
        {
            log = inputLog;
            Boolean returnCode = true;

            foreach (XmlTargetFile file in TargetFiles.XML)
            {
                CheckAndModifySourceAndDestination(config, file);
                //If we arent doing anything else, we are doing a straight copy...
                if (file.Add == null & file.Delete == null && file.Update == null && file.Replace == null)
                {
                    log.LogMessage(MessageImportance.High, "TYPE = ".PadLeft(27) + "substitute");
                    RenderConfigEngine.Substitute(file, config.OutputDirectory);
                }
                else
                {
                    IFileModifier fileModifier = new XmlFileModifier(file, RenderConfigEngine.ResolveAndCopyDestinationFilename(file, config.OutputDirectory, true), log, config);
                    fileModifier.Run();
                }
            }

            foreach (IniTargetFile file in TargetFiles.INI)
            {
                CheckAndModifySourceAndDestination(config, file);
                //If we arent doing anything else, we are doing a straight copy...
                //TODO possibly add all these checks as a boolean get{} on the partial class..so if (file.IsSimpleCopy)
                if (file.Add == null & file.Delete == null && file.Update == null && file.Replace == null)
                {
                    log.LogMessage(MessageImportance.High, "TYPE = ".PadLeft(27) + "substitute");
                    RenderConfigEngine.Substitute(file, config.OutputDirectory);
                }
                else
                {
                    IFileModifier fileModifier = new IniFileModifier(file, RenderConfigEngine.ResolveAndCopyDestinationFilename(file, config.OutputDirectory, true), log, config);
                    fileModifier.Run();
                }
            }

            foreach (TxtTargetFile file in TargetFiles.TXT)
            {
                CheckAndModifySourceAndDestination(config, file);
                //If we arent doing anything else, we are doing a straight copy...
                if (file.Replace == null)
                {
                    log.LogMessage(MessageImportance.High, "TYPE = ".PadLeft(27) + "substitute");
                    RenderConfigEngine.Substitute(file, config.OutputDirectory);
                }
                else
                {
                    IFileModifier fileModifier = new TxtFileModifier(file, RenderConfigEngine.ResolveAndCopyDestinationFilename(file, config.OutputDirectory, true), log, config);
                    fileModifier.Run();
                }
            }

            return returnCode;
        }

		static string CleansePlatformSpecificDirectoryMarkers(string filename)
		{
			char right = Path.DirectorySeparatorChar;
			char wrong;
			string returnString = string.Empty;
			
			if (right == '\\')
			{
				wrong = '/';
			}
			else
			{
				wrong = '\\';
			}
			
			filename = filename.Replace(wrong.ToString(), right.ToString());
			returnString = RemoveAdjacentDuplicateCharacters(filename, right);
			
			return returnString;
			
		}
			    
		static string RemoveAdjacentDuplicateCharacters(string text, char character)
		{
			string returnString = string.Empty;
			for (int x = 0; x < text.Length; x++)
			{
				if (x+1 < text.Length)
				{
					if (text[x] == character && text[x+1] == character)
					{
						x = x + 1;
					}
				}
				returnString = string.Concat(returnString, text[x]);
			}
						
			return returnString;		
		}
		
        /// <summary>
        /// Check and modify source and destination file based on input and output directory existence, and a set of configuration values
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="file">The file.</param>
        private void CheckAndModifySourceAndDestination(RenderConfigConfig config, ITargetFile file)
        {
            //Check to see if we want to preserve the directory structure....
            file.source = CleansePlatformSpecificDirectoryMarkers(file.source);
			FileInfo t = new FileInfo(file.source);
            if (config.PreserveSourceStructure)
            {
                 if (file.destination == null)
                {
                    file.destination = file.source;
                }
                else
                {
					//TODO This is broken
					if (file.source.IndexOf(t.Name) != Path.DirectorySeparatorChar)
					{
						char badSep = file.source[file.source.IndexOf(t.Name)];
						file.source.Replace(badSep,Path.DirectorySeparatorChar);
					}
                    file.destination = Path.Combine(file.source.Replace(t.Name, string.Empty), file.destination);
                }
            }

            //Now check to see if the input directory has been provided, and modify the file object to represent the change of relative path if so
            if (config.InputDirectory != null)
            {
                file.source = Path.Combine(config.InputDirectory, file.source);
            }
			else
			{
				file.source = t.FullName;
			}

            LogUtilities.LogTargetFileHeader(file.source, file.destination, config.InputDirectory,config.OutputDirectory, log);

            //Check if it exists now...
            if (!File.Exists(file.source))
            {
                throw new Exception("Could not find source file " + file.source);
            }
        }
    }
}
