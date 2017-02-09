using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Dapper
{
    public class DMLOptions
    {
        public char? IdentifierStartingDelimeter { get; set; }
        public char? IdentifierEndingDelimeter { get; set; }
        public char ParameterDelimeter { get; set; }
        public string UpdateParameterDelimeter { get; set; }

        /// <summary>
        /// DML options for different sql db implementations.
        /// Eg. for case sensitive db installations identifier (table/column names) delimeters should be set.
        /// </summary>
        /// <param name="parameterDelimeter">Parameter delemeter eg. @ for SqlServer or : for OracleDB</param>
        /// <param name="updateParameterDelimeter">Update clause parameter delemeter to distinguish from where clause parameters, eg. "U_".</param>
        /// <param name="identifierStartingDelimeter">Delemeter coming before identifier. If left null none is used.</param>
        /// <param name="identifierEndingDelimeter">Delemeter coming after identifier. If left null identifierStartingDelimeter is used. If identifierStartingDelimeter is also null none is used</param>
        public DMLOptions(char parameterDelimeter, string updateParameterDelimeter, char? identifierStartingDelimeter = null, char? identifierEndingDelimeter = null)
        {
            ParameterDelimeter = parameterDelimeter;
            UpdateParameterDelimeter = updateParameterDelimeter;
            IdentifierStartingDelimeter = identifierStartingDelimeter;
            IdentifierEndingDelimeter = identifierEndingDelimeter ?? identifierStartingDelimeter;
        }

        public static DMLOptions DefaultOptions
        {
            get
            {
                return new DMLOptions('@', "U_");
            }
        }

        private static DMLOptions _currentOptions;
        public static DMLOptions CurrentOptions
        {
            get
            {
                return _currentOptions ?? DefaultOptions;
            }
            set
            {
                _currentOptions = value;
            }
        }
    }
}
