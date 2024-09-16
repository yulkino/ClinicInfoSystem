using Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api;

public sealed class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Doctor>(ConfigureDoctorEntity);
        modelBuilder.Entity<Patient>(ConfigurePatientEntity);
        modelBuilder.Entity<District>(ConfigureDistrictEntity);
        modelBuilder.Entity<Room>(ConfigureRoomEntity);
        modelBuilder.Entity<Specialization>(ConfigureSpecializationEntity);
    }

    private static void ConfigureDoctorEntity(EntityTypeBuilder<Doctor> entity)
    {
        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Room)
            .WithMany()
            .HasForeignKey("RoomId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Specialization)
            .WithMany()
            .HasForeignKey("SpecializationId")
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.District)
            .WithMany()
            .HasForeignKey("DistrictId")
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurePatientEntity(EntityTypeBuilder<Patient> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Surname)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Patronymic)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.DateOfBirth)
            .IsRequired();

        entity.Property(e => e.Gender)
            .IsRequired()
            .HasMaxLength(10);

        entity.HasOne(e => e.District)
            .WithMany()
            .HasForeignKey("DistrictId")
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureDistrictEntity(EntityTypeBuilder<District> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Number)
            .IsRequired();
    }

    private static void ConfigureRoomEntity(EntityTypeBuilder<Room> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Number)
            .IsRequired();
    }

    private static void ConfigureSpecializationEntity(EntityTypeBuilder<Specialization> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
