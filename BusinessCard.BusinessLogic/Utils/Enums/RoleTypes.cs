using BusinessCard.BusinessLogicLayer.Utils.Attributes;
using BusinessCard.Entities;
using System;

namespace BusinessCard.BusinessLogicLayer.Utils.Enums
{
    /// <summary>
    /// Уровень прав доступа
    /// </summary>
    [Flags]
    public enum RoleTypes
    {
        /// <summary>
        /// MAXon28 (создательи владелец)
        /// </summary>
        [MAXon28(Roles.MAXon28)]
        [MAXonTeam]
        MAXon28 = 1,

        /// <summary>
        /// Администратор
        /// </summary>
        [MAXon28(Roles.Admin)]
        [MAXonTeam]
        Admin = 2,

        /// <summary>
        /// Пользователь
        /// </summary>
        [MAXon28(Roles.User)]
        User = 3,

        /// <summary>
        /// Бот-секретарь (автоматизирует часть операций в системе)
        /// </summary>
        [MAXon28(Roles.MAXon28Bot)]
        [MAXonTeam]
        MAXon28Bot = 4,

        /// <summary>
        /// Общее значение для команды MAXon28
        /// </summary>
        MAXon28Team = 5
    }
}