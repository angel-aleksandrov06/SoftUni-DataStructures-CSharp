namespace _02.MaxHeap
{
    using System;
    using System.Collections.Generic;

    public class MaxHeap<T> : IAbstractHeap<T>
        where T : IComparable<T>
    {
        private List<T> elements;

        public MaxHeap()
        {
            this.elements = new List<T>();
        }

        public int Size => elements.Count;

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

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        private bool IndexIsValid(int index)
        {
            return index > 0;
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
