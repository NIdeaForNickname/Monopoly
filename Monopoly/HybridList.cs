using System.Collections;

namespace Utilities;

public class HybridList<T>: IEnumerable<T>
{
    private List<T> _list;
    private Dictionary<string, T> _dictionary;

    public HybridList()
    {
        _list = new List<T>();
        _dictionary = new Dictionary<string, T>();
    }
    public HybridList<T> Add(string key, T item)
    {
        if (_dictionary.ContainsKey(key))
        {
            throw new ArgumentException("An item with the same key has already been added.");
        }

        _list.Add(item);
        _dictionary[key] = item;
        return this;
    }

    public bool Remove(string key)
    {
        if (_dictionary.TryGetValue(key, out T item))
        {
            _list.Remove(item);
            _dictionary.Remove(key);
            return true;
        }
        return false;
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= _list.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        T item = _list[index];
        _list.RemoveAt(index);

        string keyToRemove = null;
        foreach (var kvp in _dictionary)
        {
            if (EqualityComparer<T>.Default.Equals(kvp.Value, item))
            {
                keyToRemove = kvp.Key;
                break;
            }
        }

        if (keyToRemove != null)
        {
            _dictionary.Remove(keyToRemove);
            return true;
        }

        return false;
    }

    public T this[string key]
    {
        get
        {
            if (_dictionary.TryGetValue(key, out T item))
            {
                return item;
            }
            throw new KeyNotFoundException("The given key was not present in the dictionary.");
        }
        set
        {
            if (_dictionary.ContainsKey(key))
            {
                T oldItem = _dictionary[key];
                int index = _list.IndexOf(oldItem);
                if (index != -1)
                {
                    _list[index] = value;
                }
                _dictionary[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return _list[index];
        }
        set
        {
            if (index < 0 || index >= _list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            T oldItem = _list[index];
            _list[index] = value;

            string keyToUpdate = null;
            foreach (var kvp in _dictionary)
            {
                if (EqualityComparer<T>.Default.Equals(kvp.Value, oldItem))
                {
                    keyToUpdate = kvp.Key;
                    break;
                }
            }

            if (keyToUpdate != null)
            {
                _dictionary[keyToUpdate] = value;
            }
        }
    }

    public int Count => _list.Count;
    public IEnumerable<string> Keys => _dictionary.Keys;
    public IEnumerable<T> Values => _list;
    
    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator(); // Iterate in insertion order
    }

    public int IndexOf(T i)
    {
        return _list.IndexOf(i);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}