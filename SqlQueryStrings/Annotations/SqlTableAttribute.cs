using System;

namespace SqlQueryStrings.Annotations
{
    /// <summary>
    /// Атрибут, который позволяет указать классу-модели .NET соответствующее ему название таблицы в базе данных
    /// </summary>
    public class SqlTableAttribute : Attribute
    {
        public SqlTableAttribute(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// Название таблицы
        /// </summary>
        public string TableName { get; }
    }
}