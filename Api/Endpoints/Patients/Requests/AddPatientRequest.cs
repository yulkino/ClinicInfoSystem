using System.ComponentModel.DataAnnotations;

namespace Api.Endpoints.Patients.Requests;

/// <summary>
/// Модель для добавления информации о пациенте.
/// </summary>
/// <param name="Name">Имя.</param>
/// <param name="Surname">Фамилия.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Address">Адрес.</param>
/// <param name="BirthDate">Дата рождения.</param>
/// <param name="Gender">Пол.</param>
/// <param name="District">Номер участка.</param>
public sealed record AddPatientRequest(
    [Required] string Name,
    [Required] string Surname,
    [Required] string Patronymic,
    [Required] string Address,
    [Required] DateTime BirthDate,
    [Required] string Gender,
    [Required] int District);
