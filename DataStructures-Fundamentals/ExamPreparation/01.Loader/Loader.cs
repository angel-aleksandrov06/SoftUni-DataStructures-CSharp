namespace _01.Loader
{
    using _01.Loader.Interfaces;
    using _01.Loader.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Loader : IBuffer
    {
        private List<IEntity> entities;

        public Loader()
        {
            this.entities = new List<IEntity>();
        }

        public int EntitiesCount => this.entities.Count;

        // O(1) amortized
        public void Add(IEntity entity)
        {
            this.entities.Add(entity);
        }

        public void Clear()
        {
            this.entities.Clear();
        }

        public bool Contains(IEntity entity)
        {
            // return this.entities.Contains(entity);
            return this.GetById(entity.Id) != null;
        }

        // TODO: Possible optimization
        public IEntity Extract(int id)
        {
            IEntity found = this.GetById(id);

            if (found != null)
            {
                this.entities.Remove(found);
                return found;
            }

            return null;
        }

        public IEntity Find(IEntity entity)
        {
            return this.GetById(entity.Id);
        }

        public List<IEntity> GetAll()
        {
            return new List<IEntity>(this.entities);
        }

        public void RemoveSold()
        {
            this.entities.RemoveAll(x => x.Status == BaseEntityStatus.Sold);
        }

        public void Replace(IEntity oldEntity, IEntity newEntity)
        {
            int indexOfEntity = this.entities.IndexOf(oldEntity);
            this.ValidateEntity(indexOfEntity);
            this.entities[indexOfEntity] = newEntity;
        }

        public List<IEntity> RetainAllFromTo(BaseEntityStatus lowerBound, BaseEntityStatus upperBound)
        {
            var result = new List<IEntity>(this.EntitiesCount);
            int lowerBoundIndex = (int)lowerBound;
            int upperBoundIndex = (int)upperBound;

            for (int i = 0; i < this.entities.Count; i++)
            {
                var currEntity = this.entities[i];
                var entityStatusNumber = (int)currEntity.Status;

                if (entityStatusNumber >= lowerBoundIndex && entityStatusNumber <= upperBoundIndex)
                {
                    result.Add(currEntity);
                }
            }

            return result;
        }

        public void Swap(IEntity first, IEntity second)
        {
            int indexOfFirstEntity = this.entities.IndexOf(first);
            int indexOfSecondEntity = this.entities.IndexOf(second);

            this.ValidateEntity(indexOfFirstEntity);
            this.ValidateEntity(indexOfSecondEntity);

            var temp = this.entities[indexOfFirstEntity];
            this.entities[indexOfFirstEntity] = this.entities[indexOfSecondEntity];
            this.entities[indexOfSecondEntity] = temp;
        }

        public IEntity[] ToArray()
        {
            return this.entities.ToArray();
        }

        public void UpdateAll(BaseEntityStatus oldStatus, BaseEntityStatus newStatus)
        {
            for (int i = 0; i < this.EntitiesCount; i++)
            {
                var currEntity = this.entities[i];

                if (currEntity.Status == oldStatus)
                {
                    currEntity.Status = newStatus;
                }
            }
        }
        
        public IEnumerator<IEntity> GetEnumerator()
        {
            return this.entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEntity GetById(int id)
        {
            //this.entities.Find(e => e.Id == id);

            for (int i = 0; i < this.EntitiesCount; i++)
            {
                var currEntity = this.entities[i];

                if (currEntity.Id == id)
                {
                    return currEntity;
                }
            }
            return null;
        }

        private void ValidateEntity(int index)
        {
            if (index < 0)
            {
                throw new InvalidOperationException("Entity not found");
            }
        }
    }
}
