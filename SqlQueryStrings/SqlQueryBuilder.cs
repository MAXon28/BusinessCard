using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SqlQueryStrings
{
    /// <summary>
    /// Основной класс библиотеки, который составляет базовые (INSERT, SELECT, UPDATE, DELETE) запросы к таблице в базе данных по определённой сущности (.NET тип = таблица в SQL Server)
    /// </summary>
    public static class SqlQueryBuilder
    {
        /// <summary>
        /// Шаблон запроса вставки новой записи в базу данных
        /// </summary>
        private static readonly string _insertQueryTemplate = @"INSERT INTO *table_name* (*table_columns*)
                                                                VALUES (*values*)";

        /// <summary>
        /// Шаблон запроса полной выборки из базы данных
        /// </summary>
        private static readonly string _selectAllQueryTemplate = @"SELECT * 
                                                                   FROM *table_name*";

        /// <summary>
        /// Шаблон запроса выборки определённой строки из базы данных
        /// </summary>
        private static readonly string _selectOneQueryTemplate = @"SELECT * 
                                                                   FROM *table_name*
                                                                   WHERE Id = @id_value";

        /// <summary>
        /// Шаблон запроса обновления определённой строки в базе данных
        /// </summary>
        private static readonly string _updateQueryTemplate = @"UPDATE *table_name*
                                                                SET *table_column* = @update_value
                                                                WHERE Id = @id_value";

        /// <summary>
        /// Шаблон запроса удаления определённой строки из базы данных
        /// </summary>
        private static readonly string _deleteQueryTemplate = @"DELETE *table_name*
                                                                WHERE Id = @id_value";

        /// <summary>
        /// Шаблон JOIN
        /// </summary>
        private static readonly string _joinTemplate = @"               INNER JOIN *second_table_name*
                                                                        ON *table_name*.*foreign_key* = *second_table_name*.Id";

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

            queriesDictionary.Add("INSERT", GetInsertQuery(sqlTableName, sqlTableColumns));
            queriesDictionary.Add("SELECT_ALL", GetSelectQuery(sqlTableName, keysDictionary, TypeOfSelect.All));
            queriesDictionary.Add("SELECT", GetSelectQuery(sqlTableName, keysDictionary, TypeOfSelect.One));
            queriesDictionary.Add("UPDATE", GetUpdateQuery(sqlTableName, sqlTableColumns));
            queriesDictionary.Add("DELETE", GetDeleteQuery(sqlTableName));

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
        /// Получить список столбцов в SQL-таблице
        /// </summary>
        /// <param name="type"> Тип .NET, по которому определяются столбцы таблицы в SQL </param>
        /// <param name="keysDictionary"> Выходной параметр, который хранит в себе словарь соответствия внешнго ключа и связанной таблицы </param>
        /// <returns> Список столбцов в SQL-таблице </returns>
        private static List<string> GetSqlTableColumns(Type type, out Dictionary<string,string> keysDictionary)
        {
            var sqlTableColumns = new List<string>();

            keysDictionary = new Dictionary<string, string>();

            var entityProperties = type.GetProperties();

            foreach (var entityProperty in entityProperties)
            {
                var columnName = entityProperty.Name.ToString();

                var attributesEnumerator = entityProperty.CustomAttributes.GetEnumerator();

                while (attributesEnumerator.MoveNext())
                {
                    if (attributesEnumerator.Current.AttributeType.Name.Equals("SqlForeignKeyAttribute"))
                    {
                        var foreignKeyTableName = attributesEnumerator.Current.ConstructorArguments[0].Value.ToString();
                        var foreignKey = attributesEnumerator.Current.ConstructorArguments[1].Value.ToString();

                        if (!string.IsNullOrEmpty(foreignKey))
                            columnName = foreignKey;

                        keysDictionary.Add(columnName, foreignKeyTableName);
                    }
                }

                sqlTableColumns.Add(columnName);
            }

            return sqlTableColumns;
        }

        /// <summary>
        /// Получить запрос вставки 
        /// </summary>
        /// <param name="sqlTableName"> Название таблицы в SQL </param>
        /// <param name="sqlTableColumns"> Список столбцов в SQL-таблице </param>
        /// <returns> Запрос вставки </returns>
        private static string GetInsertQuery(string sqlTableName, List<string> sqlTableColumns)
        {
            var insertQuery = new StringBuilder(_insertQueryTemplate.Replace("*table_name*", sqlTableName));
            insertQuery.Replace("*table_columns*", string.Join(", ", sqlTableColumns));
            insertQuery.Replace("*values*", string.Join(", ", sqlTableColumns.Select(sqlTableColumn => $"@{sqlTableColumn}")));
            return insertQuery.ToString();
        }

        /// <summary>
        /// Получить запрос выборки
        /// </summary>
        /// <param name="sqlTableName"> Название таблицы в SQL </param>
        /// <param name="keysDictionary"> Словарь соответствия внешнго ключа и связанной таблицы </param>
        /// <param name="typeOfSelect"> Тип выборки </param>
        /// <returns> Запрос выборки </returns>
        private static string GetSelectQuery(string sqlTableName, Dictionary<string, string> keysDictionary, TypeOfSelect typeOfSelect)
        {
            var selectQueryTemplate = typeOfSelect == TypeOfSelect.One ? _selectOneQueryTemplate : _selectAllQueryTemplate;

            var selectAllQuery = new StringBuilder(selectQueryTemplate.Replace("*table_name*", sqlTableName));

            foreach (var keyDictionary in keysDictionary)
            {
                selectAllQuery.Insert(selectAllQuery.Length - (typeOfSelect == TypeOfSelect.One ? 21 : 0), _joinTemplate);
                selectAllQuery.Replace("*second_table_name*", keyDictionary.Value);
                selectAllQuery.Replace("*table_name*", sqlTableName);
                selectAllQuery.Replace("*foreign_key*", keyDictionary.Key);
            }

            return selectAllQuery.ToString();
        }

        /// <summary>
        /// Получить запрос обновления
        /// </summary>
        /// <param name="sqlTableName"> Название таблицы в SQL </param>
        /// <param name="sqlTableColumns"> Список столбцов в SQL-таблице </param>
        /// <returns> Запрос обновления </returns>
        private static string GetUpdateQuery(string sqlTableName, List<string> sqlTableColumns)
        {
            var updateQuery = new StringBuilder(_updateQueryTemplate.Replace("*table_name*", sqlTableName));

            var listOfUpdateStrings = new List<string>();

            foreach (var sqlTableColumn in sqlTableColumns)
                listOfUpdateStrings.Add($"{sqlTableColumn} = @{sqlTableColumn}");

            var updateStrings = string.Join(",\n\t", listOfUpdateStrings);

            updateQuery.Replace("*table_column* = @update_value", updateStrings);

            return updateQuery.ToString();
        }

        /// <summary>
        /// Получить запрос удаления
        /// </summary>
        /// <param name="sqlTableName"> Название таблицы в SQL </param>
        /// <returns> Запрос удаления </returns>
        private static string GetDeleteQuery(string sqlTableName)
        {
            return _deleteQueryTemplate.Replace("*table_name*", sqlTableName);
        }
    }
}