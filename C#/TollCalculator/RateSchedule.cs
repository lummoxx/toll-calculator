using System;

namespace TollCalculator
{
    public class RateSchedule
    {
        private int[] RateByMinute = new int[24 * 60];
        private HashSet<int> TollFreeMonths = new();
        private HashSet<DateOnly> TollFreeDates = new();
        private HashSet<DayOfWeek> TollFreeDays = new();


        public RateSchedule(Tuple<TimeOnly, TimeOnly, int>[] rates, DayOfWeek[] tollFreeDays, DateOnly[] tollFreeDates, int[] tollFreeMonths)
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
                TollFreeDates.Contains(DateOnly.FromDateTime(date));
        }
    }
}