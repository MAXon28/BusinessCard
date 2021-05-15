using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SqlQueryStrings
{
    /// <summary>
    /// Основной класс библиотеки, который составляет базовые (INSERT, SELECT, UPDATE, DELETE) запросы к таблице в базе данных по определённой сущности (.NET тип = таблица в SQL Server)
    /// </summary>
    public static class SqlQueryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly string _insertQueryTemplate = @"INSERT INTO table_name (table_columns)
                                                                VALUES (values);";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _selectAllQueryTemplate = @"SELECT * 
                                                                   FROM table_name
                                                                        INNER JOIN second_table_name
                                                                        ON table_name.foreign_key = second_table_name.Id;";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _selectQueryTemplate = @"SELECT * 
                                                                FROM table_name
                                                                     INNER JOIN second_table_name
                                                                     ON table_name.foreign_key = second_table_name.Id
                                                                WHERE Id = @value;";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _updateQueryTemplate = @"UPDATE table_name
                                                                SET table_column = value
                                                                WHERE Id = @value;";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _deleteQueryTemplate = @"DELETE table_name
                                                                WHERE Id = @value;";

        /// <summary>
        /// Получить базовые SQL-запросы по текущему типу
        /// </summary>
        /// <param name="type"> .NET тип, которому соответствует определённая таблица в SQL </param>
        /// <returns> 
        /// Словарь базовых SQL-запросов по текущему типу (соответствие типов запросов к самим запросам: 
        /// INSERT - запрос добавление новых данных в таблицу,
        /// SELECT_ALL - запрос на выборку всех данных из таблицы,
        /// SELECT - запрос на выборку определённой строки из таблицы,
        /// UPDATE - запрос на обновление определённой строки в таблице
        /// DELETE - удаление определённой строки в таблице)
        /// </returns>
        public static Dictionary<string, string> GetQueries(Type type)
        {
            var queriesDictionary = new Dictionary<string, string>();

            var sqlTableName = GetSqlTableName(type);
            var sqlTableColumns = GetSqlTableColumns(type, out var keysDictionary);

            var insertQuery = "";
            var selectAllQuery = "";
            var selectQuery = "";
            var updateQuery = "";
            var deleteQuery = "";

            return queriesDictionary;
        }

        /// <summary>
        /// Получить название таблицы в SQL
        /// </summary>
        /// <param name="type"> Тип .NET, по которому определяется название таблицы в SQL </param>
        /// <returns> Название таблицы в SQL </returns>
        private static string GetSqlTableName(Type type)
        {
            var sqlTableName = type.Name.ToString();

            try
            {
                var attributesEnumerator = type.CustomAttributes.GetEnumerator();

                while (attributesEnumerator.MoveNext())
                {
                    if (attributesEnumerator.Current.AttributeType.Name.Equals("SqlTableAttribute"))
                        sqlTableName = attributesEnumerator.Current.ConstructorArguments[0].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при получении названия таблицы: {ex}.");
            }

            return sqlTableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keysDictionary"></param>
        /// <returns></returns>
        private static List<SqlTableColumn> GetSqlTableColumns(Type type, out Dictionary<string,string> keysDictionary)
        {
            var sqlTableColumns = new List<SqlTableColumn>();

            keysDictionary = new Dictionary<string, string>();

            var entityProperties = type.GetProperties();

            foreach (var entityProperty in entityProperties)
            {
                var columnName = entityProperty.Name.ToString();
                var isForeignKey = false;

                var attributesEnumerator = entityProperty.CustomAttributes.GetEnumerator();

                while (attributesEnumerator.MoveNext())
                {
                    if (attributesEnumerator.Current.AttributeType.Name.Equals("SqlForeignKeyAttribute"))
                    {
                        columnName = attributesEnumerator.Current.ConstructorArguments[0].Value.ToString();
                        isForeignKey = true;
                        keysDictionary.Add(columnName, attributesEnumerator.Current.ConstructorArguments[1].Value.ToString());
                    }
                }

                sqlTableColumns.Add(new SqlTableColumn(columnName, isForeignKey));
            }

            return sqlTableColumns;
        }

        private static string GetInsertQuery()
        {
            return "";
        }
    }
}