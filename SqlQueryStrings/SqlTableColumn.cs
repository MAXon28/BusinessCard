namespace SqlQueryStrings
{
    /// <summary>
    /// 
    /// </summary>
    internal class SqlTableColumn
    {
        public SqlTableColumn(string name, bool isForeignKey)
        {
            Name = name;
            IsForeignKey = isForeignKey;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsForeignKey { get; }
    }
}