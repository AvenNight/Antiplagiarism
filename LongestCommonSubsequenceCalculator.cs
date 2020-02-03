using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second).Reverse().ToList();
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];

            for (int i = 1; i <= first.Count; i++)
                for (int j = 1; j <= second.Count; j++)
                    opt[i, j] = first[i - 1] == second[j - 1] ?
                        opt[i - 1, j - 1] + 1 :
                        Math.Max(opt[i - 1, j], opt[i, j - 1]);

            return opt;
        }

        private static IEnumerable<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var i = opt.GetLength(0) - 1;
            var j = opt.GetLength(1) - 1;
            while (i >= 1 && j >= 1)
                if (first[i - 1] == second[j - 1])
                {
                    yield return first[i - 1];
                    i--;
                    j--;
                }
                else if (opt[i - 1, j] > opt[i, j - 1])
                    i--;
                else
                    j--;
        }
    }
}