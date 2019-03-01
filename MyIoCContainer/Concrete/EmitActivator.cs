using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MyIoCContainer.Concrete
{
    public class EmitActivator
    {
        public static object CreateInstance(Type type, params object[] parameters)
        {
            Type[] parametersTypes = parameters.Select(p => p.GetType()).ToArray();
            DynamicMethod dynamicMethod = new DynamicMethod("", type, parametersTypes);
            ILGenerator il = dynamicMethod.GetILGenerator();

            //Загружаем каждый параметр в стек
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldarg, i);
            }

            ConstructorInfo constructorInfo = type.GetConstructor(parametersTypes);
            il.Emit(OpCodes.Newobj, constructorInfo);
            il.Emit(OpCodes.Ret);

            return dynamicMethod.Invoke(null, parameters);
        }
    }
}
