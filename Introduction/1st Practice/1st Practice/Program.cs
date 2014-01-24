using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1st_Practice
{
    class square
    {
        static void Main()
        {
            int[] a1 = new int[5];
            for (int i = 0; i < a1.Length; i++)
            {
                a1[i] = i * i;
                Console.WriteLine("a1{0}={1}", i, a1[i]);
            }

        }
    }
}
