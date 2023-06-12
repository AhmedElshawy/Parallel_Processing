using System;
using System.Collections.Generic;

namespace Parallel_Processing
{
    public class ThreadObj
    {
        public int Id { get; set; }
        public long Time { get; set; }
    }
    public class ThreadsPriorityQueue
    {
        private List<ThreadObj> _values = new List<ThreadObj>();

        public int GetSize()
        {
            return _values.Count;
        }

        private int GetParent(int i)
        {
            return (i - 1) / 2;
        }

        private void Swap(int thread1Index , int thread2Index)
        {
            var temp = _values[thread1Index];
            _values[thread1Index] = _values[thread2Index];
            _values[thread2Index] = temp;
        }

        private bool IsAvaliableForSiftDown(int leftOrRightIndex , int minIndex)
        {
            bool flag = false;

            if (_values[leftOrRightIndex].Time < _values[minIndex].Time)
            {
                flag = true;
            }
            else if (_values[leftOrRightIndex].Time == _values[minIndex].Time)
            {
                if (_values[leftOrRightIndex].Id < _values[minIndex].Id)
                {
                    flag = true;
                }
            }

            return flag;
        }

        private void SiftDown(int i)
        {
            int minIdenx = i;
            int leftIdenx = i * 2 + 1;

            if (leftIdenx < _values.Count && IsAvaliableForSiftDown(leftIdenx,minIdenx))
            {
                minIdenx = leftIdenx;
            }

            int rightIndex = i * 2 + 2;
            if (rightIndex < _values.Count && IsAvaliableForSiftDown(rightIndex, minIdenx))
            {
                minIdenx = rightIndex;
            }

            if (minIdenx != i)
            {
                Swap(i, minIdenx);
                SiftDown(minIdenx);
            }
        }

        private bool IsAvaliableForSiftUp(int i)
        {
            bool flag = false;

            if (_values[GetParent(i)].Time > _values[i].Time)
            {
                flag = true;
            }
            else if(_values[GetParent(i)].Time == _values[i].Time)
            {
                if (_values[GetParent(i)].Id > _values[i].Id)
                {
                    flag = true;
                }
            }

            return flag;
        }
        private void SiftUp(int i)
        {
            while (i > 0 && IsAvaliableForSiftUp(i))
            {
                Swap(GetParent(i), i);
                i = GetParent(i);
            }
        }

        public void Insert(ThreadObj value)
        {
            _values.Add(value);
            SiftUp(GetSize() - 1);
        }

        public ThreadObj GetHeighstPriority()
        {
            return _values[0];
        }

        public void ChangePriority(int i, ThreadObj updatedthreadObj)
        {
            var oldThread = _values[i];
            _values[i] = updatedthreadObj;

            if (updatedthreadObj.Time < oldThread.Time || updatedthreadObj.Id < oldThread.Id)
                SiftUp(i);
            else
                SiftDown(i);
        }

        public ThreadObj ExtractMax()
        {
            var result = _values[0];
            _values[0] = _values[GetSize() - 1];
            _values.RemoveAt(GetSize() - 1);

            SiftDown(0);
            return result;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            string[] firstLineInputs = Console.ReadLine().Split(' ');
            int n = int.Parse(firstLineInputs[0]);
            int m = int.Parse(firstLineInputs[1]);

            string[] secondLineInputs = Console.ReadLine().Split(' ');
            int[] jobs = new int[m];

            for (int i = 0; i < jobs.Length; i++)
            {
                jobs[i] = int.Parse(secondLineInputs[i]);
            }

            List<ThreadObj> threads = new List<ThreadObj>();
            for (int i = 0; i < n; i++)
            {
                threads.Add(new ThreadObj() { Id = i, Time = 0 });
            }

            //create threads queue
            var threadsQueue = new ThreadsPriorityQueue();
            foreach (var item in threads)
            {
                threadsQueue.Insert(item);
            }

            //Excute jobs
            foreach (var job in jobs)
            {
                var heighstPriorityThread = threadsQueue.ExtractMax();
                Console.WriteLine($"{heighstPriorityThread.Id} {heighstPriorityThread.Time}");
                var accTime = heighstPriorityThread.Time + job;
                threadsQueue.Insert(new ThreadObj() { Id = heighstPriorityThread.Id, Time = accTime });
            }
        }
    }
}