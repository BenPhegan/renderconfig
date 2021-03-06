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
using RenderConfig.Core;

namespace RenderConfig.MSBuild
{
    /// <summary>
    /// Provides the RenderConfig functionality to MSBuild as a plugin
    /// </summary>
    public class RenderConfig : Task
    {
        private string configFile;
        private string configuration;
        private string outputDirectory;
        private Boolean deleteOutputDirectory = true;
        private Boolean cleanOutput = false;
        private Boolean breakOnNoMatch = false;
        private string inputDirectory;
        private Boolean preserveSourceStructure = false;
		private Boolean subDirectoryEachConfiguration = false;

		/// <summary>
		/// Gets or sets a value indicating whether a subdirectory will be created for each configuration rendered. 
		/// </summary>
		public Boolean SubDirectoryEachConfiguration 
		{
			get {	return this.subDirectoryEachConfiguration;}
			set {	subDirectoryEachConfiguration = value;}
		}
		
		/// <summary>
        /// Gets or sets a value indicating whether [preserve source structure] when creating the target files.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [preserve source structure]; otherwise, <c>false</c>.
        /// </value>
        public Boolean PreserveSourceStructure
        {
            get { return preserveSourceStructure; }
            set { preserveSourceStructure = value; }
        }

        /// <summary>
        /// Gets or sets the input directory to use as the base for all relative source file paths.
        /// </summary>
        /// <value>The input directory.</value>
        public string InputDirectory
        {
            get { return inputDirectory; }
            set { inputDirectory = value; }
        }


        /// <summary>
        /// Gets or sets the config file to parse.
        /// </summary>
        /// <value>The config file.</value>
        [Required]
        public string ConfigFile
        {
            get { return configFile; }
            set { configFile = value; }
        }

        /// <summary>
        /// Gets or sets the configuration we want to output.  Must exist in the configuration file.
        /// </summary>
        /// <value>The configuration.</value>
        [Required]
        public string Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        /// <summary>
        /// Gets or sets the config output directory.
        /// </summary>
        /// <value>The config directory.</value>
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [delete output directory].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [delete output directory]; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteOutputDirectory
        {
            get { return deleteOutputDirectory; }
            set { deleteOutputDirectory = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to clean the output of the applied xml modifications.
        /// </summary>
        /// <value><c>true</c> if [clean ouptut]; otherwise, <c>false</c>.</value>
        public bool CleanOutput
        {
            get { return cleanOutput; }
            set { cleanOutput = value; }
        }

        /// <summary>
        /// Forces the RenderConfig task to fail if it does not find a match for a particular XPath change
        /// </summary>
        public bool BreakOnNoMatch
        {
            get { return breakOnNoMatch; }
            set { breakOnNoMatch = value; }
        }

        /// <summary>
        /// When overridden in a derived class, executes the task.
        /// </summary>
        /// <returns>
        /// true if the task successfully executed; otherwise, false.
        /// </returns>
        public override bool Execute()
        {
            RenderConfigConfig config = new RenderConfigConfig();
            config.BreakOnNoMatch = breakOnNoMatch;
            config.CleanOutput = cleanOutput;
            config.ConfigFile = configFile;
            config.Configuration = configuration;
            config.DeleteOutputDirectory = deleteOutputDirectory;
            config.OutputDirectory = outputDirectory;
            config.InputDirectory = inputDirectory;
			config.SubDirectoryEachConfiguration = SubDirectoryEachConfiguration;
			config.PreserveSourceStructure = PreserveSourceStructure;

            IRenderConfigLogger log = new MSBuildLogger(Log);
            
            Boolean returnVal = true;
            try
            {
                RenderConfigEngine.RunAllConfigurations(config, log);
            }
            catch (Exception i)
            {
                log.LogError("Failed to render configuration : " + i.Message);
                return false;
            }

            return returnVal;
           
        }
    }
}
