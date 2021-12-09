using System;
using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day7 : Day
    {
        class TreeElement
        {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public string[] ChildrenNames { get; set; }
            public string Name { get; set; }
            public int Weight { get; set; }
            private TreeElement _parent;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public TreeElement Parent { get => _parent; set { _parent = value; value.Children.Add(this); } }
            public List<TreeElement> Children { get; } = new List<TreeElement>();
            public int TotalWeight => Weight + Children.Sum(c => c.TotalWeight);
            public override string ToString() => $"{Name} ({Weight} | {TotalWeight}) --> {string.Join(", ", Children.Select(c => $"{c.Name} ({c.Weight} | {c.TotalWeight})"))}";
        }

        public override object ExecutePart1()
        {
            var tree = Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(r =>
            {
                var tokens = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                return (name: tokens[0], weight: int.Parse(tokens[1].Trim('(', ')')), children: tokens.Skip(2).Select(t => t.Trim(',')).ToList());
            });
            return tree.Single(t => tree.All(c => !c.children.Contains(t.name))).name;
        }

        public override object ExecutePart2()
        {
            var tree = Input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(r =>
            {
                var tokens = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                return new TreeElement { Name = tokens[0], Weight = int.Parse(tokens[1].Trim('(', ')')), ChildrenNames = tokens.Skip(3).Select(t => t.Trim(',')).ToArray() };
            }).ToDictionary(k => k.Name);

            foreach (var e in tree.Values)
                foreach (var c in e.ChildrenNames.Select(n => tree[n]))
                    c.Parent = e;

            var smallestUneven = tree.Where(e => e.Value.Children.Select(c => c.TotalWeight).Distinct().Count() > 1).OrderBy(c => c.Value.TotalWeight).First().Value;
            return smallestUneven.Children.GroupBy(g => g.TotalWeight).Where(g => g.Count() == 1).First().First().Weight
                - (smallestUneven.Children.Max(c => c.TotalWeight) - smallestUneven.Children.Min(c => c.TotalWeight));
        }
    }
}
