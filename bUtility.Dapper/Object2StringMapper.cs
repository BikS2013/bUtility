using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace bUtility.Dapper
{
    public class Object2StringMapper<T> : SqlMapper.TypeHandler<T>
    {
        public override T Parse(object value)
        {
            return JsonConvert.DeserializeObject<T>((string)value);

        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }

    }
}
