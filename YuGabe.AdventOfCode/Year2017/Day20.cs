using System.Numerics;

namespace YuGabe.AdventOfCode.Year2017
{
    public class Day20 : Day<(Vector3 p, Vector3 v, Vector3 a)[]>
    {
        public override (Vector3 p, Vector3 v, Vector3 a)[] ParseInput(string input)
        {
            return input.Trim().Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(s =>
            {
                var vectors = s.Split('=');
                return (ParseVector(vectors[1]), ParseVector(vectors[2]), ParseVector(vectors[3]));
            }
            ).ToArray();

            Vector3 ParseVector(string s)
            {
                var tokens = new string(s.SkipWhile(c => c != '<').Skip(1).TakeWhile(c => c != '>').ToArray()).Split(',').Select(float.Parse).ToArray();
                return new Vector3(tokens[0], tokens[1], tokens[2]);
            }
        }

        public override object ExecutePart1()
        {
            void Increase(ref Vector3 p, ref Vector3 v, ref Vector3 a) => p += (v += a);

            (float value, float index) min = (0, 0);

            for (var e = 0; e < Input.Length; e++)
            {
                var v = Input[e];
                for (var x = 0; x < 1000; x++)
                    Increase(ref v.p, ref v.v, ref v.a);
                if (e == 0)
                    min = (v.p.Length(), e);
                else
                {
                    var l = v.p.Length();
                    if (l < min.value)
                        min = (l, e);
                }
            }

            return min.index;
        }

        public override object ExecutePart2()
        {
            for (var x = 0; x < 1000; x++)
            {
                for (var e = 0; e < Input.Length; e++)
                    Input[e].p += (Input[e].v += Input[e].a);
                foreach (var collided in Input.GroupBy(v => v.p).Where(v => v.Count() > 1).ToList())
                    Input = Input.Except(Input.Where(v => v.p == collided.Key)).ToArray();
            }

            return Input.Length;
        }
    }
}
