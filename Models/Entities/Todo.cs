using Todo_API.Models.Abstracts;

namespace Todo_API.Models.Entities;

public class Todo: ITimedEntity
{
    public Guid Id { get; set; }

    public string Label { get; set; }
    public string Description { get; set; }

    public bool isCompleted { get; set; }
    public bool isArchived { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
