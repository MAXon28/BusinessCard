using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <summary>
    /// Настройки запроса проектов
    /// </summary>
    internal class ProjectRequestSettings : IRequestSettings
    {
        internal ProjectRequestSettings(string searchText, List<int> typesId, List<int> categoriesId, List<int> compatibilitiesId, int typeOfSort, int offset)
        {
            SearchText = searchText;

            FilterValuesDictionary[FilterConstants.ProjectType] = typesId;
            FilterValuesDictionary[FilterConstants.ProjectCategory] = categoriesId;
            FilterValuesDictionary[FilterConstants.ProjectCompatibility] = compatibilitiesId;

            TypeOfSort = (SortTypes)typeOfSort;

            Offset = offset;
        }

        /// <summary>
        /// Поисковый запрос (название проекта или его часть)
        /// </summary>
        internal string SearchText { get; }

        /// <summary>
        /// Словарь значений для фильтрации
        /// </summary>
        internal Dictionary<string, List<int>> FilterValuesDictionary { get; } = new Dictionary<string, List<int>>
        {
            [FilterConstants.ProjectType] = new List<int>(),
            [FilterConstants.ProjectCategory] = new List<int>(),
            [FilterConstants.ProjectCompatibility] = new List<int>()
        };

        /// <summary>
        /// Тип сортировки
        /// </summary>
        internal SortTypes TypeOfSort { get; }

        /// <summary>
        /// Сдвиг проектов на конкретное число (для пагинации)
        /// </summary>
        internal int Offset { get; }
    }
}