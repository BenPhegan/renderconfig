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
using System.Collections.Generic;
using System.Text;
using GraphSearch;

namespace RenderConfig.Core
{
    public class RenderConfigConfig : ICloneable
    {
        private string configFile;
        private string configuration;
        private string outputDirectory;
        private Boolean deleteOutputDirectory = true;
        private Boolean cleanOutput = false;
        private Boolean breakOnNoMatch = false;
        private string inputDirectory;
        private Boolean preserveSourceStructure;
        private Boolean subDirectoryEachConfiguration = true;
        private Boolean stampRenderData = false;


        /// <summary>
        /// Gets or sets a value indicating whether the user wants to stamp a comment into the rendered file.
        /// </summary>
        /// <value><c>true</c> if [stamp render data]; otherwise, <c>false</c>.</value>
        public Boolean StampRenderData
        {
            get { return stampRenderData; }
            set { stampRenderData = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether each configuration passed in should be output to a sub-drectory.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [sub directory each configuration]; otherwise, <c>false</c>.
        /// </value>
        public Boolean SubDirectoryEachConfiguration
        {
            get { return subDirectoryEachConfiguration; }
            set { subDirectoryEachConfiguration = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [preserve source structure] when copying files.
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
        /// Gets or sets the input directory.  This will rebase all files to this directory.
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
        public string ConfigFile
        {
            get { return configFile; }
            set { configFile = value; }
        }

        /// <summary>
        /// Gets or sets the configuration we want to output.  Must exist in the configuration file.
        /// </summary>
        /// <value>The configuration.</value>
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

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone(); ;
        }

        #endregion
    }
}
