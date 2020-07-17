using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<QueueItem<T>> Queue;

    public PriorityQueue()
    {
        Queue = new List<QueueItem<T>>();
    }

    public void Put(T item, int priority)
    {
        for(int i = 0; i < Queue.Count - 1; i++)
        {
            if(Queue[i].Priority > priority)
            {
                Queue.Insert(i, new QueueItem<T>(item, priority));
                return;
            }
        }
        Queue.Add(new QueueItem<T>(item, priority));
    }

    public T Get()
    {
        var temp = Queue[0].Item;
        Queue.RemoveAt(0);
        return temp;
    }

    public bool IsEmpty()
    {
        return Queue.Count == 0;
    }

    public void Clear()
    {
        Queue.Clear();
    }

    private class QueueItem<ItemClass>
    {
        public ItemClass Item;
        public int Priority;

        public QueueItem(ItemClass _Item, int _Priority)
        {
            Item = _Item;
            Priority = _Priority;
        }
    }
}
