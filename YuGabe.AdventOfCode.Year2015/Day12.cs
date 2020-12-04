using System;
using Tidy.AdventOfCode;

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
            throw new NotImplementedException();
            // This was done with Newtonsoft's API originally.
            //var input = (JToken)JsonConvert.DeserializeObject(Input);
            //var queue = new Queue<JToken>(new[] { input });
            //long sum = 0;
            //while (queue.TryDequeue(out var current))
            //{
            //    switch (current)
            //    {
            //        case JArray array:
            //            foreach (var child in array.Children())
            //                queue.Enqueue(child);
            //            break;
            //        case JObject @object:
            //            var red = false;
            //            foreach (var (k, v) in @object)
            //            {
            //                if (v.ToString() == "red")
            //                {
            //                    red = true;
            //                    break;
            //                }
            //            }
            //            if (!red)
            //                foreach (var (k, child) in @object)
            //                    queue.Enqueue(child);
            //            break;
            //        case JValue val:
            //            if (val.Value.GetType() == typeof(long))
            //                sum += (long)val.Value;
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //return sum;
        }
    }
}
