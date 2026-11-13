using Lab3_5.BLL.Models;
using Lab3_5.DAL;
using Lab3_5.DAL.Entities;
using Mapster;

namespace Lab3_5.BLL.EntityServices;

public class StudentService : EntityService<StudentEntity, Student>, IStudentService
{
    public StudentService(IEntityContext<StudentEntity> entityContext) : base(entityContext)
    {
    }

    public IReadOnlyList<Student> GetStudentsWithHealthyWeight()
    {
        var entities = _entityContext.Load();
        return entities.Where(s => s.Height - 110 == s.Weight).Select(s => s.Adapt<Student>()).ToList().AsReadOnly();
    }
}
