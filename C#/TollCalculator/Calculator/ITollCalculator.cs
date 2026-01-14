
namespace TollCalculator.Calculator
{
    public interface IDailyTollCalculator
    {
        int GetTollFee(VehicleTollRatePolicy vehicle, DateTime[] dates);
        
    }
}

