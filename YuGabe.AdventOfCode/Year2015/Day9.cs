using System.Collections.Generic;
using System.Linq;
using Tidy.AdventOfCode;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day9 : Day<Day9.Edge[]>
    {
        public override Edge[] ParseInput(string input)
        {
            return input.Trim().Split("\n").Select(r => new Edge
            {
                FromName = r.Split(' ')[0],
                ToName = r.Split(' ').Skip(2).First(),
                Weight = int.Parse(r.Split(' ').Last())
            }).ToArray();
        }

        public override object ExecutePart1()
        {
            var allVisits = new List<List<(string vertexName, int weight)>>();
            var vertices = Input.SelectMany(p => new List<Edge> { p, new Edge { FromName = p.ToName, ToName = p.FromName, Weight = p.Weight } }).GroupBy(p => p.FromName).ToDictionary(g => g.Key, g => g.ToArray());
            void Visit(string vertexName, int weight, IEnumerable<Edge> outEdges, List<(string name, int weight)> visits)
            {
                visits.Add((vertexName, weight));
                if (!outEdges.Where(e => visits.All(v => v.name != e.ToName)).Any())
                    allVisits.Add(visits);
                else
                    foreach (var e in outEdges.Where(e => visits.All(v => v.name != e.ToName)))
                        Visit(e.ToName, e.Weight, vertices[e.ToName], new List<(string name, int weight)>(visits));
            }

            foreach (var vertex in vertices)
            {
                Visit(vertex.Key, 0, vertices[vertex.Key], new List<(string name, int weight)>());
            }

            return allVisits.Min(av => av.Sum(v => v.weight));
        }

        public override object ExecutePart2()
        {
            var allVisits = new List<List<(string vertexName, int weight)>>();
            var vertices = Input.SelectMany(p => new List<Edge> { p, new Edge { FromName = p.ToName, ToName = p.FromName, Weight = p.Weight } }).GroupBy(p => p.FromName).ToDictionary(g => g.Key, g => g.ToArray());
            void Visit(string vertexName, int weight, IEnumerable<Edge> outEdges, List<(string name, int weight)> visits)
            {
                visits.Add((vertexName, weight));
                if (!outEdges.Where(e => visits.All(v => v.name != e.ToName)).Any())
                    allVisits.Add(visits);
                else
                    foreach (var e in outEdges.Where(e => visits.All(v => v.name != e.ToName)))
                        Visit(e.ToName, e.Weight, vertices[e.ToName], new List<(string name, int weight)>(visits));
            }

            foreach (var vertex in vertices)
            {
                Visit(vertex.Key, 0, vertices[vertex.Key], new List<(string name, int weight)>());
            }

            return allVisits.Max(av => av.Sum(v => v.weight));
        }

        public class Edge
        {
            public string FromName { get; init; } = null!;
            public string ToName { get; init; } = null!;
            public int Weight { get; init; }
        }
    }
}
