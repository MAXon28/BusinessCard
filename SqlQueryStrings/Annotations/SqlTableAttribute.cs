using System;

namespace SqlQueryStrings.Annotations
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlTableAttribute : Attribute
    {
        public SqlTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TableName { get; }
    }
}