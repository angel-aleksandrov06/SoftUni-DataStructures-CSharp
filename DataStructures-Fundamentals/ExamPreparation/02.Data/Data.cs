namespace _02.Data
{
    using _02.Data.Interfaces;
    using _02.Data.Models;
    using System;
    using System.Collections.Generic;
    using Wintellect.PowerCollections;

    public class Data : IRepository
    {
        private OrderedBag<IEntity> bag;

        public Data()
        {
            this.bag = new OrderedBag<IEntity>();
        }

        public Data(Data copy)
        {
            this.bag = copy.bag;
        }

        public int Size => this.bag.Count;

        public void Add(IEntity entity)
        {
            this.bag.Add(entity);

            var parentNode = this.GetById((int)entity.ParentId);

            if (parentNode != null)
            {
                parentNode.Children.Add(entity);
            }
        }

        public IRepository Copy()
        {
            Data copy = (Data)this.MemberwiseClone();

            return new Data(copy);
        }

        public IEntity DequeueMostRecent()
        {
            this.EnsureNotEmpty();

            return this.bag.RemoveFirst();
        }

        public List<IEntity> GetAll()
        {
            return new List<IEntity>(this.bag);
        }

        public List<IEntity> GetAllByType(string type)
        {
            if (type != typeof(Invoice).Name && type != typeof(StoreClient).Name && type != typeof(User).Name)
            {
                throw new InvalidOperationException("Invalid type: " + type);
            }

            var result = new List<IEntity>(this.Size);

            for (int i = 0; i < this.Size; i++)
            {
                var currEntity = this.bag[i];

                if (currEntity.GetType().Name == type)
                {
                    result.Add(currEntity);
                }
            }

            return result;
        }

        public IEntity GetById(int id)
        {
            if (id < 0 || id >= this.Size)
            {
                return null;
            }

            return this.bag[this.Size -1 - id];
        }

        public List<IEntity> GetByParentId(int parentId)
        {
            var parentNode = this.GetById(parentId);

            if (parentNode == null)
            {
                return new List<IEntity>();
            }

            return parentNode.Children;
        }

        public IEntity PeekMostRecent()
        {
            this.EnsureNotEmpty();

            return this.bag.GetFirst();
        }

        private void EnsureNotEmpty()
        {
            if (this.Size <= 0)
            {
                throw new InvalidOperationException("Operation on empty Data");
            }
        }
    }
}
