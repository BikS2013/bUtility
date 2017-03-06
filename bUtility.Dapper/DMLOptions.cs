namespace bUtility.Dapper
{
    public class DMLOptions
    {
        /// <summary>
        /// Delemeter coming before identifier. If left null none is used.
        /// </summary>
        public string IdentifierStartingDelimeter { get; set; }
        /// <summary>
        /// Delemeter coming after identifier. If left null identifierStartingDelimeter is used. If identifierStartingDelimeter is also null none is used
        /// </summary>
        public string IdentifierEndingDelimeter { get; set; }
        /// <summary>
        /// Where clause parameter delemeter to avoid reserved words, default is "P_". Must not be equal to UpdateParameterDelimeter.
        /// </summary>
        public string ParameterDelimeter { get; set; }
        /// <summary>
        /// Parameter symbol eg. "@" for SqlServer or ":" for OracleDB
        /// </summary>
        public string ParameterSymbol { get; set; }
        /// <summary>
        /// Update clause parameter delemeter to distinguish from where clause parameters, default is "U_". Must not be equal to ParameterDelimeter.
        /// </summary>
        public string UpdateParameterDelimeter { get; set; }

        /// <summary>
        /// DML options for different sql db implementations.
        /// Eg. for case sensitive db installations identifier (table/column names) delimeters should be set.
        /// </summary>
        /// <param name="parameterSymbol">Parameter symbol eg. @ for SqlServer or : for OracleDB</param>
        /// <param name="identifierStartingDelimeter">Delemeter coming before identifier. If left null none is used.</param>
        /// <param name="identifierEndingDelimeter">Delemeter coming after identifier. If left null identifierStartingDelimeter is used. If identifierStartingDelimeter is also null none is used</param>
        public DMLOptions(string parameterSymbol, string identifierStartingDelimeter = null, string identifierEndingDelimeter = null)
        {
            ParameterSymbol = parameterSymbol;
            IdentifierStartingDelimeter = identifierStartingDelimeter;
            IdentifierEndingDelimeter = identifierEndingDelimeter ?? identifierStartingDelimeter;
            ParameterDelimeter = "P_";
            UpdateParameterDelimeter = "U_";
        }
        /// <summary>
        /// Minimum options for SqlServer
        /// </summary>
        public static DMLOptions DefaultOptions
        {
            get
            {
                return new DMLOptions("@");
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
