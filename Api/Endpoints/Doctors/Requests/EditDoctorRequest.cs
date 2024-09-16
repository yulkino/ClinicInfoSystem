namespace Api.Endpoints.Doctors.Requests;

/// <summary>
/// Модель для редактирования информации о враче.
/// </summary>
/// <param name="FullName">Полное имя.</param>
/// <param name="Room">Номер кабинета.</param>
/// <param name="Specialization">Специализация.</param>
/// <param name="IsDistrictUpdated">Признак обновления участка.</param>
/// <param name="District">Номер участка.</param>
public sealed record EditDoctorRequest(
    string? FullName,
    int? Room,
    string? Specialization,
    bool IsDistrictUpdated,
    int? District);
