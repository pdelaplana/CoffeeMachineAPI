namespace CoffeeMachineAPI.Endpoints;

public interface IEndpoint
{
    delegate void Invoke();
}
