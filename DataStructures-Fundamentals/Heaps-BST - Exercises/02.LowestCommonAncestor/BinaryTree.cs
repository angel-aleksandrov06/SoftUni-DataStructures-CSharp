namespace _02.LowestCommonAncestor
{
    using System;

    public class BinaryTree<T> : IAbstractBinaryTree<T>
        where T : IComparable<T>
    {
        public BinaryTree(
            T value,
            BinaryTree<T> leftChild,
            BinaryTree<T> rightChild)
        {
            this.Value = value;
            this.RightChild = rightChild;
            this.LeftChild = leftChild;

            if (this.RightChild != null)
            {
                this.RightChild.Parent = this;
            }

            if (this.LeftChild != null)
            {
                this.LeftChild.Parent = this;
            }
        }

        public T Value { get; set; }

        public BinaryTree<T> LeftChild { get; set; }

        public BinaryTree<T> RightChild { get; set; }

        public BinaryTree<T> Parent { get; set; }

        public T FindLowestCommonAncestor(T first, T second)
        {
            throw new NotImplementedException();
        }
    }
}
