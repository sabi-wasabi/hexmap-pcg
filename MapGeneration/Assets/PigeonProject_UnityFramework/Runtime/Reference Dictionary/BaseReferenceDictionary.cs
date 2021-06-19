using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    public abstract class BaseReferenceDictionary<TKey, TValue> : ScriptableObject, IDictionary<TKey, TValue>
    {
        #region Dictionary properties
        protected IDictionary<TKey, TValue> _register = new Dictionary<TKey, TValue>();

        public TValue this[TKey key] { get => _register[key]; set => _register[key] = value; }
        public ICollection<TKey> Keys => _register.Keys;
        public ICollection<TValue> Values => _register.Values;
        public int Count => _register.Count;
        public bool IsReadOnly => _register.IsReadOnly;
        #endregion

        #region Dictionary methods
        public void Add(TKey key, TValue value) => _register.Add(key, value);
        public void Add(KeyValuePair<TKey, TValue> item) => _register.Add(item);
        public bool ContainsKey(TKey key) => _register.ContainsKey(key);
        public bool Contains(KeyValuePair<TKey, TValue> item) => _register.Contains(item);
        public bool Remove(TKey key) => _register.Remove(key);
        public bool Remove(KeyValuePair<TKey, TValue> item) => _register.Remove(item);
        public bool TryGetValue(TKey key, out TValue value) => _register.TryGetValue(key, out value);
        public void Clear() => _register.Clear();
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _register.CopyTo(array, arrayIndex);
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => _register.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _register.GetEnumerator();
        #endregion
    }
}
