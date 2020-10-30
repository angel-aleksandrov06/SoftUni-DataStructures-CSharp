namespace _01.Red_Black_Tree
{
    using System;
    using System.Collections.Generic;

    public class RedBlackTree<T> 
        : IBinarySearchTree<T> where T : IComparable
    {
        public const bool RED = true;
        public const bool BLACK = false;

        private Node root;

        public RedBlackTree()
        {
        }

        private RedBlackTree(Node node)
        {
            this.PreOrderCopy(node);
        }

        public int Count => this.GetCount(this.root);

        public void Insert(T element)
        {
            this.root = this.Insert(this.root, element);
            this.root.Color = BLACK;
        }

        public T Select(int rank)
        {
            Node node = this.Select(rank, this.root);
            if (node == null)
            {
                throw new InvalidOperationException();
            }

            return node.Value;
        }

        public int Rank(T element)
        {
            return this.Rank(element, this.root);
        }

        public bool Contains(T element)
        {
            Node node =  this.FindElement(element);

            return node != null;
        }

        public IBinarySearchTree<T> Search(T element)
        {
            Node currentNode = this.FindElement(element);

            return new RedBlackTree<T>(currentNode);
        }

        public void DeleteMin()
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }

            this.root = this.DeleteMin(this.root);
        }

        public void DeleteMax()
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }

            this.root = this.DeleteMax(this.root);
        }

        public IEnumerable<T> Range(T startRange, T endRange)
        {
            Queue<T> queue = new Queue<T>();

            this.Range(this.root, queue, startRange, endRange);

            return queue;
        }

        public  void Delete(T element)
        {
            if (this.root == null)
            {
                throw new InvalidOperationException();
            }

            this.root = this.Delete(element, this.root);
        }

        public T Ceiling(T element)
        {
            return this.Select(this.Rank(element) + 1);
        }

        public T Floor(T element)
        {
            return this.Select(this.Rank(element) - 1);
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(this.root, action);
        }

        private class Node
        {
            public Node(T value)
            {
                this.Value = value;
                this.Color = RED;
            }

            public T Value { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public bool Color { get; set; }
            public int Count { get; set; }
        }

        private int GetCount(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            return node.Count;
        }

        private bool IsRed(Node node)
        {
            return node?.Color == RED;
        }

        private void PreOrderCopy(Node node)
        {
            if (node == null)
            {
                return;
            }

            this.Insert(node.Value);
            this.PreOrderCopy(node.Left);
            this.PreOrderCopy(node.Right);
        }

        private void Range(Node node, Queue<T> queue, T startRange, T endRange)
        {
            if (node == null)
            {
                return;
            }

            int nodeInLowerRange = startRange.CompareTo(node.Value);
            int nodeInHigherRange = endRange.CompareTo(node.Value);

            if (nodeInLowerRange < 0)
            {
                this.Range(node.Left, queue, startRange, endRange);
            }
            if (nodeInLowerRange <= 0 && nodeInHigherRange >= 0)
            {
                queue.Enqueue(node.Value);
            }
            if (nodeInHigherRange > 0)
            {
                this.Range(node.Right, queue, startRange, endRange);
            }
        }

        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        }

        private int Rank(T element, Node node)
        {
            if (node == null)
            {
                return 0;
            }

            int compare = element.CompareTo(node.Value);

            if (compare < 0)
            {
                return this.Rank(element, node.Left);
            }

            if (compare > 0)
            {
                return 1 + this.GetCount(node.Left) + this.Rank(element, node.Right);
            }

            return this.GetCount(node.Left);
        }

        private Node Insert(Node node, T element)
        {
            if (node == null)
            {
                node = new Node(element);
            }
            else if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.Insert(node.Left, element);
            }
            else if (element.CompareTo(node.Value) > 0)
            {
                node.Right = this.Insert(node.Right, element);
            }

            // Rotate left
            if (this.IsRed(node.Right) && !IsRed(node.Left))
                node = this.RotateLeft(node);

            // Rotate right
            if (this.IsRed(node.Left) && IsRed(node.Left.Left))
                node = this.RotateRight(node);

            // Flip colors
            if (this.IsRed(node.Left) && this.IsRed(node.Right))
                node = this.FlipColors(node);

            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);
            return node;
        }

        private Node FlipColors(Node node)
        {
            node.Left.Color = BLACK;
            node.Right.Color = BLACK;
            node.Color = RED;

            return node;
        }

        private Node RotateRight(Node node)
        {
            Node temp = node.Left;
            node.Left = temp.Right;
            temp.Right = node;

            temp.Color = node.Color;
            node.Color = BLACK;
            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

            return temp;
        }

        private Node RotateLeft(Node node)
        {
            Node temp = node.Right;
            node.Right = temp.Left;
            temp.Left = node;

            temp.Color = node.Color;
            node.Color = RED;
            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

            return temp;
        }

        private Node FindElement(T element)
        {
            Node currentNode = this.root;

            while (currentNode != null)
            {
                if (currentNode.Value.CompareTo(element) > 0)
                {
                    currentNode = currentNode.Left;
                }
                else if (currentNode.Value.CompareTo(element) < 0)
                {
                    currentNode = currentNode.Right;
                }
                else
                {
                    break;
                }
            }

            return currentNode;
        }

        private Node DeleteMin(Node node)
        {
            if (node.Left == null)
            {
                return node.Right;
            }

            node.Left = this.DeleteMin(node.Left);
            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

            return node;
        }

        private Node DeleteMax(Node node)
        {
            if (node.Right == null)
            {
                return node.Left;
            }

            node.Right = this.DeleteMax(node.Right);
            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

            return node;
        }

        private Node Delete(T element, Node node)
        {
            int compare = element.CompareTo(node.Value);

            if (compare < 0)
            {
                node.Left = this.Delete(element, node.Left);
            }
            else if (compare > 0)
            {
                node.Right = this.Delete(element, node.Right);
            }
            else
            {
                if (node.Right == null)
                {
                    return node.Left;
                }

                if (node.Left == null)
                {
                    return node.Right;
                }

                Node temp = node;
                node = this.FindMin(temp.Right);
                node.Right = this.DeleteMin(temp.Right);
                node.Left = temp.Left;
            }

            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

            return node;
        }

        private Node FindMin(Node node)
        {
            if (node.Left == null)
                return node;

            return this.FindMin(node.Left);
        }

        private Node Select(int rank, Node node)
        {
            if (node == null)
            {
                return null;
            }

            int leftCount = this.GetCount(node.Left);
            if (leftCount == rank)
            {
                return node;
            }

            if (leftCount > rank)
            {
                return this.Select(rank, node.Left);
            }

            return this.Select(rank - (leftCount + 1), node.Right);
        }
    }
}