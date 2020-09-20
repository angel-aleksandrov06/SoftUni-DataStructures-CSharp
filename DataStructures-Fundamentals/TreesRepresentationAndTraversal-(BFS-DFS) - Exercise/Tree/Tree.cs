namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tree<T> : IAbstractTree<T>
    {
        private readonly List<Tree<T>> _children;

        public Tree(T key, params Tree<T>[] children)
        {
            this.Key = key;
            this._children = new List<Tree<T>>();

            foreach (var child in children)
            {
                this.AddChild(child);
                child.Parent = this;
            }
        }

        public T Key { get; private set; }

        public Tree<T> Parent { get; private set; }


        public IReadOnlyCollection<Tree<T>> Children
            => this._children.AsReadOnly();

        public void AddChild(Tree<T> child)
        {
            this._children.Add(child);
        }

        public void AddParent(Tree<T> parent)
        {
            this.Parent = parent;
        }

        public string GetAsString()
        {
            StringBuilder sb = new StringBuilder();

            this.OrderDFSForString(0, sb, this);

            return sb.ToString().Trim();
        }

        public Tree<T> GetDeepestLeftomostNode()
        {
            var leafNodes = this.OrderBFSNodes()
                .Where(node => this.IsLeaf(node));

            var deepestNodeDepth = 0;
            Tree<T> deepestNode = null;

            foreach (var leafNode in leafNodes)
            {
                int currDepth = this.GetDepthFromLeafToParent(leafNode);

                if (currDepth > deepestNodeDepth)
                {
                    deepestNodeDepth = currDepth;
                    deepestNode = leafNode;
                }
            }

            return deepestNode;
        }

        public List<T> GetLeafKeys()
        {
            Func<Tree<T>, bool> leafKeysPredicate = (node) => this.IsLeaf(node);

            return GetNodeKeys(leafKeysPredicate);
        }

        public List<T> GetMiddleKeys()
        {
            Func<Tree<T>, bool> middleKeysPredicate = (node) => this.IsMiddleNode(node);
            return GetNodeKeys(middleKeysPredicate);
        }

        public List<T> GetLongestPath()
        {
            var deepestNode = this.GetDeepestLeftomostNode();

            var longestPathNodes = new Stack<T>();
            var currNode = deepestNode;

            while (currNode != null)
            {
                longestPathNodes.Push(currNode.Key);
                currNode = currNode.Parent;
            }

            return new List<T>(longestPathNodes);

        }

        public List<List<T>> PathsWithGivenSum(int sum)
        {
            var allPaths = new List<List<T>>();
            var currPath = new List<T>();

            currPath.Add(this.Key);

            this.GetPathsWithDFS(this, allPaths, currPath,sum);

            return allPaths;
        }

        public List<Tree<T>> SubTreesWithGivenSum(int sum)
        {
            var SubtreesWithGivenSum = new List<Tree<T>>();

            var allNodes = this.OrderBFSNodes();

            foreach (var currNode in allNodes)
            {
                int subTreeSum = this.GetSubtreeSumDFS(currNode);

                if (subTreeSum == sum)
                {
                    SubtreesWithGivenSum.Add(currNode);
                }
            }

            return SubtreesWithGivenSum;
        }

        private void OrderDFSForString(int depth, StringBuilder sb, Tree<T> subTree)
        {
            sb.AppendLine(new string(' ', depth) + subTree.Key);


            foreach (var child in subTree.Children)
            {
                this.OrderDFSForString(depth + 2, sb, child);
            }
        }

        private bool IsLeaf(Tree<T> node)
        {
            return node.Children.Count == 0;
        }

        private bool IsRoot(Tree<T> node)
        {
            return node.Parent == null;
        }

        private bool IsMiddleNode(Tree<T> node)
        {
            return !this.IsLeaf(node) && !this.IsRoot(node);
        }

        private List<T> GetNodeKeys(Func<Tree<T>, bool> predicate)
        {
            var keys = new List<T>();
            var nodes = new Queue<Tree<T>>();

            nodes.Enqueue(this);

            while (nodes.Count > 0)
            {
                var currNode = nodes.Dequeue();

                if (predicate.Invoke(currNode))
                {
                    keys.Add(currNode.Key);
                }

                foreach (var child in currNode.Children)
                {
                    nodes.Enqueue(child);
                }
            }

            return keys;
        }

        private List<Tree<T>> OrderBFSNodes()
        {
            var bfsNodes = new List<Tree<T>>();
            var nodes = new Queue<Tree<T>>();

            nodes.Enqueue(this);

            while (nodes.Count > 0)
            {
                var currNode = nodes.Dequeue();

                bfsNodes.Add(currNode);

                foreach (var child in currNode.Children)
                {
                    nodes.Enqueue(child);
                }
            }

            return bfsNodes;
        }

        private int GetDepthFromLeafToParent(Tree<T> node)
        {
            int depth = 0;
            var curNode = node;

            while (curNode.Parent != null)
            {
                depth++;
                curNode = curNode.Parent;
            }

            return depth;
        }

        private void GetPathsWithDFS(Tree<T> currNode, List<List<T>> allPaths, List<T> currPath,int SerchedSum)
        {
            foreach (var child in currNode.Children)
            {
                currPath.Add(child.Key);
                this.GetPathsWithDFS(child, allPaths, currPath, SerchedSum);
            }

            var currSum = currPath.Select((e) => Convert.ToInt32(e)).Sum();

            if (currSum == SerchedSum)
            {
                allPaths.Add(new List<T>(currPath));
            }

            currPath.RemoveAt(currPath.Count - 1);
        }

        private int GetSubtreeSumDFS(Tree<T> currNode)
        {
            int currSum = Convert.ToInt32(currNode.Key);
            int childSum = 0;

            foreach (var child in currNode.Children)
            {
                childSum += this.GetSubtreeSumDFS(child);
            }

            return currSum + childSum;
        }
    }
}
