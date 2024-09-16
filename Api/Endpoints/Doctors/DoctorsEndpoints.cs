using System.Linq.Expressions;
using Api.Endpoints.Doctors.Requests;
using Api.Endpoints.Doctors.Responses;
using Api.Entities;
using Api.Extensions;
using Api.Mapping;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Doctors;

public static class DoctorsEndpoints
{
    public static IEndpointRouteBuilder MapDoctorsEndpoints(this IEndpointRouteBuilder builder)
    {
        var doctorsGroup = builder
            .MapGroup("doctors")
            .WithTags("Doctors");

        doctorsGroup.MapGet("", GetDoctors);
        doctorsGroup.MapPost("", AddDoctor);
        doctorsGroup.MapGet("{doctorId:guid}", GetDoctor);
        doctorsGroup.MapPatch("{doctorId:guid}", EditDoctor);
        doctorsGroup.MapDelete("{doctorId:guid}", DeleteDoctor);

        return builder;
    }

    /// <summary>
    /// Получить страницу из списка врачей.
    /// </summary>
    /// <param name="orderBy">Параметр для сортировки списка.</param>
    /// <param name="count">Количество элементов на странице.</param>
    /// <param name="page">Номер страницы для отображения.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Страница из списка врачей.</returns>
    private static async Task<Ok<DoctorsResponse>> GetDoctors(
        [FromQuery] string? orderBy,
        [FromQuery] int count,
        [FromQuery] int page,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        Expression<Func<Doctor, object?>>? orderByExpression = orderBy?.ToLowerInvariant() switch
        {
            "fullname" => d => d.FullName,
            "room" => d => d.Room.Number,
            "specialization" => d => d.Specialization.Name,
            "district" => d => d.District != null ? d.District.Number : null,
            _ => null
        };

        var doctorPaginatedList = await context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Room)
            .Include(d => d.District)
            .GetPaginatedListAsync(
                count,
                page,
                orderByExpression,
                cancellationToken);

        return TypedResults.Ok(new DoctorsResponse(
            doctorPaginatedList.TotalCount,
            doctorPaginatedList.TotalPages,
            doctorPaginatedList.HasPreviousPage,
            doctorPaginatedList.HasNextPage,
            doctorPaginatedList.Items.Select(d => d.ToDto())));
    }

    /// <summary>
    /// Добавить информацию о враче.
    /// </summary>
    /// <param name="request">Модель для добавления информации о враче.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус попытки добавления информации о враче.</returns>
    private static async Task<Created> AddDoctor(
        [FromBody] AddDoctorRequest request,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var specialization = await context.GetOrCreateSpecializationAsync(request.Specialization, cancellationToken);
        var room = await context.GetOrCreateRoomAsync(request.Room, cancellationToken);
        District? district = default;
        if (request.District is not null)
            district = await context.GetOrCreateDistrictAsync(request.District.Value, cancellationToken);

        context.Doctors.Add(new Doctor
        {
            FullName = request.FullName,
            Specialization = specialization,
            Room = room,
            District = district
        });

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created();
    }

    /// <summary>
    /// Получить идентификаторы моделей врача.
    /// </summary>
    /// <param name="doctorId">Идентификатор врача.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификаторы моделей врача.</returns>
    private static async Task<Results<Ok<DoctorResponse>, NotFound>> GetDoctor(
        [FromRoute] Guid doctorId,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var doctor = await context.GetDoctorOrDefault(doctorId, cancellationToken);

        if (doctor is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(new DoctorResponse(doctor.Id, doctor.Room.Id, doctor.Specialization.Id, doctor.District?.Id));
    }

    /// <summary>
    /// Редактировать информацию о враче.
    /// </summary>
    /// <param name="doctorId">Идентификатор врача.</param>
    /// <param name="request">Модель для редактирования информации о враче.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус попытки реадктирования информации о враче.</returns>
    private static async Task<Results<NoContent, NotFound>> EditDoctor(
        [FromRoute] Guid doctorId,
        [FromBody] EditDoctorRequest request,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var doctor = await context.GetDoctorOrDefault(doctorId, cancellationToken);

        if (doctor is null)
            return TypedResults.NotFound();

        if (request.FullName is not null)
            doctor.FullName = request.FullName;

        if (request.Specialization is not null)
        {
            var specialization = await context.GetOrCreateSpecializationAsync(request.Specialization, cancellationToken);
            doctor.Specialization = specialization;
        }

        if (request.Room is not null)
        {
            var room = await context.GetOrCreateRoomAsync(request.Room.Value, cancellationToken);

            doctor.Room = room;
        }

        if (request.IsDistrictUpdated)
        {
            District? district = null;
            if (request.District is not null)
                district = await context.GetOrCreateDistrictAsync(request.District.Value, cancellationToken);
            doctor.District = district;
        }

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Удалить информацию о враче.
    /// </summary>
    /// <param name="doctorId">Идентификатор врача.</param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Статус попытки удаления информации о враче.</returns>
    private static async Task<Results<NoContent, NotFound>> DeleteDoctor(
        [FromRoute] Guid doctorId,
        [FromServices] ApplicationContext context,
        CancellationToken cancellationToken)
    {
        var doctor = await context.GetDoctorOrDefault(doctorId, cancellationToken);

        if (doctor is null)
            return TypedResults.NotFound();

        context.Doctors.Remove(doctor);

        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
