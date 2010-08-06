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
using System.Xml;
using Nini.Config;
using NUnit.Core;
using NUnit.Framework;
using RenderConfig.Console;


namespace RenderConfig.Core.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        DirectoryInfo od = new DirectoryInfo(String.Concat("testing", Path.DirectorySeparatorChar, "config"));

        RenderConfigConfig config;
        IRenderConfigLogger log = new ConsoleLogger();
        RenderConfigEngine engine;

        [SetUp]
        public void Setup()
        {
            config = new RenderConfigConfig();
            config.ConfigFile = String.Concat("examples",Path.DirectorySeparatorChar,"config.other.xml");
            config.Configuration = "config";
            config.OutputDirectory = od.FullName;
            config.InputDirectory = "Examples";
            config.BreakOnNoMatch = false;
        }

        [Test]
        public void CheckXMLSingleVariableSubstitution()
        {
            engine = new RenderConfigEngine(config, log);
            engine.Render();

            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName, "variablesubsingle.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/Random").InnerText, Environment.GetEnvironmentVariable("RenderTestVar1"));
        }

        [Test]
        public void NonExistentDependencyCausesException()
        {
            config.ConfigFile = String.Concat("examples", Path.DirectorySeparatorChar, "config.bad.xml");
            config.Configuration = "incorrectdependency";
            config.OutputDirectory = String.Concat("testing",Path.DirectorySeparatorChar,"incorrectdependency");
            config.InputDirectory = "TestFiles";
            RenderConfigEngine engine = new RenderConfigEngine(config, log);
            Assert.Throws<ApplicationException>(delegate { engine.Render(); });
        }


        [Test]
        public void CheckXMLMultipleVariableSubstitution()
        {
            engine = new RenderConfigEngine(config, log);
            engine.Render(); 
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName, "variablesubmultiple.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/Random").InnerText, string.Concat(Environment.GetEnvironmentVariable("RenderTestVar1"), string.Concat(Environment.GetEnvironmentVariable("RenderTestVar2"))));
        }
        [Test]
        public void CheckXMLMultipleVariableSubstitutionInterspersed()
        {
			Environment.SetEnvironmentVariable("RenderTestVar1", "EnvVarBlah1");
			Environment.SetEnvironmentVariable("RenderTestVar2", "EnvVarBlah2");
			engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName, "variablesubinterspersed.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/Random").InnerText, string.Concat(Environment.GetEnvironmentVariable("RenderTestVar1"), "blah",Environment.GetEnvironmentVariable("RenderTestVar2")));
        }

        [Test]
        public void CheckINISingleVariableSubstitution()
        {
			Environment.SetEnvironmentVariable("RenderTestVar1", "EnvVarBlah");
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.AreEqual(ini.Configs["Logging"].Get("File Name"), Environment.GetEnvironmentVariable("RenderTestVar1"));
        }

        [Test]
        public void CheckINIMultipleVariableSubstitution()
        {
			Environment.SetEnvironmentVariable("RenderTestVar1", "EnvVarBlah1");
			Environment.SetEnvironmentVariable("RenderTestVar2", "EnvVarBlah2");
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.AreEqual(ini.Configs["Logging"].Get("Expansion1"), string.Concat(Environment.GetEnvironmentVariable("RenderTestVar1"), string.Concat(Environment.GetEnvironmentVariable("RenderTestVar2"))));
        }
        [Test]
        public void CheckINIMultipleVariableSubstitutionInterspersed()
        {
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.AreEqual(ini.Configs["Logging"].Get("Expansion2"), string.Concat(Environment.GetEnvironmentVariable("RenderTestVar1"), "blah", Environment.GetEnvironmentVariable("RenderTestVar2")));
        }

        [Test]
        public void MultipleDependencyCheck()
        {
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            Assert.IsTrue(File.Exists(Path.Combine(od.FullName, "MultipleDependencies.xml")));
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName, "multipledependencies.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/Random").InnerText, "Child1");

        }

        [Test]
        public void CanReferenceInputDirectory()
        {
            config.Configuration = "inputdirectory";
            config.OutputDirectory = String.Concat("testing",Path.DirectorySeparatorChar,"InputDirectory");
            config.InputDirectory = "Examples";
            config.BreakOnNoMatch = false;
            engine = new RenderConfigEngine(config, log);
            engine.Render();

            Assert.True(File.Exists(String.Concat("testing",Path.DirectorySeparatorChar,"InputDirectory",Path.DirectorySeparatorChar,"test.xml")));
        }

        [Test]
        public void CanPreserveSourceStructure()
        {
            CreateTestFilesDirectory();

            config.Configuration = "preservestructure";
            config.OutputDirectory = String.Concat("testing", Path.DirectorySeparatorChar, "PreserveStructure");
            config.PreserveSourceStructure = true;
            config.BreakOnNoMatch = false;
            engine = new RenderConfigEngine(config, log);
            engine.Render();

            Assert.True(File.Exists(String.Concat("testing",Path.DirectorySeparatorChar,"preservestructure",Path.DirectorySeparatorChar,"TestFiles",Path.DirectorySeparatorChar,"test.xml")));
            Directory.Delete(String.Concat("examples",Path.DirectorySeparatorChar,"testfiles"), true);
        }

        private static void CreateTestFilesDirectory()
        {
            string dir = String.Concat("examples", Path.DirectorySeparatorChar, "testfiles");
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
            File.Copy(String.Concat("examples",Path.DirectorySeparatorChar,"test.xml"), String.Concat(dir,Path.DirectorySeparatorChar,"test.xml"));
        }

        [Test]
        public void DoesntPreserveSourceStructureWhenNotAsked()
        {
            CreateTestFilesDirectory();

            string dir = String.Concat("testing", Path.DirectorySeparatorChar, "DoesntPreserveStructure");
            config.Configuration = "preservestructure";
            config.OutputDirectory = dir;
            config.BreakOnNoMatch = false;
            engine = new RenderConfigEngine(config, log);
            engine.Render();

            Assert.True(!File.Exists(String.Concat(dir,Path.DirectorySeparatorChar,"TestFiles",Path.DirectorySeparatorChar,"test.xml")));
            Assert.True(File.Exists(String.Concat(dir,Path.DirectorySeparatorChar,"test.xml")));
        }

        [Test]
        public void BadConfigDoesntLoad()
        {
            config.ConfigFile = String.Concat("examples",Path.DirectorySeparatorChar,"config.bad.xml");
            config.Configuration = "preservestructure";
            config.OutputDirectory = "blah";
            config.BreakOnNoMatch = false;
            engine = new RenderConfigEngine(config, log);
            Assert.Throws<ApplicationException>(delegate { engine.Render(); });
        }

        [Test]
        public void IncludedSnippetFilesAvailable()
        {
            string dir = String.Concat("testing", Path.DirectorySeparatorChar, "Included");
            config.ConfigFile = String.Concat("examples", Path.DirectorySeparatorChar, "config.include.xml");
            config.Configuration = "included";
            config.OutputDirectory = dir;
            config.BreakOnNoMatch = false;
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            
            Assert.True(File.Exists(String.Concat(dir,Path.DirectorySeparatorChar,"included.xml")));
        }

        [Test]
        public void IncludedFullFilesAvailable()
        {
            string dir = String.Concat("testing", Path.DirectorySeparatorChar, "FullIncluded");
            config.ConfigFile = String.Concat("examples", Path.DirectorySeparatorChar, "config.include.full.xml");
            config.Configuration = "included";
            config.OutputDirectory = dir;
            config.BreakOnNoMatch = false;
            engine = new RenderConfigEngine(config, log);
            engine.Render();

            Assert.True(File.Exists(String.Concat(dir, Path.DirectorySeparatorChar, "included.xml")));
        }

        [Test]
        public void MultipleConfigurationsOutputToSubDirectories()
        {
            string outputDir = String.Concat(config.OutputDirectory, Path.DirectorySeparatorChar, "multiconfigsub");
            config.Configuration = "copy,copy2";
            config.OutputDirectory = outputDir;
            RenderConfigEngine.RunAllConfigurations(config, log);
            Assert.IsTrue(File.Exists(Path.Combine(od.FullName, String.Concat(outputDir, Path.DirectorySeparatorChar, "copy", Path.DirectorySeparatorChar, "test.ns.xml"))));
            Assert.IsTrue(File.Exists(Path.Combine(od.FullName, String.Concat(outputDir, Path.DirectorySeparatorChar, "copy2", Path.DirectorySeparatorChar, "test.ns.xml"))));
        }

        [Test]
        public void MultipleConfigurationsOutputToSingleDirectory()
        {
            string outputDir = String.Concat(config.OutputDirectory, Path.DirectorySeparatorChar, "multiconfigsub");
            config.Configuration = "copy,copy2";
            config.OutputDirectory = outputDir;
            config.SubDirectoryEachConfiguration = false;
            RenderConfigEngine.RunAllConfigurations(config, log);
            Assert.IsTrue(!File.Exists(Path.Combine(od.FullName, String.Concat(outputDir, Path.DirectorySeparatorChar, "copy", Path.DirectorySeparatorChar, "test.ns.xml"))));
            Assert.IsTrue(!File.Exists(Path.Combine(od.FullName, String.Concat(outputDir, Path.DirectorySeparatorChar, "copy2", Path.DirectorySeparatorChar, "test.ns.xml"))));
            Assert.IsTrue(!File.Exists(Path.Combine(od.FullName, String.Concat(outputDir, "test.ns.xml"))));
        }

    }
}
