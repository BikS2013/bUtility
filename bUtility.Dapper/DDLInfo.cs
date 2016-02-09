using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Dapper
{
    public class DDLInfo
    {
        public static DDLInfo Column(string name, int? length = null, int? decimals = null, bool allowNull = true)
        {
            return new DDLInfo(name, length, decimals, allowNull);
        }

        public string Name { get; set; }
        int? _Length { get; set; }
        int? _Decimals { get; set; }
        public bool _AllowNull { get; set; }
        public DDLInfo(string name, int? length = null, int? decimals = null, bool allowNull = true)
        {
            Name = name;
            _Length = length;
            _Decimals = decimals;
            _AllowNull = allowNull;
        }

        public DDLInfo Length(int? value)
        {
            _Length = value;
            return this;
        }
        public DDLInfo Decimals(int? value)
        {
            _Decimals = value;
            return this;
        }
        public DDLInfo AllowNull(bool option = true)
        {
            _AllowNull = option;
            return this;
        }

        public string LengthExpression
        {
            get
            {
                if (_Length != null && _Length > 0)
                {
                    if (_Decimals != null && _Decimals > 0)
                    {
                        return $"({_Length},{_Decimals})";
                    }
                    return $"({_Length})";
                }
                return null;
            }
        }

        public string NullableExpression
        {
            get
            {
                if (_AllowNull)
                {
                    return "default null";
                }
                return "not null";
            }
        }
    }
}
