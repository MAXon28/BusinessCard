using System;

namespace SqlQueryStrings.Annotations
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlServerForeignKeyAttribute : Attribute
    {
        public SqlServerForeignKeyAttribute(string foreignKeyTableName, string foreignKeyName)
        {
            ForeignKeyTableName = foreignKeyTableName;
            ForeignKeyName = foreignKeyName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ForeignKeyTableName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ForeignKeyName { get; }
    }
}