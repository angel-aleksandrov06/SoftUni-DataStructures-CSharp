namespace Problem02.Stack
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Stack<T> : IAbstractStack<T>
    {
        private Node<T> _top;

        public Stack()
        {
            this._top = null;
            this.Count = 0;
        }

        public Stack(Node<T> top)
        {
            this._top = top;
            this.Count = 1; 
        }

        public int Count { get; private set; }

        public bool Contains(T item)
        {
            Node<T> currNode = this._top;

            while (currNode != null)
            {
                if (currNode.Value.Equals(item))
                {
                    return true;
                }

                currNode = currNode.Next;
            }

            return false;
        }

        public T Peek()
        {
            this.ValidateIfEmpty();

            return this._top.Value;
        }

        public T Pop()
        {
            this.ValidateIfEmpty();

            Node<T> toReturn = this._top;

            this._top = this._top.Next;

            this.Count--;

            return toReturn.Value;
        }

        public void Push(T item)
        {
            Node<T> toInsert = new Node<T>(item);

            toInsert.Next = this._top;
            this._top = toInsert;

            this.Count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> currNode = this._top;

            while (currNode != null)
            {
                yield return currNode.Value;
                currNode = currNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() 
            => this.GetEnumerator();


        private void ValidateIfEmpty()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty!");
            }
        }
    }
}