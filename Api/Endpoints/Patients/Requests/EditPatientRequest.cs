namespace Api.Endpoints.Patients.Requests;

/// <summary>
/// Модель для редактирования информации о пациенте.
/// </summary>
/// <param name="Name">Имя.</param>
/// <param name="Surname">Фамилиия.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Address">Адрес.</param>
/// <param name="BirthDate">Дата рожения.</param>
/// <param name="Gender">Пол.</param>
/// <param name="District">Участок.</param>
public sealed record EditPatientRequest(
    string? Name,
    string? Surname,
    string? Patronymic,
    string? Address,
    DateTime? BirthDate,
    string? Gender,
    int? District);
