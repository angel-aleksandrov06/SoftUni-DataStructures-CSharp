namespace _03.PriorityQueue
{
    using System;
    using System.Collections.Generic;

    public class PriorityQueue<T> : IAbstractHeap<T>
        where T : IComparable<T>
    {
        private List<T> elements;

        public PriorityQueue()
        {
            this.elements = new List<T>();
        }

        public int Size => this.elements.Count;

        public T Dequeue()
        {
            var firstElement = this.Peek();

            this.Swap(0, this.Size - 1);
            this.elements.RemoveAt(this.Size - 1);
            this.HeapifyDown();

            return firstElement;
        }

        public void Add(T element)
        {
            this.elements.Add(element);

            this.HeapifyUp();
        }

        public T Peek()
        {
            this.EnsureNotEmpty();

            return this.elements[0];
        }

        private void HeapifyUp()
        {
            int currIndex = this.Size - 1;
            var parentIndex = this.GetParentIndex(currIndex);

            while (this.IndexIsValid(currIndex) && this.IsGreater(currIndex, parentIndex))
            {
                this.Swap(currIndex, parentIndex);

                currIndex = parentIndex;
                parentIndex = this.GetParentIndex(currIndex);
            }
        }

        private void HeapifyDown()
        {
            int index = 0;
            int leftChildIndex = this.GetLeftChildIndex(index);

            while (this.IndexIsValid(leftChildIndex) && this.IsLess(index, leftChildIndex))
            {
                int toSwapWith = leftChildIndex;
                int rigthChildIndex = this.GetRigthChildIndex(index);

                if (this.IndexIsValid(rigthChildIndex) && this.IsLess(toSwapWith, rigthChildIndex))
                {
                    toSwapWith = rigthChildIndex;
                }

                this.Swap(toSwapWith, index);

                index = toSwapWith;
                leftChildIndex = this.GetLeftChildIndex(index);
            }
        }

        private void Swap(int currIndex, int parentIndex)
        {
            var temp = this.elements[currIndex];
            this.elements[currIndex] = this.elements[parentIndex];
            this.elements[parentIndex] = temp;
        }

        private bool IsGreater(int childIndex, int parentIndex)
        {
            return this.elements[childIndex].CompareTo(this.elements[parentIndex]) > 0;
        }

        private bool IsLess(int childIndex, int parentIndex)
        {
            return this.elements[childIndex].CompareTo(this.elements[parentIndex]) < 0;
        }

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        private int GetLeftChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 1;
        }

        private int GetRigthChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 2;
        }

        private bool IndexIsValid(int index)
        {
            return index < this.Size;
        }

        private void EnsureNotEmpty()
        {
            if (this.Size == 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
