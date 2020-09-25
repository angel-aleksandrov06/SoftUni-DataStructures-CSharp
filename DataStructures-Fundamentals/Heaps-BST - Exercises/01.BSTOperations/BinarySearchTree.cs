namespace _01.BSTOperations
{
    using System;
    using System.Collections.Generic;

    public class BinarySearchTree<T> : IAbstractBinarySearchTree<T>
        where T : IComparable<T>
    {
        public BinarySearchTree()
        {
        }

        public BinarySearchTree(Node<T> root)
        {
            this.Copy(root);
        }

        public Node<T> Root { get; private set; }

        public Node<T> LeftChild { get; private set; }

        public Node<T> RightChild { get; private set; }

        public T Value => this.Root.Value;

        public int Count => this.Root.Count;

        public bool Contains(T element)
        {
            Node<T> currNode = this.Root;

            while (currNode != null)
            {
                if (this.IsLess(element, currNode.Value))
                {
                    currNode = currNode.LeftChild;
                }
                else if (this.IsGreate(element, currNode.Value))
                {
                    currNode = currNode.RightChild;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public void Insert(T element)
        {
            Node<T> toInsert = new Node<T>(element, null, null);

            if (this.Root == null)
            {
                this.Root = toInsert;
            }
            else
            {
                this.InsertElementDFS(this.Root, null, toInsert);
            }
        }

        public IAbstractBinarySearchTree<T> Search(T element)
        {
            Node<T> currNode = this.Root;

            while (currNode != null)
            {
                if (this.IsLess(element, currNode.Value))
                {
                    currNode = currNode.LeftChild;
                }
                else if (this.IsGreate(element, currNode.Value))
                {
                    currNode = currNode.RightChild;
                }
                else
                {
                    break;
                }
            }

            return new BinarySearchTree<T>(currNode);
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrderDFS(this.Root, action);
        }

        public List<T> Range(T lower, T upper)
        {
            var result = new List<T>();
            var nodes = new Queue<Node<T>>();
            nodes.Enqueue(this.Root);

            while (nodes.Count > 0)
            {
                var currNode = nodes.Dequeue();

                if (this.IsLess(lower, currNode.Value) && this.IsGreate(upper, currNode.Value))
                {
                    result.Add(currNode.Value);
                }
                else if (this.AreEqual(lower, currNode.Value) || this.AreEqual(upper, currNode.Value))
                {
                    result.Add(currNode.Value);
                }

                if (currNode.LeftChild != null)
                {
                    nodes.Enqueue(currNode.LeftChild);
                }

                if (currNode.RightChild != null)
                {
                    nodes.Enqueue(currNode.RightChild);
                }
            }

            return result;
        }

        public void DeleteMin()
        {
            this.EnsureNotEmpty();

            Node<T> currNode = this.Root;
            Node<T> prevNode = null;

            if (this.Root.LeftChild == null)
            {
                this.Root = this.Root.RightChild;
            }
            else
            {
                while (currNode.LeftChild != null)
                {
                    prevNode = currNode;
                    currNode = currNode.LeftChild;
                }

                prevNode.LeftChild = currNode.RightChild;
                this.Root.Count--;
            }
        }

        public void DeleteMax()
        {
            this.EnsureNotEmpty();

            Node<T> currNode = this.Root;
            Node<T> prevNode = null;

            if (this.Root.RightChild == null)
            {
                this.Root = this.Root.LeftChild;
            }
            else
            {
                while (currNode.RightChild != null)
                {
                    prevNode = currNode;
                    currNode = currNode.RightChild;
                }

                prevNode.RightChild = currNode.LeftChild;
                this.Root.Count--;
            }
        }

        public int GetRank(T element)
        {
            return this.GetRankDFS(this.Root, element);
        }

        private int GetRankDFS(Node<T> currNode, T element)
        {
            if (currNode == null)
            {
                return 0;
            }

            if (this.IsLess(element, currNode.Value))
            {
                return this.GetRankDFS(currNode.LeftChild, element);
            }
            else if (this.AreEqual(element, currNode.Value))
            {
                return this.GetNodeCount(currNode);
            }

            return this.GetNodeCount(currNode.LeftChild) + 1 + this.GetRankDFS(currNode.RightChild, element);
        }

        private int GetNodeCount(Node<T> current)
        {
            return current == null ? 0 : current.Count;
        }

        private void InsertElementDFS(Node<T> currNode, Node<T> previous, Node<T> toInsert)
        {
            if (currNode == null && this.IsLess(toInsert.Value, previous.Value))
            {
                previous.LeftChild = toInsert;

                if (this.LeftChild == null)
                {
                    this.LeftChild = toInsert;
                }
                
                return;
            }

            if (currNode == null && this.IsGreate(toInsert.Value, previous.Value))
            {
                previous.RightChild = toInsert;

                if (this.RightChild == null)
                {
                    this.RightChild = toInsert;
                }

                return;
            }

            if (this.IsLess(toInsert.Value, currNode.Value))
            {
                this.InsertElementDFS(currNode.LeftChild, currNode, toInsert);
                currNode.Count++;
            }
            else if (this.IsGreate(toInsert.Value, currNode.Value))
            {
                this.InsertElementDFS(currNode.RightChild, currNode, toInsert);
                currNode.Count++;
            }
        }

        private bool IsLess(T firstEl, T secondEl)
        {
            return firstEl.CompareTo(secondEl) < 0;
        }

        private bool IsGreate(T firstEl, T secondEl)
        {
            return firstEl.CompareTo(secondEl) > 0;
        }

        private bool AreEqual(T firstEl, T secondEl)
        {
            return firstEl.CompareTo(secondEl) == 0;
        }

        private void EachInOrderDFS(Node<T> currNode, Action<T> action)
        {
            if (currNode != null)
            {
                this.EachInOrderDFS(currNode.LeftChild, action);
                action.Invoke(currNode.Value);
                this.EachInOrderDFS(currNode.RightChild, action);
            }
        }

        private void Copy(Node<T> currNode)
        {
            if (currNode != null)
            {
                this.Insert(currNode.Value);
                this.Copy(currNode.LeftChild);
                this.Copy(currNode.RightChild);
            }
        }

        private void EnsureNotEmpty()
        {
            if (this.Root == null)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
