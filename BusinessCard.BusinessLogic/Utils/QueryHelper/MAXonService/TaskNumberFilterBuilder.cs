using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using Dapper;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService
{
    /// <summary>
    /// Строитель фильтра по номеру задачи
    /// </summary>
    internal class TaskNumberFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string TaskNumberConditionValue = "@TaskNumber";

        /// <summary>
        /// Фильтр номера задачи
        /// </summary>
        private readonly string _taskNumberFilter = $"task.TaskNumber LIKE '%' + {TaskNumberConditionValue} + '%'";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            var taskNumber = value as string;
            sqlQuery.Append(_taskNumberFilter);
            parameters.Add(TaskNumberConditionValue, taskNumber, DbType.String, ParameterDirection.Input);
        }
    }
}