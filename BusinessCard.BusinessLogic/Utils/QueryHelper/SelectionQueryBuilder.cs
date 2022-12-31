using Dapper;
using System.Collections.Generic;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SelectionQueryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        protected const string TableNameTemplate = "*table_name*";

        /// <summary>
        /// 
        /// </summary>
        protected const string TableElementNameTemplate = "*table_element_name*";

        /// <summary>
		/// Шаблон выборки данных
		/// </summary>
		protected const string SelectionSetTemplate = "*selection_set*";

        /// <summary>
        /// Первая часть шаблона соединения
        /// </summary>
        protected const string FirstJoinTemplate = "*join*";

        /// <summary>
        /// Вторая часть шаблона соединения
        /// </summary>
        protected const string SecondJoinTemplate = "*join2*";

        /// <summary>
        /// Шаблон фильтрации
        /// </summary>
        protected const string WhereTemplate = "*where*";

        /// <summary>
        /// Шаблон сортировки
        /// </summary>
        protected const string OrderByTemplate = "*order_by*";

        /// <summary>
        /// Шаблон полей сортировки
        /// </summary>
        protected const string OrderByValueTemplate = "*order_by_column type_of_sort*";

        /// <summary>
        /// Шаблон выборки для подсчёта количества
        /// </summary>
        protected const string CountTemplate = "*count_element*";

        /// <summary>
        /// Параметр в запросе для сдвига проектов на конкретное число (для пагинации)
        /// </summary>
        protected const string OffsetParameter = "@Offset";

        /// <summary>
        /// Шаблон запроса
        /// </summary>
        protected readonly string _sqlQueryTemplate = @$"SELECT {SelectionSetTemplate}
												         FROM {TableNameTemplate} {TableElementNameTemplate} 
														    {FirstJoinTemplate}
														    {SecondJoinTemplate}
												        {WhereTemplate}
												            {OrderByTemplate};";

        /// <summary>
        /// Параметры запроса
        /// </summary>
        protected readonly DynamicParameters _parameters = new DynamicParameters();

        /// <summary>
        /// Запрос
        /// </summary>
        protected StringBuilder _sqlQuery;

        /// <summary>
        /// Словарь строителей фильтров
        /// </summary>
        protected Dictionary<string, IFilterBuilder> _filterBuildersDictionary;

        public SelectionQueryBuilder() => TypeOfSelect = SelectTypes.Data;

        /// <summary>
        /// Тип выборки
        /// </summary>
        public SelectTypes TypeOfSelect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestSettings">  </param>
        /// <returns>  </returns>
        public abstract QueryData GetQueryData(IRequestSettings requestSettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">  </param>
        /// <param name="tableElementName">  </param>
        protected void SetTableNamings(string tableName, string tableElementName)
        {
            _sqlQuery.Replace(TableNameTemplate, tableName);
            _sqlQuery.Replace(TableElementNameTemplate, tableElementName);
        }

        /// <summary>
        /// Установить выборку
        /// </summary>
        /// <param name="offset"> Сдвиг на конкретное число (для пагинации) </param>
        protected void SetSelect(int offset)
        {
            switch (TypeOfSelect)
            {
                case SelectTypes.Data:
                    SetDataSelect(offset);
                    break;
                case SelectTypes.Count:
                    SetCountSelect();
                    break;
            }
        }

        /// <summary>
        /// Установить выборку данных
        /// </summary>
        /// <param name="offset"> Сдвиг на конкретное число (для пагинации) </param>
        protected abstract void SetDataSelect(int offset);

        /// <summary>
        /// Устноавить выборку количества данных
        /// </summary>
        protected abstract void SetCountSelect();

        /// <summary>
        /// Установить соединение
        /// </summary>
        protected abstract void SetJoin();

        /// <summary>
        /// Установить фильтрацию
        /// </summary>
        protected abstract void SetWhere();

        /// <summary>
        /// Установить фильтр
        /// </summary>
        /// <param name="whereBuilder"> Собираемая строка фильтрации </param>
        /// <param name="filterBuilder"> Строитель фильтра </param>
        /// <param name="filterValues"> Значения для фильтрации </param>
        protected void SetFilter<T>(StringBuilder whereBuilder, IFilterBuilder filterBuilder, T filterValues)
        {
            const string where = "WHERE ";
            const string and = " AND ";

            whereBuilder.Append(whereBuilder.ToString() != where ? and : string.Empty);
            filterBuilder.SetFilter(whereBuilder, _parameters, filterValues);
        }
    }
}