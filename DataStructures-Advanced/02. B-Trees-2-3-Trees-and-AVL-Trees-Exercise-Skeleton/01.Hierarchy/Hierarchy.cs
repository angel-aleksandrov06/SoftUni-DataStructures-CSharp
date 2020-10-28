namespace _01.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    public class Hierarchy<T> : IHierarchy<T>
    {
        private readonly Node<T> root;
        private Dictionary<T, Node<T>> elements = new Dictionary<T, Node<T>>();

        public Hierarchy(T root)
        {
            this.root = this.CreateNode(root);
        }

        public int Count => this.elements.Count;

        public void Add(T element, T child)
        {
            this.IsElementExist(element);

            if ( this.elements.ContainsKey(child))
            {
                throw new ArgumentException();
            }

            var node = CreateNode(child);
            node.Parent = elements[element];
            elements[element].Children.Add(node);
        }

        public void Remove(T element)
        {
            if (element.Equals(this.root.Value))
            {
                throw new InvalidOperationException();
            }

            this.IsElementExist(element);

            this.RemoveElement(element);
        }

        public IEnumerable<T> GetChildren(T element)
        {
            this.IsElementExist(element);

            foreach (var child in this.elements[element].Children)
            {
                yield return child.Value;
            }
        }

        public T GetParent(T element)
        {
            this.IsElementExist(element);

            var node = this.elements[element];

            if (node.Parent != null)
            {
                return node.Parent.Value;
            }

            return default;
        }

        public bool Contains(T element)
        {
            if (this.elements.ContainsKey(element))
            {
                return true;
            }

            return false;
        }

        public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
        {
            foreach (var element in this.elements)
            {
                if (other.Contains(element.Value.Value))
                {
                    yield return element.Value.Value;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var queue = new Queue<Node<T>>();

            queue.Enqueue(this.root);

            while (queue.Count > 0)
            {
                var curNode = queue.Dequeue();

                foreach (var child in curNode.Children)
                {
                    queue.Enqueue(child);
                }

                yield return curNode.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void IsElementExist(T element)
        {
            if (!this.elements.ContainsKey(element))
            {
                throw new ArgumentException();
            }
        }

        private Node<T> CreateNode(T element)
        {
            var node = new Node<T>(element);
            elements[element] = node;

            return node;
        }

        private void RemoveElement(T element)
        {
            var node = elements[element];

            node.Parent?.Children.Remove(node);

            if (node.Parent != null && node.Children.Count > 0)
            {
                foreach (var child in node.Children)
                {
                    child.Parent = node.Parent;
                    node.Parent.Children.Add(child);
                }
            }
            elements.Remove(element);
        }
    }
}