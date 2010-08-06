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
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using GraphSearch;
using RenderConfig.Core.Interfaces;

namespace RenderConfig.Core
{

    /// <summary>
    /// Runs a set of configurations against a set of source files.
    /// </summary>
    public class RenderConfigEngine
    {
        private Stack<Node<string>> dependencyStack;
        private Stack<Node<string>> variableStack;
        private List<Node<string>> nodeList;
        IRenderConfigLogger log;
        RenderConfigConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderConfigEngine"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="log">The IRenderConfigLogger to use.</param>
        public RenderConfigEngine(RenderConfigConfig config, IRenderConfigLogger log)
        {
            this.log = log;
            this.config = config;
        }

        /// <summary>
        /// Renders the configuration specified.
        /// </summary>
        /// <returns></returns>
        public bool Render()
        {
            LogUtilities.LogSettings(config,log);
            nodeList = new List<Node<string>>();
            Boolean returnCode = true;
            if (File.Exists(config.ConfigFile))
            {
                log.LogMessage("Reading in configuration file...");
                RenderConfig renderConfig = ReadConfigurationFile();


                if (renderConfig != null)
                {
                    //HACK We should be using a deep copy so that all this becomes "variableStack = dependencyStack.Clone();"
                    //Get the list of nodes
                    nodeList = GenerateNodeList(renderConfig, log);
                    if (SearchForNodeByIdentity(nodeList, config.Configuration) == null)
                    {
                        throw new ApplicationException("Could not find Configuration : " + config.Configuration);
                    }
                    List<Node<string>> n2 = GenerateNodeList(renderConfig, log);
                    //Generate the dependency path
                    DepthFirstSearch<string> dfs = new DepthFirstSearch<string>(nodeList);
                    DepthFirstSearch<string> dfs2 = new DepthFirstSearch<string>(n2);
                    dependencyStack = dfs.GetDependencyPath(config.Configuration);
                    //HACK Need to write a deep copy Queue Clone to get rid of this...
                    variableStack = dfs2.GetDependencyPath(config.Configuration);

                }
                if (Directory.Exists(config.OutputDirectory))
                {
                    if (config.DeleteOutputDirectory)
                    {
                        log.LogMessage("Deleting and recreating output directory...");
                        Directory.Delete(config.OutputDirectory, true);
                        Directory.CreateDirectory(config.OutputDirectory);
                    }
                }
                else
                {
                    log.LogMessage("Creating output directory...");
                    Directory.CreateDirectory(config.OutputDirectory);
                }


                //Create a queue of mods to run and a queue of EnvironmentVariables to implement
                //Variables have to be put in place before the mods...
                log.LogMessage("Building dependency queue...");
                Queue<Configuration> configsToRun = CreateConfigProcessQueue(renderConfig, dependencyStack);
                //HACK This is ugly, needs a deep copy here....
                Queue<Configuration> envQueue = CreateConfigProcessQueue(renderConfig, variableStack);

                while (envQueue.Count > 0)
                {
                    Configuration varConfig = envQueue.Dequeue();

                    //First, we need to get all the Variables and create them.

                    foreach (EnvironmentVariable variable in varConfig.EnvironmentVariables)
                    {
                        Environment.SetEnvironmentVariable(variable.variable, variable.Value);
                    }
                }

                while (configsToRun.Count > 0)
                {
                    Configuration currentConfig = configsToRun.Dequeue();
                    log.LogMessage(MessageImportance.High, "Running modification: " + currentConfig.Name);

                    if (currentConfig.TargetFiles != null)
                    {
                        if (!currentConfig.Apply(config, log))
                        {
                            log.LogError("Failed to apply configuration: " + currentConfig.Name);
                            returnCode = false;
                        }
                    }
                }

            }
            else
            {
                log.LogError("Could not find configuration file: " + config.ConfigFile);
                returnCode = false;
            }

            //Let 'em know
            if (returnCode)
            {
                log.LogMessage(MessageImportance.High, "Configuration rendered!");
            }
            else
            {
                log.LogError("Failed to render configuration.");
            }

            return returnCode;
        }

