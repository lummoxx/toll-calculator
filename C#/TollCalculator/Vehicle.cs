namespace TollCalculator
{
    public interface IVehicle
    {
        public bool IsTollable();
    }
    public class TollableVehicle : IVehicle
    {
        public bool IsTollable() => true;
    }
    public class TollFreeVehicle : IVehicle
    {
        public bool IsTollable() => false;
    }

}