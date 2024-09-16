using System.Linq.Expressions;
using Api.Endpoints.Patients.Requests;
using Api.Endpoints.Patients.Responses;
using Api.Entities;
using Api.Extensions;
using Api.Mapping;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Patients;

public static class PatientsEndpoints
{
    public static IEndpointRouteBuilder MapPatientsEndpoints(this IEndpointRouteBuilder builder)
    {
        var doctorsGroup = builder
            .MapGroup("patients")
            .WithTags("Patients");

        doctorsGroup.MapGet("", GetPatients);
        doctorsGroup.MapPost("", AddPatient);
        doctorsGroup.MapGet("{patientId:guid}", GetPatient);
        doctorsGroup.MapPatch("{patientId:guid}", EditPatient);
        doctorsGroup.MapDelete("{patientId:guid}", DeletePatient);

        return builder;
    }

    /// <summary>
    /// Получить страницу из списка пациентов.
    /// </summary>
    /// <param name="orderBy">Параметр для сортировки списка.</param>
    /// <param name="count">Количество элементов на странице.</param>
    /// <param name="page">Номер страницы для отображения.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Страница из списка пациентов.</returns>
    private static async Task<Ok<PatientsResponse>> GetPatients(
        [FromQuery] string? orderBy,
        [FromQuery] int count,
        [FromQuery] int page,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        Expression<Func<Patient, object?>>? orderByExpression = orderBy?.ToLowerInvariant() switch
        {
            "name" => p => p.Name,
            "surname" => p => p.Surname,
            "patronymic" => p => p.Patronymic,
            "address" => p => p.Address,
            "birthdate" => p => p.DateOfBirth,
            "gender" => p => p.Gender,
            "district" => p => p.District.Number,
            _ => null
        };

        var patientsPaginatedList = await context.Patients
            .Include(d => d.District)
            .GetPaginatedListAsync(
                count,
                page,
                orderByExpression,
                cancellationToken);

        return TypedResults.Ok(new PatientsResponse(
            patientsPaginatedList.TotalCount,
            patientsPaginatedList.TotalPages,
            patientsPaginatedList.HasPreviousPage,
            patientsPaginatedList.HasNextPage,
            patientsPaginatedList.Items.Select(d => d.ToDto())));
    }

    /// <summary>
    /// Добавить информацию о пациенте.
    /// </summary>
    /// <param name="request">Модель для добавления информации о пациенте.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус попытки добавления информации о пациенте.</returns>
    private static async Task<Created> AddPatient(
        [FromBody] AddPatientRequest request,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        context.Patients.Add(new Patient
        {
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            Address = request.Address,
            DateOfBirth = request.BirthDate,
            Gender = request.Gender,
            District = await context.GetOrCreateDistrictAsync(request.District, cancellationToken)
        });

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created();
    }

    /// <summary>
    /// Получить идентификаторы моделей пациента.
    /// </summary>
    /// <param name="patientId">Идентификатор пациента.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификаторы моделей пациента.</returns>
    private static async Task<Results<Ok<PatientResponse>, NotFound>> GetPatient(
        [FromRoute] Guid patientId,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var patient = await context.GetPatientOrDefault(patientId, cancellationToken);

        if (patient is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(new PatientResponse(patient.Id, patient.District.Id));
    }

    /// <summary>
    /// Редактировать информацию о пациенте.
    /// </summary>
    /// <param name="patientId">Идентификатор пациента.</param>
    /// <param name="request">Модель для редактирования информации о пациенте.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус попытки реадктирования информации о пациенте.</returns>
    private static async Task<Results<NoContent, NotFound>> EditPatient(
        [FromRoute] Guid patientId,
        [FromBody] EditPatientRequest request,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var patient = await context.GetPatientOrDefault(patientId, cancellationToken);

        if (patient is null)
            return TypedResults.NotFound();

        if (request.Name is not null)
            patient.Name = request.Name;

        if (request.Surname is not null)
            patient.Surname = request.Surname;

        if (request.Patronymic is not null)
            patient.Patronymic = request.Patronymic;

        if (request.Address is not null)
            patient.Address = request.Address;

        if (request.BirthDate is not null)
            patient.DateOfBirth = request.BirthDate.Value;

        if (request.Gender is not null)
            patient.Gender = request.Gender;

        if (request.District is not null)
        {
            var district = await context.GetOrCreateDistrictAsync(request.District.Value, cancellationToken);
            patient.District = district;
        }

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить информацию о пациенте.
    /// </summary>
    /// <param name="patientId">Идентификатор пациента.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус попытки удаления информации о пациенте.</returns>
    private static async Task<Results<NoContent, NotFound>> DeletePatient(
        [FromRoute] Guid patientId,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var patient = await context.GetPatientOrDefault(patientId, cancellationToken);

        if (patient is null)
            return TypedResults.NotFound();

        context.Patients.Remove(patient);

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
