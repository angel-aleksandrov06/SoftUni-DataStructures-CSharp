namespace _01.BinaryTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BinaryTree<T> : IAbstractBinaryTree<T>
    {
        public BinaryTree(T value
            , IAbstractBinaryTree<T> leftChild
            , IAbstractBinaryTree<T> rightChild)
        {
            this.Value = value;
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        public T Value { get; private set; }

        public IAbstractBinaryTree<T> LeftChild { get; private set; }

        public IAbstractBinaryTree<T> RightChild { get; private set; }

        public string AsIndentedPreOrder(int indent)
        {
            StringBuilder sb = new StringBuilder();

            this.AsIndentedPreOrderDFS(this, indent, sb);

            return sb.ToString();
        }

        public List<IAbstractBinaryTree<T>> InOrder()
        {
            var inOrderElements = new List<IAbstractBinaryTree<T>>();

            if (this.LeftChild != null)
            {
                inOrderElements.AddRange(this.LeftChild.InOrder());
            }

            inOrderElements.Add(this);

            if (this.RightChild != null)
            {
                inOrderElements.AddRange(this.RightChild.InOrder());
            }

            return inOrderElements;
        }

        public List<IAbstractBinaryTree<T>> PostOrder()
        {
            var postOrderElements = new List<IAbstractBinaryTree<T>>();

            if (this.LeftChild != null)
            {
                postOrderElements.AddRange(this.LeftChild.PostOrder());
            }

            if (this.RightChild != null)
            {
                postOrderElements.AddRange(this.RightChild.PostOrder());
            }

            postOrderElements.Add(this);

            return postOrderElements;
        }

        public List<IAbstractBinaryTree<T>> PreOrder()
        {
            var preOrderElements = new List<IAbstractBinaryTree<T>>();

            preOrderElements.Add(this);

            if (this.LeftChild != null)
            {
                preOrderElements.AddRange(this.LeftChild.PreOrder());
            }

            if (this.RightChild != null)
            {
                preOrderElements.AddRange(this.RightChild.PreOrder());
            }

            return preOrderElements;
        }

        public void ForEachInOrder(Action<T> action)
        {
            if (this.LeftChild != null)
            {
                this.LeftChild.ForEachInOrder(action);
            }

            action.Invoke(this.Value);

            if (this.RightChild != null)
            {
                this.RightChild.ForEachInOrder(action);
            }
        }

        private void AsIndentedPreOrderDFS(IAbstractBinaryTree<T> curr, int indent, StringBuilder sb)
        {
            sb.AppendLine($"{new string(' ', indent)}{curr.Value}");

            if (curr.LeftChild != null)
            {
                this.AsIndentedPreOrderDFS(curr.LeftChild, indent + 2, sb);
            }

            if (curr.RightChild != null)
            {
                this.AsIndentedPreOrderDFS(curr.RightChild, indent + 2, sb);
            }
        }
    }
}
