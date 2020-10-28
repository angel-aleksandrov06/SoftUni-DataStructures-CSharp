namespace _01.Hierarchy
{
    using System.Collections.Generic;

    public class Node<T>
    {
        public Node(T value)
        {
            this.Value = value;
            this.Children = new List<Node<T>>();
        }

        public T Value { get; private set; }

        public Node<T> Parent { get; set; }

        public List<Node<T>> Children { get; private set; }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
