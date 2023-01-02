using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCTiS.Knx.Model.Patterns
{
    //If there are any patterns, this guy will find them
    public static class PatternFinder
    {
        static Random r;
        static PatternFinder()
        {
            r = new Random();
        }

        public static IEnumerable<PatternMatch> FindPatterns<T>(IEnumerable<T> list, Func<T, string> extractor, int order = 3)
        {
            var strings = list.Select(row => extractor(row)).ToList();
            return strings
                .SelectMany(row => OrderSplit(row, order, ' '))
                .Distinct()
                .Where(row => !string.IsNullOrWhiteSpace(row))
                .Select(row => new PatternMatch { MatchingWord = row, Confidence = row.Length * (double)strings.Count(str => str.Contains(row)) / (double)strings.Count })
                .OrderByDescending(row => row.Confidence);
        }

        private static IEnumerable<string> OrderSplit(string str, int order, char arg)
        {
            var splits = str.Split(arg);
            for (var i = 0; i < splits.Length; i++)
            {
                string res = splits[i];
                yield return res;
                for (var j = i + 1; j < splits.Length && j < (order + i); j++)
                {
                    res += arg + splits[j];
                    yield return res;
                }
            }
        }
    }

    public class PatternMatch
    {
        public string MatchingWord { get; set; }
        public double Confidence { get; set; }
    }
}
