namespace adventcalendar.day1
{
    internal class NumberFinder
    {
        private readonly List<string> _text;

        internal NumberFinder()
        {
            _text = File.ReadAllLines("day1/input.txt").ToList();
        }

        public int GetNumbers()
        {
            var total = 0;
            _text.ForEach(x =>
            {
                total += GetFirstAndLastNumber(x);
            });

            return total;
        }

        public int GetNumbersIncludingWrittenNumbers()
        {
            var total = 0;
            _text.ForEach(x =>
            {
                var parsedText = ParseWrittenNumbers(x);
                total += GetFirstAndLastNumber(parsedText);
            });

            return total;
        }

        private static int GetFirstAndLastNumber(string text)
        {
            var numberArray = text.ToCharArray().Where(char.IsDigit).ToList();

            var number = int.Parse($"{char.GetNumericValue(numberArray.First())}{char.GetNumericValue(numberArray.Last())}");
            return number;
        }

        private static string ParseWrittenNumbers(string text)
        {
            string[] writtenNumbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            var foundNumbers = writtenNumbers
               .Where(num => text.Contains(num))
               .SelectMany(num =>
                    Enumerable.Range(0, text.Length - num.Length + 1)
                        .Where(index => text.Substring(index, num.Length) == num)
                        .Select(index => new { Index = index, Number = WordsToNumber(num) })
                )
               .ToDictionary(item => item.Index, item => item.Number);

            var combinedNumbers = foundNumbers
                .Concat
                (
                    text.Select((c, i) => new { Char = c, Index = i })
                    .Where(x => char.IsDigit(x.Char))
                    .ToDictionary(x => x.Index, x => (int)char.GetNumericValue(x.Char))
                )
                .OrderBy(x => x.Key)
                .Select(x => x.Value);

            Console.WriteLine($"{text} => {combinedNumbers.First()}{combinedNumbers.Last()}");

            return $"{combinedNumbers.First()}{combinedNumbers.Last()}";
        }

        private static int WordsToNumber(string text) => text switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => 0
        };

    }
}
