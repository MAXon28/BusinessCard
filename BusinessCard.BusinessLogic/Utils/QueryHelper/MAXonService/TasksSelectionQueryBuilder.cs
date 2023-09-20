using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using System.Collections.Generic;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    internal class TasksSelectionQueryBuilder : SelectionQueryBuilder
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        private const string TableName = "TasksPersonalInformation";

        /// <summary>
        /// Название переменной соответствующей таблицы
        /// </summary>
        private const string TableElementName = "taskPersonalInformation";

        /// <summary>
        /// 
        /// </summary>
        private const string JoinTableElementName = "task";

        /// <summary>
        /// 
        /// </summary>
        private TaskRequestSettings _taskRequestSettings;

        public TasksSelectionQueryBuilder()
        {
            _filterBuildersDictionary = new Dictionary<string, IFilterBuilder>
            {
                [FilterConstants.Id] = new IdFilterBuilder(JoinTableElementName),
                [FilterConstants.User] = new UserIdFilterBuilder(TableElementName),
                [FilterConstants.TaskNumber] = new TaskNumberFilterBuilder(),
                [FilterConstants.TaskStatusCode] = new StatusCodeFilterBuilder()
            };
        }

        /// <inheritdoc/>
        public override QueryData GetQueryData(RequestSettings requestSettings)
        {
            _dataCountInPackage = requestSettings.CountInPackage;

            _taskRequestSettings = requestSettings as TaskRequestSettings;

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
            var condition = _taskRequestSettings.UserId == -1
                ? "taskRecord.ReadByMAXon28Team = 0"
                : "taskRecord.ReadByUser = 0";
            var tasksSelectionSet = $@"{JoinTableElementName}.Id,
                                       {JoinTableElementName}.TaskNumber,
	                                   {JoinTableElementName}.FixedPrice,
	                                   {JoinTableElementName}.TaskCreationDate,
	                                   {JoinTableElementName}.StatusUpdateDate,
                                       {JoinTableElementName}.Url,
	                                   service.Name AS ServiceName,
	                                   rate.Name AS RateName,
	                                   taskStatus.Status,
	                                   taskStatus.StatusCode,
                                       (SELECT COUNT(1)
		                                FROM TaskRecords taskRecord
		                                WHERE taskRecord.TaskId = {JoinTableElementName}.Id AND {condition}) AS UnreadRecordsCount";
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
        protected override void SetJoin()
        {
            var firstJoin = $@"INNER JOIN Tasks {JoinTableElementName}
	                            ON {TableElementName}.TaskId = {JoinTableElementName}.Id
	                            INNER JOIN TaskStatuses taskStatus
	                            ON {JoinTableElementName}.TaskStatusId = taskStatus.Id";

            var secondJoin = $@"INNER JOIN Services service
	                            ON {JoinTableElementName}.ServiceId = service.Id
	                            LEFT JOIN Rates rate
	                            ON {JoinTableElementName}.RateId = rate.Id";

            if (TypeOfSelect == SelectTypes.Data)
            {
                _sqlQuery.Replace(FirstJoinTemplate, firstJoin);
                _sqlQuery.Replace(SecondJoinTemplate, secondJoin);
            }
            else
            {
                if (string.IsNullOrEmpty(_taskRequestSettings.SearchText) is false && _taskRequestSettings.StatusType == TaskStatusTypes.All)
                    _sqlQuery.Replace(FirstJoinTemplate,
                                $@"INNER JOIN Tasks {JoinTableElementName}
	                               ON {TableElementName}.TaskId = {JoinTableElementName}.Id");
                else
                    _sqlQuery.Replace(FirstJoinTemplate,
                        _taskRequestSettings.StatusType != TaskStatusTypes.All
                        ? firstJoin
                        : string.Empty);
                _sqlQuery.Replace(SecondJoinTemplate, string.Empty);
            }
        }

        /// <inheritdoc/>
        protected override void SetWhere()
        {
            const string where = "WHERE ";

            if (_taskRequestSettings.UserId == -1 && _taskRequestSettings.LastElementId == -1 && string.IsNullOrEmpty(_taskRequestSettings.SearchText) && _taskRequestSettings.StatusType == TaskStatusTypes.All)
            {
                _sqlQuery.Replace(WhereTemplate, string.Empty);
                return;
            }

            var whereBuilder = new StringBuilder(where);

            if (_taskRequestSettings.UserId != -1)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.User], _taskRequestSettings.UserId);

            if (_taskRequestSettings.LastElementId != -1 && TypeOfSelect == SelectTypes.Data)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.Id], _taskRequestSettings.LastElementId);

            if (string.IsNullOrEmpty(_taskRequestSettings.SearchText) is false)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.TaskNumber], _taskRequestSettings.SearchText);

            if (_taskRequestSettings.StatusType != TaskStatusTypes.All)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.TaskStatusCode], _taskRequestSettings.StatusType.ToInt());

            _sqlQuery.Replace(WhereTemplate, whereBuilder.ToString());
        }

        /// <inheritdoc/>
        protected override void SetOrderBy()
        {
            var orderBy = $@"ORDER BY {JoinTableElementName}.Id DESC";
            _sqlQuery.Replace(OrderByTemplate, orderBy);
        }
    }
}