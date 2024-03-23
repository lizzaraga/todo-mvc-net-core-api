namespace Todo_API.Models.Abstracts;

public interface ITimedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
