namespace HashTable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HashTable<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
    {
        private const float LoadFactor = 0.7f;
        private const int DefaultCapacity = 16;

        private LinkedList<KeyValue<TKey, TValue>>[] elements;

        public HashTable(int capacity = DefaultCapacity)
        {
            this.elements = new LinkedList<KeyValue<TKey, TValue>>[capacity];
            this.Count = 0;
        }

        public int Count { get; private set; }

        public int Capacity => this.elements.Length;

        public void Add(TKey key, TValue value)
        {
            GrowIfNeeded();
            int index = FindIndex(key);

            if (this.elements[index] == null)
            {
                this.elements[index] = new LinkedList<KeyValue<TKey, TValue>>();
            }

            foreach (var item in this.elements[index])
            {
                if (item.Key.Equals(key))
                {
                    throw new ArgumentException("Key already exists: " + key);
                }
            }

            KeyValue<TKey, TValue> kvp = new KeyValue<TKey, TValue>(key, value);
            this.elements[index].AddLast(kvp);
            this.Count++;
        }

        public bool AddOrReplace(TKey key, TValue value)
        {
            GrowIfNeeded();
            int index = FindIndex(key);

            if (this.elements[index] == null)
            {
                this.elements[index] = new LinkedList<KeyValue<TKey, TValue>>();
            }

            foreach (var item in this.elements[index])
            {
                if (item.Key.Equals(key))
                {
                    item.Value = value;
                    return false;
                }
            }

            KeyValue<TKey, TValue> kvp = new KeyValue<TKey, TValue>(key, value);
            this.elements[index].AddLast(kvp);
            this.Count++;
            return true;
        }

        public TValue Get(TKey key)
        {
            var kvp = this.Find(key);

            if (kvp == null)
            {
                throw new KeyNotFoundException();
            }

            return kvp.Value;
        }

        public TValue this[TKey key]
        {
            get => this.Get(key);

            set => this.AddOrReplace(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var kvp = this.Find(key);

            if (kvp != null)
            {
                value = kvp.Value;
                return true;
            }

            value = default;
            return false;
        }

        public KeyValue<TKey, TValue> Find(TKey key)
        {
            int index = this.FindIndex(key);

            if (this.elements[index] != null)
            {
                foreach (var kvp in this.elements[index])
                {
                    if (kvp.Key.Equals(key))
                    {
                        return kvp;
                    }
                }
            }

            return null;
        }

        public bool ContainsKey(TKey key)
        {
            return this.Find(key) != null;
        }

        public bool Remove(TKey key)
        {
            var index = this.FindIndex(key);

            var curLinkedList = this.elements[index];
            if (curLinkedList != null)
            {
                var curElement = curLinkedList.First;
                while (curElement != null)
                {
                    if (curElement.Value.Key.Equals(key))
                    {
                        curLinkedList.Remove(curElement);
                        this.Count--;
                        return true;
                    }

                    curElement = curElement.Next;
                }
            }

            return false;
        }

        public void Clear()
        {
            this.elements = new LinkedList<KeyValue<TKey, TValue>>[DefaultCapacity];
            this.Count = 0;
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var curLinkedList in this.elements)
                {
                    if (curLinkedList == null)
                    {
                        continue;
                    }
                    foreach (var kvp in curLinkedList)
                    {
                        yield return kvp.Key;
                    }
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var curLinkedList in this.elements)
                {
                    if (curLinkedList == null)
                    {
                        continue;
                    }
                    foreach (var kvp in curLinkedList)
                    {
                        yield return kvp.Value;
                    }
                }
            }
        }

        public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
        {
            foreach (var curLinkedList in this.elements)
            {
                if (curLinkedList == null)
                {
                    continue;
                }
                foreach (var kvp in curLinkedList)
                {
                    yield return kvp;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private int FindIndex(TKey key)
        {
            return Math.Abs(key.GetHashCode()) % this.Capacity;
        }

        private void GrowIfNeeded()
        {
            float loadFactor = ((float)this.Count + 1) / this.Capacity;

            if (loadFactor >= LoadFactor)
            {
                this.Grow();
            }
        }

        private void Grow()
        {
            var newTable = new HashTable<TKey, TValue>(this.Capacity * 2);

            foreach (var curLinkedList in this.elements)
            {
                if (curLinkedList == null)
                {
                    continue;
                }

                foreach (var kvp in curLinkedList)
                {
                    newTable.Add(kvp.Key, kvp.Value);
                }
            }

            this.elements = newTable.elements;
        }
    }
}
