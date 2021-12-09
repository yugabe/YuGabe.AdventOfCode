using System.Text.Json;

namespace YuGabe.AdventOfCode.Year2015
{
    public class Day12 : Day
    {
        public override object ExecutePart1()
        {
            var sum = 0;
            bool Predicate(int num)
                => int.TryParse(Input[num].ToString(), out var _) || (Input[num] == '-' && int.TryParse(Input[num + 1].ToString(), out var _));
            for (var i = 0; i < Input.Length; i++)
            {
                if (Predicate(i))
                {
                    var end = i;
                    while (Predicate(++end)) ;
                    sum += int.Parse(Input[i..end]);
                    i = end;
                }
            }
            return sum;
        }

        public override object ExecutePart2()
        {
            var queue = new Queue<JsonElement>(new[] { JsonDocument.Parse(Input).RootElement });
            long sum = 0;
            while (queue.TryDequeue(out var current))
            {
                queue.Enqueue(current.ValueKind switch
                {
                    JsonValueKind.Array => current.EnumerateArray(),
                    JsonValueKind.Object => current.EnumerateObject().All(element => element.Value.ToString() != "red") ? current.EnumerateObject().Select(o => o.Value) : Enumerable.Empty<JsonElement>(),
                    _ => Enumerable.Empty<JsonElement>()
                });

                sum += current.ValueKind == JsonValueKind.Number ? current.GetInt64() : 0;
            }
            return sum;
        }
    }
}
