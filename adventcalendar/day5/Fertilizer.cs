using System.Text.RegularExpressions;

namespace adventcalendar.day5
{
    internal class Fertilizer
    {
        private record Mapping(double Source, double Destination);

        List<double> Seeds = new();
        private readonly string _text = File.ReadAllText("day5/input.txt");

        public Fertilizer()
        {
            Seeds = _text
                .Split("seeds:")[1]
                .Split("seed-to-soil map:")[0]
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(double.Parse)
                .ToList();
        }

        public double ex2()
        {
            var pairedSeeds = Seeds
            .Select((seed, index) => new { Seed = seed, amount = index })
            .GroupBy(obj => obj.amount / 2)
            .Select(group => group.Select(obj => obj.Seed).ToList())
            .ToList();

            var location = new List<double>();

            foreach (var item in pairedSeeds)
            {
                Console.WriteLine($"Processing: {item[0]}");

                var maxSeed = item[1] + item[0];
                var minSeed = item[0];
                var bestseed = FindBestSeed(minSeed, maxSeed, 1000000);

                for (int i = 0; i < 5000; i++)
                {
                    var midSeed = (minSeed + maxSeed) / 2;
                    if (FindLocationFast(bestseed) < FindLocationFast(midSeed))
                    {
                        maxSeed = midSeed;
                    }
                    else
                    {
                        minSeed = midSeed;
                    }
                    bestseed = FindBestSeed(minSeed, maxSeed, i);
                }

                // FINAL
                var batches = CreateBatch(maxSeed, bestseed, 1);
                location.AddRange(batches.Select(FindLocationFast));
            }

            return location.Min();
        }

        // Function to find the best seed in a given range
        double FindBestSeed(double minSeed, double maxSeed, int batchSize)
        {
            var bestseeddict = new Dictionary<double, double>();

            var batches = CreateBatch(maxSeed, minSeed, batchSize);
            foreach (var seed in batches)
            {
                if (!bestseeddict.ContainsKey(seed))
                    bestseeddict.Add(seed, FindLocationFast(seed));
            }

            return bestseeddict.FirstOrDefault(x => x.Value == bestseeddict.Values.Min()).Key;
        }

        private static List<double> CreateBatch(double number, double min, int batchsize)
        {
            List<double> batches = new();

            while (number >= min)
            {
                batches.Add(number);
                number -= batchsize;
            }

            if (number < min)
            {
                batches.Add(min);
            }

            return batches;
        }
        private double FindLocationFast(double seed)
        {
            var location = seed;

            _text
            .Split("seed-to-soil map:", StringSplitOptions.TrimEntries)[1]
            .Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(lines => Regex.Replace(lines, "[^0-9 _]", " "))
            .ToList()
            .ForEach(lines =>
            {
                var line = lines.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries).Where(x => x != "").ToList();
                for (int i = 0; i < line.Count() / 3; i++)
                {
                    var elements = line.Skip(i * 3).Take(3).ToList();
                    var min = double.Parse(elements[1]);
                    var max = min + double.Parse(elements[2]) - 1;
                    var dest = double.Parse(elements[0]);
                    if (min <= location && location <= max)
                    {
                        location = dest + (location - min);
                        break;
                    }
                }
            });

            return location;
        }


        public double ex1()
        {
            var location = new List<double>();
            Seeds.ForEach(seed =>
            {
                location.Add(FindLocationFast(seed));
            });
            return location.Min();
        }

        private double FindLocationFast2(string start, string end, double seed)
        {
            var sections = _text
                .Split(start, StringSplitOptions.TrimEntries)[1]
                .Split(end, StringSplitOptions.TrimEntries)[0]
                .Split(Environment.NewLine)
                .Select(line => line.Split(" ", StringSplitOptions.TrimEntries))
                .Select(numbers => new { Min = double.Parse(numbers[1]), Max = double.Parse(numbers[1]) + double.Parse(numbers[2]) - 1, Dest = double.Parse(numbers[0]) })
                .Where(obj => obj.Min <= seed && seed <= obj.Max)
                .Select(obj => obj.Dest + (seed - obj.Min))
                .FirstOrDefault();

            return sections > 0 ? sections : seed;
        }

        private double FindLocationSlow(string start, string end, double seed)
        {
            var sections = _text
                .Split(start, StringSplitOptions.TrimEntries)[1]
                .Split(end, StringSplitOptions.TrimEntries)[0]
                .Split(Environment.NewLine)
                .Select(line => line.Split(" ", StringSplitOptions.TrimEntries))
                .ToList();

            foreach (var section in sections)
            {
                var sectionStart = double.Parse(section[1]);
                var sectionEnd = double.Parse(section[0]);
                var sectionCount = int.Parse(section[2]);

                for (int i = 0; i < sectionCount; i++)
                {
                    var currentSource = sectionStart + i;
                    var currentDestination = sectionEnd + i;

                    if (currentSource == seed)
                    {
                        return currentDestination;
                    }
                }
            }

            return seed;
        }

        private double FindLocationNeverEnding(string start, string end, double seed) => _text
            .Split(start, StringSplitOptions.TrimEntries)[1]
            .Split(end, StringSplitOptions.TrimEntries)[0]
            .Split(Environment.NewLine)
            .Select(line => line.Split(" ", StringSplitOptions.TrimEntries))
            .SelectMany(pair =>
                Enumerable.Range(0, int.Parse(pair[2]))
                    .Select(i => new Mapping(double.Parse(pair[1]) + i, double.Parse(pair[0]) + i))
            )
            .FirstOrDefault(mapping => mapping.Source == seed)?.Destination ?? seed;
    }
}
