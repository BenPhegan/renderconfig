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

namespace GraphSearch
{
    public class DepthFirstSearch<T>
    {
        Stack<Node<T>> sorted;
        List<Node<T>> nodes;

        public List<Node<T>> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        public DepthFirstSearch(List<Node<T>> nodes)
        {
            sorted = new Stack<Node<T>>();
            this.nodes = nodes;
        }

        public Boolean? CanResolveAllDependencies
        {
            get
            {
                if (sorted.Count == 0)
                {
                    return null;
                }
                else
                {
                    return CheckDependencyResolution();
                }
            }
        }

        private Boolean CheckDependencyResolution()
        {
            //If we have only one node, and no dependencies, then we are fine.
            if (sorted.Count == 1 && sorted.Peek().Dependencies.Count == 0)
            {
                return true;
            }

            Boolean found = false;
            foreach (Node<T> first in sorted)
            {
                found = false;
                foreach (Node<T> dependency in first.Dependencies)
                {
                    foreach (Node<T> second in sorted)
                    {
                        if (second.Identity.ToString() == dependency.Identity.ToString())
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            return found;
        }

        public Stack<Node<T>> GetDependencyPath(T target)
        {
            foreach (Node<T> node in nodes)
            {
                if (!node.Visited)
                {
                    if (node.Identity.Equals(target))
                    {
                        sorted.Push(node);

                        if (node.Dependencies.Count != 0)
                        {
                            foreach (Node<T> dependency in node.Dependencies)
                            {
                                GetDependencyPath(dependency.Identity);
                            }
                        }
                        node.Visited = true;
                        break;
                    }
                }
            }
            return sorted;
        }

    }


}