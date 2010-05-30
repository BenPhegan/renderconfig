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


using Nini.Config;
using NUnit.Core;
using NUnit.Framework;
using RenderConfig.Console;
using System.IO;
using System;


namespace RenderConfig.Core.Tests
{
    [TestFixture]
    public class IniFileModifierTests
    {
        DirectoryInfo od = new DirectoryInfo(String.Concat("testing", Path.DirectorySeparatorChar, "ini"));

        [SetUp]
        public void Setup()
        {
            RenderConfigConfig config = GetConfigObject();
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);
            engine.Render();
        }

        private static RenderConfigConfig GetConfigObject()
        {
            RenderConfigConfig config = new RenderConfigConfig();
            config.ConfigFile = String.Concat("examples", Path.DirectorySeparatorChar, "config.ini.xml");
            config.Configuration = "inifilemodifier";
            config.OutputDirectory = String.Concat("testing", Path.DirectorySeparatorChar, "ini");
            config.InputDirectory = "Examples";
            config.BreakOnNoMatch = false;
            return config;
        }

        [Test]
        public void AddKeyValue()
        {
            //<Modification type="add" section="Logging" key="CommonSetting">Value for common setting</Modification>

            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName,"test.ini"));
            Assert.AreEqual(ini.Configs["Logging"].Get("CommonSetting"), "Value for common setting");
        }

        [Test]
        public void AddSectionKeyValue()
        {
            //<Modification type="add" section="NewSection" key="FromCommon">BLAH!</Modification>

            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.AreEqual(ini.Configs["NewSection"].Get("FromCommon"), "BLAH!");

        }

        [Test]
        public void DeleteKeyValue()
        {
            //<Modification type="delete" section="Logging" key="MessageColumns"/>

            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.AreEqual(ini.Configs["Logging"].Get("MessageColumns"), null);
        }

        [Test]
        public void UpdateKeyValue()
        {
            //<Modification type="update" section="Logging" key="MaxFileSize">69</Modification>

            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.AreEqual(ini.Configs["Logging"].Get("MaxFileSize"), "69");
        }

        [Test]
        public void ExceptionOnUpdateMissingSection()
        {
            RenderConfigConfig config = GetConfigObject();
            config.BreakOnNoMatch = true;
            config.Configuration = "inimissingsection";
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);

            Assert.Throws<System.Exception>(delegate { engine.Render(); });
            
        }

        [Test]
        public void ExceptionOnUpdateMissingKey()
        {
            RenderConfigConfig config = GetConfigObject();
            config.BreakOnNoMatch = true;
            config.Configuration = "inimissingkey";
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);

            Assert.Throws<System.Exception>(delegate { engine.Render(); });

        }

        [Test]
        public void ReplaceRepeatingSubString()
        {
            RenderConfigConfig config = GetConfigObject();
            config.BreakOnNoMatch = true;
            config.Configuration = "inireplace";
            IRenderConfigLogger log = new ConsoleLogger();
            RenderConfigEngine engine = new RenderConfigEngine(config, log);

            engine.Render();
            IConfigSource ini = new IniConfigSource(Path.Combine(od.FullName, "test.ini"));
            Assert.IsTrue(ini.Configs["Logging"].Contains("Replacement1"));
            Assert.IsTrue(ini.Configs["Logging"].Contains("Replacement2"));
            Assert.IsTrue(ini.Configs["Logging"].Contains("Replacement3"));


        }
    }
}
