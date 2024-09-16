namespace Api.Endpoints.Doctors.Responses;

/// <summary>
/// Страница со списком врачей.
/// </summary>
/// <param name="TotalCount">Общее количество записей.</param>
/// <param name="TotalPages">Общее количество страниц.</param>
/// <param name="HasPreviousPage">Признак существования предыдущей страницы.</param>
/// <param name="HasNextPage">Признак существования следующей страницы.</param>
/// <param name="Doctors">Список врачей.</param>
public sealed record DoctorsResponse(
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage,
    IEnumerable<DoctorDto> Doctors);

/// <summary>
/// Список врчаей.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="FullName">Полное имя.</param>
/// <param name="Room">Номер кабинета.</param>
/// <param name="Specialization">Специализация.</param>
/// <param name="District">Номер участка.</param>
public sealed record DoctorDto(
    Guid Id,
    string FullName,
    int Room,
    string Specialization,
    int? District);
