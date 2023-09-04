namespace Itmo.Dev.Asap.Core.Application.Dto.Study;

public class StudentGroupDto
{
    public StudentGroupDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}