using Lab3_5.BLL.Models;
using Lab3_5.DAL;
using Lab3_5.DAL.Entities;

namespace Lab3_5.BLL.EntityServices;

public class LibrarianService : EntityService<LibrarianEntity, Librarian>, ILibrarianService
{
    public LibrarianService(IEntityContext<LibrarianEntity> entityContext) : base(entityContext)
    {
    }
}
