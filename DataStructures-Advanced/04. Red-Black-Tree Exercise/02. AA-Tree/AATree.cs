namespace _02._AA_Tree
{
    using System;

    public class AATree<T> : IBinarySearchTree<T>
        where T : IComparable<T>
    {
        private Node<T> root;

        public AATree()
        {

        }

        public int CountNodes()
        {
            return this.GetCount(this.root);
        }

        public bool IsEmpty()
        {
            return this.root == null;
        }

        public void Clear()
        {
            this.root = null;
        }

        public void Insert(T element)
        {
            this.root = this.Insert(element, this.root);
        }

        public bool Search(T element)
        {
            Node<T> current = this.FindElement(this.root, element);

            return current != null ? true : false;
        }

        // Left Root Right
        public void InOrder(Action<T> action)
        {
            this.VisitInOrder(this.root, action);
        }

        // Root Left Right
        public void PreOrder(Action<T> action)
        {
            this.VisitPreOrder(this.root, action);
        }

        // Left Right Root
        public void PostOrder(Action<T> action)
        {
            this.VisitPostOrder(this.root, action);
        }

        private Node<T> Insert(T element, Node<T> node)
        {
            if (node == null)
                return new Node<T>(element);

            var comp = element.CompareTo(node.Value);

            if (comp > 0)
                node.Right = this.Insert(element, node.Right);
            if (comp < 0)
                node.Left = this.Insert(element, node.Left);

            node = this.Skew(node);
            node = this.Split(node);
            node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

            return node;
        }

        private int GetCount(Node<T> node)
        {
            if (node == null)
            {
                return 0;
            }
            return node.Count;
        }

        private Node<T> Split(Node<T> node)
        {
            if (node.Level == node.Right?.Right?.Level)
            {
                var temp = node.Right; // 55
                node.Right = temp.Left;
                temp.Left = node;
                temp.Level = node.Level + 1;

                node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

                return temp;
            }
            else
            {
                return node;
            }
        }

        private Node<T> Skew(Node<T> node)
        {
            if (node.Level == node.Left?.Level)
            {
                var temp = node.Left;
                node.Left = temp.Right;
                temp.Right = node;
                node.Count = 1 + this.GetCount(node.Left) + this.GetCount(node.Right);

                return temp;
            }
            else
            {
                return node;
            }
        }

        private void VisitInOrder(Node<T> node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.VisitInOrder(node.Left, action);
            action(node.Value);
            this.VisitInOrder(node.Right, action);
        }

        private void VisitPreOrder(Node<T> node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            action(node.Value);
            this.VisitPreOrder(node.Left, action);
            this.VisitPreOrder(node.Right, action);
        }

        private void VisitPostOrder(Node<T> node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.VisitPostOrder(node.Left, action);
            this.VisitPostOrder(node.Right, action);
            action(node.Value);
        }

        private Node<T> FindElement(Node<T> node, T element)
        {
            if (node == null)
            {
                return null;
            }

            var comp = element.CompareTo(node.Value);

            if (comp > 0)
            {
                return this.FindElement(node.Right, element);
            }
            else if (comp < 0)
            {
                return this.FindElement(node.Left, element);
            }
            else
            {
                return node;
            }
        }
    }
}