using System.Linq;

namespace adventcalendar.day5
{
    internal class Fertilizer
    {
        private record Mapping(double Source, double Destination);

        List<int> seeds = new() { 79, 14, 55, 13 };

        private readonly string _text;

        public Fertilizer()
        {
            _text = File.ReadAllText("day5/input.txt");
        }

        public double ex1()
        {
            var location = new List<double>();
            _text
            .Split("seeds:")[1]
            .Split("seed-to-soil map:")[0]
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(double.Parse)
            .ToList()
            .ForEach(seed =>
            {
                var seedsoil = FindLocation("seed-to-soil map:", "soil-to-fertilizer map:", seed);
                var soilfertilizer = FindLocation("soil-to-fertilizer map:", "fertilizer-to-water map:", seedsoil);
                var fertilizerwater = FindLocation("fertilizer-to-water map:", "water-to-light map:", soilfertilizer);
                var waterlight = FindLocation("water-to-light map:", "light-to-temperature map:", fertilizerwater);
                var lighttemperature = FindLocation("light-to-temperature map:", "temperature-to-humidity map:", waterlight);
                var temphumidity = FindLocation("temperature-to-humidity map:", "humidity-to-location map:", lighttemperature);
                var humiditylocation = FindLocation("humidity-to-location map:", "", temphumidity);
                location.Add(FindLocation("humidity-to-location map:", "", temphumidity));
            });

            return location.Min();
        }

        private double FindLocation2(string start, string end, double seed) => _text
            .Split(start, StringSplitOptions.TrimEntries)[1]
            .Split(end, StringSplitOptions.TrimEntries)[0]
            .Split(Environment.NewLine)
            .Select(line => line.Split(" ", StringSplitOptions.TrimEntries))
            .SelectMany(pair =>
                Enumerable.Range(0, int.Parse(pair[2]))
                    .Select(i => new Mapping(double.Parse(pair[1]) + i, double.Parse(pair[0]) + i))
            )
            .FirstOrDefault(mapping => mapping.Source == seed)?.Destination ?? seed;

        private double FindLocation(string start, string end, double seed)
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
    }
}
