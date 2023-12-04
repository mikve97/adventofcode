namespace adventcalendar.day_4
{
    internal class Scratchcards
    {
        private readonly IEnumerable<string> _text;

        internal Scratchcards()
        {
            _text = File.ReadAllLines("day4/input.txt").AsEnumerable();
        }

        public int ex1()
        {
            var result = _text.Select(lines =>
            {
                var leftResults = lines.Split("|")[0].Split(":")[1].Trim();
                var rightResults = lines.Split("|")[1].Trim();

                var matchingItems = leftResults.Split(" ", StringSplitOptions.RemoveEmptyEntries).Intersect(rightResults.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToList();

                return matchingItems.Count();
            }).Sum(value => (int)Math.Pow(2, value - 1));
            return result;
        }

    }
}
