using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PriorityQueue<TPriority, TItem> : IEnumerable<TItem>, IEnumerable<KeyValuePair<TPriority, TItem>>
{
    private readonly SortedDictionary<TPriority, Queue<TItem>> _storage;

    public PriorityQueue() : this(Comparer<TPriority>.Default)
    {

    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
        _storage = new SortedDictionary<TPriority, Queue<TItem>>(comparer);
    }

    public int Count
    {
        get;
        private set;
    }

    public void Enqueue(TPriority priority, TItem item)
    {
        bool can = false;
        int canNumber;
        foreach (var a in _storage)
        {
            canNumber = 0;
            foreach (var b in a.Value)
            {
                if (b.Equals(item))
                {
                    can = true;
                    break;
                }
                canNumber++;
            }
            if (can)
            {
                Queue<TItem> ts = new Queue<TItem>();
                foreach (var b in a.Value)
                {
                    if (!b.Equals(item))
                    {
                        ts.Enqueue(b);
                    }
                }
                TPriority asd = a.Key;
                if (ts.Count == 0)
                {
                    _storage.Remove(a.Key);
                }
                else
                {
                    _storage[a.Key] = ts;
                }
                Count--;
                break;
            }
        }
        if (!_storage.TryGetValue(priority, out var queue))
            _storage[priority] = queue = new Queue<TItem>();
        queue.Enqueue(item);

        Count++;
    }

    public TItem Dequeue()
    {

        if (Count == 0)
            throw new InvalidOperationException("Queue is empty");

        var queue = _storage.First();
        var item = queue.Value.Dequeue();

        if (queue.Value.Count == 0)
            _storage.Remove(queue.Key);

        Count--;
        return item;
    }

    public IEnumerator<KeyValuePair<TPriority, TItem>> GetEnumerator()
    {
        var items = from pair in _storage
                    from item in pair.Value
                    select new KeyValuePair<TPriority, TItem>(pair.Key, item);

        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
    {
        var items = _storage.SelectMany(pair => pair.Value);
        return items.GetEnumerator();
    }
} 

