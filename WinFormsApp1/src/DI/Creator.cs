using System.Reflection;

namespace WinFormsApp1.DI;

public static class Creator
{
    //реализация не всё моя, с дженериком были затупы, не знал про такую перегрузку T Create<T>
    private static object Create(Type type)
    {
        Type? typeFromRegister = ServiceProvider.GetTpyeBy(type);
        if (typeFromRegister == null)
        {
            throw new Exception("Unable to create Tpye");
        }

        ConstructorInfo constructor = typeFromRegister.GetConstructors()[0];
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length < 1)
        {
            return Activator.CreateInstance(typeFromRegister)!;
        }

        object[] argsForCreate = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            Type parameterType = parameters[i]!.ParameterType;
            if (parameterType.IsEnum || parameterType.IsPrimitive)
            {
                throw new Exception($"bad parameter type ${parameterType}");
            }

            if (parameterType.IsAbstract || parameterType.IsInterface)
            {
                if (ServiceProvider.HasType(parameterType))
                {
                    argsForCreate[i] = Create(parameterType);
                }
                else
                {
                    throw new Exception($"pls register ${parameterType} in serviceProvider");
                }
            }
            else
            {
                argsForCreate[i] = Activator.CreateInstance(parameterType)!;
            }
        }

        return constructor.Invoke(argsForCreate);
    }

    public static T Create<T>()
    {
        return (T)Create(typeof(T));
    }
}