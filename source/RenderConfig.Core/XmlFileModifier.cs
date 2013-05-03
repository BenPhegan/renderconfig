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
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace RenderConfig.Core
{
    /// <summary>
    /// Applies modifications to an XML File
    /// </summary>
    public class XmlFileModifier : IFileModifier
    {

        XmlDocument document;
        XmlReaderSettings settings;
        Boolean returnCode;
        XmlTargetFile file;
        IRenderConfigLogger log;
        RenderConfigConfig config;
        string targetFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFileModifier"/> class.
        /// </summary>
        public XmlFileModifier(XmlTargetFile file, string targetFile, IRenderConfigLogger log, RenderConfigConfig config)
        {
            this.file = file;
            this.log = log;
            this.config = config;
            this.targetFile = targetFile;

            //Get the document and set up the XmlReaderSettings
            document = new XmlDocument();
            settings = GetXmlReaderSettings(config.CleanOutput, document);

            using (XmlReader xml = XmlReader.Create(targetFile, settings))
            {
                document.Load(xml);
            }

        }

        /// <summary>
        /// Applies the xml changes to this instance.
        /// </summary>
        /// <returns></returns>
        public Boolean Run()
        {
            //Add them in a specific order
            foreach (XmlAdd mod in file.Add)
            {
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "ADD", 27, MessageImportance.High, log);
                LogModificationDetails(mod.xpath, mod.attribute, mod.Value, string.Empty, log);

                Add(mod);
            }

            foreach (XmlUpdate mod in file.Update)
            {
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "UPDATE", 27, MessageImportance.High, log);
                LogModificationDetails(mod.xpath, string.Empty, mod.Value, string.Empty, log);

                Update(mod);
            }

            foreach (XmlReplace mod in file.Replace)
            {
                //HACK: This is not pretty...anytime I use out I swear a puppy dies somewhere...
                mod.Value = RenderConfigEngine.ReplaceEnvironmentVariables(mod.Value);
                LogUtilities.LogKeyValue("TYPE", "REPLACE", 27, MessageImportance.High, log);
                LogModificationDetails(string.Empty, string.Empty, mod.Value, mod.regex, log);

                int count = 0;
                document.LoadXml(RenderConfigEngine.RegExParseAndReplace(out count, mod.regex, mod.Value, document.OuterXml.ToString()));
                LogUtilities.LogCount(count, log);
            }

            foreach (XmlDelete mod in file.Delete)
            {
                LogUtilities.LogKeyValue("TYPE", "DELETE", 27, MessageImportance.High, log);
                LogModificationDetails(mod.xpath, string.Empty, mod.Value, string.Empty, log);

                Delete(mod);
            }
                
            //TODO: Make sure that we stamp the renderconfig data if required
            if (config.StampRenderData)
            {
                XmlComment comment = document.CreateComment("Test Comment");
                document.FirstChild.AppendChild(comment);
            }

            //HACK Why oh why are XmlWriterSettings and XmlReaderSettings SOOO SIMILAR, and yet....
            XmlWriterSettings writerSettings = GetXmlWriterSettings(config.CleanOutput, document);
            using (XmlWriter writer = XmlWriter.Create(targetFile, writerSettings))
            {
                document.Save(writer);
            }
            //HACK BAD
            returnCode = true;
            return returnCode;
        }

        /// <summary>
        /// Adds the specified XPATH.
        /// </summary>
        /// <param name="mod">The mod.</param>
        private void Add(XmlAdd mod)
        {
            try
            {
                XmlNodeList nodes = ReturnMatchingNodes(mod.xpath);

                if (nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (mod.attribute != null)
                        {
                            XmlAttribute a = document.CreateAttribute(mod.attribute);
                            a.Value = mod.Value;
                            node.Attributes.Append(a);
                        }
                        else
                        {
                            //See if we have a namespace in a parent...
                            //XmlNode tableNode = GetFirstParentNodeWithNamespaceUri(node);

                            XmlDocumentFragment frag = document.CreateDocumentFragment();
                            frag.InnerXml = mod.Value;
                            
                            XmlNode imported = document.ImportNode(frag, true);
                            node.AppendChild(imported);

                            //TODO This does not work as yet....may need 3.0 and XDocument, as this stuff sucks.....
                            //if (tableNode != null)
                            //{
                            //    XmlNode test = document.CreateNode(XmlNodeType.DocumentFragment, "xmlns", imported.Name, tableNode.NamespaceURI);
                            //    test.InnerXml = imported.InnerXml;
                            //    node.AppendChild(test);
                            //}
                            //else
                            //{
                            //    node.AppendChild(imported);
                            //}
                        }
                    }
                }
            }
            catch (Exception i)
            {
                LogFailureAndOptionallyThrow(mod.xpath, "update", i);
            }
        }

        /// <summary>
        /// Gets the first parent node with namespace URI, and returns the XmlNode.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private XmlNode GetFirstParentNodeWithNamespaceUri(XmlNode node)
        {
            XmlNode returnNode = null;

            if (!string.IsNullOrEmpty(node.NamespaceURI))
            {
                return node.ParentNode;
            }
            else
            {
                if (node.ParentNode != null)
                {
                    returnNode = GetFirstParentNodeWithNamespaceUri(node.ParentNode);
                }
            }
            return returnNode;

        }

        /// <summary>
        /// Returns the matching nodes.
        /// </summary>
        /// <returns></returns>
        private XmlNodeList ReturnMatchingNodes(string xpath)
        {
            XmlNodeList nodes = document.SelectNodes(xpath);

            if (nodes.Count == 0)
            {
                //Resolve any XML namespaces.  These are the spawn of the devil.
                XmlNamespaceManager nameSpaces = ParseAndReturnXmlNamespaceManager(document, log);
                xpath = ModifyXPathWithNamespace(xpath, nameSpaces);
                log.LogMessage(MessageImportance.High, string.Concat("Attempting match with modified XPATH = ".PadLeft(27), xpath));
                nodes = document.SelectNodes(xpath, nameSpaces);
            }

            LogNodeCount(nodes, config.BreakOnNoMatch);

            return nodes;
        }

        /// <summary>
        /// Deletes a specified XPATH
        /// </summary>
        /// <param name="mod">The mod.</param>
        private void Delete(XmlDelete mod)
        {
            try
            {
                XmlNodeList nodes = ReturnMatchingNodes(mod.xpath);
                if (nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        //If we are working on an attribute, we need to cast it and delete it from its owner element.
                        if (node.NodeType == XmlNodeType.Attribute)
                        {
                            ((XmlAttribute)node).OwnerElement.Attributes.Remove((XmlAttribute)node);
                        }
                        else
                        {
                            node.ParentNode.RemoveChild(node);
                        }
                    }
                }
            }
            catch (Exception i)
            {
                LogFailureAndOptionallyThrow(mod.xpath, "delete", i);
            }
        }

        private void LogFailureAndOptionallyThrow(string xpath, string updateType, Exception exception)
        {
            var message = string.Format("Failed to process {0} modification to xpath = {1} : {2}", updateType, xpath, exception);
            if (config.BreakOnNoMatch)
                throw new Exception(message, exception);

            log.LogError(MessageImportance.High,message);
        }

        /// <summary>
        /// Updates the specified XPATH.
        /// </summary>
        /// <param name="mod">The mod.</param>
        private void Update(XmlUpdate mod)
        {
            try
            {
                XmlNodeList nodes = ReturnMatchingNodes(mod.xpath);
                if (nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                            node.InnerXml = mod.Value;
                    }
                }
            }
            catch (Exception i)
            {
                this.LogFailureAndOptionallyThrow(mod.xpath,"update", i);
            }
        }

        /// <summary>
        /// Splits an XPath so that we can look to add a namespace.  Although the method says intelligently, it wont be until someone rewrites this drivel.  Tests pass though. 
        /// </summary>
        /// <param name="xpath">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see>
        ///       <cref>System.String[]</cref>
        ///   </see>
        /// </returns>
        static string[] SplitXPathIntelligently (string xpath)
		{
			//First we split the string, then looked for unmatched quotes
			string[] firstParse = xpath.Split('/');
			List<string> returnList = new List<string>();
			Boolean inTextBlock = false;
			string concat = string.Empty;
			
			foreach (string s in firstParse)
			{
				string varS = s;
				
				//If we find an unmatched set of quotes, we have to look at contatenating the preceding strings...
				if (varS.Contains("\'") && RenderConfigEngine.CharacterCountInString(varS, '\'') != 2)
				{
					if (!inTextBlock)
					{
						concat = string.Empty;
						inTextBlock = true;
					}
					else
					{
						varS = string.Concat(concat, "/", varS);						
						inTextBlock = false;
					}
				}
				
				if (inTextBlock)
				{
					concat = string.Concat(concat, varS);
				}
				else
				{
					returnList.Add(varS);
				}
			}
			
			return returnList.ToArray();					
		}

        /// <summary>
        /// Checks and modifies the XPath namespace.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        private static string ModifyXPathWithNamespace(string xpath, XmlNamespaceManager manager)
        {
            if (manager.HasNamespace("r"))
            {
                string newXpath = string.Empty;
                string[] xpathArray = SplitXPathIntelligently(xpath);

                foreach (string s in xpathArray)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (s.StartsWith("@"))
                        {
                            newXpath = string.Concat(newXpath, "/", s);
                        }
                        else
                        {
                            if (!xpath.StartsWith("/"))
                            {
                                newXpath = string.Concat(newXpath, "r:", s);
                            }
                            else
                            {
                                newXpath = string.Concat(newXpath, "/r:", s);
                            }
                        }
                    }
                }

                return newXpath;
            }
            else
            {
                return xpath;
            }
        }

        /// <summary>
        /// Logs the node count.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="breakOnNoMatch">if set to <c>true</c> [break on no match].</param>
        private void LogNodeCount(XmlNodeList nodes, Boolean breakOnNoMatch)
        {
        
            LogUtilities.LogCount(nodes.Count, log);
            if (breakOnNoMatch)
            {
                throw new Exception("Could not find match");
            }
        
        }

        /// <summary>
        /// Parses an XMLDocument and returns an XML namespace manager.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        private static XmlNamespaceManager ParseAndReturnXmlNamespaceManager(XmlDocument document, IRenderConfigLogger log)
        {
            Regex namespaces = new Regex(@"xmlns:?(?<ns>[a-zA-Z]*)=""(?<url>((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*))""");
            MatchCollection matches = namespaces.Matches(document.OuterXml);
            XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    if (!manager.HasNamespace(match.Groups["ns"].ToString()))
                    {
                        //We will use "r" as our pretend namespace
                        string ns = "r";
                        if (!String.IsNullOrEmpty(match.Groups["ns"].Value))
                        {
                            ns = match.Groups["ns"].ToString();
                        }

                        log.LogMessage(MessageImportance.High, string.Concat("Adding XML Namespace : ".PadLeft(27) + ns + " = " + match.Groups["url"]));
                        manager.AddNamespace(ns, match.Groups["url"].ToString());
                    }
                }
            }
            return manager;
        }

        /// <summary>
        /// Logs the modification details.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        /// <param name="regex">The regex.</param>
        /// <param name="log">The log.</param>
        private static void LogModificationDetails(string xpath, string attribute, string value, string regex, IRenderConfigLogger log)
        {
            if (!String.IsNullOrEmpty(xpath))
            {
                LogUtilities.LogKeyValue("XPATH", xpath, 27, MessageImportance.Normal, log);
            }
            if (!String.IsNullOrEmpty(attribute))
            {
                LogUtilities.LogKeyValue("ATTRIBUTE", attribute, 27, MessageImportance.Normal, log);
            }
            if (!String.IsNullOrEmpty(value))
            {
                LogUtilities.LogKeyValue("VALUE", value, 27, MessageImportance.Normal, log);
            } 
            if (!String.IsNullOrEmpty(regex))
            {
                LogUtilities.LogKeyValue("REGEX", regex, 27, MessageImportance.Normal, log);
            }
        }

        /// <summary>
        /// Gets the XML reader settings.
        /// </summary>
        /// <param name="cleanOutput">if set to <c>true</c> [clean output].</param>
        /// <param name="doc">The doc.</param>
        /// <returns></returns>
        private static XmlReaderSettings GetXmlReaderSettings(Boolean cleanOutput, XmlDocument doc)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            settings.XmlResolver = null;
            if (cleanOutput)
            {
                doc.PreserveWhitespace = false;
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                settings.IgnoreProcessingInstructions = true;
            }
            else
            {
                doc.PreserveWhitespace = true;
                settings.IgnoreComments = false;
                settings.IgnoreWhitespace = false;
                settings.IgnoreProcessingInstructions = false;
            }

            return settings;
        }

        /// <summary>
        /// Gets the XML writer settings.
        /// </summary>
        /// <param name="cleanOutput">if set to <c>true</c> [clean output].</param>
        /// <param name="doc">The doc.</param>
        /// <returns></returns>
        private static XmlWriterSettings GetXmlWriterSettings(Boolean cleanOutput, XmlDocument doc)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);

            if (cleanOutput)
            {
                doc.PreserveWhitespace = false;
            }
            else
            {
                doc.PreserveWhitespace = true;
            }

            return settings;
        }
    }
}
