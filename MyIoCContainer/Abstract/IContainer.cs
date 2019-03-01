using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoCContainer.Abstract
{
    public interface IContainer
    {
        /// <summary>
        /// Добавляет объект в коллекцию зависимостей
        /// </summary>
        /// <param name="type">Тип объекта, который требует внедрение зависимости</param>
        void AddType(Type type);

        /// <summary>
        /// Добавляет объект в коллекцию зависимостей
        /// </summary>
        /// <param name="type">Тип объекта, который требует внедрение зависимости</param>
        /// <param name="baseType">Тип объекта, от которого зависит класс</param>
        void AddType(Type type, Type baseType);

        /// <summary>
        /// Добавляет в контейнер все размеченные атрибутами [ImportConstructor], [Import] и [Export],
        /// указав сборку
        /// </summary>
        /// <param name="assembly">Сборка</param>
        void AddAssembly(Assembly assembly);

        /// <summary>
        /// Получает экземпляр ранее зарегистрированного класса со всеми зависимостями
        /// </summary>
        /// <param name="type">Тип класса</param>
        /// <returns></returns>
        object CreateInstance(Type type);

        /// <summary>
        /// Получает экземпляр ранее зарегистрированного класса со всеми зависимостями
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <returns></returns>
        T CreateInstance<T>();
    }
}
