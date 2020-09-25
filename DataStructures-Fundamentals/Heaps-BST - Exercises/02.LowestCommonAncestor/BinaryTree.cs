﻿namespace _02.LowestCommonAncestor
{
    using System;
    using System.Collections.Generic;

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
            var firstList = new List<BinaryTree<T>>();
            var secondList = new List<BinaryTree<T>>();

            this.FindNodeDFS(this, first, firstList);
            this.FindNodeDFS(this, second, secondList);

            var firstNode = firstList[0];
            var secondNode = secondList[0];

            T parentToLookFor = firstNode.Parent.Value;
            while (!parentToLookFor.Equals(firstNode.Value) || !parentToLookFor.Equals(secondNode.Value))
            {
                if (!parentToLookFor.Equals(firstNode.Value))
                {
                    firstNode = firstNode.Parent;
                }

                if (!parentToLookFor.Equals(secondNode.Value))
                {
                    secondNode = secondNode.Parent;
                }
            }

            return firstNode.Value;
        }

        private void FindNodeDFS(BinaryTree<T> currNode, T lookUpValue, List<BinaryTree<T>> list)
        {
            if (currNode == null)
            {
                return;
            }

            if (currNode.Value.Equals(lookUpValue))
            {
                list.Add(currNode);
                return;
            }

            this.FindNodeDFS(currNode.LeftChild, lookUpValue, list);
            this.FindNodeDFS(currNode.RightChild, lookUpValue, list);
        }
    }
}
