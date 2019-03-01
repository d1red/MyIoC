using MyIoCContainer.Abstract;
using MyIoCContainer.Attributes;
using MyIoCContainer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyIoCContainer
{
    public class Container : IContainer
    {
        #region Fields
        private readonly IDictionary<Type, Type> _types;
        private readonly bool _emit;
        #endregion

        #region Constructors
        public Container()
        {
            _types = new Dictionary<Type, Type>();
            _emit = false;
        }

        public Container(bool emit)
        {
            _types = new Dictionary<Type, Type>();
            _emit = emit;
        }
        #endregion

        #region Public methods
        public void AddType(Type type)
        {
            _types.Add(type, type);
        }

        public void AddType(Type type, Type baseType)
        {
            _types.Add(baseType, type);
        }

        public void AddAssembly(Assembly assembly)
        {
            var types = assembly.ExportedTypes;
            foreach (var type in types)
            {
                var importConstructorAttr = type.GetCustomAttribute<ImportConstructor>();
                if (importConstructorAttr != null || GetProperties<Import>(type).Any())
                {
                    _types.Add(type, type);
                }

                var exportAttr = type.GetCustomAttributes<Export>();
                foreach (var attr in exportAttr)
                {
                    _types.Add(attr.Type ?? type, type);
                }
            }
        }

        public object CreateInstance(Type type)
        {
            var instance = ConstructInstanceOfType(type);
            return instance;
        }

        public T CreateInstance<T>()
        {
            var type = typeof(T);
            var instance = (T)ConstructInstanceOfType(type);
            return instance;
        }
        #endregion

        #region Private methods
        //Получает свойства объекта, в зависимоти от атрибута
        private IEnumerable<PropertyInfo> GetProperties<A>(Type type) where A : Attribute
        {
            return type.GetProperties().Where(p => p.GetCustomAttribute<A>() != null);
        }

        //Создает экземпляр объекта
        private object ConstructInstanceOfType(Type type)
        {
            if (!_types.ContainsKey(type))
            {
                throw new Exception($"Не удается создать экземпляр типа {type.FullName}");
            }
            
            Type instanceType = _types[type];
            ConstructorInfo constructorInfo = GetConstructor(instanceType);
            object instance = ActivatorCreateInstance(instanceType, constructorInfo);

            if (instanceType.GetCustomAttribute<ImportConstructor>() != null)
            {
                return instance;
            }
            
            return instance;
        }

        //Возвращает первый конструктор, для текущего объекта
        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors().FirstOrDefault();
            return constructor;
        }

        //Создает экземпляр типа используя конструктор
        private object ActivatorCreateInstance(Type type, ConstructorInfo constructorInfo)
        {
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            List<object> parametersInstances = new List<object>(parameters.Length);
            Array.ForEach(parameters, p => parametersInstances.Add(ConstructInstanceOfType(p.ParameterType)));

            if (_emit) //если используется System.Reflection.Emit
            {
                return EmitActivator.CreateInstance(type, parametersInstances.ToArray());
            }
            return Activator.CreateInstance(type, parametersInstances.ToArray());
        }
        #endregion
    }
}
