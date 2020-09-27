namespace _03.MinHeap
{
    using System;
    using System.Collections.Generic;

    public class MinHeap<T> : IAbstractHeap<T>
        where T : IComparable<T>
    {
        private List<T> _elements;

        public MinHeap()
        {
            this._elements = new List<T>();
        }

        public int Size => this._elements.Count;

        public T Dequeue()
        {
            var firstElement = this.Peek();

            this.Swap(0, this.Size - 1);
            this._elements.RemoveAt(this.Size - 1);
            this.heapifyDown();

            return firstElement;
        }

        private void heapifyDown()
        {
            var currIndex = 0;
            var leftChildIndex = this.GetLeftChildIndex(currIndex);

            while (this.IsIndexValid(leftChildIndex) && this.IsGreater(currIndex, leftChildIndex))
            {
                int toSwapWith = leftChildIndex;
                int rightChildIndex = this.GetRightChildIndex(currIndex);

                if (this.IsIndexValid(rightChildIndex) && this.IsGreater(toSwapWith, rightChildIndex))
                {
                    toSwapWith = rightChildIndex;
                }

                this.Swap(toSwapWith, currIndex);
                currIndex = toSwapWith;
                leftChildIndex = this.GetLeftChildIndex(currIndex);
            }
        }

        public void Add(T element)
        {
            this._elements.Add(element);

            this.heapifyUp();
        }

        public T Peek()
        {
            this.EnsureNotEmpty();

            return this._elements[0];
        }

        private void heapifyUp()
        {
            int currIndex = this.Size - 1;
            var parentIndex = this.GetParentIndex(currIndex);

            while (this.IsIndexValid(currIndex) && this.IsLess(currIndex, parentIndex))
            {
                this.Swap(currIndex, parentIndex);

                currIndex = parentIndex;
                parentIndex = this.GetParentIndex(currIndex);
            }
        }

        private void Swap(int currIndex, int parentIndex)
        {
            var temp = this._elements[currIndex];
            this._elements[currIndex] = this._elements[parentIndex];
            this._elements[parentIndex] = temp;
        }

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        private int GetLeftChildIndex(int parentIndex)
        {
            return 2 * parentIndex +1;
        }

        private int GetRightChildIndex(int parentIndex)
        {
            return 2 * parentIndex + 2;
        }

        private void EnsureNotEmpty()
        {
            if (this.Size == 0)
            {
                throw new InvalidOperationException();
            }
        }

        private bool IsIndexValid(int index)
        {
            return index >= 0 && index < this.Size;
        }

        private bool IsGreater(int childIndex, int parentIndex)
        {
            return this._elements[childIndex].CompareTo(this._elements[parentIndex]) > 0;
        }

        private bool IsLess(int childIndex, int parentIndex)
        {
            return this._elements[childIndex].CompareTo(this._elements[parentIndex]) < 0;
        }
    }
}
