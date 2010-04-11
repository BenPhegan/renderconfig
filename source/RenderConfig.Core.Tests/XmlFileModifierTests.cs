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


using System.Xml;
using NUnit.Core;
using NUnit.Framework;
using RenderConfig.Console;
using System.IO;
using System;

namespace RenderConfig.Core.Tests
{
    [TestFixture]
    public class XmlFileModifierTests
    {
        RenderConfigEngine engine;
        IRenderConfigLogger log = new ConsoleLogger();
        RenderConfigConfig config;
        DirectoryInfo od = new DirectoryInfo(String.Concat("testing" , Path.DirectorySeparatorChar , "xml"));

        [TestFixtureSetUp]
        public void Setup()
        {
            config = new RenderConfigConfig();
            config.ConfigFile = String.Concat("examples",Path.DirectorySeparatorChar, "config.xml.xml");
            config.Configuration = "xmlfilemodifier";
            config.OutputDirectory = String.Concat("testing",Path.DirectorySeparatorChar, "xml");
            config.BreakOnNoMatch = false;
            config.InputDirectory = "Examples";
        }

        [Test]
        public void AddAttributeWithoutNameSpace()
        {
            config.Configuration = "xmladd";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/configuration/configSections/section[@value='xmladd']"));
        }

        [Test]
        public void AddAttributeViaAttributeXpathWithoutNameSpace()
        {
            config.Configuration = "xmladd";
            engine = new RenderConfigEngine(config, log);
            engine.Render(); 
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/configuration/nhibernate/add[@random='xmladd']"));
        }

        [Test]
        public void AddCDATAWithoutNameSpace()
        {
            config.Configuration = "xmladd";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/configuration/nhibernate/Node"));
        }

        [Test]
        public void AddAttributeWithNameSpace()
        {
            config.Configuration = "xmladdns";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.ns.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/r:configuration/r:configSections/r:section[@value='xmladd']",GetManager(doc)));
        }

        [Test]
        public void AddAttributeViaAttributeXpathWithNameSpace()
        {
            config.Configuration = "xmladdns";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.ns.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/r:configuration/r:nhibernate/r:add[@random='xmladd']", GetManager(doc)));
        }

        [Test]
		[Ignore("Currently broken")]
        public void AddCDATAWithNameSpace()
        {
            config.Configuration = "xmladdns";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.ns.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/r:configuration/r:nhibernate/r:Node", GetManager(doc)));
        }

        [Test]
        public void AddNodeValueWithNameSpace()
        {
            config.Configuration = "xmladdns";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmladd.ns.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/r:configuration/r:nhibernate", GetManager(doc)).InnerText);
        }

        [Test]
        public void DeleteNodeByAttributeValueWithoutNameSpace()
        {
            config.Configuration = "xmldelete";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmldelete.xml"));
            Assert.IsNull(doc.SelectSingleNode("/configuration/nhibernate/add[@key='hibernate.dialect']"));
        }

        [Test]
        public void DeleteNodeWithoutNameSpace()
        {
            config.Configuration = "xmldelete";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmldelete.xml"));
            Assert.IsNull(doc.SelectSingleNode("/configuration/configSections/section"));
        }

        [Test]
        public void DeleteAttributeWithoutNameSpace()
        {
            config.Configuration = "xmldelete";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmldelete.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/configuration/nhibernate/add[@key='hibernate.connection.driver_class']"));
            Assert.IsNull(doc.SelectSingleNode("/configuration/nhibernate/add[@key='hibernate.connection.driver_class']").Attributes["Value"]);
        }

        [Test]
        public void DeleteNodeByAttributeValueWithNameSpace()
        {
            config.Configuration = "xmldeletens";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmldelete.ns.xml"));
            Assert.IsNull(doc.SelectSingleNode("/r:configuration/r:nhibernate/r:add[@key='hibernate.dialect']", GetManager(doc)));
        }

        [Test]
        public void DeleteNodeWithNameSpace()
        {
            config.Configuration = "xmldeletens";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmldelete.ns.xml"));
            Assert.IsNull(doc.SelectSingleNode("/r:configuration/r:configSections/r:section", GetManager(doc)));
        }


