using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using TollFeeCalculator;

namespace Traffic
{
    class Program
    {
        static void Main(string[] args)
        {

            FeeSchedule default_fees = new FeeSchedule();
            FeeSchedule feeSchedule = new FeeSchedule([new Tuple<TimeOnly, TimeOnly, int>(new TimeOnly(6,30),new TimeOnly(6,59), 9), new Tuple<TimeOnly, TimeOnly, int>(new TimeOnly(7,30),new TimeOnly(7,59), 19)], [DayOfWeek.Monday,DayOfWeek.Tuesday,DayOfWeek.Wednesday], [new DateOnly(1,5,1)], [4]);
            TollCalculator default_calc = new TollCalculator(default_fees, 60, 60);
            TollCalculator calc = new TollCalculator(feeSchedule, 60, 60);

            Motorbike mc = new Motorbike();
            Car c = new Car();

            Console.WriteLine(calc.GetTollFee(new DateTime(2008, 5, 2, 6, 30, 52), mc));
            Console.WriteLine(calc.GetTollFee(new DateTime(2008, 5, 2, 6, 30, 52), c));

            DateTime[] dates = [new DateTime(2008, 5, 2, 6, 30, 52), new DateTime(2008, 5, 2, 7, 30, 52),  new DateTime(2008, 5, 2, 6, 41, 52)];
            Console.WriteLine(calc.GetTollFee(c, dates));
            Console.WriteLine(default_calc.GetTollFee(c, dates));
        }
    }
}