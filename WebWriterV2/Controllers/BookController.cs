using Dao;
using Dao.IRepository;
using Dao.Model;
using Dao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebWriterV2.FrontModels;

namespace WebWriterV2.Controllers
{
    public class BookController : ApiController
    {
        private IBookRepository BookRepository { get; set; }

        public BookController(IBookRepository bookRepository)
        {
            BookRepository = bookRepository;
        }

        public FrontBook GetBook(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new FrontBook(book, true);
            return frontBook;
        }

        public List<FrontBook> GetBooks(long? userId)
        {
            List<Book> books;
            if (userId.HasValue) {
                books = BookRepository.GetByUser(userId.Value);
            } else {
                books = BookRepository.GetAll(true);
            }

            var frontBooks = books.Select(x => new FrontBook(x)).ToList();
            return frontBooks;
        }
    }
}
