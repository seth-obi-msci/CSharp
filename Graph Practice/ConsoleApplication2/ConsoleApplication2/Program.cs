using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Solvers;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] xInitial = new double[] { 1, 1 };
            double[] xLower = new double[] { -1, -1 };
            double[] xUpper = new double[] { 1, 1 };

            var solution = NelderMeadSolver.Solve(
x => (100 * Math.Pow(x[1] - Math.Pow(x[0], 2), 2)) + Math.Pow(1 - x[0], 2), xInitial, xLower, xUpper);

            WriteLine(solution.Result);
            Console.WriteLine("solution = {0}", solution.GetSolutionValue(0));
            Console.WriteLine("x = {0}", solution.GetValue(1));
            Console.WriteLine("y = {0}", solution.GetValue(2));

        }
    }
}
