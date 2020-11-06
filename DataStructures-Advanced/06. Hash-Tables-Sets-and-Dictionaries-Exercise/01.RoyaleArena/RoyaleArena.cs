namespace _01.RoyaleArena
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class RoyaleArena : IArena
    {
        private readonly Dictionary<int, BattleCard> byId = new Dictionary<int, BattleCard>();
        private readonly Dictionary<CardType, OrderedBag<BattleCard>> byType = new Dictionary<CardType, OrderedBag<BattleCard>>();

        public int Count
            => this.byId.Count;

        public void Add(BattleCard card)
        {
            if (card == null || this.Contains(card))
            {
                return;
            }

            // By Id
            this.byId[card.Id] = card;

            // By Type
            if (!this.byType.ContainsKey(card.Type))
            {
                this.byType[card.Type] = new OrderedBag<BattleCard>();
            }
            this.byType[card.Type].Add(card);
        }

        public void ChangeCardType(int id, CardType type)
        {
            if (!this.byId.ContainsKey(id))
            {
                throw new ArgumentException();
            }

            var card = this.byId[id];
            if (card.Type == type) // no change
            {
                return;
            }

            card.Type = type;
        }

        public bool Contains(BattleCard card)
            => card != null
            && this.byId.ContainsKey(card.Id);

        public IEnumerable<BattleCard> FindFirstLeastSwag(int n)
        {
            if (n < 0 || n > this.Count)
            {
                throw new InvalidOperationException();
            }

            return this.byId.Values
                .OrderBy(c => c.Swag)
                .ThenBy(c => c.Id)
                .Take(n);
        }

        public IEnumerable<BattleCard> GetAllByNameAndSwag()
        {
            var result = new List<BattleCard>();

            this.byId.Values
                 .GroupBy(c => c.Name)
                 .ToList()
                 .ForEach(group => result.Add(group.OrderBy(c => c).First()));

            return result;
        }

        public IEnumerable<BattleCard> GetAllInSwagRange(double lo, double hi)
             => this.byId.Values
            .Where(c => lo <= c.Swag && c.Swag <= hi)
            .OrderBy(c => c.Swag);

        public IEnumerable<BattleCard> GetByCardType(CardType type)
        {
            this.ValidateWithException(type);

            return this.byType[type];
        }

        public IEnumerable<BattleCard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
        {
            this.ValidateWithException(type);

            var result = this.byType[type]
                .Where(c => c.Damage <= damage);

            this.ValidateWithException(result);

            return result;
        }

        public BattleCard GetById(int id)
        {
            if (!this.byId.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            return this.byId[id];
        }

        public IEnumerable<BattleCard> GetByNameAndSwagRange(string name, double lo, double hi)
        {
            var result = this.byId.Values
                .Where(c => c.Name == name)
                .Where(c => lo <= c.Swag && c.Swag < hi)
                .OrderByDescending(c => c.Swag)
                .ThenBy(c => c.Id);

            this.ValidateWithException(result);

            return result;
        }

        public IEnumerable<BattleCard> GetByNameOrderedBySwagDescending(string name)
        {
            var result = this.byId.Values
                .Where(c => c.Name == name)
                .OrderByDescending(c => c.Swag)
                .ThenBy(c => c.Id);

            this.ValidateWithException(result);

            return result;
        }

        public IEnumerable<BattleCard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
        {
            this.ValidateWithException(type);

            return this.byType[type]
                .Where(c => lo < c.Damage && c.Damage < hi);
        }

        public void RemoveById(int id)
        {
            if (!this.byId.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            var card = this.byId[id];

            this.byId.Remove(id);

            this.byType[card.Type].Remove(card);
            if (!this.byType[card.Type].Any())
            {
                this.byType.Remove(card.Type);
            }
        }

        public IEnumerator<BattleCard> GetEnumerator()
        {
            foreach (var card in this.byId.Values)
            {
                yield return card;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void ValidateWithException(CardType type)
        {
            if (!this.byType.ContainsKey(type))
            {
                throw new InvalidOperationException();
            }
        }

        private void ValidateWithException<T>(IEnumerable<T> result)
        {
            if (!result.Any())
            {
                throw new InvalidOperationException();
            }
        }
    }
}