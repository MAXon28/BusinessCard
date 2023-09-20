namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Значение условия
    /// </summary>
    /// <param name="IsAvailable"> Выполняется ли условие в тарифе </param>
    /// <param name="Value"> Значение </param>
    public record ConditionValueDto(bool IsAvailable, string? Value = null);
}