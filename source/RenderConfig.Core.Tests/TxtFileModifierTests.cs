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


using System.IO;
using NUnit.Framework;
using RenderConfig.Console;

namespace RenderConfig.Core.Tests
{
    [TestFixture]
    public class TxtFileModifierTests
    {
        RenderConfigConfig config;
        IRenderConfigLogger log = new ConsoleLogger();
        RenderConfigEngine engine;

        [SetUp]
        public void Setup()
        {
            config = new RenderConfigConfig();
            config.ConfigFile = "examples\\config.txt.xml";
            config.Configuration = "textreplace";
            config.OutputDirectory = "testing\\text";
            config.InputDirectory = "examples";
            config.BreakOnNoMatch = false;
        }

        [Test]
        public void CanReplaceInFile()
        {
            engine = new RenderConfigEngine(config, log);
            engine.Render();

            string text = File.ReadAllText(".\\testing\\text\\textreplace.txt");
            Assert.IsTrue(text.Contains("Replacement!!!"));
        }


        
    }
}
