namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class Tree<T> : IAbstractTree<T>
    {
        private readonly List<Tree<T>> _children;

        public Tree(T value)
        {
            this.Value = value;
            this.Parent = null;
            this._children = new List<Tree<T>>();
        }

        public Tree(T value, params Tree<T>[] children)
            : this(value)
        {
            foreach (var child in children)
            {
                child.Parent = this;
                this._children.Add(child);
            }
        }

        public T Value { get; private set; }
        public Tree<T> Parent { get; private set; }
        public IReadOnlyCollection<Tree<T>> Children => this._children.AsReadOnly();

        public bool IsRootDeleted { get; private set; }

        public ICollection<T> OrderBfs()
        {
            var result = new List<T>();
            var queue = new Queue<Tree<T>>();

            if (this.IsRootDeleted)
            {
                return new List<T>();
            }

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currElement = queue.Dequeue();
                result.Add(currElement.Value);

                foreach (var child in currElement.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return result;
        }

        public ICollection<T> OrderDfs()
        {
            if (this.IsRootDeleted)
            {
                return new List<T>();
            }

            return this.OrderDfsWhitStack();
        }

        public void AddChild(T parentKey, Tree<T> child)
        {
            var parentNode = this.FindDFS(parentKey, this);
            this.CheckEmptyNode(parentNode);

            parentNode._children.Add(child);
        }

        public void RemoveNode(T nodeKey)
        {
            var currNode = this.FindBFS(nodeKey);
            this.CheckEmptyNode(currNode);

            foreach (var child in currNode.Children)
            {
                child.Parent = null;
            }

            currNode._children.Clear();

            var parentNode = currNode.Parent;

            if (parentNode == null)
            {
                this.IsRootDeleted = true;
            }
            else
            {
                parentNode._children.Remove(currNode);
            }

            currNode.Value = default;
        }

        public void Swap(T firstKey, T secondKey)
        {
            var firstNode = this.FindBFS(firstKey);
            var secondNode = this.FindBFS(secondKey);

            this.CheckEmptyNode(firstNode);
            this.CheckEmptyNode(secondNode);

            var firstParent = firstNode.Parent;
            var secondParent = secondNode.Parent;

            if (firstParent == null)
            {
                SwapRoot(secondNode);
                return;
            }

            if (secondParent == null)
            {
                SwapRoot(firstNode);
                return;
            }

            firstNode.Parent = secondParent;
            secondNode.Parent = firstParent;

            int indexOfFirst = firstParent._children.IndexOf(firstNode);
            int indexOfSecond = secondParent._children.IndexOf(secondNode);

            firstParent._children[indexOfFirst] = secondNode;
            secondParent._children[indexOfSecond] = firstNode;
        }

        private void SwapRoot(Tree<T> secondNode)
        {
            this.Value = secondNode.Value;
            this._children.Clear();
            foreach (var child in secondNode.Children)
            {
                this._children.Add(child);
            }
        }

        private void Dfs(Tree<T> subTree, List<T> result)
        {
            foreach (var child in subTree.Children)
            {
                this.Dfs(child, result);
            }

            result.Add(subTree.Value);
        }

        private ICollection<T> OrderDfsWhitStack()
        {
            var result = new Stack<T>();
            var toTraverse = new Stack<Tree<T>>();

            toTraverse.Push(this);

            while (toTraverse.Count > 0)
            {
                var subTree = toTraverse.Pop();

                foreach (var child in subTree.Children)
                {
                    toTraverse.Push(child);
                }

                result.Push(subTree.Value);
            }

            return new List<T>(result);
        }

        private Tree<T> FindBFS(T value)
        {
            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currElement = queue.Dequeue();

                if (currElement.Value.Equals(value))
                {
                    return currElement;
                }

                foreach (var child in currElement.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        private Tree<T> FindDFS(T value, Tree<T> subtree)
        {
            if (subtree.Value.Equals(value))
            {
                return subtree;
            }

            foreach (var child in subtree.Children)
            {
                Tree<T> current = this.FindDFS(value, child);

                if (current is null)
                {
                    return null;
                }

                if (current.Value.Equals(value))
                {
                    return current;
                }
            }

            return null;
        }

        private void CheckEmptyNode(Tree<T> parentNode)
        {
            if (parentNode is null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}
