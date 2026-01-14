using System;
using TollCalculator.Calculator;
using TollCalculator;

class Porgam{
    static void Main(string[] args)
    {
        RateSchedule schedule = new RateSchedule(
            new Tuple<TimeOnly, TimeOnly, int>[]
            {
                Tuple.Create(new TimeOnly(6,0), new TimeOnly(6,29), 8),
                Tuple.Create(new TimeOnly(6,30), new TimeOnly(6,59), 13),
                Tuple.Create(new TimeOnly(7,0), new TimeOnly(7,59), 18),
                Tuple.Create(new TimeOnly(8,0), new TimeOnly(8,29), 13),
                Tuple.Create(new TimeOnly(8,30), new TimeOnly(14,59), 8),
                Tuple.Create(new TimeOnly(15,0), new TimeOnly(15,29), 13),
                Tuple.Create(new TimeOnly(15,30), new TimeOnly(16,59), 18),
                Tuple.Create(new TimeOnly(17,0), new TimeOnly(17,59), 13),
                Tuple.Create(new TimeOnly(18,0), new TimeOnly(18,29), 8)
            },
            new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday },
            new DateOnly[]
            {
                new DateOnly(2023, 1, 1),   // New Year's Day
                new DateOnly(2023, 3, 28),  // March Holiday
                new DateOnly(2023, 3, 29),  // March Holiday
                new DateOnly(2023, 4, 1),   // April Holiday
                new DateOnly(2023, 4, 30),  // Walpurgis Night
                new DateOnly(2023, 5, 1),   // Labor Day
                new DateOnly(2023, 5, 8),   // Victory in Europe Day
                new DateOnly(2023, 5, 9),   // Ascension Day
                new DateOnly(2023, 6, 5),   // Pentecost
                new DateOnly(2023, 6, 6),   // Whit Monday
                new DateOnly(2023, 6, 21),  // Midsummer Eve
                new DateOnly(2023, 11, 1),  // All Saints' Day
                new DateOnly(2023, 12, 24), // Christmas Eve
                new DateOnly(2023, 12, 25), // Christmas Day
                new DateOnly(2023, 12, 26), // Second Day of Christmas
                new DateOnly(2023, 12, 31)  // New Year's Eve
            },
            new int[] { 7 } // July
        );
        DateTime[] dates = new DateTime[]
        {
            new DateTime(2023, 3, 27, 6, 15, 0),
            new DateTime(2023, 3, 27, 6, 45, 0),
            new DateTime(2023, 3, 27, 7, 15, 0),
            new DateTime(2023, 3, 27, 8, 0, 0),
            new DateTime(2023, 3, 27, 15, 30, 0),
            new DateTime(2023, 3, 27, 16, 0, 0),
            new DateTime(2023, 3, 27, 17, 15, 0)
        };

        IDailyTollCalculator calculator = new IntervalCalculator(60, 60);

        VehicleTollRatePolicy mc = new TimeSchedulePolicy(new TollFreeVehicle(), schedule);
        Console.WriteLine("Total toll for motorcycle: " + calculator.GetTollFee(mc, dates));

        VehicleTollRatePolicy car = new TimeSchedulePolicy(new TollableVehicle(), schedule);
        Console.WriteLine("Total toll for car: " + calculator.GetTollFee(car, dates));

    }
}