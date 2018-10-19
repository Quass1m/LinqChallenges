namespace LinqChallenges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using MoreLinq;

    /// <summary>
    /// The Challenge…
    /// Each of these problems can be solved using a single C# statement by making use of chained LINQ operators
    /// (although you can use more statements if you like). You'll find the String.Split function helpful to get
    /// started on each problem. Other functions you might need to use at various points are String.Join,
    /// Enumerable.Range, Zip, Aggregate, SelectMany. LINQPad would be a good choice to try out your ideas.
    /// </summary>
    /// <see cref="https://markheath.net/post/lunchtime-linq-challenge"/>
    /// <see cref="https://markheath.net/post/lunchtime-linq-challenge-2"/>
    /// <see cref="https://markheath.net/post/linq-challenge-3"/>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Linq Challenges");
            Console.WriteLine("__________________");

            Task3_6();

            Console.ReadKey();
        }

        #region Challenges part 1
        /// <summary>
        /// Take the following string
        /// "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert"
        /// and give each player a shirt number, starting from 1, to create a string of the form:
        /// "1. Davis, 2. Clyne, 3. Fonte" etc
        /// </summary>
        public static void Task1_1()
        {
            var str = "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert";

            //string res = str.Split(',').Reverse().Aggregate((x, y) => $"1. {y}, {x}");
            var arr = str.Split(',');
            var res = string.Join(", ", arr.Zip(Enumerable.Range(1, arr.Length), (x, y) => $"{y}. {x}"));

            Console.WriteLine(res);
        }

        /// <summary>
        /// Take the following string "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis,
        /// 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988"
        /// and turn it into an IEnumerable of players in order of age (bonus to show the age in the output)
        /// </summary>
        public static void Task1_2()
        {
            var str =
                "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";

            var res = str.Split(';')
                .Select(x =>
                {
                    var values = x.Split(',');

                    return new
                    {
                        Player = values[0].Trim(),
                        Born = DateTime.Parse(values[1].Trim())
                    };
                })
                .OrderBy(x => x.Born);

            foreach (var player in res)
            {
                Console.WriteLine(player.Player + " " + player.Born);
            }
        }

        /// <summary>
        /// Take the following string "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27"
        /// which represents the durations of songs in minutes and seconds,
        /// and calculate the total duration of the whole album
        /// </summary>
        public static void Task1_3()
        {
            var str = "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27";

            var res = TimeSpan.FromSeconds(str.Split(',').Aggregate(0d, (i, s) => i += TimeSpan.Parse(s).TotalSeconds));

            Console.WriteLine(res);
        }

        /// <summary>
        /// Create an enumerable sequence of strings in the form "x,y" representing all
        /// the points on a 3x3 grid. e.g. output would be: 0,0 0,1 0,2 1,0 1,1 1,2 2,0 2,1 2,2
        /// </summary>
        public static void Task1_4()
        {
            //var res = Enumerable.Range(0, 3).Cartesian(Enumerable.Range(0, 3), (i1, i2) => new { X = i1, Y = i2 })
            //.Select(coordinate => $"X={coordinate.X},Y={coordinate.Y}"))
            //.Aggregate((value, element) => value + $";{element}");
            string res =
                string.Join(';',
                Enumerable.Range(0, 3).Cartesian(Enumerable.Range(0, 3), (i1, i2) => new { X = i1, Y = i2 })
                .Select(coordinate => $"X={coordinate.X},Y={coordinate.Y}"));

            Console.WriteLine(res);
        }

        /// <summary>
        /// Take the following string "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35"
        /// which represents the times (in minutes and seconds) at which a swimmer completed each
        /// of 10 lengths. Turn this into an IEnumerable of TimeSpan objects containing the time
        /// taken to swim each length (e.g. first length was 45 seconds, second was 47 seconds etc)
        /// </summary>
        public static void Task1_5()
        {
            var str = "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35";

            var res = str.Split(',').Select(x => TimeSpan.Parse(x));

            Console.WriteLine(res.First());
            Console.WriteLine(res.Last());
        }

        /// <summary>
        /// Take the following string "2,5,7-10,11,17-18" and turn it
        /// into an IEnumerable of integers: 2 5 7 8 9 10 11 17 18
        /// </summary>
        public static void Task1_6()
        {
            var str = "2,5,7-10,11,17-18";

            var res = str.Split(',').SelectMany(
                x =>
                {
                    var values = x.Split('-');

                    // Single value
                    if (values.Length == 1)
                        return new List<int> { int.Parse(values[0]) };

                    var start = int.Parse(values[0]);
                    var count = int.Parse(values[1]) - start + 1;

                    // Range of values
                    return Enumerable.Range(start, count);
                }).Flatten();

            foreach (var item in res)
            {
                Console.WriteLine(item);
            }
        }
        #endregion

        #region Challenges part 2
        /// <summary>
        /// In a motor sport competition, a player's points total for the season is the sum of all
        /// the points earned in each race, but the three worst results are not counted towards the total.
        /// Calculate the following player's score based on the points earned in each round:
        /// </summary>
        public static void Task2_1()
        {
            var str = "10,5,0,8,10,1,4,0,10,1";

            var res = str.Split(',')
                .Select(int.Parse).OrderBy(x => x).Skip(3)
                .Aggregate(0, (sum, current) => sum + current);

            Console.WriteLine(res);
        }

        /// <summary>
        /// A chess board is an 8x8 grid, from a1 in the bottom left to h8 in the top right.
        /// A bishop can travel diagonally any number of squares. So for example a bishop on c5
        /// can go to b4 or to a3 in one move. Create an enumerable sequence of board positions
        /// that includes every square a bishop can move to in one move on an empty chess board,
        /// if its starting position is c6. e.g. output would include b7, a8, b5, a4
        /// </summary>
        public static void Task2_2()
        {
            IEnumerable<Coordinates> AllowedBishopMoves(IEnumerable<Coordinates> allCoordinateses, Coordinates start)
            {
                return allCoordinateses.Where(e => (e.X == start.X + 1 || e.X == start.X - 1)
                                             && e.X >= 65
                                             && e.X <= 72
                                             && (e.Y == start.Y + 1 || e.Y == start.Y - 1)
                                             && e.Y >= 1
                                             && e.Y <= 8);
            }

            var letters = new int[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            var all = Enumerable.Range(1, 8).Cartesian(letters, (i, s) => new Coordinates { X = s, Y = i });

            var moves1 = AllowedBishopMoves(all, new Coordinates() { X = 'C', Y = 6 });
            var moves2 = AllowedBishopMoves(all, new Coordinates() { X = 'H', Y = 8 });
        }

        struct Coordinates
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// The following sequence has 100 entries. Sample it by taking every 5th value
        /// and discarding the rest. The output should begin with 24,53,77,...
        /// </summary>
        public static void Task2_3()
        {
            var str =
                "0,6,12,18,24,30,36,42,48,53,58,63,68,72,77,80,84,87,90,92,95,96,98,99,99,100,99,"
                + "99,98,96,95,92,90,87,84,80,77,72,68,63,58,53,48,42,36,30,24,18,12,6,0,-6,-12,"
                + "-18,-24,-30,-36,-42,-48,-53,-58,-63,-68,-72,-77,-80,-84,-87,-90,-92,-95,-96,"
                + "-98,-99,-99,-100,-99,-99,-98,-96,-95,-92,-90,-87,-84,-80,-77,-72,-68,-63,"
                + "-58,-53,-48,-42,-36,-30,-24,-18,-12,-6";

            var res = str.Split(',').Select(x => int.Parse(x)).Skip(4).TakeEvery(5);

            foreach (var item in res)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Yes won the vote, but how many more Yes votes were there than No votes?
        /// </summary>
        public static void Task2_4()
        {
            var str = "Yes,Yes,No,Yes,No,Yes,No,No,No,Yes,Yes,Yes,Yes,No,Yes,No,No,Yes,Yes";

            var max = str.Split(',').GroupBy(x => x).Select(x => x.Count()).Max();
            var min = str.Split(',').GroupBy(x => x).Select(x => x.Count()).Min();

            var res = max - min;

            Console.WriteLine(res);
        }

        /// <summary>
        /// Count how many have dogs, how many have cats, and how many have other pets.
        /// e.g. output would be a structure or sequence containing: Dog:5 Cat:3 Other:4
        /// </summary>
        public static void Task2_5()
        {
            var str = "Dog,Cat,Rabbit,Dog,Dog,Lizard,Cat,Cat,Dog,Rabbit,Guinea Pig,Dog";

            var res = str.Split(',').CountBy(x => x);

            foreach (var item in res)
            {
                Console.WriteLine(item.Key + " count = " + item.Value);
            }
        }

        /// <summary>
        /// Expand strings in this form: A5B10CD3 to AAAAABBBBBBBBBBCDDD. Note that
        /// a letter may have 0 digits after it (C in the example), which means just
        /// repeat it once. It may also have more than one digit after it, such
        /// as B in this example, which is repeated 10 times. This one is a bit tricky,
        /// but there is a handy method in the .NET framework (not part of LINQ)
        /// which can help you get started.
        /// </summary>
        public static void Task2_6()
        {
            var str = "A5B10CD3";

            // solution from web:
            var res = string.Join(
                "",
                Regex.Matches("A5B10CD3", @"[A-Z]\d*")
                    .Select(m => m.Value)
                    .Select(m => new string(m[0], m.Length == 1 ? 1 : int.Parse(m.Substring(1)))));

            Console.WriteLine(res);
        }
        #endregion

        #region Challenges part 3
        /// <summary>
        /// The following string contains number of sales made per day in a month:
        /// "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0"
        /// How long is the longest sequence of days without a sale? (in this example it's 4)
        /// </summary>
        public static void Task3_1()
        {
            var str = "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0";

            var res = str.Split(',').Select(int.Parse)
                .Aggregate((max: 0, curr: 0), (days, el) => el != 0 ? (days.max, 0) : (Math.Max(days.max, days.curr + 1), days.curr + 1)).max;

            Console.WriteLine(res);

            // MoreLinq version
            var res2 = str.Split(',').Select(int.Parse).GroupAdjacent(x => x > 0).Select(x => x.Count()).Max();

            Console.WriteLine(res2);
        }

        /// <summary>
        /// In poker a hand is a "full house" if it contains three cards of one value and two of
        /// another value. The following string defines five poker hands, separated by a semicolon:
        /// "4♣ 5♦ 6♦ 7♠ 10♥;10♣ Q♥ 10♠ Q♠ 10♦;6♣ 6♥ 6♠ A♠ 6♦;2♣ 3♥ 3♠ 2♠ 2♦;2♣ 3♣ 4♣ 5♠ 6♠".
        /// "4A 5B 6B 7C 10D;10A QD 10C QC 10B;6A 6D 6C AC 6B;2A 3D 3C 2C 2B;2A 3A 4A 5C 6C".
        /// Write a LINQ expression that returns an sequence containing only the "full house" hands.
        /// </summary>
        public static void Task3_2()
        {
            var str = "4A 5B 6B 7C 10D;10A QD 10C QC 10B;6A 6D 6C AC 6B;2A 3D 3C 2C 2B;2A 3A 4A 5C 6C";

            var res = str.Split(';').GroupBy(
                    x => x.Split(' ').Select(
                        y => new { Value = y.Substring(0, y.Length - 1), Color = y[y.Length - 1] }))
                .Select(x => x.Key.CountBy(hand => hand.Value))
                .Where(x =>
                {
                    var keyValuePairs = x.ToList();
                    return keyValuePairs.Any(c => c.Value == 2) && keyValuePairs.Any(c => c.Value == 3);
                });

            Console.WriteLine(res);
        }

        /// <summary>
        /// What day of the week is Christmas day (25th December) on for the next 10 years  (starting with 2018)?
        /// The answer should be a string (or sequence of strings) starting: Tuesday,Wednesday,Friday,...
        /// </summary>
        public static void Task3_3()
        {
            /// From: https://gist.github.com/lschad/8069ddb0cafc906bfed4d9f239e0de14
            Enumerable.Range(2018, 10)
                .Select(i => new DateTime(i, 12, 25))
                .ToList()
                .ForEach(d => Console.WriteLine(d.DayOfWeek));
        }

        /// <summary>
        /// From the following dictionary of words,
        /// "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray"
        /// return all words that are an anagram of star(no leftover letters allowed).
        /// </summary>
        public static void Task3_4()
        {
            var str = "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray";
            var target = string.Concat("star".OrderBy(c => c));

            str.Split(',')
                .Where(x => x.Length == target.Length)
                .Select(x => new { Original = x, Sorted = string.Concat(x.OrderBy(c => c)) })
                .Where(x => x.Sorted == target)
                .ForEach(Console.WriteLine);
        }

        /// <summary>
        /// From the following list of names
        /// "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson,
        /// Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell"
        /// find any groups of people who share the same initials as each other.
        /// </summary>
        public static void Task3_5()
        {
            var str = "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, "
                      + "Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell";

            var res = str.Split(',').GroupBy(x => x[0] + x[x.IndexOf(' ') + 1]);

            foreach (var item in res)
            {
                Console.WriteLine(string.Join(',', item.ToList()));
            }
        }

        /// <summary>
        /// A video is two hours long exactly, and we want to make some edits, cutting out the following time ranges (expressed in H:MM:SS):
        /// "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51".
        /// (You can assume that the input ranges are in order and contain no overlapping portions)
        /// We would like to turn this into a sequence of time-ranges to keep. So in this example, the output should be:
        /// "0:00:05-0:55:12;1:05:02-1:37:47;1:37:51-2:00:00"
        /// </summary>
        public static void Task3_6()
        {
            var full = "0:00:00-2:00:00";
            var cuts = "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51";
            // ToDo:
            var res = cuts.Split(';').Select(x => x.Split('-')).Skip(1).Aggregate(string.Empty, (x, y) => x + "-" + y);

            Console.WriteLine(res);
        }
        #endregion
    }
}
