namespace Api.Entities;

public sealed class Doctor
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Room Room { get; set; } = default!;
    public Specialization Specialization { get; set; } = default!;
    public District? District { get; set; }
}
