namespace _04.BinarySearchTree
{
    using System;

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

        public bool Contains(T element)
        {
            var currNode = this.Root;

            while (currNode != null)
            {
                if (this.IsLess(element, currNode.Value))
                {
                    currNode = currNode.LeftChild;
                }
                else if (this.IsGreater(element, currNode.Value))
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
            var toInsert = new Node<T>(element, null, null);

            if (this.Root == null)
            {
                this.Root = toInsert;
            }
            else
            {
                var currNode = this.Root;
                Node<T> prevNode = null;

                while (currNode != null)
                {
                    prevNode = currNode;
                    if (this.IsLess(element, currNode.Value))
                    {
                        currNode = currNode.LeftChild;
                    }
                    else if (this.IsGreater(element, currNode.Value))
                    {
                        currNode = currNode.RightChild;
                    }
                    else
                    {
                        return;
                    }
                }

                if (this.IsLess(element, prevNode.Value))
                {
                    prevNode.LeftChild = toInsert;
                    if (this.LeftChild == null)
                    {
                        this.LeftChild = toInsert;
                    }
                }
                else
                {
                    prevNode.RightChild = toInsert;
                    if (this.RightChild == null)
                    {
                        this.RightChild = toInsert;
                    }
                }
            }
        }

        public IAbstractBinarySearchTree<T> Search(T element)
        {
            var currNode = this.Root;

            while (currNode != null && !this.AreEqual(element, currNode.Value))
            {
                if (this.IsLess(element, currNode.Value))
                {
                    currNode = currNode.LeftChild;
                }
                else if (this.IsGreater(element, currNode.Value))
                {
                    currNode = currNode.RightChild;
                }
            }

            return new BinarySearchTree<T>(currNode);
        }

        private bool IsLess(T element, T value)
        {
            return element.CompareTo(value) < 0;
        }

        private bool IsGreater(T element, T value)
        {
            return element.CompareTo(value) > 0;
        }

        private bool AreEqual(T element, T value)
        {
            return element.CompareTo(value) == 0;
        }

        private void Copy(Node<T> root)
        {
            if (root != null)
            {
                this.Insert(root.Value);
                this.Copy(root.LeftChild);
                this.Copy(root.RightChild);
            }
        }
    }
}
