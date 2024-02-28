
namespace CoffeeMachineAPI.Application
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UTCNow => DateTime.UtcNow;

        public DateTime Create(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }
    }
}
