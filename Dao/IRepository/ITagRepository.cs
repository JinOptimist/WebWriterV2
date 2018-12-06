using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Tag GetOrCreate(string tagName);
    }
}