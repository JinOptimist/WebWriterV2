using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebWriterV2.Dao.IDao;

namespace WebWriterV2.Dao
{
    public class BookDao : IBookDao
    {
        public BookDao(RepositoryContainer repositoryContainer)
        {
            Repositories = repositoryContainer;
        }

        public RepositoryContainer Repositories {get;}

        //public Book Get(id)
        //{
        //    Repositories.BookRepository.Get(id)
        //}
    }
}