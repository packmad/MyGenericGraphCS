using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGenericGraph
{

    public class PriorityQueue<TP, TV>
    {
        private readonly SortedDictionary<TP, Queue<TV>> _priorityQueue = new SortedDictionary<TP, Queue<TV>>();
        
        public void Enqueue(TP priority, TV value)
        {
            Queue<TV> q;

            if (!_priorityQueue.TryGetValue(priority, out q))
            {
                q = new Queue<TV>();
                _priorityQueue.Add(priority, q);
            }
            q.Enqueue(value);
        }


        public TV Dequeue()
        {
            if (IsEmpty)
                throw new ArgumentOutOfRangeException(GetType() + ": priority queue is empty");
            var pair = _priorityQueue.First(); // 
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                _priorityQueue.Remove(pair.Key);
            return v;
        }


        public void SafeChangePriority(TP oldPriority, TP newPriority, TV value)
        {
            if (_priorityQueue.ContainsKey(oldPriority) && _priorityQueue[oldPriority].Contains(value))
            {
                var valuesArray = _priorityQueue[oldPriority].ToArray();
                _priorityQueue.Remove(oldPriority);
                foreach (TV v in valuesArray)
                {
                    if (!v.Equals(value))
                        Enqueue(oldPriority, v);
                }
            }
            Enqueue(newPriority, value);
        }


        public bool IsEmpty
        {
            get { return !_priorityQueue.Any(); }
        }
    }
}
