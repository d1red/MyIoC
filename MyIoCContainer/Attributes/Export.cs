using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoCContainer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Export : Attribute
    {
        public Type Type { get; }

        public Export()
        { }

        public Export(Type type)
        {
            Type = type;
        }
    }
}
