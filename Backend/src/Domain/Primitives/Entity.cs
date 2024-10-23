using System.ComponentModel.DataAnnotations;

namespace Domain.Primitives;

public abstract class Entity(Guid id)
{
    [Key]
    public Guid Id { get; protected set; } = id;
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;

    [Timestamp]
    // https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#native-database-generated-concurrency-tokens
    public byte[] Version { get; protected set; } = new byte[8];
}
