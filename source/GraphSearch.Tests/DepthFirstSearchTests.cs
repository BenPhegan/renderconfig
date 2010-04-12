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
using NUnit.Core;
using NUnit.Framework;
using GraphSearch;


namespace GraphSearch.Tests
{
    public class DepthFirstSearchTests
    {
        [Test]
        public void SimplePath()
        {
            Node<string> a = new Node<string>("A");
            Node<string> b = new Node<string>("B");
            Node<string> c = new Node<string>("C");
            Node<string> d = new Node<string>("D");

            a.Dependencies.Add(b);
            b.Dependencies.Add(c);
            c.Dependencies.Add(d);
            List<Node<string>> list = new List<Node<string>>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            list.Add(d);

            DepthFirstSearch<string> sort = new DepthFirstSearch<string>(list);

            Stack<Node<string>> results = sort.GetDependencyPath(a.Identity);
            Assert.AreEqual(results.Count, 4);
            Assert.IsTrue(d.Identity.Equals(results.Pop().Identity));
            Assert.IsTrue(c.Identity.Equals(results.Pop().Identity));
            Assert.IsTrue(b.Identity.Equals(results.Pop().Identity));
            Assert.IsTrue(a.Identity.Equals(results.Pop().Identity));

        }

        public void HarderPath()
        {
            Node<string> a = new Node<string>("A");
            Node<string> b = new Node<string>("B");
            Node<string> c = new Node<string>("C");
            Node<string> d = new Node<string>("D");

            a.Dependencies.Add(b);
            a.Dependencies.Add(c);
            b.Dependencies.Add(d);
            c.Dependencies.Add(d);


            List<Node<string>> list = new List<Node<string>>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            list.Add(d);

            DepthFirstSearch<string> sort = new DepthFirstSearch<string>(list);

            Stack<Node<string>> results = sort.GetDependencyPath(a.Identity);
            foreach (Node<string> node in results)
            {
                Console.WriteLine(node.Identity);
            }
            Assert.AreEqual(results.Count, 4);
        }

        public void HarderStillPath()
        {
            Node<string> a = new Node<string>("A");
            Node<string> b = new Node<string>("B");
            Node<string> c = new Node<string>("C");
            Node<string> d = new Node<string>("D");
            Node<string> e = new Node<string>("E");

            a.Dependencies.Add(b);
            a.Dependencies.Add(c);
            a.Dependencies.Add(d);
            a.Dependencies.Add(e);


            List<Node<string>> list = new List<Node<string>>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            list.Add(d);
            list.Add(e);

            DepthFirstSearch<string> sort = new DepthFirstSearch<string>(list);

            Stack<Node<string>> results = sort.GetDependencyPath(a.Identity);
            foreach (Node<string> node in results)
            {
                Console.WriteLine(node.Identity);
            }
            Assert.AreEqual(results.Count, 5);
        }

        [Test]
        public void InSimplePath()
        {
            Node<string> a = new Node<string>("A");
            Node<string> b = new Node<string>("B");
            Node<string> c = new Node<string>("C");
            Node<string> d = new Node<string>("D");

            a.Dependencies.Add(b);
            b.Dependencies.Add(c);
            c.Dependencies.Add(d);
            List<Node<string>> list = new List<Node<string>>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            list.Add(d);

            DepthFirstSearch<string> sort = new DepthFirstSearch<string>(list);

            Stack<Node<string>> results = sort.GetDependencyPath(b.Identity);
            foreach (Node<string> node in results)
            {
                Console.WriteLine(node.Identity);
            }
            Assert.AreEqual(results.Count, 3);
            Assert.IsTrue(d.Identity.Equals(results.Pop().Identity));
            Assert.IsTrue(c.Identity.Equals(results.Pop().Identity));
            Assert.IsTrue(b.Identity.Equals(results.Pop().Identity));

        }

        [Test]
        public void MissingTarget()
        {
            Node<string> a = new Node<string>("A");
            Node<string> b = new Node<string>("B");
            Node<string> c = new Node<string>("C");
            Node<string> d = new Node<string>("D");
            Node<string> e = new Node<string>("E");

            a.Dependencies.Add(b);
            a.Dependencies.Add(c);
            a.Dependencies.Add(d);
            a.Dependencies.Add(e);


            List<Node<string>> list = new List<Node<string>>();
            list.Add(a);
            list.Add(b);
            list.Add(c);
            list.Add(d);
            list.Add(e);

            DepthFirstSearch<string> sort = new DepthFirstSearch<string>(list);

            Stack<Node<string>> results = sort.GetDependencyPath("F");
            Assert.AreEqual(results.Count, 0);

        }

    }
}
