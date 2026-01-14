using System;
using TollCalculator.Calculator;

namespace TollCalculator.Calculator
{
    public class IntervalCalculator : IDailyTollCalculator
    {

        private readonly int MinuteInterval;
        private readonly int MaxTotal;

        /**
        * Initialize a new instance of the TollCalculator class.
        *
        * @param minuteInterval - the duration in minutes of a toll interval
        * @param maxTotal - maximum total fee to pay in a day
        */
        public IntervalCalculator(int minuteInterval, int maxTotal)
        {
            this.MinuteInterval = minuteInterval;
            this.MaxTotal = maxTotal;
        }
        
        /**
        * Calculate the total toll fee for one day
        *
        * @param vehicle - the vehicle
        * @param dates   - date and time of all passes on one day
        * @return - the total toll fee for that day
        */
        public int GetTollFee(VehicleTollRatePolicy vehicle, DateTime[] dates)
        {
            dates = dates.Order().ToArray();
            DateTime intervalStart = dates[0];
            var day = intervalStart.Day;
            int totalFee = 0;

            foreach (DateTime date in dates){
                TimeSpan diff = date - intervalStart;

                if (diff.TotalDays > 1) {
                    throw new ArgumentException("All dates must be within the same day");
                };
         
                int nextFee = vehicle.GetRate(date);
                int tempFee = vehicle.GetRate(intervalStart);
                
                if (diff.TotalMinutes <= MinuteInterval)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                    intervalStart = date;
                }
            }
            if (totalFee > MaxTotal) totalFee = MaxTotal;
            return totalFee;
        }
    }
}