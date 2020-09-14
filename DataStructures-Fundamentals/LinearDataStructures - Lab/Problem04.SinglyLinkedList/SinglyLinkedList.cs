namespace Problem04.SinglyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SinglyLinkedList<T> : IAbstractLinkedList<T>
    {
        private Node<T> _head;

        public SinglyLinkedList()
        {
            this._head = null;
            this.Count = 0;
        }

        public SinglyLinkedList(Node<T> head)
        {
            this._head = head;
            this.Count = 1;
        }

        public int Count { get; private set; }

        public void AddFirst(T item)
        {
            Node<T> toInsert = new Node<T>(item, this._head);
            this._head = toInsert;
            this.Count++;
        }

        public void AddLast(T item)
        {
            Node<T> toInsert = new Node<T>(item);
            Node<T> currNode = this._head;

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

        public T GetFirst()
        {
            ValidateIfListIsNotEmpty();

            return this._head.Value;
        }


        public T GetLast()
        {
            ValidateIfListIsNotEmpty();

            Node<T> currNode = this._head;

            while (currNode.Next != null)
            {
                currNode = currNode.Next;
            }

            return currNode.Value;
        }

        public T RemoveFirst()
        {
            ValidateIfListIsNotEmpty();
            Node<T> firtsElement = this._head;
            this._head = this._head.Next;
            this.Count--;

            return firtsElement.Value;
        }

        public T RemoveLast()
        {
            ValidateIfListIsNotEmpty();
            Node<T> lastNode = null;

            if (this.Count == 1)
            {
                lastNode = this._head;
                this._head = null;
            }
            else
            {
                Node<T> currNode = this._head;

                while (currNode.Next.Next != null)
                {
                    currNode = currNode.Next;
                }

                lastNode = currNode.Next;
                currNode.Next = null;
            }

            this.Count--;

            return lastNode.Value;
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

        private void ValidateIfListIsNotEmpty()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("Linked List is empty!");
            }
        }
    }
}