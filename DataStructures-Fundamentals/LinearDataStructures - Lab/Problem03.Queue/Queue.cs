namespace Problem03.Queue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Queue<T> : IAbstractQueue<T>
    {
        private Node<T> _head;

        public Queue()
        {
            this._head = null;
            this.Count = 0;
        }

        public Queue(Node<T> head)
        {
            this._head = head;
            this.Count = 1;
        }

        public int Count { get; private set; }

        public bool Contains(T item)
        {
            Node<T> currNode = this._head;

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

        public T Dequeue()
        {
            this.ValidateIfNotEmpty();

            Node<T> currNode = this._head;
            this._head = this._head.Next;
            this.Count--;
            return currNode.Value;
        }

        public void Enqueue(T item)
        {
            Node<T> currNode = this._head;
            Node<T> toInsert = new Node<T>(item);

            if (currNode == null)
            {
                this._head = toInsert;
            }
            else
            {
                while (currNode.Next != null)
                {
                    currNode = currNode.Next;
                }

                currNode.Next = toInsert;
            }

            this.Count++;
        }

        public T Peek()
        {
            this.ValidateIfNotEmpty();

            return this._head.Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> currNode = this._head;

            while (currNode != null)
            {
                yield return currNode.Value;
                currNode = currNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void ValidateIfNotEmpty()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty!");
            }
        }
    }
}