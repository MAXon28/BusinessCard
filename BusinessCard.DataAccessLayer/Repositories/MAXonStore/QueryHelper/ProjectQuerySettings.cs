using System.Collections.Generic;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper
{
    /// <summary>
    /// Настройки запроса проектов
    /// </summary>
    public class ProjectQuerySettings
    {
        public ProjectQuerySettings(string projectName, List<int> typesId, List<int> categoriesId, List<int> compatibilitiesId, int typeOfSort, int offset)
        {
            ProjectName = projectName;

            FilterValuesDictionary[FilterConstants.ProjectType] = typesId;
            FilterValuesDictionary[FilterConstants.ProjectCategory] = categoriesId;
            FilterValuesDictionary[FilterConstants.ProjectCompatibility] = compatibilitiesId;

            TypeOfSort = (TypeOfSort) typeOfSort;

            Offset = offset;
        }

        /// <summary>
        /// Название или часть названия проекта
        /// </summary>
        internal string ProjectName { get; set; }

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
        internal TypeOfSort TypeOfSort { get; }

        /// <summary>
        /// Сдвиг проектов на конкретное число (для пагинации)
        /// </summary>
        internal int Offset { get; }
    }
}