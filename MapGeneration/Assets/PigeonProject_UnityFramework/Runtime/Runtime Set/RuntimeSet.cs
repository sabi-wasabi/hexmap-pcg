// The Pigeon Protocol

using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    public abstract class RuntimeSet<T> : ScriptableObject where T : Object
    {
        #region Properties
        [HideInInspector]
        [SerializeField]
        private List<T> _items = new List<T>();

        public int Count { get => _items.Count; }
        public List<T> Items { get => _items; }
        #endregion


        #region Public methods
        public virtual void Add(T item)
        {
            if (!_items.Contains(item))
                _items.Add(item);
            else
                Debug.LogWarning($"Can not add {item.name}: {item.name} already exists in {name}!");
        }

        public virtual void Remove(T item)
        {
            if (!_items.Remove(item))
                Debug.LogWarning($"Can not remove {item.name}: {item.name} does not exist in {name}!");
        }

        public virtual void Clear()
        {
            for (int i=_items.Count-1; i>=0; i--)
            {
                Remove(_items[i]);
            }
        }

        public virtual bool Contains(T item) => _items.Contains(item);
        #endregion

        #region Private methods
        private void OnEnable()
        {
            Clear();
        }
        #endregion
    }
}
