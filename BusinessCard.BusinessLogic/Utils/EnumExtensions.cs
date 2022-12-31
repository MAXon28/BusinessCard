using System;

namespace BusinessCard.BusinessLogicLayer.Utils
{
    /// <summary>
    /// Расширение для перечислений
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Конвертация из строки в перечисление
        /// </summary>
        /// <typeparam name="T"> Тип перечисления </typeparam>
        /// <param name="value"> Строкове значение </param>
        /// <returns> Перечисление </returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            var names = Enum.GetNames(typeof(T));

            var valueInUpperRegistr = value.ToUpper();

            foreach (var name in names)
            {
                var enumObject = Enum.Parse(typeof(T), name, true);
                if (valueInUpperRegistr == ((Enum)enumObject).ToStringAttribute())
                    return (T)enumObject;
            }
            return default;
        }

        /// <summary>
        /// Преобразованть в строковое значение
        /// </summary>
        /// <param name="value"> Перечисление </param>
        /// <returns> Строковое значение </returns>
        public static string ToStringAttribute(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (MAXon28Attribute[])fieldInfo.GetCustomAttributes(typeof(MAXon28Attribute), false);
            return attributes.Length > 0 ? attributes[0].Name.ToUpper() : value.ToString();
        }
    }
}