        [Test]
        public void DeleteAttributeWithNameSpace()
        {
            config.Configuration = "xmldeletens";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmldelete.ns.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/r:configuration/r:nhibernate/r:add[@key='hibernate.connection.driver_class']", GetManager(doc)));
            Assert.IsNull(doc.SelectSingleNode("/r:configuration/r:nhibernate/r:add[@key='hibernate.connection.driver_class']", GetManager(doc)).Attributes["Value"]);
        }

        [Test]
        public void UpdateAttributeWithoutNameSpace()
        {
            config.Configuration = "xmlupdate";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmlupdate.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/nhibernate/add[@key='hibernate.dialect']").Attributes["value"].Value, "xmlupdate");
        }

        [Test]
        public void UpdateNodeWithoutNameSpace()
        {
            config.Configuration = "xmlupdate";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmlupdate.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/Random").InnerText, "xmlupdate");
        }

        [Test]
        public void UpdateAttributeWithNameSpace()
        {
            config.Configuration = "xmlupdatens";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName,"xmlupdate.ns.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/r:configuration/r:nhibernate/r:add[@key='hibernate.dialect']",GetManager(doc)).Attributes["value"].Value, "xmlupdate");
        }

        [Test]
        public void UpdateNodeWithNameSpace()
        {
            config.Configuration = "xmlupdatens";
            engine = new RenderConfigEngine(config, log);
            engine.Render();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(od.FullName, "xmlupdate.ns.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/r:configuration/r:Random", GetManager(doc)).InnerText, "xmlupdate");
        }

        private static XmlNamespaceManager GetManager(XmlDocument doc)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("r", "http://www.w3.org/1999/whatever");
            return manager;
        }


        [Test]
        public void ReplaceMultipleSubString()
        {
            RenderConfigConfig config = new RenderConfigConfig();
            config.ConfigFile = String.Concat("examples",Path.DirectorySeparatorChar, "config.xml.xml");
            config.Configuration = "xmlreplace";
            config.OutputDirectory = String.Concat("testing",Path.DirectorySeparatorChar,"xmlreplace");
            config.InputDirectory = "examples";
            config.BreakOnNoMatch = false;
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);
            engine.Render();

            XmlDocument doc = new XmlDocument();
            doc.Load(String.Concat("testing",Path.DirectorySeparatorChar,"xmlreplace",Path.DirectorySeparatorChar,"xmlreplace.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/configuration/ReplacementRandom"));

            doc.Load(String.Concat("testing", Path.DirectorySeparatorChar, "xmlreplace", Path.DirectorySeparatorChar, "xmlreplace.ns.xml"));
            Assert.IsNotNull(doc.SelectSingleNode("/r:configuration/r:ReplacementRandom", GetManager(doc)));
        }

        [Test]
        public void ReplacementResultingInInvalidXmlThrowsException()
        {
            RenderConfigConfig config = new RenderConfigConfig();
            config.ConfigFile = String.Concat("examples",Path.DirectorySeparatorChar, "config.xml.xml");
            config.Configuration = "xmlreplacebad";
            config.OutputDirectory = String.Concat("testing",Path.DirectorySeparatorChar, "xmlreplace");
            config.InputDirectory = "examples";
            config.BreakOnNoMatch = false;
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);
            Assert.Throws<XmlException>(delegate {engine.Render();});
        }

        [Test]
        public void SettingVariables()
        {
            RenderConfigConfig config = new RenderConfigConfig();
            config.ConfigFile = String.Concat("examples",Path.DirectorySeparatorChar, "config.xml.xml");
            config.Configuration = "variabletest1";
            config.OutputDirectory = String.Concat("testing",Path.DirectorySeparatorChar, "variabletest1");
            config.InputDirectory = "examples";
            config.BreakOnNoMatch = false;
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);
            engine.Render();

            XmlDocument doc = new XmlDocument();
            doc.Load(String.Concat("testing", Path.DirectorySeparatorChar, "variabletest1", Path.DirectorySeparatorChar, "test.xml"));
            Assert.AreEqual(doc.SelectSingleNode("/configuration/Random").InnerText,"variabletest1");
        }

    }


}
