using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <summary>
    /// Настройки запроса проектов
    /// </summary>
    internal class ProjectRequestSettings : RequestSettings
    {
        public ProjectRequestSettings(
            int lastProjectId,
            int projectsCountInPackage,
            string searchText,
            List<int> typesId,
            List<int> categoriesId,
            List<int> compatibilitiesId,
            int typeOfSort,
            int offset) : base(lastProjectId, projectsCountInPackage, searchText)
        {
            FilterValuesDictionary[FilterConstants.ProjectType] = typesId;
            FilterValuesDictionary[FilterConstants.ProjectCategory] = categoriesId;
            FilterValuesDictionary[FilterConstants.ProjectCompatibility] = compatibilitiesId;
            TypeOfSort = typeOfSort.ToEnum<SortTypes>();
            Offset = offset;
        }

        /// <summary>
        /// Словарь значений для фильтрации
        /// </summary>
        public Dictionary<string, List<int>> FilterValuesDictionary { get; } = new Dictionary<string, List<int>>
        {
            [FilterConstants.ProjectType] = new List<int>(),
            [FilterConstants.ProjectCategory] = new List<int>(),
            [FilterConstants.ProjectCompatibility] = new List<int>()
        };

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortTypes TypeOfSort { get; }

        /// <summary>
        /// Сдвиг проектов на конкретное число (для пагинации)
        /// </summary>
        public int Offset { get; }
    }
}