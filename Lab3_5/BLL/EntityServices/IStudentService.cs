using Lab3_5.BLL.Models;

namespace Lab3_5.BLL.EntityServices
{
    public interface IStudentService : IEntityService<Student>
    {
        IReadOnlyList<Student> GetStudentsWithHealthyWeight();
    }
}