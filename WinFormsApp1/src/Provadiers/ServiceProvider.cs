using WinFormsApp1.Engin;
using WinFormsApp1.Interfaces;
using WinFormsApp1.Services;

namespace WinFormsApp1.DI;

public static class ServiceProvider
{
    //your can register service example 
    // {(typeof(ITest)), typeof(Test)}
    private static readonly Dictionary<Type, Type> ServiceRegister = new()
    {
        {(typeof(IGameEngine)), typeof(GameEngine)},
        {(typeof(IMovedService)), typeof(MovedService)},
        {(typeof(IValidationMovedService)), typeof(ValidationMovedService)},
        {(typeof(IStateService)), typeof(StateService)},
    };

    public static Type? GetTpyeBy(Type type)
    {
        if (!ServiceRegister.TryGetValue(type, out var implType))
        {
            return null;
        }

        return implType;
    }
    public static bool HasType(Type type)
    {
        return ServiceRegister.ContainsKey(type);
    }
}