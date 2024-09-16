using Api.Endpoints.Doctors.Responses;
using Api.Endpoints.Patients.Responses;
using Api.Entities;
using Riok.Mapperly.Abstractions;

namespace Api.Mapping;

[Mapper]
public static partial class MapperConfiguration
{
    [MapProperty(nameof(@Doctor.Room.Number), nameof(DoctorDto.Room))]
    [MapProperty(nameof(@Doctor.Specialization.Name), nameof(DoctorDto.Specialization))]
    [MapProperty(nameof(@Doctor.District.Number), nameof(DoctorDto.District))]
    public static partial DoctorDto ToDto(this Doctor doctor);

    [MapProperty(nameof(@Patient.District.Number), nameof(PatientDto.District))]
    [MapProperty(nameof(Patient.DateOfBirth), nameof(PatientDto.BirthDate))]
    public static partial PatientDto ToDto(this Patient patient);
}
