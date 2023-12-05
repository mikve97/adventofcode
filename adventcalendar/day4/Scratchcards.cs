namespace adventcalendar.day_4
{
    internal class Scratchcards
    {
        private readonly IEnumerable<string> _text;

        internal Scratchcards()
        {
            _text = File.ReadAllLines("day4/input.txt").AsEnumerable();
        }

        public int Ex1() => 
            _text
            .Select(line =>
                line.Split("|")[0].Split(":", StringSplitOptions.TrimEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Intersect(line.Split("|", StringSplitOptions.TrimEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .Count()
            ).Sum(value => (int)Math.Pow(2, value - 1));

        public void Ex2()
        {
            var dict = _text
                .Select((line, index) =>
                {
                    var intersectionCount =
                        line.Split("|")[0].Split(":", StringSplitOptions.TrimEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Intersect(line.Split("|", StringSplitOptions.TrimEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries))
                        .Count();

                    return new { Index = index, Values = new int[,] { { intersectionCount, 1 } } };
                })
                .ToDictionary(o => o.Index, o => o.Values);

            dict.Where(kvp => kvp.Value[0, 0] > 0)
                .ToList()
                .ForEach(kvp => SolveDict(kvp.Value, kvp.Key, dict));

            var sum = dict.Sum(kvp => kvp.Value[0, 1]);
        }

        private static void SolveDict(int[,] value, int gamenum, IDictionary<int, int[,]> dict)
        {
            Enumerable.Range(1, value[0, 0]).ToList().ForEach(j =>  
            {
                var index = j + gamenum;
                dict[index][0, 1] += 1 * dict[gamenum][0, 1];
            });
        }
    }
}
