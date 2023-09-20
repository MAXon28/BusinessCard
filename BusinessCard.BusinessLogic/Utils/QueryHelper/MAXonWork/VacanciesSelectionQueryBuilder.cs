using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using System.Collections.Generic;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonWork
{
    /// <summary>
    /// 
    /// </summary>
    internal class VacanciesSelectionQueryBuilder : SelectionQueryBuilder
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        private const string TableName = "Vacancies";

        /// <summary>
        /// Название переменной соответствующей таблицы
        /// </summary>
        private const string TableElementName = "vacancy";

        /// <summary>
        /// 
        /// </summary>
        private VacancyRequestSettings _vacancyRequestSettings;

        public VacanciesSelectionQueryBuilder()
        {
            _filterBuildersDictionary = new Dictionary<string, IFilterBuilder>
            {
                [FilterConstants.Id] = new IdFilterBuilder(TableElementName),
                [FilterConstants.VacancyName] = new VacancyNameFilterBuilder()
            };
        }

        /// <inheritdoc/>
        public override QueryData GetQueryData(RequestSettings requestSettings)
        {
            _dataCountInPackage = requestSettings.CountInPackage;

            _vacancyRequestSettings = requestSettings as VacancyRequestSettings;

            _sqlQuery = new StringBuilder();
            _sqlQuery.Append(_sqlQueryTemplate);
            SetTableNamings(TableName, TableElementName);
            SetSelect();
            SetJoin();
            SetWhere();
            if (TypeOfSelect == SelectTypes.Data)
            {
                SetTop();
                SetOrderBy();
            }

            return new(_sqlQuery.ToString(), _parameters);
        }

        /// <inheritdoc/>
        protected override void SetDataSelect()
        {
            var tasksSelectionSet = $@"{TableElementName}.Id,
                                       {TableElementName}.Name,
	                                   {TableElementName}.Company,
	                                   {TableElementName}.SalaryFrom,
	                                   {TableElementName}.SalaryTo,
                                       {TableElementName}.Currency,
                                       {TableElementName}.CreationDate,
                                       {TableElementName}.ViewedByMAXon28Team";
            _sqlQuery.Replace(SelectionSetTemplate, tasksSelectionSet);
        }

        /// <inheritdoc/>
        protected override void SetCountSelect()
        {
            var postsCountSelectionSet = $@"COUNT ({TableElementName}.Id)";

            _sqlQuery.Replace(SelectionSetTemplate, postsCountSelectionSet);
            _sqlQuery.Replace(TopTemplate, string.Empty);
            _sqlQuery.Replace(OrderByTemplate, string.Empty);
        }

        /// <inheritdoc/>
        protected override void SetWhere()
        {
            const string where = "WHERE ";

            if (_vacancyRequestSettings.LastElementId == -1 && string.IsNullOrEmpty(_vacancyRequestSettings.SearchText))
            {
                _sqlQuery.Replace(WhereTemplate, string.Empty);
                return;
            }

            var whereBuilder = new StringBuilder(where);

            if (_vacancyRequestSettings.LastElementId != -1 && TypeOfSelect == SelectTypes.Data)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.Id], _vacancyRequestSettings.LastElementId);

            if (string.IsNullOrEmpty(_vacancyRequestSettings.SearchText) is false)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.VacancyName], _vacancyRequestSettings.SearchText);

            _sqlQuery.Replace(WhereTemplate, whereBuilder.ToString());
        }

        /// <inheritdoc/>
        protected override void SetOrderBy()
        {
            var orderBy = $@"ORDER BY {TableElementName}.Id DESC";
            _sqlQuery.Replace(OrderByTemplate, orderBy);
        }
    }
}