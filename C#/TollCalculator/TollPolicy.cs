using TollCalculator;

namespace TollCalculator
{
    public abstract class VehicleTollRatePolicy : IVehicle
    {
        public IVehicle vehicle;
        public VehicleTollRatePolicy(IVehicle vehicle) => this.vehicle = vehicle;
        public bool IsTollable() => vehicle.IsTollable();
        public virtual int GetRate(DateTime date){
            if (!IsTollable()) return 0;
            return CalculateRate(date);
        }
        public abstract int CalculateRate(DateTime date);
    }

    public class TimeSchedulePolicy : VehicleTollRatePolicy
    {
        private readonly RateSchedule Schedule;
        public TimeSchedulePolicy(IVehicle vehicle, RateSchedule schedule): base(vehicle) => this.Schedule = schedule;
        public override int CalculateRate(DateTime date) => Schedule.CalculateFee(date);
    }
}