namespace Api.Endpoints.Patients.Responses;

/// <summary>
/// Идентификаторы моделей пациента.
/// </summary>
/// <param name="Id">Идентификатор пациента.</param>
/// <param name="DistrictId">Идентификатор участка.</param>
public sealed record PatientResponse(
    Guid Id,
    Guid DistrictId);
