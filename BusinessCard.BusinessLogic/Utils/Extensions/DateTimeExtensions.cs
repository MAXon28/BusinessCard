using System;

namespace BusinessCard.BusinessLogicLayer.Utils.Extensions
{
    /// <summary>
    /// Расширение для даты и времени
    /// </summary>
    internal static class DateTimeExtensions
    {
        /// <summary>
        /// Конвертировать дату в читаемый формат
        /// </summary>
        /// <param name="date"> Дата </param>
        /// <returns> Читемый формат даты </returns>
        public static string ConvertToReadableFormat(this DateTime date)
        {
            var currentDate = DateTime.Now;
            var inCurrentYear = currentDate.Year == date.Year;
            var inCurrentDay = inCurrentYear && currentDate.Month == date.Month && currentDate.Day == date.Day;
            var inPreviousDay = inCurrentYear && currentDate.Month == date.Month && currentDate.Day - 1 == date.Day;
            var inNextDay = inCurrentYear && currentDate.Month == date.Month && currentDate.Day + 1 == date.Day;
            return (inCurrentYear, inCurrentDay, inPreviousDay, inNextDay) switch
            {
                (false, false, false, false) => date.ToString("d MMMM yyyy"),
                (true, false, false, false) => date.ToString("d MMMM"),
                (true, true, false, false) => "Сегодня",
                (true, false, true, false) => "Вчера",
                (true, false, false, true) => "Завтра",
                _ => date.ToString("d")
            };
        }

        /// <summary>
        /// Конвертировать дату и время в чиатемый формат
        /// </summary>
        /// <param name="dateTime"> Дата и время </param>
        /// <returns> Читаемый формат даты и времени </returns>
        public static string ConvertToReadableFormatWithTime(this DateTime dateTime)
        {
            var currentDateTime = DateTime.Now;
            var inCurrentYear = currentDateTime.Year == dateTime.Year;
            var inCurrentDay = inCurrentYear && currentDateTime.Month == dateTime.Month && currentDateTime.Day == dateTime.Day;
            var inPreviousDay = inCurrentYear && currentDateTime.Month == dateTime.Month && currentDateTime.Day - 1 == dateTime.Day;
            var inNextDay = inCurrentYear && currentDateTime.Month == dateTime.Month && currentDateTime.Day + 1 == dateTime.Day;
            return (inCurrentYear, inCurrentDay, inPreviousDay, inNextDay) switch
            {
                (false, false, false, false) => $"{dateTime:d MMMM yyyy} в {dateTime:H:mm}",
                (true, false, false, false) => $"{dateTime:d MMMM} в {dateTime:H:mm}",
                (true, true, false, false) => $"Сегодня в {dateTime:H:mm}",
                (true, false, true, false) => $"Вчера в {dateTime:H:mm}",
                (true, false, false, true) => $"Завтра в {dateTime:H:mm}",
                _ => dateTime.ToString("g")
            };
        }
    }
}