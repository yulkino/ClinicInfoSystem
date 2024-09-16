namespace Api.Endpoints.Patients.Responses;

/// <summary>
/// Страница со списком пациентов.
/// </summary>
/// <param name="TotalCount">Общее количество записей.</param>
/// <param name="TotalPages">Общее количество страниц.</param>
/// <param name="HasPreviousPage">Признак существования предыдущей страницы.</param>
/// <param name="HasNextPage">Признак существования следующей страницы.</param>
/// <param name="Patients">Список пациентов.</param>
public sealed record PatientsResponse(
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage,
    IEnumerable<PatientDto> Patients);

/// <summary>
/// Список пациентов.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="Name">Имя.</param>
/// <param name="Surname">Фамилия.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Address">Адрес.</param>
/// <param name="BirthDate">Дата рождения.</param>
/// <param name="Gender">Пол.</param>
/// <param name="District">Номер участка.</param>
public sealed record PatientDto(
    Guid Id,
    string Name,
    string Surname,
    string Patronymic,
    string Address,
    DateTime BirthDate,
    string Gender,
    int District);
