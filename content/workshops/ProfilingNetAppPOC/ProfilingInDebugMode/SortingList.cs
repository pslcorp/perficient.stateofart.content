using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilingNetAppPOC.ProfilingInDebugMode
{
    public class SortingList : IShowProfiling
    {
        private static List<long> CreateRandomNumbers()
        {
            var random = new Random(0);
            var list = new List<long>();

            for (long i = 0; i < 100000000; i++)
            {
                list.Add(random.Next());
            }

            return list;
        }

        public void showProfiling()
        {
            Console.WriteLine("Start the profiling process");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            List<long> list = CreateRandomNumbers();  //Suspicius

            TimeSpan createTime = stopwatch.Elapsed;
            stopwatch.Restart();
            list.Sort(); //Suspicius

            TimeSpan sortTime = stopwatch.Elapsed;

            double totalTimeTicks = (createTime + sortTime).Ticks;

            Console.WriteLine("CreateList: {0}, {1:P}", createTime, createTime.Ticks / totalTimeTicks);
            Console.WriteLine("Sort:       {0}, {1:P}", sortTime, sortTime.Ticks / totalTimeTicks);
        }
    }
}