        /// <summary>
        /// Creates the config process stack.
        /// </summary>
        /// <param name="configs">The configs.</param>
        /// <param name="dependencyStack">The dependency stack.</param>
        /// <returns></returns>
        private Queue<Configuration> CreateConfigProcessQueue(RenderConfig renderConfig, Stack<Node<string>> dependencyStack)
        {
            Queue<Configuration> configQueue = new Queue<Configuration>();
            while (dependencyStack.Count > 0)
            {
                Node<string> node = dependencyStack.Pop();
                foreach (Configuration c in renderConfig.Configurations)
                {
                    if (c.Name == node.Identity)
                    {
                        configQueue.Enqueue(c);
                    }
                }
            }
            return configQueue;
        }

        /// <summary>
        /// Generates the node list.
        /// </summary>
        /// <param name="configs">The configs.</param>
        private static List<Node<string>> GenerateNodeList(RenderConfig renderConfig, IRenderConfigLogger log)
        {
            //Create list of all nodes, without dependencies
            log.LogMessage("Generating Node List...");
            List<Node<string>> nodes = CreateInitialNodeList(renderConfig);

            foreach (Configuration config in renderConfig.Configurations)
            {
                //Split the dependencies, and then for each of them find a node in nodeList, and add as a dependency
                if (!String.IsNullOrEmpty(config.Depends))
                {
                    string[] dependencies = config.Depends.Split(',', ';');
                    foreach (string depends in dependencies)
                    {
                        Node<string> dependencyNode = SearchForNodeByIdentity(nodes, depends.Trim());
                        if (dependencyNode == null)
                        {
                            throw new ApplicationException("Error in configuration, cannot resolve dependency " + depends);
                        }
                        else
                        {
                            SearchForNodeByIdentity(nodes, config.Name).Dependencies.Add(dependencyNode);
                        }
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// Creates the initial node list.
        /// </summary>
        /// <param name="configs">The configs.</param>
        private static List<Node<string>> CreateInitialNodeList(RenderConfig renderConfig)
        {
            List<Node<string>> nodes = new List<Node<string>>();
            foreach (Configuration config in renderConfig.Configurations)
            {
                if (SearchForNodeByIdentity(nodes, config.Name) != null)
                {
                    throw new ApplicationException("Configuration exists in file twice");
                }
                else
                {
                    Node<string> node = new Node<string>(config.Name);
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Searches for node by identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        private static Node<string> SearchForNodeByIdentity(List<Node<string>> nodeList, string identity)
        {
            foreach (Node<string> node in nodeList)
            {
                if (node.Identity == identity)
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Reads the configuration file.
        /// </summary>
        /// <returns></returns>
        private RenderConfig ReadConfigurationFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RenderConfig));
            
            RenderConfig configs;

            using (FileStream stream = new FileStream(config.ConfigFile, FileMode.Open))
            {
                //Set up some settings to allow DTD Entity includes
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.None;
                settings.ProhibitDtd = false;
                settings.Schemas.Add(GetXmlSchemaSet());

                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    try
                    {
                        configs = (RenderConfig)serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException iv)
                    {
                        throw new InvalidOperationException("Could not deserialise config file, please check against XSD", iv);
                    }
                    catch (Exception i)
                    {
                        throw new ApplicationException("Could not deserialise config file, please check against XSD", i);
                    }
                }
                stream.Close();
            }



            if (configs.Includes != null)
            {
                if (configs.Includes.Count != 0)
                {
                    DeserializeAndAddIncludes(configs);
                }
            }

            log.LogMessage("Completed loading configuration files.");
            log.LogMessage("Found " + configs.Configurations.Count + " configurations...");
            return configs;
        }

        /// <summary>
        /// Deserializes and Included files, and adds them as Configuration objects.
        /// </summary>
        /// <param name="configs">The configs.</param>
        private void DeserializeAndAddIncludes(RenderConfig configs)
        {
            XmlSerializer configSerializer = new XmlSerializer(typeof(Configuration));

            foreach (Include include in configs.Includes)
            {
                string includeFile = include.file;

                if (!File.Exists(includeFile))
                {
                    FileInfo configFile = new FileInfo(config.ConfigFile);
                    includeFile = Path.Combine(configFile.Directory.FullName, include.file);
                }

                if (File.Exists(includeFile))
                {
                    log.LogMessage("Attempting to include file: " + include.file);
                    try
                    {
                        XmlDocument includeXml = new XmlDocument();
                        includeXml.Load(includeFile);
                        XmlNodeList nodes = includeXml.SelectNodes(@"//Configuration");
                        if (nodes.Count > 0)
                        {
                            log.LogMessage(string.Concat(include.file, " contained ",nodes.Count," Configuration Nodes"));
                            foreach (XmlNode node in nodes)
                            {
                                using (XmlReader nodeReader = XmlReader.Create(new StringReader(node.OuterXml.ToString())))
                                {
                                    configs.Configurations.Add((Configuration)configSerializer.Deserialize(nodeReader));
                                }
                            }
                        }
                        else
                        {
                            log.LogError(string.Concat("Could not find any Configuration nodes in ",include.file));
                        }
                    }
                    catch (ApplicationException i)
                    {
                        log.LogError("Could not load include file: " + include.file);
                        throw new ApplicationException("Could not load include file: " + include.file, i);
                    }
                }
                else
                {
                    log.LogError("Could not find include file: " + include.file);
                    throw new ApplicationException("Could not find include file: " + include.file);
                }
            }
        }

        /// <summary>
        /// Gets an XML schema set, including the Configuration.xsd included as a resource in assembly.
        /// </summary>
        /// <returns></returns>
        private static XmlSchemaSet GetXmlSchemaSet()
        {
            XmlSchemaSet xsc = new XmlSchemaSet();
            ResourceManager resources = new ResourceManager("RenderConfig.Core.Resources", Assembly.GetExecutingAssembly());
            string xsd = (string)resources.GetObject("Configuration");
            using (XmlReader schemaReader = XmlReader.Create(new StringReader(xsd)))
            {
                //add the schema to the schema set
                xsc.Add(XmlSchema.Read(schemaReader, new ValidationEventHandler(delegate(Object sender, ValidationEventArgs e) { })));
            }
            return xsc;
        }

        /// <summary>
        /// Replaces the provided token in a file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="targetFile">The target file.</param>
        /// <returns></returns>
        public static int ReplaceTokenInFile(string key, string value, string targetFile)
        {
            int count = 0;
            string source = RegExParseAndReplace(out count, key, value ,File.ReadAllText(targetFile));
            File.WriteAllText(targetFile, source);

            return count;
        }

        /// <summary>
        /// Parses a string, replacing a key using a regex, returns the number of replacements.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string RegExParseAndReplace(out int count, string key, string value, string source)
        {
            Regex r = new Regex(key);
            count = 0;

            MatchCollection mc = r.Matches(source);
            if (mc.Count > 0)
            {
                source = source.Replace(key, value);
                count = mc.Count;
            }

            return source;
        }

		public static int CharacterCountInString(string stringValue, char character)
		{
			int returnVal = 0;
			foreach (char c in stringValue)
			{
				if (c == character)
				{
					returnVal++;
				}
			}
			
			return returnVal;
		}
		
        /// <summary>
        /// Regs the ex parse and replace string.
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string RegExParseAndReplaceString(string stringToParse, string regex, string replacement)
        {
            string returnString = stringToParse;
            Regex r = new Regex(regex);

            MatchCollection mc = r.Matches(stringToParse);
            if (mc.Count > 0)
            {
                returnString = returnString.Replace(regex, replacement);
            }
            return returnString;
        }

        /// <summary>
        /// Returns a string array as a single concatenated string.
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns></returns>
        public static String ReturnStringArrayAsString(string[] stringToParse)
        {
            return ReturnStringArrayAsString(stringToParse, null);
        }

        /// <summary>
        /// Substitutes the specified target, basically a simple copy operation.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="directory">The directory.</param>
        public static void Substitute(ITargetFile target, string directory)
        {
            try
            {
                FileInfo file = new FileInfo(ResolveAndCopyDestinationFilename(target, directory, false));
                if (!Directory.Exists(file.DirectoryName))
                {
                    Directory.CreateDirectory(file.DirectoryName);
                }
                File.Copy(target.source, file.FullName, true);
            }
            catch (Exception i)
            {
                throw new Exception("Failed to copy " + target.source + " to " + target.destination, i);
            }
        }

        /// <summary>
        /// Returns the string array as string, can include a delimeter where strings are joined.
        /// </summary>
        /// <param name="stringArray">The string array.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <returns></returns>
        public static string ReturnStringArrayAsString(string[] array, char? delimeter)
        {
            string concatenated = string.Empty;
            if (array != null)
            {
                if (array.Length > 0)
                {
                    foreach (string s in array)
                    {
                        if (delimeter.HasValue)
                        {
                            concatenated = string.Concat(concatenated, delimeter.ToString(), s);
                        }
                        else
                        {
                            concatenated = String.Concat(concatenated, s);
                        }
                    }
                }
            }

            return concatenated;
        }

        /// <summary>
        /// Copies the source file to destination file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="destination">The destination.</param>
        private static void CopySourceFileToDestinationFile(ITargetFile file, string destination)
        {
            //We assume that for most we will not have an existing file, and we will want to copy the target
            //to the destination.  We also need to ensure that if the directory that we want to copy into is 
            //not there that we create it.
            FileInfo fileInfo = new FileInfo(destination);
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
            File.Copy(Path.Combine(Directory.GetCurrentDirectory(), file.source), destination);
        }

        /// <summary>
        /// Gets the target file based on the source file and the target output directory. Takes into account InputDirectory and PreserveStructure flags.
        /// </summary>
        /// <param name="file">The TargetFile object we are using.</param>
        /// <param name="configDirectory">The config directory.</param>
        /// <param name="copyFile">if set to <c>true</c> [copy file], otherwise if the file is not in place it will not be copied.</param>
        /// <returns></returns>
        public static string ResolveAndCopyDestinationFilename(ITargetFile file, string configDirectory, Boolean copyFile)
        {
            string destination;

            //First, are we just dropping the file into the target root directory with the same name?
            //We need to sort out what the target filename is if we are not passing one in.
            if (String.IsNullOrEmpty(file.destination))
            {
                FileInfo fileInfo = new FileInfo(file.source);
                destination = Path.Combine(configDirectory, fileInfo.Name);
            }
            else
            {
                destination = Path.Combine(configDirectory, file.destination);
            }

            //Copy over the file if it does not already exist.  If it exists, odds on a configuration has already run...
            if (!File.Exists(destination) && copyFile)
            {
                CopySourceFileToDestinationFile(file, destination);
            }

            return destination;
        }

        /// <summary>
        /// Replaces environment variables in a string, where the variable is provided as a token eg $(variable).
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <returns></returns>
        public static String ReplaceEnvironmentVariables(string stringToParse)
        {
            Regex r = new Regex(@"\$\(([^)]*)\)");

            string returnString = stringToParse;

            MatchCollection mc = r.Matches(stringToParse);
            if (mc.Count > 0)
            {
                for (int j = 0; j < mc.Count; j++)
                {
                    Match m = mc[j];
                    if (m.Groups.Count == 2)
                    {
                        returnString = returnString.Replace(m.Value, Environment.GetEnvironmentVariable(m.Groups[1].Value));
                    }
                }
            }

            return returnString;
        }

        public static Boolean RunAllConfigurations(RenderConfigConfig config, IRenderConfigLogger log)
        {
            Boolean returnBool = false;
            if (config.Configuration.Contains(","))
            {
                string[] configs = config.Configuration.Split(',');
                foreach (string configToRun in configs)
                {
                    RenderConfigConfig newConfig = (RenderConfigConfig)config.Clone();
                    newConfig.Configuration = configToRun;
                    if (config.SubDirectoryEachConfiguration)
                    {
                        newConfig.OutputDirectory = Path.Combine(newConfig.OutputDirectory, configToRun);
                    }
                    RenderConfigEngine engine = new RenderConfigEngine(newConfig, log);
                    returnBool = engine.Render();
                }
            }
            else
            {
				if (config.SubDirectoryEachConfiguration)
				{
					if (!String.IsNullOrEmpty(config.OutputDirectory))
					{
						config.OutputDirectory = Path.Combine(config.OutputDirectory, config.Configuration);
					}
					else
					{
						config.OutputDirectory = config.Configuration;
					}
				}

                RenderConfigEngine engine = new RenderConfigEngine(config, log);
                return engine.Render();
            }

            return returnBool;
        }
    }

}
