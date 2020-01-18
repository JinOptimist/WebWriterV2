using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class BookCommentRepository : BaseRepository<BookComment>, IBookCommentRepository
    {
        public BookCommentRepository(WriterContext db) : base(db)
        {
        }
    }
}