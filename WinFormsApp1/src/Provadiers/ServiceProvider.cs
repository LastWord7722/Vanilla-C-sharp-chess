namespace WinFormsApp1.DI;

public static class ServiceProvider
{
    //your can register service example 
    // {(typeof(ITest)), typeof(Test)}
    private static readonly Dictionary<Type, Type> _serviceRegister = new()
    {
    };

    public static Type? GetTpyeBy(Type type)
    {
        if (!_serviceRegister.TryGetValue(type, out var implType))
        {
            return null;
        }

        return implType;
    }
    public static bool HasType(Type type)
    {
        return _serviceRegister.ContainsKey(type);
    }
}