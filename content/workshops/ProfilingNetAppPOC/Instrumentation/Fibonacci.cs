using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilingNetAppPOC.Instrumentation
{
    public class Fibonacci : IShowProfiling
    {
        private void Evaluate(
            int firstnumber,
            int secondnumber,
            int count,
            int length)
        {
            if (count <= length)
            {
                Console.Write("{0} ", firstnumber);
                Evaluate(secondnumber, firstnumber + secondnumber, count + 1, length);
            }

        }

        public void showProfiling()
        {
            Console.Write("Length of the Fibonacci Series: ");
            //int length = Convert.ToInt32(Console.ReadLine());
            Evaluate(0, 1, 1, 700);
            Console.ReadKey();
        }
    }
}
