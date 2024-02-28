namespace CoffeeMachineAPI.Application
{
    public interface IDateTimeProvider
    {
        DateTime UTCNow { get; }
        DateTime Create(int year, int month, int day);
    }
}
