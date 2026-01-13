using System;
using System.Globalization;

namespace TollFeeCalculator
{
    public class TollCalculator
    {

        private readonly FeeSchedule feeSchedule;
        private readonly int minuteInterval;
        private readonly int maxTotal;

        /**
        * Initialize a new instance of the TollCalculator class.
        *
        * @param feeSchedule - holds the price rates for specific times
        * @param minuteInterval - the duration in minutes of a toll interval
        * @param maxTotal - maximum total fee to pay in a day
        */
        public TollCalculator(FeeSchedule feeSchedule, int minuteInterval, int maxTotal)
        {
            this.feeSchedule = feeSchedule;
            this.minuteInterval = minuteInterval;
            this.maxTotal = maxTotal;
        }
        
        /**
        * Calculate the total toll fee for one day
        *
        * @param vehicle - the vehicle
        * @param dates   - date and time of all passes on one day
        * @return - the total toll fee for that day
        */
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
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
         
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);
                
                if (diff.TotalMinutes <= minuteInterval)
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
            if (totalFee > maxTotal) totalFee = maxTotal;
            return totalFee;
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeVehicle(vehicle)) return 0;
            return feeSchedule.CalculateFee(date);
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            String vehicleType = vehicle.GetVehicleType();
            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType); 
        }


        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
    }
}