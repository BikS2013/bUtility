using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SensitiveDataAttribute : System.Attribute
    {
        public SensitiveDataAttribute(Direction direction)
        {
            this.Direction = direction;
        }

        public Direction Direction { get; private set; }

        public string EmittedPropertyName { get; set; }
    }

    public enum Direction
    {
        Input,
        Output
    }
}
