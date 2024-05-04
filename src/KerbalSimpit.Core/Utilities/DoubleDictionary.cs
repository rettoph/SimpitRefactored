using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Utilities
{
    public class DoubleDictionary<TKey1, TKey2, TValue>
    {
        private Dictionary<TKey1, TValue> _dic1;
        private Dictionary<TKey2, TValue> _dic2;

        public TValue this[TKey1 key] => _dic1[key];
        public TValue this[TKey2 key] => _dic2[key];

        public ICollection<TValue> Values => _dic1.Values;
        public ICollection<TKey1> Keys1 => _dic1.Keys;
        public ICollection<TKey2> Keys2 => _dic2.Keys;

        public DoubleDictionary()
        {
            _dic1 = new Dictionary<TKey1, TValue>();
            _dic2 = new Dictionary<TKey2, TValue>();
        }

        public DoubleDictionary(int capacity)
        {
            _dic1 = new Dictionary<TKey1, TValue>(capacity);
            _dic2 = new Dictionary<TKey2, TValue>(capacity);
        }

        public DoubleDictionary(IEnumerable<(TKey1, TKey2, TValue)> items) : this(items.Count())
        {
            foreach (var kkv in items)
            {
                this.Add(kkv.Item1, kkv.Item2, kkv.Item3);
            }
        }

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            _dic1.Add(key1, value);

            try
            {
                _dic2.Add(key2, value);
            }
            catch (Exception ex)
            {
                _dic1.Remove(key1);
                throw ex;
            }
        }

        public bool TryGet(TKey1 key1, out TValue value)
        {
            return _dic1.TryGetValue(key1, out value);
        }

        public bool TryGet(TKey2 key2, out TValue value)
        {
            return _dic2.TryGetValue(key2, out value);
        }

        public bool Remove(TKey1 key1, TKey2 key2)
        {
            return _dic1.Remove(key1) && _dic2.Remove(key2);
        }
    }
}
