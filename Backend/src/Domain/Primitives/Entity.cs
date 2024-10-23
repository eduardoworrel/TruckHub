using System.ComponentModel.DataAnnotations;

namespace Domain.Primitives;

public abstract class Entity(Guid id)
{
    [Key]
    public Guid Id { get; protected set; } = id;
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
}
