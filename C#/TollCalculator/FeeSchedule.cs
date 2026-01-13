using System;
using System.Globalization;

namespace TollFeeCalculator
{
    public class FeeSchedule
    {
        private int[] RateByMinute = new int[24 * 60];
        private HashSet<int> TollFreeMonths = new();
        private HashSet<DateOnly> TollFreeDates = new();
        private HashSet<DayOfWeek> TollFreeDays = new();

        public FeeSchedule()
        {
            this.TollFreeDays.UnionWith(new [] { DayOfWeek.Saturday, DayOfWeek.Sunday });
            this.TollFreeDates.UnionWith(new []
            {
                new DateOnly(1, 1, 1),   // New Year's Day
                new DateOnly(1, 3, 28),  // March Holiday
                new DateOnly(1, 3, 29),  // March Holiday
                new DateOnly(1, 4, 1),   // April Holiday
                new DateOnly(1, 4, 30),  // Walpurgis Night
                new DateOnly(1, 5, 1),   // Labor Day
                new DateOnly(1, 5, 8),   // Victory in Europe Day
                new DateOnly(1, 5, 9),   // Ascension Day
                new DateOnly(1, 6, 5),   // Pentecost
                new DateOnly(1, 6, 6),   // Whit Monday
                new DateOnly(1, 6, 21),  // Midsummer Eve
                new DateOnly(1, 11, 1),  // All Saints' Day
                new DateOnly(1, 12, 24), // Christmas Eve
                new DateOnly(1, 12, 25), // Christmas Day
                new DateOnly(1, 12, 26), // Second Day of Christmas
                new DateOnly(1, 12, 31)  // New Year's Eve
            });

            this.TollFreeMonths.Add(7);

            // Define intervals as minute-of-day ranges (inclusive)
            var intervals = new (int StartMinute, int EndMinute, int Fee)[]
            {
                (6*60 + 0,   6*60 + 29,  8),   // 06:00-06:29
                (6*60 + 30,  6*60 + 59, 13),   // 06:30-06:59
                (7*60 + 0,   7*60 + 59, 18),   // 07:00-07:59
                (8*60 + 0,   8*60 + 29, 13),   // 08:00-08:29
                (8*60 + 30, 8*60 + 59, 8),    // 08:30-14:59
                (15*60 + 0, 15*60 + 29, 13),   // 15:00-15:29
                (15*60 + 30,16*60 + 59, 18),   // 15:30-16:59
                (17*60 + 0, 17*60 + 59, 13),   // 17:00-17:59
                (18*60 + 0, 18*60 + 29, 8)     // 18:00-18:29
            };
            for (int i = 8; i < 15; i++)
            {
                intervals = intervals.Append((i * 60 + 30, i * 60 + 59, 8)).ToArray();
            }

            foreach (var iv in intervals)
            {
                for (int m = iv.StartMinute; m <= iv.EndMinute && m < RateByMinute.Length; m++)
                {
                    this.RateByMinute[m] = iv.Fee;
                }
            }
        }

        public FeeSchedule(DayOfWeek[] tollFreeDays) : this()
        {
            this.TollFreeDays.UnionWith(tollFreeDays ?? Array.Empty<DayOfWeek>());
        }

        public FeeSchedule(Tuple<TimeOnly, TimeOnly, int>[] rates, DayOfWeek[] tollFreeDays) : this()
        {
            int[] minuteRates = new int[24 * 60];
            foreach (var rate in rates)
            {
                int startMinuteOfDay = rate.Item1.Hour * 60 + rate.Item1.Minute;
                int endMinuteOfDay = rate.Item2.Hour * 60 + rate.Item2.Minute;
                for (int m = startMinuteOfDay; m <= endMinuteOfDay; m++)
                {
                    minuteRates[m] = rate.Item3;
                }
            }       
            this.RateByMinute = minuteRates;
            this.TollFreeDays.UnionWith(tollFreeDays ?? Array.Empty<DayOfWeek>());
        }

        public FeeSchedule(Tuple<TimeOnly, TimeOnly, int>[] rates, DayOfWeek[] tollFreeDays, DateOnly[] tollFreeDates) : this()
        {
            int[] minuteRates = new int[24 * 60];
            foreach (var rate in rates)
            {
                int startMinuteOfDay = rate.Item1.Hour * 60 + rate.Item1.Minute;
                int endMinuteOfDay = rate.Item2.Hour * 60 + rate.Item2.Minute;
                for (int m = startMinuteOfDay; m <= endMinuteOfDay; m++)
                {
                    minuteRates[m] = rate.Item3;
                }
            }
            this.RateByMinute = minuteRates;
            this.TollFreeDays.UnionWith(tollFreeDays ?? Array.Empty<DayOfWeek>());
            this.TollFreeDates.UnionWith(tollFreeDates ?? Array.Empty<DateOnly>());
        }
        public FeeSchedule(Tuple<TimeOnly, TimeOnly, int>[] rates, DayOfWeek[] tollFreeDays, DateOnly[] tollFreeDates, int[] tollFreeMonths)
        {
            int[] minuteRates = new int[24 * 60];
            foreach (var rate in rates)
            {
                int startMinuteOfDay = rate.Item1.Hour * 60 + rate.Item1.Minute;
                int endMinuteOfDay = rate.Item2.Hour * 60 + rate.Item2.Minute;
                for (int m = startMinuteOfDay; m <= endMinuteOfDay; m++)
                {
                    minuteRates[m] = rate.Item3;
                }
            }
            this.RateByMinute = minuteRates;
            this.TollFreeDays.UnionWith(tollFreeDays ?? Array.Empty<DayOfWeek>());
            this.TollFreeDates.UnionWith(tollFreeDates ?? Array.Empty<DateOnly>());
            this.TollFreeMonths.UnionWith(tollFreeMonths ?? Array.Empty<int>());
        }

        public int CalculateFee(DateTime date)
        {
            if (IsTollFreeDate(date)) return 0;
            int minuteOfDay = date.Hour * 60 + date.Minute;
            return RateByMinute[minuteOfDay];
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            return TollFreeMonths.Contains(date.Month) || 
                TollFreeDays.Contains(date.DayOfWeek) || 
                TollFreeDates.Contains(new DateOnly(1, date.Month, date.Day));
        }
    }
}