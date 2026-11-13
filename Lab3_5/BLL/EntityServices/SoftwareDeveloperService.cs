using Lab3_5.BLL.Models;
using Lab3_5.DAL;
using Lab3_5.DAL.Entities;

namespace Lab3_5.BLL.EntityServices;

public class SoftwareDeveloperService : EntityService<SoftwareDeveloperEntity, SoftwareDeveloper>, ISoftwareDeveloperService
{
    public SoftwareDeveloperService(IEntityContext<SoftwareDeveloperEntity> entityContext) : base(entityContext)
    {
    }
}
