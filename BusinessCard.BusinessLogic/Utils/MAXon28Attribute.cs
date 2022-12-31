using System;

namespace BusinessCard.BusinessLogicLayer.Utils
{
    /// <summary>
    /// Атрибут для наименования полей (элементов перечислений)
    /// </summary>
    internal class MAXon28Attribute : Attribute
    {
        public MAXon28Attribute() => Name = string.Empty;

        public MAXon28Attribute(string name) => Name = name;

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; }
    }
}