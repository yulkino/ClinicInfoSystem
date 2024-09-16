using System.Linq.Expressions;
using Api.Entities;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions;

public static class ApplicationContextExtensions
{
    public static async Task<Doctor?> GetDoctorOrDefault(
        this ApplicationContext context,
        Guid doctorId,
        CancellationToken cancellationToken) =>
        await context.Doctors
            .Include(d => d.Specialization)
            .Include(d => d.Room)
            .Include(d => d.District)
            .SingleOrDefaultAsync(d => d.Id == doctorId, cancellationToken);

    public static async Task<Patient?> GetPatientOrDefault(
        this ApplicationContext context,
        Guid patientId,
        CancellationToken cancellationToken) =>
        await context.Patients
            .Include(d => d.District)
            .SingleOrDefaultAsync(p => p.Id == patientId, cancellationToken);

    public static async Task<PaginatedList<T>> GetPaginatedListAsync<T>(
        this IQueryable<T> entityDbSet,
        int count,
        int page,
        Expression<Func<T, object?>>? orderByExpression,
        CancellationToken cancellationToken)
        where T : class
    {
        if (orderByExpression is not null)
            entityDbSet = entityDbSet.OrderBy(orderByExpression);

        var entitiesList = await entityDbSet
            .Skip(count * (page - 1))
            .Take(count)
            .ToListAsync(cancellationToken);
        var totalCount = await entityDbSet.CountAsync(cancellationToken);
        var totalPages = (int) Math.Round((double) totalCount / count, MidpointRounding.ToPositiveInfinity);

        return new(entitiesList, page, totalCount, totalPages);
    }

    public static async Task<Specialization> GetOrCreateSpecializationAsync(
        this ApplicationContext context,
        string specializationName,
        CancellationToken cancellationToken)
    {
        var specializationExists = await context.Specializations
            .AnyAsync(s => s.Name == specializationName, cancellationToken);
        Specialization specialization;
        if (!specializationExists)
            specialization = context.Specializations
                .Add(new Specialization
                {
                    Name = specializationName
                }).Entity;
        else
            specialization = await context.Specializations
                .SingleAsync(s => s.Name == specializationName, cancellationToken);

        return specialization;
    }

    public static async Task<Room> GetOrCreateRoomAsync(
        this ApplicationContext context,
        int roomNumber,
        CancellationToken cancellationToken)
    {
        var roomExists = await context.Rooms
            .AnyAsync(r => r.Number == roomNumber, cancellationToken);
        Room room;
        if (!roomExists)
            room = context.Rooms
                .Add(new Room
                {
                    Number = roomNumber
                }).Entity;
        else
            room = await context.Rooms
                .SingleAsync(r => r.Number == roomNumber, cancellationToken);

        return room;
    }

    public static async Task<District> GetOrCreateDistrictAsync(
        this ApplicationContext context,
        int districtNumber,
        CancellationToken cancellationToken)
    {
        var districtExists = await context.Districts
            .AnyAsync(r => r.Number == districtNumber, cancellationToken);
        District district;
        if (!districtExists)
            district = context.Districts
                .Add(new District
                {
                    Number = districtNumber
                }).Entity;
        else
            district = await context.Districts
                .SingleAsync(r => r.Number == districtNumber, cancellationToken);

        return district;
    }
}
