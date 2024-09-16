using System.ComponentModel.DataAnnotations;

namespace Api.Endpoints.Doctors.Requests;

/// <summary>
/// Модель для добавления информации о враче.
/// </summary>
/// <param name="FullName">Полное имя.</param>
/// <param name="Room">Номер кабинета.</param>
/// <param name="Specialization">Специализация.</param>
/// <param name="District">Номер участка (для участковых врачей).</param>
public sealed record AddDoctorRequest(
    [Required] string FullName,
    [Required] int Room,
    [Required] string Specialization,
    int? District);
