namespace Api.Endpoints.Doctors.Responses;

/// <summary>
/// Идентификаторы моделей врача.
/// </summary>
/// <param name="Id">Идентификатор врача.</param>
/// <param name="RoomId">Идентификатор кабинета.</param>
/// <param name="SpecializationId">Идентификатор специализации.</param>
/// <param name="DistrictId">Идентификатор участка (для участковых врачей).</param>
public sealed record DoctorResponse(
    Guid Id,
    Guid RoomId,
    Guid SpecializationId,
    Guid? DistrictId);
