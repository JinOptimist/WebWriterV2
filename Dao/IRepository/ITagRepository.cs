using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Tag GetOrCreate(string tagName);
    }
}