using System;

namespace SqlQueryStrings.Annotations
{
    /// <summary>
    /// Атрибут, который позволяет указать, что поле в класса-модели .NET является внешним ключом в таблице базы данных. Нужно указать имя таблицы, которая связывается с внешним ключом и можно указать имя внешнего ключа в таблице базы данных.
    /// </summary>
    public class SqlForeignKeyAttribute : Attribute
    {
        public SqlForeignKeyAttribute(string foreignKeyTableName)
        {
            ForeignKeyTableName = foreignKeyTableName;
        }

        public SqlForeignKeyAttribute(string foreignKeyTableName, string foreignKeyName)
        {
            ForeignKeyTableName = foreignKeyTableName;
            ForeignKeyName = foreignKeyName;
        }

        /// <summary>
        /// Название таблицы связанной по внешнему ключу
        /// </summary>
        public string ForeignKeyTableName { get; set; }

        /// <summary>
        /// Имя внешнего ключа
        /// </summary>
        public string ForeignKeyName { get; }
    }
}